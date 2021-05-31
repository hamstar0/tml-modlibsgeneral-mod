using System;
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


		/// <summary></summary>
		public static int BeachWestTileX => 380;

		/// <summary></summary>
		public static int BeachEastTileX => Main.maxTilesX - 380;



		////////////////

		/// <summary>
		/// Gets the identifiable region of a given point in the world.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static WorldRegionFlags GetRegion( Vector2 worldPos ) {
			WorldRegionFlags where = 0;

			if( WorldLocationLibraries.IsSky(worldPos) ) {
				where |= WorldRegionFlags.Sky;
			} else if( WorldLocationLibraries.IsWithinUnderworld(worldPos) ) {
				where |= WorldRegionFlags.Hell;
			} else if( WorldLocationLibraries.IsAboveWorldSurface(worldPos) ) {
				where |= WorldRegionFlags.Overworld;

				if( WorldLocationLibraries.BeachEastTileX < (worldPos.X/16) ) {
					where |= WorldRegionFlags.OceanEast;
				} else if( WorldLocationLibraries.BeachWestTileX > (worldPos.X/16) ) {
					where |= WorldRegionFlags.OceanWest;
				}
			} else {
				if( WorldLocationLibraries.IsDirtLayer( worldPos ) ) {
					where |= WorldRegionFlags.CaveDirt;
				} else {
					if( WorldLocationLibraries.IsPreRockLayer( worldPos ) ) {
						where |= WorldRegionFlags.CavePreRock;
					}
					if( WorldLocationLibraries.IsRockLayer( worldPos ) ) {
						where |= WorldRegionFlags.CaveRock;

						if( WorldLocationLibraries.IsLavaLayer( worldPos ) ) {
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
			Vector2 tilePos = worldPos / 16;
			return tilePos.Y <= ( Main.worldSurface * 0.35 );   //0.34999999403953552?
		}

		/// <summary>
		/// Indicates if the given position is above the world's surface, but not in the sky.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsOverworld( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;
			return (double)tilePos.Y <= Main.worldSurface && (double)tilePos.Y > Main.worldSurface * 0.35;
		}

		/// <summary>
		/// Indicates if the given position is above the world's surface.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsAboveWorldSurface( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;
			return tilePos.Y < Main.worldSurface;
		}

		/// <summary>
		/// Indicates if the given position is within the underground dirt layer (beneath elevation 0, above the rock layer).
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsDirtLayer( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;
			return (double)tilePos.Y > Main.worldSurface && (double)tilePos.Y <= Main.rockLayer;
		}

		/// <summary>
		/// Indicates if the given position is within the underground pre-rock layer (background appears like dirt,
		/// but the game recognizes the 'rockLayer' depth).
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsPreRockLayer( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;    //between 33 and 37
			return (double)tilePos.Y > Main.rockLayer && (double)tilePos.Y <= Main.rockLayer + 34;
		}

		/// <summary>
		/// Indicates if the given position is within the underground rock layer.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsRockLayer( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;
			return (double)tilePos.Y > Main.rockLayer && tilePos.Y <= Main.maxTilesY - 200;
		}

		/// <summary>
		/// Indicates if the given position is within the underground lava layer.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsLavaLayer( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;
			return tilePos.Y <= Main.maxTilesY - 200 && (double)tilePos.Y > (Main.rockLayer + 601);
		}

		/// <summary>
		/// Indicates if the given position is within the underworld (hell).
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsWithinUnderworld( Vector2 worldPos ) {
			Vector2 tilePos = worldPos / 16;
			return tilePos.Y > (Main.maxTilesY - 200);
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
	}
}
