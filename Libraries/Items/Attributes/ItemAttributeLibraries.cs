﻿using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Classes.DataStructures;


namespace ModLibsGeneral.Libraries.Items.Attributes {
	/// <summary>
	/// Assorted static library functions pertaining to gameplay attributes of items.
	/// </summary>
	public partial class ItemAttributeLibraries {
		private static IDictionary<int, int> _ProjPene = new Dictionary<int, int>();



		////////////////

		/// <summary>
		/// Indicates an item produces a penetrating projectile.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static bool IsPenetrator( Item item ) {
			if( item.shoot <= ProjectileID.None ) { return false; }

			if( !ItemAttributeLibraries._ProjPene.Keys.Contains( item.shoot ) ) {
				var proj = new Projectile();
				proj.SetDefaults( item.shoot );

				ItemAttributeLibraries._ProjPene[ item.shoot ] = proj.penetrate;
			}

			return  ItemAttributeLibraries._ProjPene[ item.shoot ] == -1 ||
					ItemAttributeLibraries._ProjPene[ item.shoot ] >= 3;   // 3 seems fair?
		}


		/// <summary>
		/// Indicates an item is a tool.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static bool IsTool( Item item ) {
			return ( item.useStyle > 0 ||
					item.damage > 0 ||
					item.crit > 0 ||
					item.knockBack > 0 ||
					item.CountsAsClass(DamageClass.Melee) ||
					item.CountsAsClass(DamageClass.Magic) ||
					item.CountsAsClass(DamageClass.Ranged) ||
					item.CountsAsClass(DamageClass.Throwing) ||
					item.CountsAsClass(DamageClass.Summon) ||
					item.pick > 0 ||
					item.hammer > 0 ||
					item.axe > 0 )
				&& !item.accessory
				&& !item.potion
				&& !item.consumable
				&& !item.vanity
				&& item.type != ItemID.Actuator;   // Actuators are not consumable, apparently
		}


		/// <summary>
		/// Indicates an item is an armor piece.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static bool IsArmor( Item item ) {
			return ( item.headSlot != -1 ||
					item.bodySlot != -1 ||
					item.legSlot != -1 ) &&
				!item.accessory &&
				!item.potion &&
				!item.consumable &&
				!item.vanity;
		}


		/// <summary>
		/// Indicates an item is a grapple item.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static bool IsGrapple( Item item ) {
			return Main.projHook[item.shoot];
		}


		/// <summary>
		/// Indicates an item is a type of yoyo.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static bool IsYoyo( Item item ) {
			if( item.shoot > ProjectileID.None && item.useStyle == ItemUseStyleID.Shoot && item.CountsAsClass(DamageClass.Melee) && item.channel ) {
				var proj = new Projectile();
				proj.SetDefaults( item.shoot );

				return proj.aiStyle == 99;
			}
			return false;
		}


		/// <summary>
		/// Indicates an item is "gameplay relevant" (not vanity, passive, or purely cosmetic).
		/// </summary>
		/// <param name="item"></param>
		/// <param name="toysRelevant">"Toy" items are counted as gameplay relevant.</param>
		/// <param name="junkRelevant">"Junk" items are counted as gameplay relevant.</param>
		/// <returns></returns>
		public static bool IsGameplayRelevant( Item item, bool toysRelevant = false, bool junkRelevant = false ) {
			if( !toysRelevant ) {
				switch( item.type ) {
				case ItemID.WaterGun:
				case ItemID.SlimeGun:
				case ItemID.BeachBall:
					return false;
				}
			}
			if( !junkRelevant && item.rare < ItemRarityID.White ) {
				return false;
			}
			return !item.vanity && item.dye <= 0 && item.hairDye <= 0 && item.paint == 0 && !Main.vanityPet[item.buffType];
		}


		/// <summary>
		/// "Appraises" an item's "value" based on its sell value and rarity, as if to rank items by a general metric (loosely).
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static float LooselyAppraise( Item item ) {
			var cloneItem = new Item();
			cloneItem.SetDefaults( item.type, true );

			float appraisal = (float)(item.rare + item.rare + cloneItem.rare) / 3f;

			if( item.value > 0 ) {
				float value = (float)item.value / 8000f;
				appraisal = ( ( appraisal * 4f ) + value ) / 5f;
			}

			return appraisal;
		}


		/// <summary>
		/// Gets the context name of a container item (bag, box, present, etc.).
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static string GetVanillaContainerContext( Item item ) {
			if( ( item.type >= ItemID.KingSlimeBossBag && item.type <= ItemID.MoonLordBossBag )
					|| item.type == ItemID.BossBagBetsy || item.type == ItemID.BossBagDarkMage || item.type == ItemID.BossBagOgre
					|| ItemID.Sets.BossBag[item.type] ) {
				return "bossBag";
			}
			if( ( item.type >= ItemID.WoodenCrate && item.type <= ItemID.GoldenCrate )
					|| ( item.type >= ItemID.CorruptFishingCrate && item.type <= ItemID.JungleFishingCrate ) ) {
				return "crate";
			}
			if( item.type == ItemID.HerbBag ) {
				return "herbBag";
			}
			if( item.type == ItemID.GoodieBag ) {
				return "goodieBag";
			}
			if( item.type == ItemID.LockBox ) {
				return "lockBox";
			}
			if( item.type == ItemID.Present ) {
				return "present";
			}
			return "";
		}


		/// <summary>
		/// Finds all item types of a given sell value.
		/// </summary>
		/// <param name="buyValue"></param>
		/// <param name="includeCoins">Includes coin items (defaults to `false`).</param>
		/// <returns></returns>
		public static ReadOnlySet<int> FindItemsByValue( long buyValue, bool includeCoins = false ) {
			ItemAttributeLibraries itemAttr = ModContent.GetInstance<ItemAttributeLibraries>();

			if( !itemAttr.PurchasableItems.Keys.Contains(buyValue) ) {
				itemAttr.PurchasableItems[buyValue] = new HashSet<int>();
			} else {
				return new ReadOnlySet<int>( itemAttr.PurchasableItems[buyValue] );
			}

			for( int i = 0; i < TextureAssets.Item.Length; i++ ) {
				if( !includeCoins && i == 71 ) { i = 75; }

				Item item = new Item();
				item.SetDefaults( i );
				if( item.value <= 0 ) { continue; }

				if( (buyValue % item.value) == 0 ) {
					itemAttr.PurchasableItems[ buyValue ].Add( i );
				}
			}

			return new ReadOnlySet<int>( itemAttr.PurchasableItems[buyValue] );
		}
	}
}
