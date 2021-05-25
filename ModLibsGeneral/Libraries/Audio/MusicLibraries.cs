using System;
using Terraria.ModLoader;


namespace ModLibsGeneral.Libraries.Audio {
	/// <summary>
	/// Assorted static "helper" functions pertaining to game music.
	/// </summary>
	public partial class MusicLibraries {
		/// <summary>
		/// Adjusts the volume scale for the currently playing music.
		/// </summary>
		/// <param name="scale"></param>
		public static void SetVolumeScale( float scale ) {
			ModContent.GetInstance<MusicLibraries>().Scale = scale;
		}
	}
}
