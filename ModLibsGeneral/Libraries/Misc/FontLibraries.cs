using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Terraria;


namespace ModLibsGeneral.Libraries.Misc {
	/// <summary>
	/// Assorted static library font functions.
	/// </summary>
	public partial class FontLibraries {
		/// <summary>
		/// Fits all lines of a string of text to a given width, subdividing lines as needed.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="width"></param>
		/// <returns>List of lines. Use `list.ToStringJoined("\n");` to retrieve results.</returns>
		public static IList<string> FitText( DynamicSpriteFont font, string text, int width ) {
			Vector2 dim = font.MeasureString( text );
			if( dim.X <= width ) {
				return new List<string> { text };
			}

			var fittedLines = new List<string>();

			string[] lines = text.Split( '\n' );

			foreach( string line in lines ) {
				dim = font.MeasureString( line );

				if( dim.X <= width ) {
					fittedLines.Add( line );
				} else {
					IList<string> subLines = FontLibraries._FitTextLine( font, line, width );
					fittedLines.AddRange( subLines );
				}
			}

			return fittedLines;
		}

		private static IList<string> _FitTextLine( DynamicSpriteFont font, string line, int width ) {
			var fittedLines = new List<string>();

			string[] words = line.Split( ' ' );
			string ahead = "";
			string before = words.FirstOrDefault() ?? "";

			foreach( string word in words ) {
				ahead += word;

				Vector2 dim = font.MeasureString( ahead );
				if( dim.X <= width ) {
					before = ahead;
				} else {
					fittedLines.Add( before );

					before = word;
					ahead = word;
				}

				ahead += " ";
			}

			if( !string.IsNullOrEmpty(before) ) {
				fittedLines.Add( before );
			}

			return fittedLines;
		}
	}
}
