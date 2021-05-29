using System;


namespace ModLibsGeneral.Libraries.Steam {
	/// <summary>
	/// Assorted static library functions pertaining to the Steam platform.
	/// </summary>
	public class SteamLibraries {
		/// <summary>
		/// Attempts to get the current Steam user's Steam ID.
		/// </summary>
		/// <returns></returns>
		public static string GetSteamID() {
			return Steamworks.SteamUser.GetSteamID().ToString();
		}
	}
}
