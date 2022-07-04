using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Services.Hooks.ExtendedHooks;


namespace ModLibsGeneral {
	/// @private
	partial class ModLibsGeneralNPC : GlobalNPC {
		public override bool PreNPCLoot( NPC npc ) {
			ExtendedItemHooks.BeginScanningForLootDrops( npc );

			return base.PreNPCLoot( npc );
		}

		public override void NPCLoot( NPC npc ) {
			ExtendedItemHooks.FinishScanningForLootDropsAndThenRunHooks();

			if( npc.lastInteraction >= 0 && npc.lastInteraction < Main.player.Length ) {
				this.NPCKilledByPlayer( npc );
			}
		}


		////////////////

		private void NPCKilledByPlayer( NPC npc ) {
			if( Main.netMode == NetmodeID.Server ) {
				Player toPlayer = Main.player[npc.lastInteraction];

				if( toPlayer != null && toPlayer.active ) {
					ExtendedPlayerHooks.RunNpcKillHooks( toPlayer, npc );
				}
			} else if( Main.netMode == NetmodeID.SinglePlayer ) {
				ExtendedPlayerHooks.RunNpcKillHooks( Main.LocalPlayer, npc );
			}
		}
	}
}
