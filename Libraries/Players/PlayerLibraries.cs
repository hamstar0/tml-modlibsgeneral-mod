﻿using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsGeneral.Libraries.Items;
using ModLibsGeneral.Libraries.Items.Attributes;
using ModLibsGeneral.Internals.NetProtocols;
using System.Reflection;

namespace ModLibsGeneral.Libraries.Players {
	/// <summary>
	/// Assorted static library functions pertaining to players.
	/// </summary>
	public partial class PlayerLibraries {
		/// <summary></summary>
		public const int InventorySize = 58;
		/// <summary></summary>
		public const int InventoryHotbarSize = 10;
		/// <summary></summary>
		public const int InventoryMainSize = 40;



		////////////////

		/// <summary>
		/// Predicts impending fall damage amount for a given player at the current time (if any).
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static int ComputeImpendingFallDamage( Player player ) {
			if( player.mount.CanFly() ) {
				return 0;
			}
			if( player.mount.Cart && Minecart.OnTrack( player.position, player.width, player.height ) ) {
				return 0;
			}
			if( player.mount.Type == 1 ) {
				return 0;
			}

			int safetyMin = 25 + player.extraFall;
			int damage = (int)(player.position.Y / 16f) - player.fallStart;

			if( player.stoned ) {
				return (int)(((float)damage * player.gravDir - 2f) * 20f);
			}

			if( (player.gravDir == 1f && damage > safetyMin) || (player.gravDir == -1f && damage < -safetyMin) ) {
				if( player.noFallDmg ) {
					return 0;
				}
				for( int n = 3; n < 10; n++ ) {
					if( player.armor[n].stack > 0 && player.armor[n].wingSlot > -1 ) {
						return 0;
					}
				}

				int finalDamage = (int)((float)damage * player.gravDir - (float)safetyMin) * 10;
				if( player.mount.Active ) {
					finalDamage = (int)((float)finalDamage * player.mount.FallDamage);
				}
				return finalDamage;
			}

			return 0;
		}


		/// <summary>
		/// Loosely assesses player's relative level of power. Factors include appraisals of inventory items, player's defense,
		/// and player's life.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static float LooselyAssessPower( Player player ) {
			float armorCount=0, miscCount=0;
			float hotbarTech=0, armorTech=0, miscTech=0;

			for( int i=0; i<PlayerLibraries.InventoryHotbarSize; i++ ) {
				Item item = player.inventory[i];
				if( item == null || item.IsAir || !ItemAttributeLibraries.IsGameplayRelevant(item) ) { continue; }

				float tech = ItemAttributeLibraries.LooselyAppraise( item );
				hotbarTech = hotbarTech > tech ? hotbarTech : tech;
			}

			int maxArmorSlot = 8 + player.extraAccessorySlots;
			for( int i=0; i<maxArmorSlot; i++ ) {
				Item item = player.inventory[i];
				if( item == null || item.IsAir || !ItemAttributeLibraries.IsGameplayRelevant( item ) ) { continue; }

				armorTech += ItemAttributeLibraries.LooselyAppraise( item );
				armorCount += 1;
			}

			for( int i = 0; i < player.miscEquips.Length; i++ ) {
				Item item = player.miscEquips[i];
				if( item == null || item.IsAir || !ItemAttributeLibraries.IsGameplayRelevant( item ) ) { continue; }

				miscTech += ItemAttributeLibraries.LooselyAppraise( item );
				miscCount += 1;
			}

			float techFactor = armorTech / (armorCount * ItemRarityAttributeLibraries.HighestVanillaRarity);
			techFactor += miscTech / (miscCount * ItemRarityAttributeLibraries.HighestVanillaRarity);
			techFactor += hotbarTech + hotbarTech;
			techFactor /= 4;

			float defenseFactor = 1f + ((float)player.statDefense * 0.01f);
			float addedVitality = (float)player.statLifeMax / 20f;
			float vitalityFactor = addedVitality * defenseFactor;

			return (techFactor + techFactor + vitalityFactor) / 3f;
		}

		////

