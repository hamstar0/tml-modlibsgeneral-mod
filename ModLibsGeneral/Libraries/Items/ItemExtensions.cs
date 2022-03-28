using System;
using Terraria;


namespace ModLibsGeneral.Libraries.Items {
	/// <summary>
	/// Assorted helpful item extension methods.
	/// </summary>
	public static class ItemExtensions {
		/// <summary>
		/// A standardized way to check an item's existential nature. Avoids subtle debugging woes.
		/// </summary>
		/// <param name="self"></param>
		/// <param name="itemType"></param>
		/// <param name="minStack"></param>
		/// <param name="maxStack"></param>
		/// <returns></returns>
		public static bool Is(
					this Item self,
					int? itemType = null,
					int minStack = 1,
					int maxStack = int.MaxValue ) {
			if( !itemType.HasValue ) {
				return self.active && self.stack >= minStack && self.stack <= maxStack;
			} else {
				return self.active
					&& self.stack >= minStack
					&& self.stack <= maxStack
					&& self.type == itemType.Value;
			}
		}
	}
}
