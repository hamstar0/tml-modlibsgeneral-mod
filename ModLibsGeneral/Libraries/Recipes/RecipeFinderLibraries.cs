using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.DotNET.Extensions;


namespace ModLibsGeneral.Libraries.Recipes {
	/// <summary>
	/// Assorted static library functions pertaining to recipe finding.
	/// </summary>
	public partial class RecipeFinderLibraries {
		/// <summary>
		/// Gets the `Main.recipe` indexes of each recipe that crafts a given item.
		/// </summary>
		/// <param name="itemType"></param>
		/// <returns></returns>
		public static ISet<int> GetRecipeIndexesOfItem_Cached( int itemType ) {
			var rfLib = ModContent.GetInstance<RecipeFinderLibraries>();
			IDictionary<int, ISet<int>> recipeIdxLists = rfLib.RecipeIndexesByItemType;
			int recipeIdxCount = 0;

			lock( RecipeFinderLibraries.MyLock ) {
				recipeIdxCount = recipeIdxLists.Count;
			}

			if( recipeIdxCount == 0 ) {
				rfLib.CacheItemRecipes();
			}

			lock( RecipeFinderLibraries.MyLock ) {
				return recipeIdxLists.GetOrDefault( itemType )
					?? new HashSet<int>();
			}
		}
		
		/// <summary>
		/// Gets each `Recipe` that crafts a given item.
		/// </summary>
		/// <param name="itemType"></param>
		/// <returns></returns>
		public static IList<Recipe> GetRecipesOfItem_Cached( int itemType ) {
			return RecipeFinderLibraries.GetRecipeIndexesOfItem_Cached( itemType )
				.Select( idx=>Main.recipe[idx] )
				.ToList();
		}


		////////////////
		
		/// <summary>
		/// Indicates if any given item types have recipes with the given ingredients. Does not check tile requirements.
		/// </summary>
		/// <param name="filterItemTypes">Item types to find recipes for. If empty, all recipes are matched against the given
		/// ingredients.</param>
		/// <param name="ingredients">Minimum (<) and maximum (>=) quantities of ingredient item types.</param>
		/// <returns>`true` if recipe exists.</returns>
		public static bool RecipeExists_Cached( ISet<int> filterItemTypes, IDictionary<int, (int min, int max)> ingredients ) {
			return RecipeFinderLibraries.GetRecipes_Cached( filterItemTypes, ingredients, 1 ).Count > 0;
		}


		/// <summary>
		/// Gets all recipes of the given item types with the given ingredients. Does not check tile requirements.
		/// </summary>
		/// <param name="createItemTypes">Item types to find recipes for. If empty, all recipes are matched against the given
		/// ingredients.</param>
		/// <param name="ingredients">Minimum (<) and maximum (>) quantities of ingredient item types.</param>
		/// <param name="until">Maximum number of recipes to return.</param>
		/// <returns>Indexes (indices?) of matching recipes in `Main.recipe`.</returns>
		public static ISet<int> GetRecipes_Cached(
					ISet<int> createItemTypes,
					IDictionary<int, (int min, int max)> ingredients,
					int until = Int32.MaxValue ) {
			int counted = 0;
			var matchRecipeIdxs = new HashSet<int>();

			//

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

			foreach( int i in GetNextRecipeIdx() ) {
				Recipe recipe = Main.recipe[i];
				if( recipe.createItem.type == ItemID.None || recipe.createItem.stack <= 0 ) {
					continue;
				}

				var minIngreds = new HashSet<int>( ingredients.Keys );

				for( int j = 0; j < recipe.requiredItem.Length; j++ ) {
					Item reqItem = recipe.requiredItem[j];
					if( reqItem.IsAir ) {
						continue;
					}

					if( minIngreds.Count > 0 ) {
						if( !minIngreds.Contains(reqItem.type) ) {
							continue;
						}

						(int min, int max) ingredAmt = ingredients[reqItem.type];
						if( reqItem.stack < ingredAmt.min || reqItem.stack > ingredAmt.max ) { // Not an acceptable amount
							break;
						}

						minIngreds.Remove( reqItem.type );
					}

					if( minIngreds.Count == 0 ) {	// All ingredients accounted for
						matchRecipeIdxs.Add( i );

						counted++;
						break;
					}
				}

				if( counted >= until ) {
					break;
				}
			}

			return matchRecipeIdxs;
		}


		////////////////

		/// <summary>
		/// Clears cached recipes.
		/// </summary>
		public static void ClearCache() {
			var rfLib = ModContent.GetInstance<RecipeFinderLibraries>();

			lock( RecipeFinderLibraries.MyLock ) {
				rfLib.RecipeIndexesByItemType.Clear();
				rfLib.RecipeIndexesOfIngredientItemTypes.Clear();
			}
		}
	}
}
