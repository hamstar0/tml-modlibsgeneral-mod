using System;
using System.Collections.Generic;
using Terraria;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;


namespace ModLibsGeneral.Libraries.Recipes {
	/// <summary>
	/// Assorted static library functions pertaining to recipe identification.
	/// </summary>
	public partial class RecipeIdentityLibraries {
		/// <summary>
		/// Indicates if any 2 recipes are identical.
		/// </summary>
		/// <param name="recipe1"></param>
		/// <param name="recipe2"></param>
		/// <returns></returns>
		public static bool Equals( Recipe recipe1, Recipe recipe2 ) {
			if( recipe1.needHoney != recipe2.needHoney ) { return false; }
			if( recipe1.needLava != recipe2.needLava ) { return false; }
			if( recipe1.needSnowBiome != recipe2.needSnowBiome ) { return false; }
			if( recipe1.needWater != recipe2.needWater ) { return false; }

			if( recipe1.alchemy != recipe2.alchemy ) { return false; }
			if( recipe1.anyFragment != recipe2.anyFragment ) { return false; }
			if( recipe1.anySand != recipe2.anySand ) { return false; }
			if( recipe1.anyPressurePlate != recipe2.anyPressurePlate ) { return false; }
			if( recipe1.anyIronBar != recipe2.anyIronBar ) { return false; }
			if( recipe1.anyWood != recipe2.anyWood ) { return false; }

			if( recipe1.createItem.IsNotSameTypePrefixAndStack( recipe2.createItem ) ) { return false; }

			var reqTile1 = new HashSet<int>( recipe1.requiredTile );
			var reqTile2 = new HashSet<int>( recipe2.requiredTile );
			if( !reqTile1.SetEquals( reqTile2 ) ) { return false; }

			var reqItem1 = new HashSet<Item>( recipe1.requiredItem );
			var reqItem2 = new HashSet<Item>( recipe2.requiredItem );
			if( !reqItem1.SetEquals( reqItem2 ) ) { return false; }

			var reqAcceptedGrps1 = new HashSet<int>( recipe1.acceptedGroups );
			var reqAcceptedGrps2 = new HashSet<int>( recipe2.acceptedGroups );
			if( !reqAcceptedGrps1.SetEquals( reqAcceptedGrps2 ) ) { return false; }

			return true;
		}
	}
}
