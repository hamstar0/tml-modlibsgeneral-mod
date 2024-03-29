﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ObjectData;


namespace ModLibsGeneral.Libraries.Tiles {
	/// <summary>
	/// Assorted static library functions pertaining to tile objects.
	/// </summary>
	public class TileObjectLibraries {
		/// <summary>
		/// Predicts the top left position of the given tile object of a given tile, assuming a contiguous object.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <returns></returns>
		public static Point? PredictTopLeftOfObject( int tileX, int tileY ) {
			Tile tile = Framing.GetTileSafely( tileX, tileY );
			TileObjectData tileData = TileObjectData.GetTileData( tile.TileType, 0, 0 );
			if( tileData == null ) {
				return null;
			}

			int frameX = tile.TileFrameX;
			int frameY = tile.TileFrameY;
			int frameCol = frameX / tileData.CoordinateFullWidth;
			int frameRow = frameY / tileData.CoordinateFullHeight;
			int wrap = tileData.StyleWrapLimit == 0
				? 1
				: tileData.StyleWrapLimit;
			int subTile = tileData.StyleHorizontal
				? (frameRow * wrap) + frameCol
				: (frameCol * wrap) + frameRow;
			int style = subTile / tileData.StyleMultiplier;
			int alternate = subTile % tileData.StyleMultiplier;
			//for( int k = 0; k < tileData.AlternatesCount; k++ ) {
			//	if( alternate >= tileData.Alternates[k].Style && alternate <= tileData.Alternates[k].Style + tileData.RandomStyleRange ) {
			//		alternate = k;
			//		break;
			//	}
			//}

			tileData = TileObjectData.GetTileData( tile.TileType, style, alternate + 1 );
			int subFrameX = frameX % tileData.CoordinateFullWidth;
			int subFrameY = frameY % tileData.CoordinateFullHeight;
			int tileOfFrameX = subFrameX / ( tileData.CoordinateWidth + tileData.CoordinatePadding );
			int tileOfFrameY = 0;

			int remainingFrameY = subFrameY;
			while( remainingFrameY > 0 ) {
				remainingFrameY -= tileData.CoordinateHeights[tileOfFrameY] + tileData.CoordinatePadding;
				tileOfFrameY++;
			}

			if( tileOfFrameX == 0 && tileOfFrameY == 0 ) {
				return null;
			}

			int leftmostTileX = tileX - tileOfFrameX;
			int topmostTileY = tileY - tileOfFrameY;
			
			//int originX = tileX + tileData.Origin.X;
			//int originY = tileY + tileData.Origin.Y;
			return new Point( leftmostTileX, topmostTileY );
		}
	}
}
