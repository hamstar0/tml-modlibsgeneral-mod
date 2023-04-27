using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Libraries.Debug;
using Terraria.DataStructures;

namespace ModLibsGeneral.Libraries.Tiles {
	/// <summary>
	/// Assorted static library functions pertaining to tiles.
	/// </summary>
	public partial class TileLibraries {
		/// <summary>
		/// Places a given tile of a given type. Synced.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="tileType"></param>
		/// <param name="placeStyle"></param>
		/// <param name="muted"></param>
		/// <param name="forced"></param>
		/// <param name="plrWho"></param>
		/// <returns>`true` if tile placement succeeded.</returns>
		public static bool PlaceTileSynced( int tileX, int tileY, int tileType, int placeStyle = 0, bool muted = false, bool forced = false, int plrWho = -1 ) {
			if( tileType == TileID.Containers || tileType == TileID.Containers2 ) {
				int chestIdx = WorldGen.PlaceChest( tileX, tileY, (ushort)tileType, false, placeStyle );
				if( chestIdx == -1 ) { return false; }

				int? chestType = Attributes.TileAttributeLibraries.GetChestTypeCode( tileType );
				if( !chestType.HasValue ) { return false; }

				NetMessage.SendData(
					msgType: MessageID.ChestUpdates,
					remoteClient: -1,
					ignoreClient: -1,
					text: null,
					number: chestType.Value,
					number2: (float)tileX,
					number3: (float)tileY,
					number4: 0f,
					number5: chestIdx,
					number6: tileType,
					number7: 0 );
				int itemSpawn = Chest.chestItemSpawn[placeStyle];//b8 < 100 ?  : TileLoader.GetTile( tileType ).chestDrop;
				if( itemSpawn > 0 ) {
					// Should this be EntitySource_TileBreak instead?
					var itemSource = new EntitySource_TileUpdate( tileX, tileY );

					Item.NewItem( itemSource, tileX<<4, tileY<<4, 32, 32, itemSpawn, 1, true, 0, false, false );
				}
			} else if( !WorldGen.PlaceTile( tileX, tileY, tileType, muted, forced, plrWho, placeStyle ) ) {
				return false;
			}

			if( Main.netMode != NetmodeID.SinglePlayer ) {
				NetMessage.SendData( MessageID.TileChange, plrWho, -1, null, 1, (float)tileX, (float)tileY, (float)tileType, placeStyle, 0, 0 );
			}

			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				if( tileType == TileID.Chairs ) {
					NetMessage.SendTileSquare( -1, tileX - 1, tileY - 1, 3, TileChangeType.None );
				} else if( tileType == TileID.Beds || tileType == TileID.Bathtubs ) {
					NetMessage.SendTileSquare( -1, tileX, tileY, 5, TileChangeType.None );
				}
			}

			return true;
		}


		////////////////

		/// <summary>
		/// Kills a given tile. Results are synced.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="effectOnly">Only a visual effect; tile is not actually killed (nothing to sync).</param>
		/// <param name="dropsItem"></param>
		/// <param name="forceSyncIfUnchanged"></param>
		/// <param name="syncIfClient"></param>
		/// <param name="syncIfServer"></param>
		/// <param name="suppressErrors"></param>
		/// <returns>`true` when no complications occurred during removal. Does not force tile to be removed, if
		/// complications occur.</returns>
		public static bool KillTile(
					int tileX,
					int tileY,
					bool effectOnly,
					bool dropsItem,
					bool forceSyncIfUnchanged,
					bool syncIfClient=false,
					bool syncIfServer=true,
					bool suppressErrors=true ) {
			bool sync = (syncIfClient && Main.netMode == NetmodeID.MultiplayerClient)
				|| (syncIfServer && Main.netMode == NetmodeID.Server);
			Tile tile = Framing.GetTileSafely( tileX, tileY );

			if( !tile.HasTile ) {
				if( forceSyncIfUnchanged && sync ) {
					NetMessage.SendData( MessageID.TileChange, -1, -1, null, 4, (float)tileX, (float)tileY, 0f, 0, 0, 0 );
				}

				return false;
			}

			//

			bool isTileKilled = false;
			bool isContainer = tile.TileType == TileID.Containers || tile.TileType == TileID.Containers2;

			try {
				if( isContainer ) {
					isTileKilled = TileLibraries.KillContainerTile( tileX, tileY, effectOnly, dropsItem, syncIfClient, syncIfServer );
				} else {
					WorldGen.KillTile( tileX, tileY, false, effectOnly, !dropsItem );
					isTileKilled = effectOnly || !Main.tile[ tileX, tileY ].HasTile;
				}
			} catch( Exception e ) {
				if( !suppressErrors ) {
					LogLibraries.WarnOnce( "Could not kill type (with sync) at "+tileX+", "+tileY
						+" (effectOnly:"+effectOnly+", dropsItem:"+dropsItem
						+": "+e.Message );
					throw e;
				}
				return false;
			}

			//

			if( !isContainer && !effectOnly ) {
				if( sync ) {
					NetMessage.SendData(
						msgType: MessageID.TileChange,
						remoteClient: -1,
						ignoreClient: -1,
						text: null,
						number: dropsItem ? 0 : 4,
						number2: (float)tileX,
						number3: (float)tileY,
						number4: 0f,
						number5: 0,
						number6: 0,
						number7: 0
					);
				}
			}

			return isTileKilled;
		}


