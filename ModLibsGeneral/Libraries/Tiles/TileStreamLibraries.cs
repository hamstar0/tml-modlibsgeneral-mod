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

			bits1[0] = tile.active();
			bits1[2] = tile.wall > 0;
			bits1[3] = tile.liquid > 0 && Main.netMode == NetmodeID.Server;
			bits1[5] = tile.halfBrick();
			bits1[6] = tile.actuator();
			bits1[7] = tile.inActive();

			bits1[4] = tile.wire();
			bits2[0] = tile.wire2();
			bits2[1] = tile.wire3();
			bits2[7] = tile.wire4();

			if( tile.active() && tile.color() > 0 ) {
				bits2[2] = true;
				fColor = tile.color();
			}
			if( tile.wall > 0 && tile.wallColor() > 0 ) {
				bits2[3] = true;
				wColor = tile.wallColor();
			}
			bits2 += (byte)(tile.slope() << 4);

			writer.Write( (byte)bits1 );
			writer.Write( (byte)bits2 );

			if( fColor > 0 ) {
				writer.Write( (byte)fColor );
			}
			if( wColor > 0 ) {
				writer.Write( (byte)wColor );
			}

			if( tile.active() ) {
				writer.Write( (ushort)tile.type );

				if( forceFrames || Main.tileFrameImportant[(int)tile.type] ) {
					writer.Write( (short)tile.frameX );
					writer.Write( (short)tile.frameY );
				}
			}

			if( tile.wall > 0 ) {
				if( ModNet.AllowVanillaClients ) {
					writer.Write( (byte)tile.wall );
				} else {
					writer.Write( (ushort)tile.wall );
				}

				if( forceWallFrames ) {
					writer.Write( (short)tile.wallFrameX() );
					writer.Write( (short)tile.wallFrameY() );
				}
			}

			if( forceLiquids || (tile.liquid > 0 && Main.netMode == NetmodeID.Server) ) {
				writer.Write( (byte)tile.liquid );
				writer.Write( (byte)tile.liquidType() );
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

			//

			bool isActive = bits1[2];
			bool hasWall = bits1[2];
			bool hasLiquid = bits1[3];
			bool isHalfBrick = bits1[5];
			bool isActuator = bits1[6];
			bool isActuated = bits1[7];
			bool wire1 = bits1[4];
			bool wire2 = bits2[0];
			bool wire3 = bits2[1];
			bool wire4 = bits2[7];
			bool hasColor = bits2[2];
			bool hasWallColor = bits2[3];
			bool hasSlope1 = bits2[4];
			bool hasSlope2 = bits2[5];
			bool hasSlope3 = bits2[6];

			//

			bool wasActive = tile.active();

			tile.active( isActive );
			tile.wall = (byte)(hasWall ? 1 : 0);

			tile.halfBrick( isHalfBrick );
			tile.actuator( isActuator );
			tile.inActive( isActuated );

			tile.wire( wire1 );
			tile.wire2( wire2 );
			tile.wire3( wire3 );
			tile.wire4( wire4 );

			if( hasColor ) {
				tile.color( reader.ReadByte() );
			}
			if( hasWallColor ) {
				tile.wallColor( reader.ReadByte() );
			}

			if( tile.active() ) {
				ushort oldTileType = tile.type;

				tile.type = reader.ReadUInt16();

				if( forceFrames || Main.tileFrameImportant[(int)tile.type] ) {
					tile.frameX = reader.ReadInt16();
					tile.frameY = reader.ReadInt16();
				} else if( !wasActive || tile.type != oldTileType ) {
					tile.frameX = -1;
					tile.frameY = -1;
				}

				byte slope = 0;
				if( hasSlope1 ) {
					slope += 1;
				}
				if( hasSlope2 ) {
					slope += 2;
				}
				if( hasSlope3 ) {
					slope += 4;
				}
				tile.slope( slope );
			}

			if( tile.wall > 0 ) {
				tile.wall = ModNet.AllowVanillaClients
					? reader.ReadByte()
					: reader.ReadUInt16();

				if( forceWallFrames ) {
					tile.wallFrameX( (int)reader.ReadInt16() );
					tile.wallFrameY( (int)reader.ReadInt16() );
				}
			}

			if( hasLiquid ) {
				tile.liquid = reader.ReadByte();
				tile.liquidType( (int)reader.ReadByte() );
			} else {
				if( forceLiquids || Main.netMode != NetmodeID.Server ) {    // liquids are only 'real' on the server
					tile.liquid = 0;
				}
			}
		}
	}
}
