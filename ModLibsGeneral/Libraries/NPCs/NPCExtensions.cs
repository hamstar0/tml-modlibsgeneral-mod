using System;
using Terraria;


namespace ModLibsGeneral.Libraries.NPCs {
	/// <summary>
	/// Assorted helpful item extension methods.
	/// </summary>
	public static class NPCExtensions {
		/// <summary>
		/// Self explanatory.
		/// </summary>
		/// <param name="self"></param>
		/// <param name="npcType"></param>
		/// <returns></returns>
		public static bool Is( this NPC self, int npcType = -1 ) {
			if( npcType == -1 ) {
				return self.active;
			} else {
				return self.active && self.netID == npcType;
			}
		}
	}
}
