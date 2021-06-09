using System.Collections.Generic;
using Terraria.ModLoader;
using ModLibsCore.Classes.Loadable;


namespace ModLibsGeneral.Libraries.Mods {
	/// <summary>
	/// Assorted static library functions pertaining to mod meta data features.
	/// </summary>
	public partial class ModMetaDataLibraries : ILoadable {
		internal IDictionary<string, Mod> GithubMods;


		////////////////

		internal ModMetaDataLibraries() {
			this.GithubMods = new Dictionary<string, Mod>();
		}


		////////////////

		/// @private
		void ILoadable.OnModsLoad() { }

		/// @private
		void ILoadable.OnModsUnload() { }

		/// @private
		void ILoadable.OnPostModsLoad() {
			this.GithubMods = new Dictionary<string, Mod>();

			foreach( Mod mod in ModLoader.Mods ) {
				if( ModMetaDataLibraries.DetectGithub( mod ) ) {
					this.GithubMods[mod.Name] = mod;
				}
			}
		}
	}
}
