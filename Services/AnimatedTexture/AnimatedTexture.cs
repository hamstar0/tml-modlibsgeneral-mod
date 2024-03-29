﻿using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;


namespace ModLibsGeneral.Services.AnimatedTexture {
	/// <summary>
	/// Supplies a handy way to animate between frames within a given texture. Adjustable.
	/// </summary>
	public partial class AnimatedTexture {
		/// <summary>
		/// Creates an animated texture object.
		/// </summary>
		/// <param name="frames"></param>
		/// <param name="maxFrames">Ticks of time to spend between each frame.</param>
		/// <param name="animator">Number of frames to cycle through.</param>
		/// <returns></returns>
		public static AnimatedTexture Create( Texture2D frames,
					int maxFrames,
					Func<AnimatedTexture, (int NextFrame, int TickDelay)> animator ) {
			var mngr = ModContent.GetInstance<AnimatedTextureManager>();
			var def = new AnimatedTexture( frames, maxFrames, animator );

			mngr.AddAnimation( def );

			return def;
		}
	}
}
