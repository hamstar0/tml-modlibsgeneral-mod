using System;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;


namespace ModLibsGeneral.Libraries.Tiles.Attributes {
	/// <summary></summary>
	public enum VanillaTileCuttingContext {
		/// <summary></summary>
		Unknown = 0,
		/// <summary></summary>
		AttackMelee = 1,
		/// <summary></summary>
		AttackProjectile = 2,
		/// <summary></summary>
		TilePlacement = 3
	}




	/// <summary>
	/// Assorted static library functions pertaining to tile attributes.
	/// </summary>
	public partial class TileAttributeLibraries {
		/// <summary>
		/// Indicates if a given tile type is an "object" (container, sign, station, etc.).
		/// </summary>
		/// <param name="tileType"></param>
		/// <returns></returns>
		public static bool IsObject( int tileType ) {
			return Main.tileFrameImportant[tileType]
				|| Main.tileContainer[tileType]
				|| Main.tileSign[tileType]
				|| Main.tileAlch[tileType]
				|| Main.tileTable[tileType]; //tileFlame
		}


		/// <summary>
		/// Indicates if a given specific tile is "breakable" (typically to normal attacks).
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public static bool IsBreakable( int tileX, int tileY, VanillaTileCuttingContext context = VanillaTileCuttingContext.AttackMelee ) {
			Tile tile = Framing.GetTileSafely( tileX, tileY );

			return Main.tileCut[tile.TileType]
				&& WorldGen.CanCutTile(tileX, tileY, (TileCuttingContext)context);
		}
		
		////

		/// <summary>
		/// Indicates if a given tile cannot be destroyed by vanilla explosives.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <returns></returns>
		public static bool IsNotVanillaBombable( int tileX, int tileY ) {
			Tile tile = Framing.GetTileSafely( tileX, tileY );
			return !TileLoader.CanExplode( tileX, tileY ) || TileAttributeLibraries.IsNotVanillaBombableType( tile.TileType );
		}

		/// <summary>
		/// Indicates if a given tile type cannot be destroyed by vanilla explosives.
		/// </summary>
		/// <param name="tileType"></param>
		/// <returns></returns>
		public static bool IsNotVanillaBombableType( int tileType ) {
			return Main.tileDungeon[tileType] ||
				tileType == TileID.Dressers ||
				tileType == TileID.Containers ||
				tileType == TileID.DemonAltar ||
				tileType == TileID.Cobalt ||
				tileType == TileID.Mythril ||
				tileType == TileID.Adamantite ||
				tileType == TileID.LihzahrdBrick ||
				tileType == TileID.LihzahrdAltar ||
				tileType == TileID.Palladium ||
				tileType == TileID.Orichalcum ||
				tileType == TileID.Titanium ||
				tileType == TileID.Chlorophyte ||
				tileType == TileID.DesertFossil ||
				( !Main.hardMode && tileType == TileID.Hellstone );
		}

		////

