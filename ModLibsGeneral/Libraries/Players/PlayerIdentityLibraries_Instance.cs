using System;
using System.Collections.Generic;
using ModLibsCore.Classes.Errors;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Hooks.LoadHooks;


namespace ModLibsGeneral.Libraries.Players {
	/// @private
	public partial class PlayerIdentityLibraries : ILoadable {
		internal IDictionary<int, string> PlayerIds = new Dictionary<int, string>();



		////////////////


		/// @private
		void ILoadable.Load( Mod mod ) {
			LoadHooks.AddPostWorldUnloadEachHook( () => {
				this.PlayerIds = new Dictionary<int, string>();
			} );
		}

		/// @private
		void ILoadable.Unload() { }
	}
}
