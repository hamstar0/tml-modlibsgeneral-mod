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
	}
}
