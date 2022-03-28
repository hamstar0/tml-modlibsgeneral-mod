using System;
using Terraria;


namespace ModLibsGeneral.Libraries.Projectiles {
	/// <summary>
	/// Assorted helpful projectile extension methods.
	/// </summary>
	public static class ProjectileExtensions {
		/// <summary>
		/// Self explanatory.
		/// </summary>
		/// <param name="self"></param>
		/// <param name="projType"></param>
		/// <returns></returns>
		public static bool Is( this Projectile self, int projType = -1 ) {
			if( projType == -1 ) {
				return self.active;
			} else {
				return self.active && self.type == projType;
			}
		}
	}
}
