using System;
using Microsoft.Xna.Framework;
using Terraria;
using ModLibsCore.Libraries.Debug;


namespace ModLibsGeneral.Libraries.XNA {
	/// <summary>
	/// Assorted static library functions pertaining to XNA. 
	/// </summary>
	public partial class XnaLibraries {
		/// <summary>
		/// "Scans" a rectangle with a provided custom function, returning on the function reporting success. Skips over
		/// another given rectangle, if overlapping.
		/// </summary>
		/// <param name="scanner"></param>
		/// <param name="rect"></param>
		/// <param name="notRect"></param>
		public static void ScanRectangleWithout( Func<int, int, bool> scanner, Rectangle rect, Rectangle? notRect ) {
			int i, j;
			Rectangle nRect = notRect.HasValue ? notRect.Value : default(Rectangle);

			for( i = rect.X; i < (rect.X + rect.Width); i++ ) {
				for( j = rect.Y; j < (rect.Y + rect.Height); j++ ) {
					if( notRect.HasValue ) {
						if( i > nRect.X && i <= (nRect.X + nRect.Width) ) {
							if( j > nRect.Y && j <= (nRect.Y + nRect.Height) ) {
								j = nRect.Y + nRect.Height;

								if( j >= (rect.Y + rect.Height) ) {
									break;
								}
							}
						}
					}

					if( !scanner( i, j ) ) { return; }
				}
			}
		}
	}
}
