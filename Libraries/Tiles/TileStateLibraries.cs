using System;
using Terraria;
using Terraria.ID;


namespace ModLibsGeneral.Libraries.Tiles {
	/// <summary></summary>
	public enum TileSlopeType : byte {
		/// <summary></summary>
		None = 0,
		/// <summary></summary>
		TopRightSlope = 1,
		/// <summary></summary>
		TopLeftSlope = 2,
		/// <summary></summary>
		BottomRightSlope = 3,
		/// <summary></summary>
		BottomLeftSlope = 4,
	}




	/// <summary></summary>
	public enum TileShapeType {
		/// <summary></summary>
		None = 0,
		/// <summary></summary>
		Any = -1,
		/// <summary></summary>
		TopRightSlope = 1,
		/// <summary></summary>
		TopLeftSlope = 2,
		/// <summary></summary>
		BottomRightSlope = 4,
		/// <summary></summary>
		BottomLeftSlope = 8,
		/// <summary></summary>
		TopSlope = TopRightSlope + TopLeftSlope,
		/// <summary></summary>
		BottomSlope = BottomRightSlope + BottomLeftSlope,
		/// <summary></summary>
		LeftSlope = TopLeftSlope + BottomLeftSlope,
		/// <summary></summary>
		RightSlope = TopRightSlope + BottomRightSlope,
		/// <summary></summary>
		HalfBrick = 16
	}



	/// <summary>
	/// Credit to https://tshock.readme.io/docs/multiplayer-packet-structure
	/// </summary>
	public enum TileChangeNetMessageType : int {
		/// <summary></summary>
		KillTile = 0,
		/// <summary></summary>
		PlaceTile = 1,
		/// <summary></summary>
		KillWall = 2,
		/// <summary></summary>
		PlaceWall = 3,
		/// <summary></summary>
		KillTileNoItem = 4,
		/// <summary></summary>
		PlaceWire = 5,
		/// <summary></summary>
		KillWire = 6,
		/// <summary></summary>
		PoundTile = 7,
		/// <summary></summary>
		PlaceActuator = 8,
		/// <summary></summary>
		KillActuator = 9,
		/// <summary></summary>
		PlaceWire2 = 10,
		/// <summary></summary>
		KillWire2 = 11,
		/// <summary></summary>
		PlaceWire3 = 12,
		/// <summary></summary>
		KillWire3 = 13,
		/// <summary></summary>
		SlopeTile = 14,
		/// <summary></summary>
		FrameTrack = 15,
		/// <summary></summary>
		PlaceWire4 = 16,
		/// <summary></summary>
		KillWire4 = 17,
		/// <summary></summary>
		PokeLogicGate = 18,
		/// <summary></summary>
		Actuate = 19
	}




	/// <summary>
	/// Assorted static library functions pertaining to tile state (slope, actuation, etc.).
	/// </summary>
	public class TileStateLibraries {
		/// <summary></summary>
		/// <param name="tile"></param>
		public static void FlipSlopeHorizontally( Tile tile ) {
			switch( tile.Slope ) {
			case 1:
				tile.Slope = 2;
				break;
			case 2:
				tile.Slope = 1;
				break;
			case 3:
				tile.Slope = 4;
				break;
			case 4:
				tile.Slope = 3;
				break;
			}
		}

		/// <summary></summary>
		/// <param name="tile"></param>
		public static void FlipSlopeVertically( Tile tile ) {
			switch( tile.Slope ) {
			case 1:
				tile.Slope = 3;
				break;
			case 2:
				tile.Slope = 4;
				break;
			case 3:
				tile.Slope = 1;
				break;
			case 4:
				tile.Slope = 2;
				break;
			}
		}



		/// <summary>
		/// Attempts to "smartly" smooth a given tile against its adjacent tiles. TODO: Verify correctness.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="isSynced"></param>
		/// <returns></returns>
		public static bool SmartSlope( int tileX, int tileY, bool isSynced ) {
			Tile mid = Framing.GetTileSafely( tileX, tileY );
			Tile up = Framing.GetTileSafely( tileX, tileY - 1 );
			Tile down = Framing.GetTileSafely( tileX, tileY + 1 );
			Tile left = Framing.GetTileSafely( tileX - 1, tileY );
			Tile right = Framing.GetTileSafely( tileX + 1, tileY );

			bool upSolid = up.HasTile && up.Slope == 0 && Main.tileSolid[up.TileType] && !Main.tileSolidTop[up.TileType];
			bool downSolid = down.HasTile && down.Slope == 0 && Main.tileSolid[down.TileType] && !Main.tileSolidTop[down.TileType];
			bool leftSolid = left.HasTile && left.Slope == 0 && Main.tileSolid[left.TileType] && !Main.tileSolidTop[left.TileType];
			bool rightSolid = right.HasTile && right.Slope == 0 && Main.tileSolid[right.TileType] && !Main.tileSolidTop[right.TileType];

			upSolid = upSolid
				&& (up.Slope == (byte)TileSlopeType.TopLeftSlope || up.Slope == (byte)TileSlopeType.TopRightSlope);
			downSolid = downSolid
				&& (down.Slope == (byte)TileSlopeType.BottomLeftSlope || down.Slope == (byte)TileSlopeType.BottomRightSlope );
			leftSolid = leftSolid
				&& (left.Slope == (byte)TileSlopeType.TopRightSlope || left.Slope == (byte)TileSlopeType.BottomRightSlope );
			rightSolid = rightSolid
				&& ( right.Slope == (byte)TileSlopeType.TopLeftSlope || right.Slope == (byte)TileSlopeType.BottomLeftSlope );
			int changed = 0;

			// Up
			if( !upSolid && downSolid && leftSolid && !rightSolid ) {
				mid.Slope = (byte)TileSlopeType.TopLeftSlope;
				changed = 1;
			}
			if( !upSolid && downSolid && !leftSolid && rightSolid ) {
				mid.Slope = (byte)TileSlopeType.TopRightSlope;
				changed = 1;
			}
			if( !upSolid && downSolid && !leftSolid && !rightSolid ) {
				mid.IsHalfBlock = true;
				changed = 2;
			}

			// Down
			if( upSolid && !downSolid && leftSolid && !rightSolid ) {
				mid.Slope = (byte)TileSlopeType.BottomLeftSlope;
				changed = 1;
			}
			if( upSolid && !downSolid && !leftSolid && rightSolid ) {
				mid.Slope = (byte)TileSlopeType.BottomRightSlope;
				changed = 1;
			}

			if( isSynced && Main.netMode == NetmodeID.MultiplayerClient ) {
				if( changed != 0 ) {
					NetMessage.SendData( MessageID.TileChange, -1, -1, null, (int)TileChangeNetMessageType.SlopeTile, (float)tileX, (float)tileY, (float)mid.Slope, 0, 0, 0 );
				}
			}

			return false;
		}
	}
}