		/// <summary>
		/// Indicates if player counts as 'incapacitated'.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="freedomNeeded">Pulley, grappling, or minecarting count as incapacitation. Default `false`.</param>
		/// <param name="armsNeeded">Use of arms (e.g. no Cursed debuff) count as incapacitation. Default `false`.</param>
		/// <param name="sightNeeded">Visual occlusion (e.g. Blackout debuff) counts as incapacitation. Default `false`.</param>
		/// <param name="sanityNeeded">Control complications (e.g. Confused debuff) counts as incapacition. Default `false`.</param>
		/// <returns></returns>
		public static bool IsIncapacitated(
					Player player,
					bool freedomNeeded=false,
					bool armsNeeded=false,
					bool sightNeeded=false,
					bool sanityNeeded=false ) {
			if( player == null || !player.active || player.dead || player.stoned || player.frozen || player.ghost ||
				player.gross || player.webbed || player.mapFullScreen ) { return true; }
			if( freedomNeeded && (player.pulley || player.grappling[0] >= 0 || player.mount.Cart) ) { return true; }
			if( armsNeeded && player.noItems ) { return true; }
			if( sightNeeded && player.blackout ) { return true; }
			if( sanityNeeded && player.confused ) { return true; }
			return false;
		}


		////

		/// <summary>
		/// Apply armor-bypassing damage to player, killing if needed (syncs via. Player.Hurt(...)).
		/// </summary>
		/// <param name="player"></param>
		/// <param name="deathReason"></param>
		/// <param name="damage"></param>
		/// <param name="direction"></param>
		/// <param name="pvp"></param>
		/// <param name="quiet"></param>
		/// <param name="crit"></param>
		public static void RawHurt(
					Player player,
					PlayerDeathReason deathReason,
					int damage,
					int direction,
					bool pvp=false,
					bool quiet=false,
					bool crit=false ) {
			int def = player.statDefense;

			player.statDefense = 0;
			player.Hurt( deathReason, damage, direction, pvp, quiet, crit );
			player.statDefense = def;
		}

		////

