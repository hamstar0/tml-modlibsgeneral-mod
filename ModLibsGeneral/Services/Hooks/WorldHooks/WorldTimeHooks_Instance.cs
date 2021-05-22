using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.TModLoader;


namespace ModLibsGeneral.Services.Hooks.WorldHooks {
	/// @private
	public partial class WorldTimeHooks : ILoadable {
		private bool IsDay;

		internal IDictionary<string, Action> DayHooks = new Dictionary<string, Action>();
		internal IDictionary<string, Action> NightHooks = new Dictionary<string, Action>();



		////////////////

		/// @private
		void ILoadable.OnModsLoad() { }

		/// @private
		void ILoadable.OnModsUnload() { }

		/// @private
		void ILoadable.OnPostModsLoad() { }


		////////////////

		internal void Update() {
			var wtHooks = ModContent.GetInstance<WorldTimeHooks>();

			if( !LoadLibraries.IsWorldSafelyBeingPlayed() ) {
				this.IsDay = Main.dayTime;
			} else {
				if( this.IsDay != Main.dayTime ) {
					if( !this.IsDay ) {
						foreach( Action hook in wtHooks.DayHooks.Values ) {
							hook();
						}
					} else {
						foreach( Action hook in wtHooks.NightHooks.Values ) {
							hook();
						}
					}
				}

				this.IsDay = Main.dayTime;
			}
		}
	}
}
