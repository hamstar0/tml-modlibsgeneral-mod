using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;

namespace ModLibsGeneral.Libraries.Tiles.Walls {
	/// <summary>
	/// Assorted static library functions pertaining to players relative to tile Wall resources (e.g. textures).
	/// </summary>
	public partial class TileWallResourceLibraries {
		/// <summary>
		/// Gets a wall's texture. Loads walls as needed.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static Texture2D SafelyGetTexture( int type ) {
			Main.instance.LoadWall( type );
			return TextureAssets.Wall[type].Value;
		}
	}
}
