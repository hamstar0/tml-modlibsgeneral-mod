using System;
using System.Collections.Generic;
using Terraria;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;


namespace ModLibsGeneral.Libraries.Recipes {
	/// <summary>
	/// Assorted static library functions pertaining to recipe finding.
	/// </summary>
	public partial class RecipeFinderLibraries {
		/// <summary>
		/// Gets all recipes of the given item types with the given ingredients. Does not check tile requirements.
		/// </summary>
		/// <param name="createItemTypes">Item types to find recipes for. If empty, all recipes are matched against the given
		/// ingredients.</param>
		/// <param name="allIngredients">Minimum (<) and maximum (>) quantities of ingredient item types. All required.
		/// Accepts `null`.</param>
		/// <param name="anyIngredients">Minimum (<) and maximum (>) quantities of ingredient item types. Any will suffice.
		/// Accepts `null`.</param>
		/// <param name="until">Maximum number of recipes to return.</param>
		/// <returns>Indexes (indices?) of matching recipes in `Main.recipe`.</returns>
		public static ISet<int> FindRecipes_Cached(
					ISet<int> createItemTypes,
					IDictionary<int, (int min, int max)> allIngredients,
					IDictionary<int, (int min, int max)> anyIngredients,
					int until = Int32.MaxValue ) {
			Func<IEnumerable<int>> GetNextRecipeIdx;

			if( createItemTypes.Count == 0 ) {
				IEnumerable<int> _GetNextRecipeIdx() {
					for( int ri=0; ri<Main.recipe.Length; ri++ ) {
						yield return ri;
					}
				};

				GetNextRecipeIdx = _GetNextRecipeIdx;
			} else {
				var itemRecipeIdxs = new HashSet<int>();

				foreach( int itemType in createItemTypes ) {
					itemRecipeIdxs.UnionWith( RecipeFinderLibraries.GetRecipeIndexesOfItem_Cached(itemType) );
				}

				IEnumerable<int> _GetNextRecipeIdx() {
					foreach( int ri in itemRecipeIdxs ) {
						yield return ri;
					}
				};

				GetNextRecipeIdx = _GetNextRecipeIdx;
			}

			//

//bool isIronPick = createItemTypes.Contains( ItemID.IronPickaxe );
//bool needsIronBar = ingredients.ContainsKey( ItemID.IronBar );
//if( isIronPick && needsIronBar ) {
//	LogLibraries.Log( "MIN INGREDIENTS "+ingredients
//		.Select( kv=>ItemNameAttributeLibraries.GetQualifiedName(kv.Key)+"-"+kv.Value )
//		.ToStringJoined(", ")
//	);
//}
			return RecipeFinderLibraries.FilterRecipes( GetNextRecipeIdx, allIngredients, anyIngredients, until );
		}


		/// <summary>
		/// Filters a given set of recipes upon the given ingredients. Does not check tile requirements.
		/// </summary>
		/// <param name="recipeSource">Enumerates the set of `Main.recipe` indexes of the given recipes to filter upon.</param>
		/// <param name="allIngredients">Minimum (<) and maximum (>) quantities of ingredient item types. All required.
		/// Accepts `null`.</param>
		/// <param name="anyIngredients">Minimum (<) and maximum (>) quantities of ingredient item types. Any will suffice.
		/// Accepts `null`.</param>
		/// <param name="until">Maximum number of recipes to return.</param>
		/// <returns>Indexes (indices?) of matching recipes in `Main.recipe`.</returns>
		public static ISet<int> FilterRecipes(
					Func<IEnumerable<int>> recipeSource,
					IDictionary<int, (int min, int max)> allIngredients,
					IDictionary<int, (int min, int max)> anyIngredients,
					int until = Int32.MaxValue ) {
			int counted = 0;
			var matchRecipeIdxs = new HashSet<int>();

			foreach( int i in recipeSource() ) {
				Recipe recipe = Main.recipe[i];
				if( recipe.createItem.IsAir ) {
					continue;
				}

				if( RecipeFinderLibraries.MatchRecipe(recipe, allIngredients, anyIngredients) ) {
					matchRecipeIdxs.Add( i );

					counted++;
				}

				if( counted >= until ) {
					break;
				}
			}

			return matchRecipeIdxs;
		}


		////////////////

		private static bool MatchRecipe(
					Recipe recipe,
					IDictionary<int, (int min, int max)> allIngredients = null,
					IDictionary<int, (int min, int max)> anyIngredients = null ) {
			if( allIngredients != null ) {
				if( !RecipeFinderLibraries.MatchRecipeAll(recipe, allIngredients) ) {
					return false;
				}
			}
			
			return anyIngredients != null
				? RecipeFinderLibraries.MatchRecipeAny( recipe, anyIngredients )
				: true;
		}

		////

		private static bool MatchRecipeAll( Recipe recipe, IDictionary<int, (int min, int max)> allIngredients ) {
//if( isIronPick && needsIronBar ) {
//	LogLibraries.Log( "IRON PICK RECIPE "+i+": "+recipe.requiredItem
//		.Where( it=>!it.IsAir )
//		.Select( it=>it.Name )
//		.ToStringJoined( ", " ) );
//}
			var minIngreds = new HashSet<int>( allIngredients.Keys );

			foreach( Item reqItem in recipe.requiredItem ) {
				if( reqItem.IsAir ) {
					continue;
				}

				if( minIngreds.Count > 0 ) {
					if( !minIngreds.Contains(reqItem.type) ) {
						continue;
					}

					(int min, int max) ingredAmt = allIngredients[reqItem.type];
					if( reqItem.stack >= ingredAmt.min && reqItem.stack <= ingredAmt.max ) {
						minIngreds.Remove( reqItem.type );
					}
				}

				if( minIngreds.Count == 0 ) {	// All ingredients accounted for
					return true;
				}
			}

			return false;
		}

		private static bool MatchRecipeAny( Recipe recipe, IDictionary<int, (int min, int max)> anyIngredients ) {
			foreach( Item reqItem in recipe.requiredItem ) {
				if( reqItem.IsAir ) {
					continue;
				}

				if( !anyIngredients.ContainsKey(reqItem.type) ) {
					continue;
				}

				(int min, int max) ingredAmt = anyIngredients[ reqItem.type ];
				if( reqItem.stack >= ingredAmt.min && reqItem.stack <= ingredAmt.max ) {
					return true;
				}
			}

			return false;
		}
	}
}
