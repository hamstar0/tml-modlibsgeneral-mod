using System;
using System.Collections.Generic;
using Terraria;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using System.Linq;

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
			if( !recipe1.Conditions.SequenceEqual( recipe2.Conditions ) ) {
				return false;
			}

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
