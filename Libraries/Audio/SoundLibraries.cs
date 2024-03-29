﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;


namespace ModLibsGeneral.Libraries.Audio {
	/// <summary>
	/// Assorted static library functions pertaining to sounds.
	/// </summary>
	public class SoundLibraries : ModSystem {
		/// <summary>
		/// Gets volume and pan data for a sound that would play at a given point.
		/// </summary>
		/// <param name="worldX"></param>
		/// <param name="worldY"></param>
		/// <returns></returns>
		public static (float Volume, float Pan) GetSoundDataFromSource( int worldX, int worldY ) {
			var maxRange = new Rectangle(
				(int)( Main.screenPosition.X - (Main.screenWidth * 2) ),
				(int)( Main.screenPosition.Y - (Main.screenHeight * 2) ),
				Main.screenWidth * 5,
				Main.screenHeight * 5 );
			var source = new Rectangle( worldX, worldY, 1, 1 );

			Vector2 screenCenter = new Vector2(
				Main.screenPosition.X + (float)Main.screenWidth * 0.5f,
				Main.screenPosition.Y + (float)Main.screenHeight * 0.5f );

			if( !source.Intersects(maxRange) ) {
				return (0, 0);
			}

			float pan = (float)(worldX - screenCenter.X) / (float)(Main.screenWidth / 2);
			float distX = (float)worldX - screenCenter.X;
			float distY = (float)worldY - screenCenter.Y;
			float dist = (float)Math.Sqrt( (distX * distX) + (distY * distY) );
			float vol = 1f - (dist / ((float)Main.screenWidth * 1.5f));

			pan = MathHelper.Clamp( pan, -1f, 1f );
			vol = Math.Max( vol, 0f );

			return (vol, pan);
		}
	}
}
