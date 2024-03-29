﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using ModLibsCore.Libraries.Debug;


namespace ModLibsGeneral.Libraries.World {
	/// <summary>
	/// Assorted static library functions pertaining to locating things in the world.
	/// </summary>
	public partial class WorldLocationLibraries {
		/// <summary></summary>
		public static int SurfaceLayerTopTileY => WorldLocationLibraries.SkyLayerBottomTileY;

		/// <summary></summary>
		public static int SurfaceLayerBottomTileY => (int)Main.worldSurface;


		/// <summary></summary>
		public static int DirtLayerTopTileY => (int)Main.worldSurface;

		/// <summary></summary>
		public static int DirtLayerBottomTileY => (int)Main.rockLayer;


		/// <summary></summary>
		public static int RockLayerTopTileY => (int)Main.rockLayer;

		/// <summary></summary>
		public static int RockLayerBottomTileY => WorldLocationLibraries.UnderworldLayerTopTileY;


		/// <summary></summary>
		public static int SkyLayerTopTileY => 0;

		/// <summary></summary>
		public static int SkyLayerBottomTileY => (int)(Main.worldSurface * 0.35d);


		/// <summary></summary>
		public static int UnderworldLayerTopTileY => Main.maxTilesY - 200;

		/// <summary></summary>
		public static int UnderworldLayerBottomTileY => Main.maxTilesY;

		////

		//WorldGen.waterLine = (int) (Main.rockLayer + (double) Main.maxTilesY ) / 2;
		//WorldGen.waterLine += WorldGen.genRand.Next( -100, 20 );
		//WorldGen.lavaLine = WorldGen.waterLine + WorldGen.genRand.Next( 50, 80 );

		/// <summary></summary>
		public static int WaterLineHighestTileY => (((int)Main.rockLayer + Main.maxTilesY) / 2) - 100;

		/// <summary></summary>
		public static int WaterLineLowestTileY => (((int)Main.rockLayer + Main.maxTilesY) / 2) + 20;

		/// <summary></summary>
		public static int LavaLineHighestTileY => WorldLocationLibraries.WaterLineHighestTileY + 50;

		/// <summary></summary>
		public static int LavaLineLowestTileY => WorldLocationLibraries.WaterLineLowestTileY + 100;

		////

		/// <summary></summary>
		public static int BeachWestTileX => 380;

		/// <summary></summary>
		public static int BeachEastTileX => Main.maxTilesX - 380;



		////////////////

		/// <summary>
		/// Gets the identifiable region of a given point in the world.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <param name="percentWithinRNGLavaRange">Percent within an "RNG" (guess-only) range of the lava layer.</param>
		/// <returns></returns>
		public static WorldRegionFlags GetRegion( Vector2 worldPos, out float percentWithinRNGLavaRange ) {
			percentWithinRNGLavaRange = 0f;

			//

			WorldRegionFlags where = 0;
			int tileX = (int)worldPos.X / 16;
			int tileY = (int)worldPos.Y / 16;

			//

			if( WorldLocationLibraries.IsSky(tileY) ) {
				where |= WorldRegionFlags.Sky;
			} else if( WorldLocationLibraries.IsWithinUnderworld(tileY) ) {
				where |= WorldRegionFlags.Hell;
			} else if( WorldLocationLibraries.IsAboveWorldSurface(tileY) ) {
				where |= WorldRegionFlags.Overworld;

				if( WorldLocationLibraries.BeachEastTileX < tileX ) {
					where |= WorldRegionFlags.OceanEast;
				} else if( WorldLocationLibraries.BeachWestTileX > tileX ) {
					where |= WorldRegionFlags.OceanWest;
				}
			} else {
				if( WorldLocationLibraries.IsDirtLayer(tileY) ) {
					where |= WorldRegionFlags.CaveDirt;
				} else {
					if( WorldLocationLibraries.IsPreRockLayer(tileY) ) {
						where |= WorldRegionFlags.CavePreRock;
					}
					if( WorldLocationLibraries.IsRockLayer(tileY) ) {
						where |= WorldRegionFlags.CaveRock;

						if( WorldLocationLibraries.IsLavaLayer(tileY, out percentWithinRNGLavaRange) ) {
							where |= WorldRegionFlags.CaveLava;
						}
					}
				}
			}

			return where;
		}


		////////////////

		/// <summary>
		/// Indicates if the given position is in the sky/space.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsSky( Vector2 worldPos ) {
			return WorldLocationLibraries.IsSky( (int)worldPos.Y / 16 );
		}

