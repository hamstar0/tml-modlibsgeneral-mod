﻿using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace ModLibsGeneral.Libraries.Projectiles {
	/// <summary>
	/// Assorted static library functions pertaining to players relative to projectile identification
	/// </summary>
	public partial class ProjectileIdentityLibraries {
		/// <summary>
		/// Gets a (human readable) unique key (as segments) from a given projectile type.
		/// </summary>
		/// <param name="projType"></param>
		/// <returns></returns>
		public static Tuple<string, string> GetUniqueKeySegs( int projType ) {
			if( projType < 0 || projType >= ProjectileLoader.ProjectileCount ) {
				throw new ArgumentOutOfRangeException( "Invalid type: " + projType );
			}
			if( projType < ProjectileID.Count ) {
				return Tuple.Create( "Terraria", ProjectileID.Search.GetName( projType ) );
			}

			var modProjectile = ProjectileLoader.GetProjectile( projType );
			return Tuple.Create( modProjectile.Mod.Name, modProjectile.Name );
		}


		// TODO: GetVanillaSnapshotHash()
	}
}
