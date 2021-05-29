using System;
using Terraria;


namespace ModLibsGeneral.Libraries.HUD {
	/// <summary>
	/// Assorted static library functions pertaining to general HUD. 
	/// </summary>
	public partial class HUDLibraries {
		/// <summary>
		/// Indicates if `Main.LocalPlayer.mouseInterface` was true at the end of the previous tick.
		/// </summary>
		public static bool IsMouseInterfacingWithUI => ModLibsGeneralMod.Instance.MouseInterface;
	}
}
