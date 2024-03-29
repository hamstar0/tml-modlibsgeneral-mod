﻿using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.TModLoader;


namespace ModLibsGeneral.Services.Hooks.ExtendedHooks {
	/// <summary>
	/// Supplies custom tModLoader-like, delegate-based hooks for item-relevant functions not currently available in
	/// tModLoader.
	/// </summary>
	public partial class ExtendedItemHooks : ILoadable {
		/// <summary></summary>
		/// <param name="npc"></param>
		/// <param name="itemWhos">List of index values of loot items in `Main.item`.</param>
		public delegate void NPCLootDelegate( NPC npc, IList<int> itemWhos );



		////////////////
		
		internal static void BeginScanningForLootDrops( NPC npc ) {
			var eih = TmlLibraries.SafelyGetInstance<ExtendedItemHooks>();

			if( eih.OnNPCLootHooks.Count > 0 ) {
				eih.LootNPC = npc;
				eih.ItemsSnapshot = (Item[])Main.item.Clone();
			}
		}
		
		internal static void FinishScanningForLootDropsAndThenRunHooks() {
			var eih = TmlLibraries.SafelyGetInstance<ExtendedItemHooks>();
			if( eih.ItemsSnapshot == null ) {
				return;
			}

			var newItemWhos = new List<int>();

			int max = Main.item.Length;
			for( int i=0; i<max; i++ ) {
				Item newItem = Main.item[i];
				if( newItem?.active != true ) {
					continue;
				}

				Item oldItem = eih.ItemsSnapshot[i];
				if( oldItem?.active != true ) {
					newItemWhos.Add( i );
				}
			}

			foreach( NPCLootDelegate hook in eih.OnNPCLootHooks ) {
				hook( eih.LootNPC, newItemWhos );
			}

			eih.LootNPC = null;
			eih.ItemsSnapshot = null;
		}



		////////////////

		private ISet<NPCLootDelegate> OnNPCLootHooks = new HashSet<NPCLootDelegate>();

		private NPC LootNPC = null;

		private Item[] ItemsSnapshot = null;



		////////////////

		private ExtendedItemHooks() { }

		////

		/// @private
		void ILoadable.Load( Mod mod ) { }

		/// @private
		void ILoadable.Unload() { }
	}
}
