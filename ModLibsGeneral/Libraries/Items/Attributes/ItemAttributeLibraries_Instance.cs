using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace ModLibsGeneral.Libraries.Items.Attributes {
	/// <summary>
	/// Assorted static library functions pertaining to gameplay attributes of items.
	/// </summary>
	public partial class ItemAttributeLibraries : ILoadable {
		private IDictionary<long, ISet<int>> PurchasableItems = new Dictionary<long, ISet<int>>();



		////////////////

		internal ItemAttributeLibraries() { }

		/// @private
		void ILoadable.Load( Mod mod ) { }

		/// @private
		void ILoadable.Unload() { }
	}
}
