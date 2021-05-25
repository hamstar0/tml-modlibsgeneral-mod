using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Audio;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Services.Hooks.LoadHooks;
using ModLibsCore.Services.Timers;


namespace ModLibsGeneral.Libraries.Audio {
	/// @private
	public partial class MusicLibraries : ILoadable {
		private float Scale = 1f;

		private Func<bool> OnTickGet;



		////////////////

		internal MusicLibraries() {
			this.OnTickGet = Timers.MainOnTickGet();
			Main.OnTick += MusicLibraries._Update;
		}

		/// @private
		void ILoadable.OnModsLoad() { }

		/// @private
		void ILoadable.OnModsUnload() {
			try {
				Main.OnTick -= MusicLibraries._Update;
			} catch { }
		}

		/// @private
		void ILoadable.OnPostModsLoad() { }


		////////////////

		private static void _Update() { // <- Just in case references are doing something funky...
			var musicLibs = ModContent.GetInstance<MusicLibraries>();
			if( musicLibs == null ) { return; }
			if( Main.dedServ ) { return; }
			
			if( musicLibs.OnTickGet() ) {
				musicLibs.Update();
			}
		}

		internal void Update() {
			if( this.Scale == 1f ) { return; }

			Music music = Main.music[ Main.curMusic ];
			float fade = Main.musicFade[ Main.curMusic ];

			if( music != null && music.IsPlaying ) {
				if( fade > this.Scale ) {
					Main.musicFade[ Main.curMusic ] = Math.Max( 0f, fade - 0.01f );
				} else {
					Main.musicFade[ Main.curMusic ] = Math.Min( 1f, fade + 0.01f );
				}
				//music.SetVariable( "Volume", fade * Main.musicVolume * this.Scale );
			}
			
			this.Scale = Math.Min( 1f, this.Scale + 0.05f );
		}
	}
}
