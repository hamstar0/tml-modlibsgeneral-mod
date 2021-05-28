using System.Collections.Generic;
using Terraria;
using ModLibsCore.Classes.Loadable;


namespace ModLibsGeneral.Libraries.Items.Attributes {
	/// <summary>
	/// Assorted static "helper" functions pertaining to gameplay attributes of items.
	/// </summary>
	public partial class ItemAttributeLibraries : ILoadable {
		private IDictionary<long, ISet<int>> PurchasableItems = new Dictionary<long, ISet<int>>();



		////////////////

		internal ItemAttributeLibraries() { }

		/// @private
		void ILoadable.OnModsLoad() { }

		/// @private
		void ILoadable.OnModsUnload() { }

		/// @private
		void ILoadable.OnPostModsLoad() { }
	}
}
