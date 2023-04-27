using System.Runtime.CompilerServices;
using ModLibsCore.Libraries.Debug;
using Terraria;


namespace ModLibsGeneral.Libraries.Tiles {
	/// <summary>
	/// Assorted static library functions pertaining to tiles.
	/// </summary>
	public partial class TileLibraries {
		/// <summary>
		/// Reports if a tile is exactly identical to another tile.
		/// </summary>
		/// <param name="tile1"></param>
		/// <param name="tile2"></param>
		/// <returns></returns>
		public static bool IsEqual( Tile tile1, Tile tile2 ) {
			ref var stateData1 = ref tile1.Get<TileWallWireStateData>();
			ref var stateData2 = ref tile2.Get<TileWallWireStateData>();

			return tile1.TileType == tile2.TileType
				&& tile1.WallType == tile2.WallType
				&& Unsafe.As<TileWallWireStateData, ulong>( ref stateData1 ) == Unsafe.As<TileWallWireStateData, ulong>( ref stateData2 )
				&& tile1.LiquidType == tile2.LiquidType
				&& tile1.LiquidAmount == tile2.LiquidAmount
			;
		}


		/// <summary>
		/// Indicates if a given tile is "air" (including no walls).
		/// </summary>
		/// <param name="tile"></param>
		/// <param name="isWireAir"></param>
		/// <param name="isLiquidAir"></param>
		/// <returns></returns>
		public static bool IsAir( Tile tile, bool isWireAir = false, bool isLiquidAir = false ) {
			if( tile == null ) {
				return true;
			}
			if( tile.HasTile || tile.WallType > 0 ) {/*|| tile.type == 0*/
				return false;
			}
			if( !isWireAir && TileLibraries.IsWire(tile) ) {
				return false;
			}
			if( !isLiquidAir && tile.LiquidAmount != 0 ) {
				return false;
			}

			return true;
		}

		
		/// <summary>
		/// Indicates if a given tile is "solid".
		/// </summary>
		/// <param name="tile"></param>
		/// <param name="isPlatformSolid"></param>
		/// <param name="isActuatedSolid"></param>
		/// <returns></returns>
		public static bool IsSolid( Tile tile, bool isPlatformSolid = false, bool isActuatedSolid = false ) {
			if( TileLibraries.IsAir(tile) ) { return false; }
			if( !Main.tileSolid[tile.TileType] || !tile.HasTile ) { return false; }

			if( !isPlatformSolid ) {
				bool isTopSolid = Main.tileSolidTop[tile.TileType];
				if( isTopSolid ) {
					return false;
				}
			}

			if( !isActuatedSolid && tile.IsActuated ) {
				return false;
			}

			return true;
		}


		/// <summary>
		/// Indicates if a given tile has wires.
		/// </summary>
		/// <param name="tile"></param>
		/// <returns></returns>
		public static bool IsWire( Tile tile ) {
			if( tile == null /*|| !tile.active()*/ ) { return false; }
			return tile.RedWire || tile.BlueWire || tile.GreenWire || tile.YellowWire;
		}
	}
}