		/// <summary>
		/// Kills a given container tile. Results are synced.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="effectOnly">Only a visual effect; tile is not actually killed (nothing to sync).</param>
		/// <param name="dropsItem"></param>
		/// <param name="syncIfClient"></param>
		/// <param name="syncIfServer"></param>
		/// <returns>`true` when no complications occurred during removal. Does not force tile to be removed, if
		/// complications occur.</returns>
		public static bool KillContainerTile(
					int tileX,
					int tileY,
					bool effectOnly,
					bool dropsItem,
					bool syncIfClient = false,
					bool syncIfServer = true ) {
			Tile tile = Framing.GetTileSafely( tileX, tileY );
			if( tile.TileType != TileID.Containers && tile.TileType != TileID.Containers2 ) {
				return false;
			}

			bool sync = ( syncIfClient && Main.netMode == NetmodeID.MultiplayerClient )
				|| ( syncIfServer && Main.netMode == NetmodeID.Server );

			int chestIdx = Chest.FindChest( tileX, tileY );
			int chestType = tile.TileType == TileID.Containers2 ? 5 : 1;

			if( chestIdx != -1 && Chest.DestroyChest(tileX, tileY) ) {
				//if( Main.tile[x, y].type >= TileID.Count ) {
				//	number2 = 101;
				//}

				if( sync ) {
					NetMessage.SendData(
						msgType: MessageID.ChestUpdates,
						remoteClient: -1,
						ignoreClient: -1,
						text: null,
						number: chestType,
						number2: (float)tileX,
						number3: (float)tileY,
						number4: 0f,
						number5: chestIdx,
						number6: tile.TileType,
						number7: 0
					);
					NetMessage.SendTileSquare( -1, tileX, tileY, 3, TileChangeType.None );
				}
			}

			WorldGen.KillTile( tileX, tileY, false, effectOnly, !dropsItem );
			return effectOnly || !Main.tile[ tileX, tileY ].HasTile;
		}


		/// <summary>
		/// Swaps 1x1 tiles. Destructive.
		/// </summary>
		/// <param name="fromTileX"></param>
		/// <param name="fromTileY"></param>
		/// <param name="toTileX"></param>
		/// <param name="toTileY"></param>
		/// <param name="preserveWall"></param>
		/// <param name="preserveWire"></param>
		/// <param name="preserveLiquid"></param>
		/// <param name="syncIfClient"></param>
		/// <param name="syncIfServer"></param>
		public static void Swap1x1(
					int fromTileX,
					int fromTileY,
					int toTileX,
					int toTileY,
					bool preserveWall = false,
					bool preserveWire = false,
					bool preserveLiquid = false,
					bool syncIfClient = false,
					bool syncIfServer = true ) {
			bool sync = ( syncIfClient && Main.netMode == NetmodeID.MultiplayerClient )
				|| ( syncIfServer && Main.netMode == NetmodeID.Server );

			Tile fromTile = Framing.GetTileSafely( fromTileX, fromTileY );
			Tile fromTileCopy = (Tile)fromTile.Clone();
			Tile toTile = Framing.GetTileSafely( toTileX, toTileY );

			fromTile.CopyFrom( toTile );
			toTile.CopyFrom( fromTileCopy );

			if( preserveWall ) {
				ushort oldToWall = fromTile.WallType;
				fromTile.WallType = fromTileCopy.WallType;
				toTile.WallType = oldToWall;
			}

			if( preserveWire ) {
				bool oldToWire = fromTile.RedWire;
				fromTile.RedWire = fromTileCopy.RedWire;
				toTile.RedWire = oldToWire;

				bool oldToWire2 = fromTile.BlueWire;
				fromTile.BlueWire = fromTileCopy.BlueWire;
				toTile.BlueWire = oldToWire2;

				bool oldToWire3 = fromTile.GreenWire;
				fromTile.GreenWire = fromTileCopy.GreenWire;
				toTile.GreenWire = oldToWire3;

				bool oldToWire4 = fromTile.YellowWire;
				fromTile.YellowWire = fromTileCopy.YellowWire;
				toTile.YellowWire = oldToWire4;
			}

			if( preserveLiquid ) {
				byte oldToLiquid = fromTile.LiquidAmount;
				fromTile.LiquidAmount = fromTileCopy.LiquidAmount;
				toTile.LiquidAmount = oldToLiquid;

				int oldToLiquidType = fromTile.LiquidType;
				fromTile.LiquidType = fromTileCopy.LiquidType;
				toTile.LiquidType = oldToLiquidType;
			}

			if( sync ) {
				NetMessage.SendData( MessageID.TileChange, -1, -1, null, 4, (float)fromTileX, (float)fromTileY, 0f, 0, 0, 0 );
				NetMessage.SendData( MessageID.TileChange, -1, -1, null, 4, (float)toTileX, (float)toTileY, 0f, 0, 0, 0 );
			}
		}
	}
}
