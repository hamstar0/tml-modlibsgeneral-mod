using System;
using System.Collections.Generic;
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
		/// Gets each `Main.recipe` index of each recipe that uses a given item as an ingredient.
		/// </summary>
		/// <param name="itemType"></param>
		/// <returns></returns>
		public static ISet<int> GetRecipeIndexesUsingIngredient( int itemType ) {
			var rfLib = ModContent.GetInstance<RecipeFinderLibraries>();
			IDictionary<int, ISet<int>> recipeIdxSets = rfLib.RecipeIndexesOfIngredientItemTypes;

			lock( RecipeFinderLibraries.MyLock ) {
				if( recipeIdxSets.Count == 0 ) {
					rfLib.CacheIngredientRecipes();
				}

				return recipeIdxSets.GetOrDefault( itemType )
					?? new HashSet<int>();
			}
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
	}
}
