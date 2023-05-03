using System;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Services.Timers;


namespace ModLibsGeneral.Libraries.Audio {
	/// @private
	public partial class MusicLibraries : ModSystem {
		private float Scale = 1f;

		private Func<bool> OnTickGet;



		////////////////

		internal MusicLibraries() {
			this.OnTickGet = Timers.MainOnTickGet();
		}

		/// @private
		public override void Load() {
			Main.OnTickForInternalCodeOnly += MusicLibraries._Update;
		}

		/// @private
		public override void Unload() {
			try {
				Main.OnTickForInternalCodeOnly -= MusicLibraries._Update;
			} catch { }
		}



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

			float fade = Main.musicFade[ Main.curMusic ];
			bool isPlaying = Main.audioSystem.IsTrackPlaying( Main.curMusic );

			if( isPlaying ) {
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
