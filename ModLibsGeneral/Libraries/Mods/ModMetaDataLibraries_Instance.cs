using ModLibsCore.Services.Hooks.LoadHooks;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ModLoader;


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
		void ILoadable.Load( Mod mod ) {
			LoadHooks.AddPostModLoadHook( () => {
				this.GithubMods = new Dictionary<string, Mod>();

				foreach (Mod thismod in ModLoader.Mods) {
					if( ModMetaDataLibraries.DetectGithub(thismod) ) {
						this.GithubMods[thismod.Name] = thismod;
					}
				}
			} );
		}

		/// @private
		void ILoadable.Unload() { }
	}
}
