using System;
using Microsoft.Xna.Framework;
using Terraria;
using ModLibsCore.Libraries.Debug;


namespace ModLibsGeneral.Libraries.World {
	/// <summary>
	/// Assorted static library functions pertaining to locating things in the world.
	/// </summary>
	public partial class WorldLocationLibraries {
		[Obsolete( "use alt", true )]
		public static WorldRegionFlags GetRegion( Vector2 worldPos ) {
			WorldRegionFlags flags = WorldLocationLibraries.GetRegion( worldPos, out float perLava );
			if( (flags & WorldRegionFlags.CaveLava) != 0 ) {
				if( perLava < 0.5f ) {
					flags -= WorldRegionFlags.CaveLava;
				}
			}

			return flags;
		}

		[Obsolete( "use IsLavaLayer(int, out float)", true )]
		public static bool IsLavaLayer( Vector2 worldPos ) {
			return WorldLocationLibraries.IsLavaLayer( (int)worldPos.Y / 16, out _ );
		}
	}
}
