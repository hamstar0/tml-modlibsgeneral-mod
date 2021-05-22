using System.Collections.Generic;
using Terraria.ModLoader;
using ModLibsCore.Classes.Loadable;


namespace ModLibsGeneral.Libraries.ModHelpers {
	/// @private
	public partial class ModFeaturesLibraries : ILoadable {
		internal IDictionary<string, Mod> GithubMods;
		internal IDictionary<string, Mod> ConfigMods;
		internal IDictionary<string, Mod> ConfigDefaultsResetMods;


		////////////////

		internal ModFeaturesLibraries() {
			this.GithubMods = new Dictionary<string, Mod>();
			this.ConfigMods = new Dictionary<string, Mod>();
			this.ConfigDefaultsResetMods = new Dictionary<string, Mod>();
		}


		////////////////

		/// @private
		void ILoadable.OnModsLoad() { }

		/// @private
		void ILoadable.OnModsUnload() { }

		/// @private
		void ILoadable.OnPostModsLoad() {
			this.GithubMods = new Dictionary<string, Mod>();
			this.ConfigMods = new Dictionary<string, Mod>();
			this.ConfigDefaultsResetMods = new Dictionary<string, Mod>();

			foreach( Mod mod in ModLoader.Mods ) {
				if( ModFeaturesLibraries.DetectGithub( mod ) ) {
					this.GithubMods[mod.Name] = mod;
				}
			}
		}
	}
}
