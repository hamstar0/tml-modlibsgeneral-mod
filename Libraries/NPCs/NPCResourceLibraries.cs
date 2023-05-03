using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;


namespace ModLibsGeneral.Libraries.NPCs {
	/// <summary>
	/// Assorted static library functions pertaining to players relative to NPC resources (e.g. textures).
	/// </summary>
	public partial class NPCResourceLibraries {
		/// <summary>
		/// Gets a NPC's texture. Loads NPC as needed.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static Texture2D SafelyGetTexture( int type ) {
			Main.instance.LoadNPC( type );
			return TextureAssets.Npc[type].Value;
		}
	}
}
