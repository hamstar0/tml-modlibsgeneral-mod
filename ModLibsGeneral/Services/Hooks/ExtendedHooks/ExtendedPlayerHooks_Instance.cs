using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace ModLibsGeneral.Services.Hooks.ExtendedHooks {
	/// @private
	public partial class ExtendedPlayerHooks : ILoadable {
		private static object MyLock = new object();



		////////////////

		private IDictionary<string, Action<Player, int>> BuffExpireHooks = new Dictionary<string, Action<Player, int>>();
		private IDictionary<string, Action<Player, int, Item>> ArmorEquipHooks = new Dictionary<string, Action<Player, int, Item>>();
		private IDictionary<string, Action<Player, int, int>> ArmorUnequipHooks = new Dictionary<string, Action<Player, int, int>>();

		private ISet<Action<Player, NPC>> NpcKillHooks = new HashSet<Action<Player, NPC>>();



		////////////////

		/// @private
		void ILoadable.Load( Mod mod ) { }

		/// @private
		void ILoadable.Unload() { }
	}
}