		/// <summary>
		/// Gets the damage scale (amount to multiply a pickaxe hit by) of a given tile.
		/// </summary>
		/// <param name="tile"></param>
		/// <param name="baseDamage">Amount of damage (e.g. pickaxe power) applied to the tile.</param>
		/// <param name="isAbsolute">Returns indication that tile will guarantee destruction on this hit.</param>
		/// <returns></returns>
		public static float GetDamageScale( Tile tile, float baseDamage, out bool isAbsolute ) {
			isAbsolute = false;
			float scale = 0f;

			if( Main.tileNoFail[(int)tile.TileType] ) {
				isAbsolute = true;
			}
			if( Main.tileDungeon[(int)tile.TileType] || tile.TileType == 25 || tile.TileType == 58 || tile.TileType == 117 || tile.TileType == 203 ) {
				scale = 1f / 2f;
			} else if( tile.TileType == 48 || tile.TileType == 232 ) {
				scale = 1f / 4f;
			} else if( tile.TileType == 226 ) {
				scale = 1f / 4f;
			} else if( tile.TileType == 107 || tile.TileType == 221 ) {
				scale = 1f / 2f;
			} else if( tile.TileType == 108 || tile.TileType == 222 ) {
				scale = 1f / 3f;
			} else if( tile.TileType == 111 || tile.TileType == 223 ) {
				scale = 1f / 4f;
			} else if( tile.TileType == 211 ) {
				scale = 1f / 5f;
			} else {
				int moddedDamage = 0;
				TileLoader.MineDamage( (int)baseDamage, ref moddedDamage );

				scale = (float)moddedDamage / baseDamage;
			}

			if( tile.TileType == 211 && baseDamage < 200 ) {
				scale = 0f;
			}
			if( ( tile.TileType == 25 || tile.TileType == 203 ) && baseDamage < 65 ) {
				scale = 0f;
			} else if( tile.TileType == 117 && baseDamage < 65 ) {
				scale = 0f;
			} else if( tile.TileType == 37 && baseDamage < 50 ) {
				scale = 0f;
			} else if( tile.TileType == 404 && baseDamage < 65 ) {
				scale = 0f;
			} else if( ( tile.TileType == 22 || tile.TileType == 204 ) && /*(double)y > Main.worldSurface &&*/ baseDamage < 55 ) {
				scale = 0f;
			} else if( tile.TileType == 56 && baseDamage < 65 ) {
				scale = 0f;
			} else if( tile.TileType == 58 && baseDamage < 65 ) {
				scale = 0f;
			} else if( ( tile.TileType == 226 || tile.TileType == 237 ) && baseDamage < 210 ) {
				scale = 0f;
			} else if( Main.tileDungeon[(int)tile.TileType] && baseDamage < 65 ) {
				//if( (double)x < (double)Main.maxTilesX * 0.35 || (double)x > (double)Main.maxTilesX * 0.65 ) {
				//	scale = 0f;
				//}
				scale = 0f;
			} else if( tile.TileType == 107 && baseDamage < 100 ) {
				scale = 0f;
			} else if( tile.TileType == 108 && baseDamage < 110 ) {
				scale = 0f;
			} else if( tile.TileType == 111 && baseDamage < 150 ) {
				scale = 0f;
			} else if( tile.TileType == 221 && baseDamage < 100 ) {
				scale = 0f;
			} else if( tile.TileType == 222 && baseDamage < 110 ) {
				scale = 0f;
			} else if( tile.TileType == 223 && baseDamage < 150 ) {
				scale = 0f;
			} else {
				if( TileLoader.GetTile( tile.TileType ) != null ) {
					int moddedDamage = 0;
					TileLoader.PickPowerCheck( tile, (int)baseDamage, ref moddedDamage );

					scale = (float)moddedDamage / baseDamage;
				} else {
					scale = 1f;
				}
			}

			if( tile.TileType == 147 || tile.TileType == 0 || tile.TileType == 40 || tile.TileType == 53 || tile.TileType == 57 || tile.TileType == 59 || tile.TileType == 123 || tile.TileType == 224 || tile.TileType == 397 ) {
				scale = 1f;
			}
			if( tile.TileType == 165 || Main.tileRope[(int)tile.TileType] || tile.TileType == 199 || Main.tileMoss[(int)tile.TileType] ) {
				isAbsolute = true;
			}

			return scale;
			//if( this.hitTile.AddDamage( tileId, scale, false ) >= 100 && ( tile.type == 2 || tile.type == 23 || tile.type == 60 || tile.type == 70 || tile.type == 109 || tile.type == 199 || Main.tileMoss[(int)tile.type] ) ) {
			//	scale = 0f;
			//}
			//if( tile.type == 128 || tile.type == 269 ) {
			//	if( tile.frameX == 18 || tile.frameX == 54 ) {
			//		x--;
			//		tile = Main.tile[x, y];
			//		this.hitTile.UpdatePosition( tileId, x, y );
			//	}
			//	if( tile.frameX >= 100 ) {
			//		scale = 0f;
			//		Main.blockMouse = true;
			//	}
			//}
			//if( tile.type == 334 ) {
			//	if( tile.frameY == 0 ) {
			//		y++;
			//		tile = Main.tile[x, y];
			//		this.hitTile.UpdatePosition( tileId, x, y );
			//	}
			//	if( tile.frameY == 36 ) {
			//		y--;
			//		tile = Main.tile[x, y];
			//		this.hitTile.UpdatePosition( tileId, x, y );
			//	}
			//	int i = (int)tile.frameX;
			//	bool flag = i >= 5000;
			//	bool flag2 = false;
			//	if( !flag ) {
			//		int num2 = i / 18;
			//		num2 %= 3;
			//		x -= num2;
			//		tile = Main.tile[x, y];
			//		if( tile.frameX >= 5000 ) {
			//			flag = true;
			//		}
			//	}
			//	if( flag ) {
			//		i = (int)tile.frameX;
			//		int num3 = 0;
			//		while( i >= 5000 ) {
			//			i -= 5000;
			//			num3++;
			//		}
			//		if( num3 != 0 ) {
			//			flag2 = true;
			//		}
			//	}
			//	if( flag2 ) {
			//		scale = 0f;
			//		Main.blockMouse = true;
			//	}
			//}
		}

		/// <summary>
		/// Gets a chest's internal type code.
		/// </summary>
		/// <param name="tileType"></param>
		/// <returns></returns>
		public static int? GetChestTypeCode( int tileType ) {
			switch( tileType ) {
			case TileID.Containers:
				return 0;
			case TileID.Containers2:
				return 4;
			case TileID.Dressers:
				return 2;
			default:
				if( TileID.Sets.BasicChest[tileType] ) {
					return 100;
				} else if( TileLoader.IsDresser( tileType ) ) {
					return 102;
				}
				break;
			}
			return null;//1?
		}
	}
}
