using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.Players;


namespace ModLibsGeneral.Libraries.Recipes {
	/// <summary></summary>
	[Flags]
	public enum RecipeCraftFailReason {
		/// <summary></summary>
		NeedsNearbyWater = 1,
		/// <summary></summary>
		NeedsNearbyHoney = 2,
		/// <summary></summary>
		NeedsNearbyLava = 4,
		/// <summary></summary>
		NeedsNearbySnowBiome = 8,
		/// <summary></summary>
		MissingTile = 16,
		/// <summary></summary>
		MissingItem = 32
	}




	/// <summary>
	/// Assorted static library functions pertaining to recipes.
	/// </summary>
	public partial class RecipeLibraries {
		/// <summary>
		/// Reports all the reasons a given recipe for a givne player will fail with a given set of ingredients (defaults to
		/// the player's inventory).
		/// </summary>
		/// <param name="player"></param>
		/// <param name="recipe"></param>
		/// <param name="missingTile">Returns the tile IDs (crafting stations) needed for the recipe.</param>
		/// <param name="missingItemTypesStacks">Returns the missing item ids and amounts for the recipe.</param>
		/// <param name="availableIngredients"></param>
		/// <returns></returns>
		public static RecipeCraftFailReason GetRecipeFailReasons(
					Player player,
					Recipe recipe,
					out int[] missingTile,
					out IDictionary<int, int> missingItemTypesStacks,
					IEnumerable<Item> availableIngredients = null ) {
			RecipeCraftFailReason reason = 0;
			var missingTileList = new List<int>();
			missingItemTypesStacks = new Dictionary<int, int>();

			// Get available item ingredients
			if( availableIngredients == null ) {
				availableIngredients = player.inventory
					.Take( 58 )
					.Where( item => !item.IsAir );

				bool? _;
				Item[] chest = PlayerItemLibraries.GetCurrentlyOpenChest( player, out _ );
				if( chest != null ) {
					availableIngredients = availableIngredients.Concat( chest );
				}
			}

			// Process ingredients list into id + stack map
			IDictionary<int, int> availIngredientInfo = new Dictionary<int, int>( availableIngredients.Count() );
			foreach( Item item in availableIngredients ) {
				if( availIngredientInfo.ContainsKey( item.netID) ) {
					availIngredientInfo[ item.netID ] += item.stack;
				} else {
					availIngredientInfo[ item.netID ] = item.stack;
				}
			}

			// Tiles
			for( int i=0; i < recipe.requiredTile.Length; i++ ) {
				int reqTileType = recipe.requiredTile[i];
				if( reqTileType == -1 ) { break; }

				if( !player.adjTile[ reqTileType ] ) {
					missingTileList.Add( reqTileType );
					reason |= RecipeCraftFailReason.MissingTile;
				}
			}

			// Items
			for( int i = 0; i < recipe.requiredItem.Length; i++ ) {
				Item reqItem = recipe.requiredItem[i];
				if( reqItem == null || reqItem.type == ItemID.None ) { break; }

				int reqStack = reqItem.stack;
				bool hasCheckedGroups = false;

				foreach( var kv in availIngredientInfo ) {
					int itemType = kv.Key;
					int itemStack = kv.Value;

					if( recipe.useWood( itemType, reqItem.type )
							|| recipe.useSand( itemType, reqItem.type )
							|| recipe.useIronBar( itemType, reqItem.type )
							|| recipe.useFragment( itemType, reqItem.type )
							|| recipe.usePressurePlate( itemType, reqItem.type )
							|| recipe.AcceptedByItemGroups( itemType, reqItem.type ) ) {
						reqStack -= itemStack;
						hasCheckedGroups = true;
					}
				}
				if( !hasCheckedGroups && availIngredientInfo.ContainsKey(reqItem.netID) ) {
					reqStack -= availIngredientInfo[ reqItem.netID ];
				}

				// Account for missing ingredients:
				if( reqStack > 0 ) {
					missingItemTypesStacks[ reqItem.netID ] = reqStack;
					reason |= RecipeCraftFailReason.MissingItem;
				}
			}
			
			if( recipe.needWater && !player.adjWater && !player.adjTile[172] ) {
				reason |= RecipeCraftFailReason.NeedsNearbyWater;
			}
			if( recipe.needHoney && !player.adjHoney ) {
				reason |= RecipeCraftFailReason.NeedsNearbyHoney;
			}
			if( recipe.needLava && !player.adjLava ) {
				reason |= RecipeCraftFailReason.NeedsNearbyLava;
			}
			if( recipe.needSnowBiome && !player.ZoneSnow ) {
				reason |= RecipeCraftFailReason.NeedsNearbySnowBiome;
			}

			missingTile = missingTileList.ToArray();
			return reason;
		}
		
