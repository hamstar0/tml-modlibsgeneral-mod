﻿using System;
using Terraria;
using Terraria.ModLoader;

namespace ModLibsGeneral.Libraries.World {
	/// <summary>
	/// Assorted static library functions pertaining to the current world's state.
	/// </summary>
	public partial class WorldStateLibraries {
		/// <summary>
		/// Tick duration of a daytime period.
		/// </summary>
		public readonly static int VanillaDayDuration = 54000;
		/// <summary>
		/// Tick duration of a nighttime period.
		/// </summary>
		public readonly static int VanillaNightDuration = 32400;



		////////////////

		/// <summary>
		/// Indicates if an invasion is in session.
		/// </summary>
		/// <returns></returns>
		public static bool IsBeingInvaded() {
			return Main.invasionType > 0 && Main.invasionDelay == 0 && Main.invasionSize > 0;
		}


		////////////////

		/// <summary>
		/// Returns elapsed time having played the current world (current session).
		/// </summary>
		/// <returns></returns>
		public static int GetElapsedPlayTime() {
			return (int)(ModContent.GetInstance<WorldStateLibraries>().TicksElapsed / 60);
		}

		/// <summary>
		/// Returns elapsed "half" days (day and night cycles; not actual halfway points of full day cycles).
		/// </summary>
		/// <returns></returns>
		public static int GetElapsedHalfDays() {
			return ModContent.GetInstance<WorldStateLibraries>().HalfDaysElapsed;
		}

		/// <summary>
		/// Returns percent of day or night completed.
		/// </summary>
		/// <returns></returns>
		public static double GetDayOrNightPercentDone() {
			if( Main.dayTime ) {
				return Main.time / (double)WorldStateLibraries.VanillaDayDuration;
			} else {
				return Main.time / (double)WorldStateLibraries.VanillaNightDuration;
			}
		}
	}
}