		/// <summary>
		/// Indicates if the given position is above the world's surface, but not in the sky.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsOverworld( Vector2 worldPos ) {
			return WorldLocationLibraries.IsOverworld( (int)worldPos.Y / 16 );
		}

		/// <summary>
		/// Indicates if the given position is above the world's surface.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsAboveWorldSurface( Vector2 worldPos ) {
			return WorldLocationLibraries.IsAboveWorldSurface( (int)worldPos.Y / 16 );
		}

		/// <summary>
		/// Indicates if the given position is within the underground dirt layer (beneath elevation 0, above the rock layer).
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsDirtLayer( Vector2 worldPos ) {
			return WorldLocationLibraries.IsDirtLayer( (int)worldPos.Y / 16 );
		}

		/// <summary>
		/// Indicates if the given position is within the underground pre-rock layer (background appears like dirt,
		/// but the game recognizes the 'rockLayer' depth).
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsPreRockLayer( Vector2 worldPos ) {
			return WorldLocationLibraries.IsPreRockLayer( (int)worldPos.Y / 16 );
		}

		/// <summary>
		/// Indicates if the given position is within the underground rock layer.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsRockLayer( Vector2 worldPos ) {
			return WorldLocationLibraries.IsRockLayer( (int)worldPos.Y / 16 );
		}

		/// <summary>
		/// Indicates if the given position is within the underworld (hell).
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsWithinUnderworld( Vector2 worldPos ) {
			return WorldLocationLibraries.IsWithinUnderworld( (int)worldPos.Y / 16 );
		}

		////

		/// <summary>
		/// Indicates if the given position is at a beach/ocean area.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsBeach( Vector2 worldPos ) {
			if( !WorldLocationLibraries.IsOverworld( worldPos ) ) {
				return false;
			}
			return IsBeachRegion( worldPos );
		}

		/// <summary>
		/// Indicates if the given position is within the region of a beach (regardless of elevation).
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsBeachRegion( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;
			return tilePos.X < 380 || tilePos.X > (Main.maxTilesX - 380);
		}


		////////////////

		/// <summary>
		/// Indicates if the given position is in the sky/space.
		/// </summary>
		/// <param name="tileY"></param>
		/// <returns></returns>
		public static bool IsSky( int tileY ) {
			return tileY <= ( Main.worldSurface * 0.35 );   //0.34999999403953552?
		}

		/// <summary>
		/// Indicates if the given position is above the world's surface, but not in the sky.
		/// </summary>
		/// <param name="tileY"></param>
		/// <returns></returns>
		public static bool IsOverworld( int tileY ) {
			return (double)tileY <= Main.worldSurface
				&& (double)tileY > Main.worldSurface * 0.35;
		}

		/// <summary>
		/// Indicates if the given position is above the world's surface.
		/// </summary>
		/// <param name="tileY"></param>
		/// <returns></returns>
		public static bool IsAboveWorldSurface( int tileY ) {
			return tileY < Main.worldSurface;
		}

		/// <summary>
		/// Indicates if the given position is within the underground dirt layer (beneath elevation 0, above the rock layer).
		/// </summary>
		/// <param name="tileY"></param>
		/// <returns></returns>
		public static bool IsDirtLayer( int tileY ) {
			return (double)tileY > Main.worldSurface
				&& (double)tileY <= Main.rockLayer;
		}
		
		/// <summary>
		/// Indicates if the given position is within the underground pre-rock layer (background appears like dirt,
		/// but the game recognizes the 'rockLayer' depth).
		/// </summary>
		/// <param name="tileY"></param>
		/// <returns></returns>
		public static bool IsPreRockLayer( int tileY ) {
			return (double)tileY > Main.rockLayer
				&& (double)tileY <= Main.rockLayer + 34;	//between 33 and 37
		}

		/// <summary>
		/// Indicates if the given position is within the underground rock layer.
		/// </summary>
		/// <param name="tileY"></param>
		/// <returns></returns>
		public static bool IsRockLayer( int tileY ) {
			return (double)tileY > Main.rockLayer
				&& tileY <= Main.maxTilesY - 200;
		}

		/// <summary>
		/// Indicates if the given position is within the underground lava layer.
		/// </summary>
		/// <param name="tileY"></param>
		/// <param name="percentWithinRNGRange">What percent amount into the world gen's RNG</param>
		/// <returns></returns>
		public static bool IsLavaLayer( int tileY, out float percentWithinRNGRange ) {
			if( tileY > Main.maxTilesY - 200 ) {
				percentWithinRNGRange = 0f;

				return false;
			}

			int minY = WorldLocationLibraries.LavaLineHighestTileY;
			int maxY = WorldLocationLibraries.LavaLineHighestTileY;

			percentWithinRNGRange = (float)(tileY - minY) / (float)(maxY - minY);
			if( percentWithinRNGRange > 1f ) {
				percentWithinRNGRange = 1f;
			}

			return tileY >= minY;
			//&& tileY > ((int)Main.rockLayer + 37);  //601 world units?
		}

		/// <summary>
		/// Indicates if the given position is within the underworld (hell).
		/// </summary>
		/// <param name="tileY"></param>
		/// <returns></returns>
		public static bool IsWithinUnderworld( int tileY ) {
			return tileY > (Main.maxTilesY - 200);
		}
	}
}
