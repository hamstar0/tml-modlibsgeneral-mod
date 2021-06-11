using System;
using System.Collections.Generic;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Hooks.LoadHooks;


namespace ModLibsGeneral.Libraries.NPCs {
	/// <summary>
	/// Assorted static library functions pertaining to NPC-dropped banners.
	/// </summary>
	public partial class NPCBannerLibraries : ILoadable {
		private IDictionary<int, int> NpcTypesToBannerItemTypes;
		private ISet<int> BannerItemTypes;
		private IDictionary<int, ISet<int>> BannerItemTypesToNpcTypes;



		////////////////

		/// @private
		void ILoadable.OnModsLoad() {
			LoadHooks.AddPostContentLoadHook( () => {
				this.BannerItemTypesToNpcTypes = new Dictionary<int, ISet<int>>();
				this.NpcTypesToBannerItemTypes = NPCBannerLibraries.GetNpcToBannerItemTypes();
				
				foreach( var kv in this.NpcTypesToBannerItemTypes ) {
					if( !this.BannerItemTypesToNpcTypes.ContainsKey( kv.Value ) ) {
						this.BannerItemTypesToNpcTypes[kv.Value] = new HashSet<int>();
					}
					this.BannerItemTypesToNpcTypes[kv.Value].Add( kv.Key );
				}

				this.BannerItemTypes = new HashSet<int>( this.BannerItemTypesToNpcTypes.Keys );
			} );
		}

		/// @private
		void ILoadable.OnPostModsLoad() { }

		/// @private
		void ILoadable.OnModsUnload() { }
	}
}
