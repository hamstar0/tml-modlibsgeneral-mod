using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;


namespace ModLibsGeneral.Libraries.Entities {
	/// <summary>
	/// Assorted static library functions pertaining to `Entity`s (parent class of Item, NPC, Player, and Projectile).
	/// </summary>
	public class EntityLibraries {
		/// <summary>
		/// Applies a given amount of force to the given entity from a given outside point.
		/// </summary>
		/// <param name="ent">Entity being acted upon.</param>
		/// <param name="worldPosFrom">Source of force.</param>
		/// <param name="force">Amount of force.</param>
		/// <param name="sync">Indicates whether to sync the entity in multiplayer.</param>
		public static void ApplyForce( Entity ent, Vector2 worldPosFrom, float force, bool sync ) {
			Vector2 offset = worldPosFrom - ent.Center;
			Vector2 forceVector = Vector2.Normalize( offset ) * force;
			ent.velocity += forceVector;

			if( sync && Main.netMode != NetmodeID.SinglePlayer ) {
				if( ent is NPC ) {
					NetMessage.SendData( MessageID.SyncNPC, -1, -1, null, ent.whoAmI );
				} else if( ent is Player ) {
					NetMessage.SendData( MessageID.SyncPlayer, -1, -1, null, ent.whoAmI );
				} else if( ent is Projectile ) {
					var proj = ent as Projectile;
					int projWho = Main.projectileIdentity[ proj.owner, proj.projUUID ];

					NetMessage.SendData( MessageID.SyncProjectile, -1, -1, null, projWho );
				}
			}
		}
	}
}
