using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;


namespace ModLibsGeneral.Libraries.Tiles {
	/// <summary>
	/// Assorted static library functions pertaining to stream IO for tiles.
	/// </summary>
	public partial class TileStreamLibraries {
		/// <summary></summary>
		/// <param name="writer"></param>
		/// <param name="tile"></param>
		/// <param name="forceFrames"></param>
		/// <param name="forceWallFrames"></param>
		/// <param name="forceLiquids"></param>
		public static void ToStream(
					BinaryWriter writer,
					Tile tile,
					bool forceFrames=false,
					bool forceWallFrames=false,
					bool forceLiquids=false ) {
			BitsByte bits1 = 0;
			BitsByte bits2 = 0;
			byte fColor = 0;
			byte wColor = 0;

			bits1[0] = tile.HasTile;
			bits1[2] = tile.WallType > 0;
			bits1[3] = tile.LiquidAmount > 0 && Main.netMode == NetmodeID.Server;
			bits1[5] = tile.IsHalfBlock;
			bits1[6] = tile.HasActuator;
			bits1[7] = tile.IsActuated;

			bits1[4] = tile.RedWire;
			bits2[0] = tile.BlueWire;
			bits2[1] = tile.GreenWire;
			bits2[7] = tile.YellowWire;

			if( tile.HasTile && tile.TileColor > 0 ) {
				bits2[2] = true;
				fColor = tile.TileColor;
			}
			if( tile.WallType > 0 && tile.WallColor > 0 ) {
				bits2[3] = true;
				wColor = tile.WallColor;
			}
			bits2 += (byte)(tile.Slope << 4);

			writer.Write( (byte)bits1 );
			writer.Write( (byte)bits2 );

			if( fColor > 0 ) {
				writer.Write( (byte)fColor );
			}
			if( wColor > 0 ) {
				writer.Write( (byte)wColor );
			}

			if( tile.HasTile ) {
				writer.Write( (ushort)tile.TileType );

				if( forceFrames || Main.tileFrameImportant[(int)tile.TileType] ) {
					writer.Write( (short)tile.TileFrameX );
					writer.Write( (short)tile.TileFrameY );
				}
			}

			if( tile.WallType > 0 ) {
				if( ModNet.AllowVanillaClients ) {
					writer.Write( (byte)tile.WallType );
				} else {
					writer.Write( (ushort)tile.WallType );
				}

				if( forceWallFrames ) {
					writer.Write( (short)tile.WallFrameX );
					writer.Write( (short)tile.WallFrameY );
				}
			}

			if( forceLiquids || (tile.LiquidAmount > 0 && Main.netMode == NetmodeID.Server) ) {
				writer.Write( (byte)tile.LiquidAmount );
				writer.Write( (byte)tile.LiquidType );
			}
		}


		/// <summary></summary>
		/// <param name="reader"></param>
		/// <param name="tile"></param>
		/// <param name="forceFrames"></param>
		/// <param name="forceWallFrames"></param>
		/// <param name="forceLiquids"></param>
		public static void FromStream(
					BinaryReader reader,
					ref Tile tile,
					bool forceFrames = false,
					bool forceWallFrames = false,
					bool forceLiquids = false ) {
			BitsByte bits1 = reader.ReadByte();
			BitsByte bits2 = reader.ReadByte();

			bool wasActive = tile.HasTile;

			tile.HasTile = bits1[0];
			tile.WallType = (byte)( bits1[2] ? 1 : 0 );

			bool isLiquid = bits1[3];
			if( forceLiquids || Main.netMode != NetmodeID.Server ) {
				tile.LiquidAmount = (byte)( isLiquid ? 1 : 0 );
			}

			tile.IsHalfBlock = bits1[5];
			tile.HasActuator = bits1[6];
			tile.IsActuated = bits1[7];

			tile.RedWire = bits1[4];
			tile.BlueWire = bits2[0];
			tile.GreenWire = bits2[1];
			tile.YellowWire = bits2[7];

			if( bits2[2] ) {
				tile.TileColor = reader.ReadByte();
			}
			if( bits2[3] ) {
				tile.WallColor = reader.ReadByte();
			}

			if( tile.HasTile ) {
				int oldTileType = (int)tile.TileType;

				tile.TileType = reader.ReadUInt16();

				if( forceFrames || Main.tileFrameImportant[(int)tile.TileType] ) {
					tile.TileFrameX = reader.ReadInt16();
					tile.TileFrameY = reader.ReadInt16();
				} else if( !wasActive || (int)tile.TileType != oldTileType ) {
					tile.TileFrameX = -1;
					tile.TileFrameY = -1;
				}

				byte slope = 0;
				if( bits2[4] ) {
					slope += 1;
				}
				if( bits2[5] ) {
					slope += 2;
				}
				if( bits2[6] ) {
					slope += 4;
				}
				tile.Slope = slope;
			}

			if( tile.WallType > 0 ) {
				tile.WallType = ModNet.AllowVanillaClients
					? reader.ReadByte()
					: reader.ReadUInt16();

				if( forceWallFrames ) {
					tile.WallFrameX = (int)reader.ReadInt16();
					tile.WallFrameY = (int)reader.ReadInt16();
				}
			}

			if( isLiquid ) {
				tile.LiquidAmount = reader.ReadByte();
				tile.LiquidType = (int)reader.ReadByte();
			}
		}
	}
}
