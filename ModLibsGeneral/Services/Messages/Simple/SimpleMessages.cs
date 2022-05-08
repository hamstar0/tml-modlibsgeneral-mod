using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.UI;


namespace ModLibsGeneral.Services.Messages.Simple {
	/// <summary>
	/// A simple way to quickly display text messages centered on-screen.
	/// </summary>
	public static class SimpleMessage {
		/// <summary>
		/// Tick duration of message.
		/// </summary>
		public static int MessageDuration = 0;

		/// <summary></summary>
		public static string Message = "";

		/// <summary>
		/// Smaller message under the bigger message.
		/// </summary>
		public static string SubMessage = "";


		/// <summary></summary>
		public static bool IsBordered = false;

		/// <summary></summary>
		public static Color Color = Color.White;

		
		/// <summary></summary>
		public static StyleDimension Left = new StyleDimension( 0f, 0.5f );

		/// <summary></summary>
		public static StyleDimension Top = new StyleDimension( 0f, 0.5f );



		////////////////
		
		/// <summary></summary>
		public static void ResetMessage() {
			SimpleMessage.MessageDuration = 0;
			SimpleMessage.Message = "";
			SimpleMessage.SubMessage = "";
			SimpleMessage.IsBordered = false;
			SimpleMessage.Color = Color.White;
			SimpleMessage.Left = new StyleDimension( 0f, 0.5f );
			SimpleMessage.Top = new StyleDimension( 0f, 0.5f );
		}


		////////////////

		/// <summary></summary>
		/// <param name="msg"></param>
		/// <param name="submsg"></param>
		/// <param name="duration">Tick duration.</param>
		public static void PostMessage( string msg, string submsg, int duration ) {
			SimpleMessage.ResetMessage();

			SimpleMessage.MessageDuration = duration;
			SimpleMessage.Message = msg;
			SimpleMessage.SubMessage = submsg;
			SimpleMessage.SubMessage = submsg;
		}

		/// <summary></summary>
		/// <param name="msg"></param>
		/// <param name="submsg"></param>
		/// <param name="duration">Tick duration.</param>
		/// <param name="isBordered"></param>
		/// <param name="color"></param>
		/// <param name="left"></param>
		/// <param name="top"></param>
		public static void PostMessage(
					string msg,
					string submsg,
					int duration,
					bool isBordered,
					Color color ) {
			SimpleMessage.ResetMessage();

			SimpleMessage.MessageDuration = duration;
			SimpleMessage.Message = msg;
			SimpleMessage.SubMessage = submsg;
			SimpleMessage.IsBordered = isBordered;
			SimpleMessage.Color = color;
		}

		/// <summary></summary>
		/// <param name="left"></param>
		/// <param name="top"></param>
		public static void SetMessagePosition( StyleDimension left, StyleDimension top ) {
			SimpleMessage.Left = left;
			SimpleMessage.Top = top;
		}


		////////////////

		internal static void UpdateMessage() { // Called from an Update function
			if( SimpleMessage.MessageDuration > 0 ) {
				SimpleMessage.MessageDuration--;
			}
		}

		internal static void DrawMessage( SpriteBatch sb ) { // Called from a Draw function
			if( SimpleMessage.MessageDuration <= 0 ) {
				return;
			}

			//

			string msg = SimpleMessage.Message;
			string submsg = SimpleMessage.SubMessage;

			var midPos = new Vector2(
				SimpleMessage.Left.GetValue( Main.screenWidth ),
				SimpleMessage.Top.GetValue( Main.screenHeight )
			);

			midPos.Y -= 56f;

			//

			Vector2 size = SimpleMessage.DrawMessageText(
				sb: sb,
				msg: msg,
				midPos: midPos,
				isBordered: SimpleMessage.IsBordered,
				color: SimpleMessage.Color,
				large: true
			);

			//

			if( submsg != "" ) {
				var subMidPos = midPos;
				subMidPos.Y += size.Y * 1.5f;

				SimpleMessage.DrawMessageText(
					sb: sb,
					msg: submsg,
					midPos: subMidPos,
					isBordered: SimpleMessage.IsBordered,
					color: SimpleMessage.Color * 0.8f,
					large: false
				);
			}
		}


		////////////////

		private static Vector2 DrawMessageText(
					SpriteBatch sb,
					string msg,
					Vector2 midPos,
					bool isBordered,
					Color color,
					bool large ) {
			if( msg == "" ) {
				return default;
			}

			//

			DynamicSpriteFont font = large ? Main.fontDeathText : Main.fontMouseText;

			Vector2 size = font.MeasureString( msg );

			Vector2 origin = size * 0.5f;

			//

			if( isBordered ) {
				Utils.DrawBorderStringFourWay(
					sb: sb,
					font: font,
					text: msg,
					x: midPos.X,
					y: midPos.Y,
					textColor: color,
					borderColor: Color.Black,
					origin: origin,
					scale: 1f
				);
			} else {
				sb.DrawString(
					spriteFont: font,
					text: msg,
					position: midPos,
					color: color,
					rotation: 0f,
					origin: origin,
					scale: 1f,
					effects: SpriteEffects.None,
					layerDepth: 1f
				);
			}

			return size;
		}
	}
}