		////////////////

		/// <summary>
		/// Gets available recipes of a given set of ingredient items for a given player.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="ingredients"></param>
		/// <returns></returns>
		public static IList<int> GetAvailableRecipesOfIngredients( Player player, IEnumerable<Item> ingredients ) {
			int[] _;
			IDictionary<int, int> __;
			IList<int> addedRecipeIndexes = new List<int>();
			ISet<int> possibleRecipeIdxs = new HashSet<int>();

			foreach( Item ingredient in ingredients ) {
				IEnumerable<int> ingredientRecipeIdxs = RecipeFinderLibraries.GetRecipeIndexesOfItem( ingredient.netID );

				foreach( int recipeIdx in ingredientRecipeIdxs ) {
					possibleRecipeIdxs.Add( recipeIdx );
				}
			}

			foreach( int recipeIdx in possibleRecipeIdxs ) {
				Recipe recipe = Main.recipe[recipeIdx];
				if( recipe.createItem.type == ItemID.None ) { continue; } // Just in case?

				if( RecipeLibraries.GetRecipeFailReasons( player, recipe, out _, out __, ingredients ) == 0 ) {
					addedRecipeIndexes.Add( recipeIdx );
				}
			}

			return addedRecipeIndexes;
		}


		////////////////

		/// <summary>
		/// Indicates if a given item type has the ingredients it needs to be crafted from any of its recipes. Does not check
		/// tile requirements.
		/// </summary>
		/// <param name="itemType">Item type to find recipes for.</param>
		/// <param name="minimumIngredients">Minimum (<) and maximum (>=) quantities of ingredient item types.</param>
		/// <returns></returns>
		public static bool ItemHasIngredients( int itemType, IDictionary<int, (int min, int max)> minimumIngredients ) {
/*void OutputShit( bool found ) {
	if( !minimumIngredients.Keys.Contains( ItemID.CopperBar ) ) {
		return;
	}
	IEnumerable<Recipe> recipes = Main.recipe.Where( r => r.createItem.type == itemType && r.createItem.stack >= 1 );
	if( recipes.Count() == 0 ) {
		return;
	}

	LogLibraries.Log( "Checking recipes for "+ItemNameAttributeLibraries.GetQualifiedName(itemType)+" ("+itemType+")"
		+" for Copper Bar (found? "+found+"):" );

	foreach( Recipe recipe in recipes ) {
		LogLibraries.Log( "  " + string.Join( ", ",
				recipe.requiredItem
					.Where( item=>item.type != ItemID.None && item.stack > 0 )
					.Select( item=>item.Name+" ("+item.stack+")" )
			)
		);
	}
}*/
			for( int i = 0; i < Main.recipe.Length; i++ ) {
				Recipe recipe = Main.recipe[i];
				if( recipe.createItem.type != itemType || recipe.createItem.stack <= 0 ) {
					continue;
				}

				var minIngreds = new HashSet<int>( minimumIngredients.Keys );

				for( int j = 0; j < recipe.requiredItem.Length; j++ ) {
					Item reqItem = recipe.requiredItem[j];
					if( !minIngreds.Contains(reqItem.type) ) {
						continue;
					}

					(int min, int max) ingredAmt = minimumIngredients[reqItem.type];
					if( reqItem.stack < ingredAmt.min || reqItem.stack >= ingredAmt.max ) {	// Not an acceptable amount
						break;
					}

					minIngreds.Remove( reqItem.type );
					if( minIngreds.Count == 0 ) {	// All ingredients accounted for
//OutputShit( true );
						return true;
					}
				}
			}
//OutputShit( false );
			return false;
		}
	}
}
