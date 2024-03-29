﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using Terraria;
using Terraria.GameContent;

namespace ModLibsGeneral.Libraries.HUD {
	/// <summary>
	/// Assorted static library functions pertaining to the HUD health bar. 
	/// </summary>
	public class HUDHealthBarLibraries {
		/// <summary>
		/// Gets the screen coordinates of the last non-empty heart in the vanilla life bar.
		/// </summary>
		/// <param name="life">Life value of the life bar.</param>
		/// <param name="x">Horizontal screen coordinate.</param>
		/// <param name="y">Vertical screen coordinate.</param>
		public static void GetTopHeartPosition( int life, ref int x, ref int y ) {
			x = Main.screenWidth - 66;
			y = 59;

			int hp = life <= 400 ? life : ( life - 400 ) * 4;
			if( hp > 500 ) { hp = 500; }
			int hearts = hp / 20;

			if( hearts % 10 != 0 ) {
				x -= ( 10 - ( hearts % 10 ) ) * 26;
			}
			if( hearts <= 10 ) {
				y -= 27;
			}
		}


		////////////////

		/// <summary>
		/// Color of an entity's health bar with the given HP and max HP.
		/// </summary>
		/// <param name="hp"></param>
		/// <param name="maxHp"></param>
		/// <param name="alpha">Alpha value. Multiplies RGBA for multiply-based opacity.</param>
		/// <returns></returns>
		public static Color GetHealthBarColor( int hp, int maxHp, float alpha ) {
			if( hp <= 0 ) { return Color.Black; }

			float ratio = (float)hp / (float)maxHp;
			if( ratio > 1f ) { ratio = 1f; }
			ratio -= 0.1f;

			float r;
			float g;
			float b = 0f;
			float a = 255f;

			if( (double)ratio > 0.5 ) {
				r = 255f * (1f - ratio) * 2f;
				g = 255f;
			} else {
				r = 255f;
				g = 255f * ratio * 2f;
			}
			r = r * alpha * 0.95f;
			g = g * alpha * 0.95f;
			a = a * alpha * 0.95f;
			if( r < 0f ) { r = 0f; } else if( r > 255f ) { r = 255f; }
			if( g < 0f ) { g = 0f; } else if( g > 255f ) { g = 255f; }
			if( a < 0f ) { a = 0f; } else if( a > 255f ) { a = 255f; }

			return new Color( (int)((byte)r), (int)((byte)g), (int)((byte)b), (int)((byte)a) );
		}


		/// <summary>
		/// Returns dimensions of a string representing a health value.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static Vector2 MeasureHealthText( string text ) {
			return FontAssets.ItemStack.Value.MeasureString( text ) * 0.75f;
		}


		/// <summary>
		/// Draws a given health value at a given position and color.
		/// </summary>
		/// <param name="sb">The spriteBatch to draw to. Typically `Main.spriteBatch`.</param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="hp"></param>
		/// <param name="color"></param>
		public static void DrawHealthText( SpriteBatch sb, float x, float y, int hp, Color color ) {
			int offsetX = (int)((Math.Log10( (double)hp ) + 1d) * 3.3d);
			//byte c = (byte)MathHelper.Clamp( (alpha+0.2f) * 254f, 0f, 255f );

			Vector2 pos = new Vector2( x - offsetX, y - 4 );
			//Color color = new Color( c, c, c, c );

			sb.DrawString( FontAssets.ItemStack.Value, hp.ToString(), pos, Color.Black, 0f, new Vector2( -1f, -1f ), 0.85f, SpriteEffects.None, 1f );
			sb.DrawString( FontAssets.ItemStack.Value, hp.ToString(), pos, color, 0f, new Vector2( 0f, 0f ), 0.82f, SpriteEffects.None, 1f );
		}


		/// <summary>
		/// Draws a health bar at a given position, color, and scale.
		/// </summary>
		/// <param name="sb">The spriteBatch to draw to. Typically `Main.spriteBatch`.</param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="hp"></param>
		/// <param name="maxHp"></param>
		/// <param name="color"></param>
		/// <param name="scale"></param>
		public static void DrawHealthBar( SpriteBatch sb, float x, float y, int hp, int maxHp, Color color, float scale = 1f ) {
			if( hp <= 0 ) { return; }

			float ratio = (float)hp / (float)maxHp;
			if( ratio > 1f ) { ratio = 1f; }
			ratio -= 0.1f;

			int ratioLarge = (int)(36f * ratio);
			float offsetX = x - (18f * scale);
			float offsetY = y + 4;
			float depth = 1f;

			if( ratioLarge < 3 ) { ratioLarge = 3; }

			Vector2 pos;
			Rectangle? rect;

			if( ratioLarge < 34 ) {
				if( ratioLarge < 36 ) {
					pos = new Vector2( offsetX + (float)ratioLarge * scale, offsetY );
					rect = new Rectangle?( new Rectangle( 2, 0, 2, TextureAssets.Hb2.Value.Height ) );
					sb.Draw( TextureAssets.Hb2.Value, pos, rect, color, 0f, new Vector2( 0f, 0f ), scale, SpriteEffects.None, depth );
				}
				if( ratioLarge < 34 ) {
					pos = new Vector2( offsetX + (float)(ratioLarge + 2) * scale, offsetY );
					rect = new Rectangle?( new Rectangle( ratioLarge + 2, 0, 36 - ratioLarge - 2, TextureAssets.Hb2.Value.Height ) );
					sb.Draw( TextureAssets.Hb2.Value, pos, rect, color, 0f, new Vector2( 0f, 0f ), scale, SpriteEffects.None, depth );
				}
				if( ratioLarge > 2 ) {
					pos = new Vector2( offsetX, offsetY );
					rect = new Rectangle?( new Rectangle( 0, 0, ratioLarge - 2, TextureAssets.Hb1.Value.Height ) );
					sb.Draw( TextureAssets.Hb1.Value, pos, rect, color, 0f, new Vector2( 0f, 0f ), scale, SpriteEffects.None, depth );
				}

				pos = new Vector2( offsetX + (float)(ratioLarge - 2) * scale, offsetY );
				rect = new Rectangle?( new Rectangle( 32, 0, 2, TextureAssets.Hb1.Value.Height ) );
				sb.Draw( TextureAssets.Hb1.Value, pos, rect, color, 0f, new Vector2( 0f, 0f ), scale, SpriteEffects.None, depth );
				return;
			}

			if( ratioLarge < 36 ) {
				pos = new Vector2( offsetX + (float)ratioLarge * scale, offsetY );
				rect = new Rectangle?( new Rectangle( ratioLarge, 0, 36 - ratioLarge, TextureAssets.Hb2.Value.Height ) );
				sb.Draw( TextureAssets.Hb2.Value, pos, rect, color, 0f, new Vector2( 0f, 0f ), scale, SpriteEffects.None, depth );
			}

			pos = new Vector2( offsetX, offsetY );
			rect = new Rectangle?( new Rectangle( 0, 0, ratioLarge, TextureAssets.Hb1.Value.Height ) );
			sb.Draw( TextureAssets.Hb1.Value, pos, rect, color, 0f, new Vector2( 0f, 0f ), scale, SpriteEffects.None, depth );
		}
	}
}
