using System;
using System.Collections.Generic;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Hooks.LoadHooks;


namespace ModLibsGeneral.Libraries.Players {
	/// @private
	public partial class PlayerIdentityLibraries : ILoadable {
		internal IDictionary<int, string> PlayerIds = new Dictionary<int, string>();



		////////////////


		/// @private
		void ILoadable.OnModsLoad() { }

		/// @private
		void ILoadable.OnModsUnload() { }

		/// @private
		void ILoadable.OnPostModsLoad() {
			LoadHooks.AddPostWorldUnloadEachHook( () => {
				this.PlayerIds = new Dictionary<int, string>();
			} );
		}
	}
}
