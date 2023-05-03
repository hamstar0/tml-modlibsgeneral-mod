using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace ModLibsGeneral.Libraries.Recipes {
	/// @private
	public partial class RecipeCommonGroupsLibraries : ILoadable {
		private IDictionary<string, RecipeGroup> _Groups = null;



		////////////////

		void ILoadable.Load( Mod mod ) { }

		void ILoadable.Unload() { }
	}
}
