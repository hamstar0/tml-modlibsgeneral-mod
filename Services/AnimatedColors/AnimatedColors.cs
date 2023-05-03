using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace ModLibsGeneral.Services.AnimatedColor {
	/// <summary>
	/// Supplies a handy way to "animate" (lerp between) colors to make animations. Adjustable.
	/// </summary>
	public partial class AnimatedColors {
		/// <summary>
		/// Color animation preset for alert-type effects.
		/// </summary>
		public static AnimatedColors Alert => ModContent.GetInstance<AnimatedColorsManager>().Alert;
		/// <summary>
		/// Color animation preset for glowing ember-like effect.
		/// </summary>
		public static AnimatedColors Ember => ModContent.GetInstance<AnimatedColorsManager>().Ember;
		/// <summary>
		/// Color animation preset to make a strobe-like effect.
		/// </summary>
		public static AnimatedColors Strobe => ModContent.GetInstance<AnimatedColorsManager>().Strobe;
		/// <summary>
		/// Color animation preset to resemble fire.
		/// </summary>
		public static AnimatedColors Fire => ModContent.GetInstance<AnimatedColorsManager>().Fire;
		/// <summary>
		/// Color animation preset of something water-like.
		/// </summary>
		public static AnimatedColors Water => ModContent.GetInstance<AnimatedColorsManager>().Water;
		/// <summary>
		/// Color animation preset of something air-themed.
		/// </summary>
		public static AnimatedColors Air => ModContent.GetInstance<AnimatedColorsManager>().Air;
		/// <summary>
		/// Color animation preset of something etherial.
		/// </summary>
		public static AnimatedColors Ether => ModContent.GetInstance<AnimatedColorsManager>().Ether;
		/// <summary>
		/// Color animation preset of something disco-like.
		/// </summary>
		public static AnimatedColors Disco => ModContent.GetInstance<AnimatedColorsManager>().Disco;
		/// <summary>
		/// Color animation preset of something disco-like at high speed.
		/// </summary>
		public static AnimatedColors DiscoFast => ModContent.GetInstance<AnimatedColorsManager>().DiscoFast;



		////////////////

		/// <summary>
		/// Creates an animated color object.
		/// </summary>
		/// <param name="durationPerColor">Ticks of time to spend between each color.</param>
		/// <param name="colors">Sequence of colors to loop through.</param>
		/// <returns></returns>
		public static AnimatedColors Create( int durationPerColor, Color[] colors ) {
			return AnimatedColors.Create( ModContent.GetInstance<AnimatedColorsManager>(), durationPerColor, colors );
		}

		internal static AnimatedColors Create( AnimatedColorsManager mngr, int durationPerColor, Color[] colors ) {
			var def = new AnimatedColors( durationPerColor, colors );

			mngr.AddAnimation( def );

			return def;
		}
	}
}
