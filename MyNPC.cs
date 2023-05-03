using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;


namespace ModLibsGeneral {
	/// @private
	partial class ModLibsGeneralNPC : GlobalNPC {
		internal Entity FakeTarget = null;
		internal Vector2? FakeTargetPosition = null;

		internal float? LockedAI0 = null;
		internal float? LockedAI1 = null;
		internal float? LockedAI2 = null;
		internal float? LockedAI3 = null;



		////////////////

		public override bool InstancePerEntity => true;



		////////////////

		/*public override void SetupShop( int type, Chest shop, ref int nextSlot ) {
			if( ModLibsGeneralConfig.Instance.GeoResonantOrbSoldByDryad ) {
				if( type == NPCID.Dryad ) {
					var orbItem = new Item();
					orbItem.SetDefaults( ModContent.ItemType<GeoResonantOrbItem>() );

					shop.item[ nextSlot++ ] = orbItem;
				}
			}
		}*/
	}
}
