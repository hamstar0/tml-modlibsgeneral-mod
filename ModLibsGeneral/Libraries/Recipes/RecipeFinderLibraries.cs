using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.DotNET.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace ModLibsGeneral.Libraries.Recipes {
	/// <summary>
	/// Assorted static "helper" functions pertaining to recipe finding.
	/// </summary>
	public partial class RecipeFinderLibraries {
		/// <summary>
		/// Gets the `Main.recipe` indexes of each recipe that crafts a given item.
		/// </summary>
		/// <param name="itemNetID"></param>
		/// <returns></returns>
		public static ISet<int> GetRecipeIndexesOfItem( int itemNetID ) {
			if( itemNetID == 0 ) {
				throw new ModLibsException( "Invalid item type" );
			}

			var rfLib = ModContent.GetInstance<RecipeFinderLibraries>();
			IDictionary<int, ISet<int>> recipeIdxLists = rfLib.RecipeIndexesByItemNetID;
			
			lock( RecipeFinderLibraries.MyLock ) {
				if( recipeIdxLists.Count == 0 ) {
					rfLib.CacheItemRecipes();
				}
				return recipeIdxLists.GetOrDefault( itemNetID )
					?? new HashSet<int>();
			}
		}
		
		/// <summary>
		/// Gets each `Recipe` that crafts a given item.
		/// </summary>
		/// <param name="itemNetID"></param>
		/// <returns></returns>
		public static IList<Recipe> GetRecipesOfItem( int itemNetID ) {
			return RecipeFinderLibraries.GetRecipeIndexesOfItem( itemNetID )
				.Select( idx=>Main.recipe[idx] )
				.ToList();
		}


		////////////////

		/// <summary>
		/// Gets each `Main.recipe` index of each recipe that uses a given item as an ingredient.
		/// </summary>
		/// <param name="itemNetID"></param>
		/// <returns></returns>
		public static ISet<int> GetRecipeIndexesUsingIngredient( int itemNetID ) {
			if( itemNetID == 0 ) {
				throw new ModLibsException( "Invalid item type" );
			}

			var rfLib = ModContent.GetInstance<RecipeFinderLibraries>();
			IDictionary<int, ISet<int>> recipeIdxSets = rfLib.RecipeIndexesOfIngredientNetIDs;

			lock( RecipeFinderLibraries.MyLock ) {
				if( recipeIdxSets.Count == 0 ) {
					rfLib.CacheIngredientRecipes();
				}
				return recipeIdxSets.GetOrDefault( itemNetID )
					?? new HashSet<int>();
			}
		}
	}
}
