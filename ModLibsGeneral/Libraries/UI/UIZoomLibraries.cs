using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace ModLibsGeneral.Libraries.UI {
	/// <summary>
	/// Assorted static library functions pertaining to the in-game UI zoom and positions.
	/// </summary>
	public class UIZoomLibraries {
		/// <summary>
		/// Applies the given zoom parameters to the given float.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="uiZoomState">If `true`, assumes zoom is applied, and removes it (`/= Main.GameZoomTarget`).
		/// `false` assumes it has been removed, and applies it (`*= Main.GameZoomTarget`).</param>
		/// <param name="gameZoomState">If `true`, assumes zoom is applied, and removes it (`/= Main.UIScale`).
		/// `false` assumes it has been removed, and applies it (`*= Main.UIScale`).</param>
		/// <returns></returns>
		public static float ApplyZoom( float value, bool? uiZoomState, bool? gameZoomState ) {
			if( uiZoomState.HasValue ) {
				if( uiZoomState.Value ) {
					value /= Main.UIScale;
				} else {
					value *= Main.UIScale;
				}
			}
			if( gameZoomState.HasValue ) {
				if( gameZoomState.Value ) {
					value /= Main.GameZoomTarget;
				} else {
					value *= Main.GameZoomTarget;
				}
			}

			return value;
		}

		/// <summary>
		/// Applies the given zoom parameters to the given Vector2.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="uiZoomState">If `true`, assumes zoom is applied, and removes it (`/= Main.GameZoomTarget`).
		/// `false` assumes it has been removed, and applies it (`*= Main.GameZoomTarget`).</param>
		/// <param name="gameZoomState">If `true`, assumes zoom is applied, and removes it (`/= Main.UIScale`).
		/// `false` assumes it has been removed, and applies it (`*= Main.UIScale`).</param>
		/// <returns></returns>
		public static Vector2 ApplyZoom(
					Vector2 value,
					bool? uiZoomState,
					bool? gameZoomState ) {
			if( uiZoomState.HasValue ) {
				if( uiZoomState.Value ) {
					value /= Main.UIScale;
				} else {
					value *= Main.UIScale;
				}
			}
			if( gameZoomState.HasValue ) {
				if( gameZoomState.Value ) {
					value /= Main.GameZoomTarget;
				} else {
					value *= Main.GameZoomTarget;
				}
			}

			return value;
		}

		////

		/// <summary>
		/// Applies the given zoom parameters to the given Vector2.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="uiZoomState">If `true`, assumes zoom is applied, and removes it (`/= Main.GameZoomTarget`).
		/// `false` assumes it has been removed, and applies it (`*= Main.GameZoomTarget`).</param>
		/// <param name="gameZoomState">If `true`, assumes zoom is applied, and removes it (`/= Main.UIScale`).
		/// `false` assumes it has been removed, and applies it (`*= Main.UIScale`).</param>
		/// <param name="uiZoomStateForCenterOffset"></param>
		/// <param name="gameZoomStateForCenterOffset"></param>
		/// <returns></returns>
		public static Vector2 ApplyZoomFromScreenCenter(
					Vector2 value,
					bool? uiZoomState,
					bool? gameZoomState,
					bool? uiZoomStateForCenterOffset,
					bool? gameZoomStateForCenterOffset ) {
			var scrMid = new Vector2( Main.screenWidth, Main.screenHeight ) * 0.5f;
			var scrMidZoomed = UIZoomLibraries.ApplyZoom(
				scrMid,
				uiZoomStateForCenterOffset,
				gameZoomStateForCenterOffset
			);

			//

			Vector2 offsetOfValueOnCenterZoomedScreen = value - scrMidZoomed;

			Vector2 offsetOfValueOnZoomedScreen = UIZoomLibraries.ApplyZoom(
				offsetOfValueOnCenterZoomedScreen,
				uiZoomState,
				gameZoomState
			);

			return offsetOfValueOnZoomedScreen + scrMid;
		}


		////////////////

		/// <summary>
		/// Gets the current screen size according to the given scales.
		/// </summary>
		/// <param name="uiZoomState">If `true`, assumes zoom is applied, and removes it (`/= Main.GameZoomTarget`).
		/// `false` assumes it has been removed, and applies it (`*= Main.GameZoomTarget`).</param>
		/// <param name="gameZoomState">If `true`, assumes zoom is applied, and removes it (`/= Main.UIScale`).
		/// `false` assumes it has been removed, and applies it (`*= Main.UIScale`).</param>
		/// <returns></returns>
		public static (float Width, float Height) GetScreenSize( bool? uiZoomState, bool? gameZoomState ) {
			float width = UIZoomLibraries.ApplyZoom( Main.screenWidth, uiZoomState, gameZoomState );
			float height = UIZoomLibraries.ApplyZoom( Main.screenHeight, uiZoomState, gameZoomState );

			return (width, height);
		}

		/// <summary>
		/// Gets the world position and range of the screen, according to the given zoom parameters.
		/// </summary>
		/// <param name="uiZoomState">If `true`, assumes zoom is applied, and removes it (`/= Main.GameZoomTarget`).
		/// `false` assumes it has been removed, and applies it (`*= Main.GameZoomTarget`).</param>
		/// <param name="gameZoomState">If `true`, assumes zoom is applied, and removes it (`/= Main.UIScale`).
		/// `false` assumes it has been removed, and applies it (`*= Main.UIScale`).</param>
		/// <returns></returns>
		public static Rectangle GetWorldFrameOfScreen( bool? uiZoomState, bool? gameZoomState ) {
			float width = UIZoomLibraries.ApplyZoom( Main.screenWidth, uiZoomState, gameZoomState );
			float height = UIZoomLibraries.ApplyZoom( Main.screenHeight, uiZoomState, gameZoomState );
			
			float xCenterOffset = ((float)Main.screenWidth - width) * 0.5f;
			float yCenterOffset = ((float)Main.screenHeight - height) * 0.5f;

			int scrX = (int)Main.screenPosition.X + (int)xCenterOffset;
			int scrY = (int)Main.screenPosition.Y + (int)yCenterOffset;

			return new Rectangle( scrX, scrY, (int)width, (int)height );
		}


		////////////////

		/// <summary>
		/// Gets the screen-space coordinates of the given world coordinates.
		/// </summary>
		/// <param name="worldCoords"></param>
		/// <param name="uiZoomState">If `true`, assumes zoom is applied, and removes it (`/= Main.GameZoomTarget`).
		/// `false` assumes it has been removed, and applies it (`*= Main.GameZoomTarget`).</param>
		/// <param name="gameZoomState">If `true`, assumes zoom is applied, and removes it (`/= Main.UIScale`).
		/// `false` assumes it has been removed, and applies it (`*= Main.UIScale`).</param>
		/// <returns></returns>
		public static Vector2 ConvertToScreenPosition( Vector2 worldCoords, bool? uiZoomState, bool? gameZoomState ) {
			var wldScrFrame = UIZoomLibraries.GetWorldFrameOfScreen( uiZoomState, gameZoomState );
			var wldScrPos = new Vector2( wldScrFrame.X, wldScrFrame.Y );

			return worldCoords - wldScrPos;
		}
	}
}
