using Terraria;
using Terraria.ID;


namespace ModLibsGeneral.Libraries.Projectiles {
	/// <summary>
	/// Assorted static library functions pertaining to players relative to projectiles.
	/// </summary>
	public partial class ProjectileLibraries {
		/// <summary>
		/// Applies projectile "hits", as if to make the effect of impacting something (including consuming penetrations).
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="syncs"></param>
		public static void Hit( Projectile projectile, bool syncs ) {
			if( projectile.penetrate <= 0 ) {
				projectile.Kill();
			} else {
				projectile.penetrate--;
				projectile.netUpdate = true;
			}

			if( syncs && Main.netMode != NetmodeID.SinglePlayer ) {
				NetMessage.SendData(
					msgType: MessageID.SyncProjectile,
					remoteClient: -1,
					ignoreClient: -1,
					text: null,
					number: Main.projectileIdentity[ projectile.owner, projectile.projUUID ]
				);
			}
		}
	}
}
