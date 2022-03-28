using System;
using Terraria;


namespace ModLibsGeneral.Libraries.Items {
	/// <summary>
	/// Assorted helpful item extension methods.
	/// </summary>
	public static class ItemExtensions {
		/// <summary>
		/// Self explanatory.
		/// </summary>
		/// <param name="self"></param>
		/// <param name="itemType"></param>
		/// <param name="minStack"></param>
		/// <param name="maxStack"></param>
		/// <returns></returns>
		public static bool Is( this Item self, int itemType = -1, int minStack = 1, int maxStack = int.MaxValue ) {
			if( itemType == -1 ) {
				return self.active && self.stack >= minStack && self.stack <= maxStack;
			} else {
				return self.active && self.stack >= minStack && self.stack <= maxStack && self.type == itemType;
			}
		}
	}
}
