﻿using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;


namespace ModLibsGeneral.Libraries.Projectiles {
	/// <summary>
	/// Assorted static library functions pertaining to players relative to projectile resources (e.g. textures).
	/// </summary>
	public partial class ProjectileResourceLibraries {
		/// <summary>
		/// Gets a projectile's texture. Loads projectiles as needed.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static Texture2D SafelyGetTexture( int type ) {
			Main.instance.LoadProjectile( type );

			return TextureAssets.Projectile[type].Value;
		}
	}
}