		/// <summary>
		/// Kills a player permanently (as if hardcore mode).
		/// </summary>
		/// <param name="player"></param>
		/// <param name="deathMsg"></param>
		public static void KillWithPermadeath( Player player, string deathMsg ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				PlayerPermaDeathProtocol.BroadcastFromClient( player.whoAmI, deathMsg );
			} else if( Main.netMode == NetmodeID.Server ) {
				PlayerPermaDeathProtocol.BroadcastFromServer( player.whoAmI, deathMsg, false );

				PlayerLibraries.ApplyPermaDeathState( player, deathMsg );
			} else {
				PlayerLibraries.ApplyPermaDeathState( player, deathMsg );
			}
		}

		internal static void ApplyPermaDeathState( Player player, string deathMsg ) {
			player.difficulty = 2;

			if( Main.netMode != NetmodeID.Server ) {    // Already syncs from client
				player.KillMe( PlayerDeathReason.ByCustomReason( deathMsg ), 9999, 0 );
			}
		}


		////

		/// <summary>
		/// Resets a player back to (vanilla) defaults.
		/// </summary>
		/// <param name="player"></param>
		public static void FullVanillaReset( Player player ) {
			for( int i = 0; i < player.inventory.Length; i++ ) {
				player.inventory[i] = new Item();
			}
			for( int i = 0; i < player.armor.Length; i++ ) {
				player.armor[i] = new Item();
			}
			for( int i = 0; i < player.bank.item.Length; i++ ) {
				player.bank.item[i] = new Item();
			}
			for( int i = 0; i < player.bank2.item.Length; i++ ) {
				player.bank2.item[i] = new Item();
			}
			for( int i = 0; i < player.bank3.item.Length; i++ ) {
				player.bank3.item[i] = new Item();
			}
			for( int i = 0; i < player.dye.Length; i++ ) {
				player.dye[i] = new Item();
			}
			for( int i = 0; i < player.miscDyes.Length; i++ ) {
				player.miscDyes[i] = new Item();
			}
			for( int i = 0; i < player.miscEquips.Length; i++ ) {
				player.miscEquips[i] = new Item();
			}

			for( int i = 0; i < player.buffType.Length; i++ ) {
				player.buffType[i] = 0;
				player.buffTime[i] = 0;
			}

			player.trashItem = new Item();
			if( player.whoAmI == Main.myPlayer ) {
				Main.mouseItem = new Item();
			}

			player.statLifeMax = 100;
			player.statManaMax = 20;

			player.extraAccessory = false;
			player.anglerQuestsFinished = 0;
			player.bartenderQuestLog = 0;
			player.downedDD2EventAnyDifficulty = false;
			player.taxMoney = 0;

			var getDefaultsMethod = typeof(Player).GetMethod("DropItems_GetDefaults", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			var vanillaItems = (IEnumerable<Item>)getDefaultsMethod.Invoke(player, null);

			PlayerLoader.SetStartInventory( player, PlayerLoader.GetStartingItems(player, vanillaItems.Where(i => !i.IsAir) ) );
		}

		////

		/// <summary>
		/// For just the current game tick, the player is under complete lockdown: No movement, actions, or damage taken.
		/// </summary>
		/// <param name="player"></param>
		public static void LockdownPlayerPerTick( Player player ) {
			player.noItems = true;
			player.noBuilding = true;
			player.stoned = true;
			player.immune = true;
			player.immuneTime = 2;
		}


		/// <summary>
		/// Standardized test for a player's condition being 'default'.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="ignoreBuffs"></param>
		/// <param name="ignoreLife"></param>
		/// <param name="ignoreMana"></param>
		/// <param name="ignoreInventory"></param>
		/// <param name="ignoreEquips"></param>
		/// <param name="ignoreBanks"></param>
		/// <param name="inventorySlotExceptions"></param>
		/// <param name="equipSlotExceptions"></param>
		/// <returns></returns>
		public static bool IsPlayerVanillaFresh(
					Player player,
					bool ignoreBuffs=true,
					bool ignoreLife=false,
					bool ignoreMana=false,
					bool ignoreInventory=false,
					bool ignoreEquips=false,
					bool ignoreBanks=false,
					ISet<int> inventorySlotExceptions=null,
					ISet<int> equipSlotExceptions=null ) {
			if( !ignoreBuffs ) {
				return player.buffTime.Any( t => t > 0 );
			}

			if( !ignoreLife && player.statLifeMax2 != 100 ) {
				return false;
			}

			if( !ignoreMana && player.statManaMax2 != 20 ) {
				return false;
			}

			if( !ignoreInventory ) {
				if( !(inventorySlotExceptions?.Contains(0) == true) ) {
					if( player.inventory[0].type != ItemID.CopperShortsword ) {
						return false;
					}
				}
				if( !(inventorySlotExceptions?.Contains(1) == true) ) {
					if( player.inventory[1].type != ItemID.CopperPickaxe ) {
						return false;
					}
				}
				if( !(inventorySlotExceptions?.Contains(2) == true) ) {
					if( player.inventory[2].type != ItemID.CopperAxe ) {
						return false;
					}
				}
				for( int i=3; i<player.inventory.Length; i++ ) {
					if( inventorySlotExceptions?.Contains(i) == true ) {
						continue;
					}
					if( player.inventory[i]?.active == true ) {
						return true;
					}
				}
			}

			if( !ignoreEquips ) {
				for( int i=3; i<player.armor.Length; i++ ) {
					if( equipSlotExceptions?.Contains(i) == true ) {
						continue;
					}
					if( player.armor[i]?.active == true ) {
						return true;
					}
				}
			}

			if( !ignoreBanks ) {
				for( int i=3; i<player.bank.item.Length; i++ ) {
					if( player.bank.item[i]?.active == true ) {
						return true;
					}
				}
				for( int i=3; i<player.bank2.item.Length; i++ ) {
					if( player.bank2.item[i]?.active == true ) {
						return true;
					}
				}
				for( int i=3; i<player.bank3.item.Length; i++ ) {
					if( player.bank3.item[i]?.active == true ) {
						return true;
					}
				}
			}

			return true;
		}
	}
}
