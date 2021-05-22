using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Services.Hooks.LoadHooks;
using ModLibsCore.Services.Timers;
using ModLibsCore.Classes.Loadable;


namespace ModLibsGeneral.Services.AnimatedColor {
	class AnimatedColorsManager : ILoadable {
		internal readonly AnimatedColors Alert;
		internal readonly AnimatedColors Ember;
		internal readonly AnimatedColors Strobe;
		internal readonly AnimatedColors Fire;
		internal readonly AnimatedColors Water;
		internal readonly AnimatedColors Air;
		internal readonly AnimatedColors Ether;
		internal readonly AnimatedColors Disco;
		internal readonly AnimatedColors DiscoFast;

		////

		internal IList<AnimatedColors> Defs = new List<AnimatedColors>();
		private Func<bool> OnTickGet;



		////////////////

		internal AnimatedColorsManager() {
			var mymod = ModHelpersMod.Instance;

			this.Alert = AnimatedColors.Create( this, 16, new Color[] { Color.Yellow, Color.Gray } );
			this.Ember = AnimatedColors.Create( this, 16, new Color[] { Color.Orange, Color.Orange * 0.65f } );
			this.Strobe = AnimatedColors.Create( this, 16, new Color[] { Color.Black, Color.White } );
			this.Fire = AnimatedColors.Create( this, 16, new Color[] { Color.Red, Color.Yellow } );
			this.Water = AnimatedColors.Create( this, 16, new Color[] { Color.Blue, Color.Turquoise } );
			this.Air = AnimatedColors.Create( this, 16, new Color[] { Color.White, Color.Gray } );
			this.Ether = AnimatedColors.Create( this, 16, new Color[] { Color.MediumSpringGreen, Color.Gray } );
			Color green = Color.Lime;	// The colors lie!
			Color indigo = new Color( 147, 0, 255, 255 );
			Color violet = new Color( 255, 139, 255, 255 );
			this.Disco = AnimatedColors.Create( this, 56, new Color[] { Color.Red, Color.Orange, Color.Yellow, green, Color.Blue, indigo, violet } );
			this.DiscoFast = AnimatedColors.Create( this, 8, new Color[] { Color.Red, Color.Orange, Color.Yellow, green, Color.Blue, indigo, violet } );

			if( !Main.dedServ ) {
				this.OnTickGet = Timers.MainOnTickGet();
				Main.OnTick += AnimatedColorsManager._Update;
			}
		}

		////


		////////////////

		/// @private
		void ILoadable.OnModsLoad() { }

		/// @private
		void ILoadable.OnModsUnload() { }

		/// @private
		void ILoadable.OnPostModsLoad() {
			if( !Main.dedServ ) {
				LoadHooks.AddModUnloadHook( () => {
					Main.OnTick -= AnimatedColorsManager._Update;
				} );
			}
		}


		////////////////

		public void AddAnimation( AnimatedColors animation ) {
			this.Defs.Add( animation );
		}


		////////////////

		private static void _Update() {
			var animColors = ModContent.GetInstance<AnimatedColorsManager>();
			if( animColors == null ) { return; }

			if( animColors.OnTickGet() ) {
				animColors.Update();
			}
		}


		internal void Update() {
			foreach( var def in this.Defs ) {
				def.AdvanceColor();
			}
		}
	}
}
