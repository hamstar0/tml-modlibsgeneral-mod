using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Libraries.Debug;


namespace ModLibsGeneral.Libraries.Tiles {
	/// <summary>
	/// Assorted static library functions pertaining to tiles.
	/// </summary>
	public partial class TileLibraries {
		[Obsolete("use KillTile", true)]
		public static bool KillTileSynced(
					int tileX,
					int tileY,
					bool effectOnly,
					bool dropsItem,
					bool forceSyncIfUnchanged,
					bool suppressErrors=true ) {
			return TileLibraries.KillTile( tileX, tileY, effectOnly, dropsItem, forceSyncIfUnchanged, true, true, suppressErrors );
		}

		[Obsolete( "use KillContainerTile", true )]
		public static bool KillContainerTileSynced( int tileX, int tileY, bool effectOnly, bool dropsItem ) {
			return TileLibraries.KillContainerTile( tileX, tileY, effectOnly, dropsItem, true, true );
		}

		[Obsolete( "use Swap1x1", true )]
		public static void Swap1x1Synced(
					int fromTileX,
					int fromTileY,
					int toTileX,
					int toTileY,
					bool preserveWall = false,
					bool preserveWire = false,
					bool preserveLiquid = false ) {
			TileLibraries.Swap1x1(
				fromTileX,
				fromTileY,
				toTileX,
				toTileY,
				preserveWall,
				preserveWire,
				preserveLiquid,
				true,
				true
			);
		}
	}
}
