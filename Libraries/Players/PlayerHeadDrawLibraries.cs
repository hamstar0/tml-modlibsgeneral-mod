using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using ModLibsCore.Libraries.DotNET.Reflection;


namespace ModLibsGeneral.Libraries.Players {
	/// <summary>
	/// Assorted static library functions pertaining to player head drawing (currently empty).
	/// </summary>
	public class PlayerHeadDrawLibraries {
		/// @private
		[Obsolete( "uses vanilla's Main.instance.DrawPlayerHead via. reflection; not library code", true)]
		public static void DrawPlayerHead( SpriteBatch sb, Player player, float x, float y, float alpha = 1f, float scale = 1f ) {
			object _;
			ReflectionLibraries.RunMethod( Main.instance, "DrawPlayerHead", new object[] { player, x, y, alpha, scale }, out _ );
		}
	}
}
