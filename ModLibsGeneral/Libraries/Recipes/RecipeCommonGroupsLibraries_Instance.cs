using System;
using System.Collections.Generic;
using Terraria;
using ModLibsCore.Classes.Loadable;


namespace ModLibsGeneral.Libraries.Recipes {
	/// @private
	public partial class RecipeCommonGroupsLibraries : ILoadable {
		private IDictionary<string, RecipeGroup> _Groups = null;



		////////////////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnModsUnload() { }

		void ILoadable.OnPostModsLoad() { }
	}
}
