using System;
using System.Collections.Generic;
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

			float spaceWidth = font.MeasureString( " " ).X;
			string[] lines = text.Split( '\n' );

			foreach( string line in lines ) {
				dim = font.MeasureString( line );

				if( dim.X <= width ) {
					fittedLines.Add( line );
				} else {
					IList<string> subLines = FontLibraries._FitTextLine( font, line, width, spaceWidth );
					fittedLines.AddRange( subLines );
				}
			}

			return fittedLines;
		}

		private static IList<string> _FitTextLine( DynamicSpriteFont font, string line, int width, float spaceWidth ) {
			var fittedLines = new List<string>();

			string[] words = line.Split( ' ' );
			string newLine = "";
			string prevLine = "";

			bool firstWord = true;

			foreach( string word in words ) {
				newLine += word;

				if( string.IsNullOrEmpty(prevLine) ) {
					prevLine = newLine;
				}

				Vector2 dim = font.MeasureString( newLine );
				if( !firstWord ) {
					dim.X += spaceWidth;
				} else {
					firstWord = false;
				}

				if( dim.X <= width ) {
					prevLine = newLine;
				} else {
					fittedLines.Add( prevLine );

					prevLine = word;
					newLine = word;
				}
			}

			if( !string.IsNullOrEmpty(prevLine) ) {
				fittedLines.Add( prevLine );
			}

			return fittedLines;
		}
	}
}
