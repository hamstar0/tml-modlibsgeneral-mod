﻿using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ModLibsCore.Classes.DataStructures;


namespace ModLibsGeneral.Libraries.Buffs {
	/// <summary>
	/// Assorted static library functions pertaining to buff attributes.
	/// </summary>
	public partial class BuffAttributesLibraries {
		/// <summary>
		/// A map of buff names to their Terraria IDs.
		/// </summary>
		public static ReadOnlyDictionaryOfSets<string, int> DisplayNamesToIds {
			get { return ModContent.GetInstance<BuffAttributesLibraries>()._NamesToIds; }
		}



		////////////////

		/// <summary>
		/// Alias for `Lang.GetBuffName(int)`.
		/// 
		/// Credit: Jofairden @ Even More Modifiers
		/// </summary>
		/// <param name="buffType"></param>
		/// <returns></returns>
		public static string GetBuffDisplayName( int buffType ) {
			if( buffType >= BuffID.Count ) {
				return BuffLoader.GetBuff( buffType )?
					.DisplayName
					.GetTranslation( LanguageManager.Instance.ActiveCulture )
					?? "null";
			}

			return Lang.GetBuffName( buffType );
		}
	}
}
