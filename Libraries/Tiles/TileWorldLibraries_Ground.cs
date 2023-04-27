using System;
using Microsoft.Xna.Framework;
using Terraria;
using ModLibsCore.Libraries.Debug;


namespace ModLibsGeneral.Libraries.Tiles {
	/// <summary>
	/// Assorted static library functions pertaining to tiles relative to the world.
	/// </summary>
	public static partial class TileWorldLibraries {
		public delegate bool IsGround( int tileX, int tileY );



		////////////////

		/// <summary>
		/// Drops from a given point to the ground.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <param name="invertGravity"></param>
		/// <param name="isGround">Defines what "ground" is.</param>
		/// <param name="groundPos"></param>
		/// <returns>`true` if a ground point was found within world boundaries.</returns>
		public static bool DropToGround( Vector2 worldPos,
				bool invertGravity,
				IsGround isGround,
				out Vector2 groundPos ) {
			int furthestTileY = invertGravity ? 42 : Main.maxTilesY - 42;
			return TileWorldLibraries.DropToGround( worldPos, invertGravity, isGround, furthestTileY, out groundPos );
		}


		/// <summary>
		/// Drops from a given point to the ground.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <param name="invertGravity"></param>
		/// <param name="isGround">Defines what "ground" is.</param>
		/// <param name="furthestTileY">Limit to check tiles down (or up) to before giving up.</param>
		/// <param name="groundPos"></param>
		/// <returns>`true` if a ground point was found within world boundaries.</returns>
		public static bool DropToGround( Vector2 worldPos,
				bool invertGravity,
				IsGround isGround,
				int furthestTileY,
				out Vector2 groundPos ) {
			bool hitGround = true;
			int tileX = (int)worldPos.X >> 4;
			int tileY = (int)worldPos.Y >> 4;

			if( invertGravity ) {
				do {
					tileY--;
					if( tileY >= furthestTileY ) {
						hitGround = false;
						break;
					}
				} while( !isGround( tileX, tileY ) );
			} else {
				do {
					tileY++;
					if( tileY >= furthestTileY ) {
						hitGround = false;
						break;
					}
				} while( !isGround( tileX, tileY ) );
			}

			groundPos = new Vector2( worldPos.X, tileY * 16 );
			return hitGround;
		}
	}
}
