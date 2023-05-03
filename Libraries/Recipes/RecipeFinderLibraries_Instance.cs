using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.DotNET.Extensions;


namespace ModLibsGeneral.Libraries.Recipes {
	/// @private
	public partial class RecipeFinderLibraries : ILoadable {
		private static object MyLock = new object();



		////////////////
		
		private IDictionary<int, ISet<int>> RecipeIndexesByItemType = new Dictionary<int, ISet<int>>();
		private IDictionary<int, ISet<int>> RecipeIndexesOfIngredientItemTypes = new Dictionary<int, ISet<int>>();



		////////////////

		/// @private
		void ILoadable.Load( Mod mod ) { }

		/// @private
		void ILoadable.Unload() { }


		////////////////

		private void CacheItemRecipes() {
			lock( RecipeFinderLibraries.MyLock ) {
				for( int i = 0; i < Main.recipe.Length; i++ ) {
					Recipe recipe = Main.recipe[i];

					if( !recipe.createItem.IsAir ) {
						this.RecipeIndexesByItemType.Append2D( recipe.createItem.type, i );
					}
				}
			}
		}


		private void CacheIngredientRecipes() {
			lock( RecipeFinderLibraries.MyLock ) {
				for( int i = 0; i < Main.recipe.Length; i++ ) {
					Recipe recipe = Main.recipe[i];
					if( recipe.createItem.IsAir ) {
						continue;
					}

					for( int j=0; j<recipe.requiredItem.Capacity; j++ ) {
						Item item = recipe.requiredItem[j];
						if( item == null || item.IsAir ) {
							break;
						}

						this.RecipeIndexesOfIngredientItemTypes.Append2D( item.type, i );
					}
				}
			}
		}
	}
}
