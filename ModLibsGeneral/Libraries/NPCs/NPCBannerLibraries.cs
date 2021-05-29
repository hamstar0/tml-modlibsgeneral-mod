using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Classes.DataStructures;
using ModLibsCore.Libraries.Debug;


namespace ModLibsGeneral.Libraries.NPCs {
	/// <summary>
	/// Assorted static library functions pertaining to NPC-dropped banners.
	/// </summary>
	public partial class NPCBannerLibraries {
		/// <summary>
		/// Gets table of npc types to their respective banner item types.
		/// </summary>
		/// <returns></returns>
		public static IDictionary<int, int> GetNpcToBannerItemTypes() {
			IDictionary<int, int> npcTypesToBannerItemTypes = new Dictionary<int, int>();

			for( int npcType = 0; npcType < Main.npcTexture.Length; npcType++ ) {
				int bannerType = Item.NPCtoBanner( npcType );
				if( bannerType == 0 ) { continue; }

				int bannerItemType = Item.BannerToItem( bannerType );
				if( bannerItemType >= Main.itemTexture.Length || bannerItemType <= 0 ) { continue; }

				try {
					Item item = new Item();
					item.SetDefaults( bannerItemType );
				} catch( Exception ) {
					LogLibraries.Log( "Could not find banner of item id " + bannerItemType + " for npc id " + npcType );
					continue;
				}

				npcTypesToBannerItemTypes[npcType] = bannerItemType;
			}

			return npcTypesToBannerItemTypes;
		}

		////////////////

		/// <summary>
		/// Gets all banner item types.
		/// </summary>
		/// <returns></returns>
		public static ReadOnlySet<int> GetBannerItemTypes() {
			var npcBannerLibs = ModContent.GetInstance<NPCBannerLibraries>();

			return new ReadOnlySet<int>( npcBannerLibs.BannerItemTypes );
		}

		/// <summary>
		/// Gets the banner item type of a given NPC type.
		/// </summary>
		/// <param name="npcType"></param>
		/// <returns></returns>
		public static int GetBannerItemTypeOfNpcType( int npcType ) {
			var npcBannerLibs = ModContent.GetInstance<NPCBannerLibraries>();

			if( !npcBannerLibs.NpcTypesToBannerItemTypes.ContainsKey(npcType) ) { return -1; }
			return npcBannerLibs.NpcTypesToBannerItemTypes[ npcType ];
		}

		/// <summary>
		/// Gets all NPC types of a given banner item type.
		/// </summary>
		/// <param name="itemType"></param>
		/// <returns></returns>
		public static ReadOnlySet<int> GetNpcTypesOfBannerItemType( int itemType ) {
			var npcBannerLibs = ModContent.GetInstance<NPCBannerLibraries>();

			if( !npcBannerLibs.BannerItemTypesToNpcTypes.ContainsKey( itemType ) ) { return new ReadOnlySet<int>( new HashSet<int>() ); }
			return new ReadOnlySet<int>( npcBannerLibs.BannerItemTypesToNpcTypes[ itemType ] );
		}
	}
}
