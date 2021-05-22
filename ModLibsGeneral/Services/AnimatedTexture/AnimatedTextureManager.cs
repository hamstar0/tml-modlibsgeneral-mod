using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Hooks.LoadHooks;
using ModLibsCore.Services.Timers;


namespace ModLibsGeneral.Services.AnimatedTexture {
	class AnimatedTextureManager : ILoadable {
		internal IList<AnimatedTexture> Animations = new List<AnimatedTexture>();

		private Func<bool> OnTickGet;



		////////////////

		internal AnimatedTextureManager() {
			var mymod = ModHelpersMod.Instance;

			if( !Main.dedServ ) {
				this.OnTickGet = Timers.MainOnTickGet();
				Main.OnTick += AnimatedTextureManager._Update;
			}
		}

		////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnPostModsLoad() {
			if( !Main.dedServ ) {
				LoadHooks.AddWorldUnloadEachHook( () => {
					this.Animations.Clear();
				} );
				LoadHooks.AddModUnloadHook( () => {
					Main.OnTick -= AnimatedTextureManager._Update;
				} );
			}
		}

		void ILoadable.OnModsUnload() { }


		////////////////

		public void AddAnimation( AnimatedTexture animation ) {
			this.Animations.Add( animation );
		}


		////////////////

		private static void _Update() {
			var animTex = ModContent.GetInstance<AnimatedTextureManager>();
			if( animTex == null ) { return; }

			if( animTex.OnTickGet() ) {
				animTex.Update();
			}
		}


		internal void Update() {
			if( Main.gameMenu ) {
				return;
			}

			foreach( var def in this.Animations ) {
				def.AdvanceFrame();
			}
		}
	}
}
