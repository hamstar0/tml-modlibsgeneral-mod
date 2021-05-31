using System;
using Microsoft.Xna.Framework;
using Terraria;
using ModLibsCore.Libraries.Debug;


namespace ModLibsGeneral.Libraries.World {
	/// <summary>
	/// Assorted static library functions pertaining to the current world.
	/// </summary>
	public partial class WorldLocationLibraries {
		/// <summary></summary>
		public static Point WorldSizeSmall => new Point( 4200, 1200 );
		/// <summary></summary>
		public static Point WorldSizeMedium => new Point( 6400, 1800 );	//6300?
		/// <summary></summary>
		public static Point WorldSizeLarge => new Point( 8400, 2400 );



		////////////////

		/// <summary>
		/// Gets the size (range) of the current world.
		/// </summary>
		/// <returns></returns>
		public static WorldSize GetSize() {
			int size = Main.maxTilesX * Main.maxTilesY;

			if( size <= (4200 * 1200) / 2 ) {
				return WorldSize.SubSmall;
			} else if( size <= 4200 * 1200 + 1000 ) {
				return WorldSize.Small;
			} else if( size <= 6400 * 1800 + 1000 ) {   //6300?
				return WorldSize.Medium;
			} else if( size <= 8400 * 2400 + 1000 ) {
				return WorldSize.Large;
			} else {
				return WorldSize.SuperLarge;
			}
		}
	}
}
