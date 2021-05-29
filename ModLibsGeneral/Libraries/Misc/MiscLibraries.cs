using System;
using Microsoft.Xna.Framework;


namespace ModLibsGeneral.Libraries.Misc {
	/// <summary>
	/// Assorted static library misc. functions.
	/// </summary>
	public partial class MiscLibraries {
		/// <summary>
		/// Renders a given game tick value as a string.
		/// </summary>
		/// <param name="ticks"></param>
		/// <returns></returns>
		public static string RenderTickDuration( int ticks ) {
			int seconds = ticks / 60;
			int minutes = seconds / 60;
			int hours = minutes / 60;

			if( ticks < 60 ) {
				return "<1 second";
			}

			seconds -= minutes * 60;
			minutes -= hours * 60;

			string output = seconds + " seconds";
			if( minutes > 0 ) {
				output = minutes + " minutes " + output;
			}
			if( hours > 0 ) {
				output = hours + " hours " + output;
			}

			return output;
		}

		/// <summary>
		/// Renders a given game tick value as a condensed string.
		/// </summary>
		/// <param name="ticks"></param>
		/// <returns></returns>
		public static string RenderCondensedTickDuration( int ticks ) {
			int seconds = ticks / 60;
			int minutes = seconds / 60;
			int hours = minutes / 60;

			if( ticks < 60 ) {
				return "<1s";
			}

			seconds -= minutes * 60;
			minutes -= hours * 60;

			string output = seconds + "s";
			if( minutes > 0 ) {
				output = minutes + "m " + output;
			}
			if( hours > 0 ) {
				output = hours + "h " + output;
			}

			return output;
		}


		////

		/// <summary>
		/// Renders a color as a hex code string.
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		public static string RenderColorHex( Color color ) {
			string r = ((int)color.R).ToString( "X2" );
			string g = ((int)color.G).ToString( "X2" );
			string b = ((int)color.B).ToString( "X2" );
			return r + g + b;
		}
	}
}
