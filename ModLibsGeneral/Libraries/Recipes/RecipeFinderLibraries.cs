using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
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
		/// <param name="createItemTypes">Item types to find recipes for. If empty, all recipes are matched against the given
		/// ingredients.</param>
		/// <param name="allIngredients">Minimum (<) and maximum (>) quantities of ingredient item types. All required.
		/// Accepts `null`.</param>
		/// <param name="anyIngredients">Minimum (<) and maximum (>) quantities of ingredient item types. Any will suffice.
		/// Accepts `null`.</param>
		/// <returns>`true` if recipe exists.</returns>
		public static bool RecipeExists_Cached(
					ISet<int> createItemTypes,
					IDictionary<int, (int min, int max)> allIngredients,
					IDictionary<int, (int min, int max)> anyIngredients ) {
			return RecipeFinderLibraries.FindRecipes_Cached( createItemTypes, allIngredients, anyIngredients, 1 ).Count > 0;
		}

		/// <summary>
		/// Indicates if any given recipes exist with the given ingredients. Does not check tile requirements.
		/// </summary>
		/// <param name="recipeIdxSource">Provides an enumerator of `Main.recipe` indexes to check.</param>
		/// <param name="allIngredients">Minimum (<) and maximum (>) quantities of ingredient item types. All required.
		/// Accepts `null`.</param>
		/// <param name="anyIngredients">Minimum (<) and maximum (>) quantities of ingredient item types. Any will suffice.
		/// Accepts `null`.</param>
		/// <returns>`true` if recipe exists.</returns>
		public static bool RecipeExists_Cached(
					Func<IEnumerable<int>> recipeIdxSource,
					IDictionary<int, (int min, int max)> allIngredients,
					IDictionary<int, (int min, int max)> anyIngredients ) {
			return RecipeFinderLibraries.FilterRecipes( recipeIdxSource, allIngredients, anyIngredients, 1 ).Count > 0;
		}


		////////////////
		
		/// <summary>
		/// Gets all recipes with the given ingredient. Does not check tile requirements.
		/// </summary>
		/// <param name="ingredientItemType"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns>`true` if recipe exists.</returns>
		public static ISet<int> GetRecipesWithIngredient_Cached( int ingredientItemType, int min=1, int max=Int32.MaxValue ) {
			return RecipeFinderLibraries.FindRecipes_Cached(
				new HashSet<int>(),
				null,
				new Dictionary<int, (int, int)> { { ingredientItemType, (min, max) } }
			);
		}


		////////////////

		/// <summary>
		/// Clears cached recipes. Use when recipe edits are suspected to have occurred.
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
