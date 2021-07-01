using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;


namespace ModLibsGeneral.Libraries.Entities {
	/// <summary>
	/// Assorted static library functions pertaining to `Entity`s (parent class of Item, NPC, Player, and Projectile), as
	/// extensions.
	/// </summary>
	public static class EntityExtensions {
		/// <summary></summary>
		/// <param name="ent"></param>
		/// <returns></returns>
		public static Rectangle GetRectangle( this Entity ent ) {
			return new Rectangle( (int)ent.position.X, (int)ent.position.Y, ent.width, ent.height );
		}
	}
}
