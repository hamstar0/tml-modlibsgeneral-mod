﻿using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Hooks.LoadHooks;
using ModLibsCore.Services.Timers;


namespace ModLibsGeneral.Services.AnimatedTexture {
	class AnimatedTextureManager : ILoadable {
		internal IList<AnimatedTexture> Animations = new List<AnimatedTexture>();

		private Func<bool> OnTickGet;



		////////////////

		internal AnimatedTextureManager() {
			var mymod = ModLibsGeneralMod.Instance;

			if( !Main.dedServ ) {
				this.OnTickGet = Timers.MainOnTickGet();
				Main.OnTickForInternalCodeOnly += AnimatedTextureManager._Update;
			}
		}

		////

		void ILoadable.Load( Mod mod ) {
			if( !Main.dedServ ) {
				LoadHooks.AddWorldUnloadEachHook( () => {
					this.Animations.Clear();
				} );
				LoadHooks.AddModUnloadHook( () => {
					Main.OnTickForInternalCodeOnly -= AnimatedTextureManager._Update;
				} );
			}
		}

		void ILoadable.Unload() { }


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
