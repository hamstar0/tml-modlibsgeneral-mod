﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using ModLibsCore.Libraries.Debug;


namespace ModLibsGeneral.Libraries.XNA {
	/// <summary>
	/// Assorted static library functions pertaining to XNA. 
	/// </summary>
	public partial class XNALibraries {
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


		/// <summary>
		/// Applies a pre-multiply pass to the colors of a texture.
		/// 
		/// Credit: @Oli
		/// </summary>
		/// <param name="texture"></param>
		public static void PremultiplyTexture( Texture2D texture ) {
			Color[] buffer = new Color[texture.Width * texture.Height];

			texture.GetData( buffer );

			for( int i = 0; i < buffer.Length; i++ ) {
				buffer[i] = Color.FromNonPremultiplied( buffer[i].R, buffer[i].G, buffer[i].B, buffer[i].A );
			}

			texture.SetData( buffer );
		}
	}
}
