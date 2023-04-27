using System;
using Terraria.ModLoader;


namespace ModLibsGeneral.Services.Hooks.WorldHooks {
	/// <summary>
	/// Supplies custom tModLoader-like, delegate-based hooks for world-relevant functions not currently available in
	/// tModLoader.
	/// </summary>
	public partial class WorldTimeHooks {
		/// <summary>
		/// Declares an action to run when day begins.
		/// </summary>
		/// <param name="hookName"></param>
		/// <param name="callback"></param>
		/// <returns>`false` if the given hook has already been claimed.</returns>
		public static bool AddDayHook( string hookName, Action callback ) {
			var wtHooks = ModContent.GetInstance<WorldTimeHooks>();

			if( wtHooks.DayHooks.ContainsKey(hookName) ) {
				return false;
			}

			wtHooks.DayHooks[hookName] = callback;

			return true;
		}

		/// <summary>
		/// Declares an action to run when day begins.
		/// </summary>
		/// <param name="hookName"></param>
		/// <param name="callback"></param>
		/// <returns>`false` if the given hook has already been claimed.</returns>
		public static bool AddNightHook( string hookName, Action callback ) {
			var wtHooks = ModContent.GetInstance<WorldTimeHooks>();

			if( wtHooks.NightHooks.ContainsKey( hookName ) ) {
				return false;
			}

			wtHooks.NightHooks[hookName] = callback;

			return true;
		}
	}
}
