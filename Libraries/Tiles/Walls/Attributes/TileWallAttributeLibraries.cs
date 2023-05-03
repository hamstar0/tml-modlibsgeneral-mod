using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.DataStructures;


namespace ModLibsGeneral.Libraries.Tiles.Walls.Attributes {
	/// <summary>
	/// Assorted static library functions pertaining to tile walls.
	/// </summary>
	public class TileWallAttributeLibraries {
		/// <summary></summary>
		public static ISet<int> UnsafeDungeonWallTypes { get; } = new ReadOnlySet<int>( new HashSet<int> {
			WallID.BlueDungeonSlabUnsafe,
			WallID.GreenDungeonSlabUnsafe,
			WallID.PinkDungeonSlabUnsafe,
			WallID.BlueDungeonTileUnsafe,
			WallID.GreenDungeonTileUnsafe,
			WallID.PinkDungeonTileUnsafe,
			WallID.BlueDungeonUnsafe,
			WallID.GreenDungeonUnsafe,
			WallID.PinkDungeonUnsafe
		} );




		////////////////

		/// <summary>
		/// Indicates if a given wall is dungeon or temple "biome" wall.
		/// </summary>
		/// <param name="tile"></param>
		/// <param name="isLihzahrd"></param>
		/// <returns></returns>
		public static bool IsDungeon( Tile tile, out bool isLihzahrd ) {
			if( tile == null ) {
				isLihzahrd = false;
				return false;
			}

			isLihzahrd = tile.WallType == (ushort)WallID.LihzahrdBrickUnsafe; /*|| tile.wall == (ushort)WallID.LihzahrdBrick*/

			// Lihzahrd Brick Wall
			if( isLihzahrd ) {
				return true;
			}
			// Dungeon Walls
			//if( (tile.wall >= 7 && tile.wall <= 9) || (tile.wall >= 94 && tile.wall <= 99) ) {
			if( tile.WallType == (ushort)WallID.BlueDungeonSlabUnsafe ||
				tile.WallType == (ushort)WallID.GreenDungeonSlabUnsafe ||
				tile.WallType == (ushort)WallID.PinkDungeonSlabUnsafe ||
				tile.WallType == (ushort)WallID.BlueDungeonTileUnsafe ||
				tile.WallType == (ushort)WallID.GreenDungeonTileUnsafe ||
				tile.WallType == (ushort)WallID.PinkDungeonTileUnsafe ||
				tile.WallType == (ushort)WallID.BlueDungeonUnsafe ||
				tile.WallType == (ushort)WallID.GreenDungeonUnsafe ||
				tile.WallType == (ushort)WallID.PinkDungeonUnsafe ) {
				return true;
			}
			return false;
		}
	}
}
