using System;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.TModLoader;
using ModLibsGeneral.Services.Messages.Player;
using ModLibsGeneral.Services.Messages.Simple;


namespace ModLibsGeneral {
	/// @private
	partial class ModLibsGeneralMod : Mod {
		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Mouse Text" ) );
			if( idx == -1 ) { return; }

			//

			GameInterfaceDrawMethod debugDrawCallback = () => {
				try {
					ModContent.GetInstance<PlayerMessages>().Draw( Main.spriteBatch );
				} catch( Exception e ) {
					LogLibraries.Warn( e.ToString() );
				}
				return true;
			};

			GameInterfaceDrawMethod simpMsgDrawCallback = () => {
				SimpleMessage.DrawMessage( Main.spriteBatch );
				return true;
			};

			//

			if( LoadLibraries.IsCurrentPlayerInGame() ) {
				var debugLayer = new LegacyGameInterfaceLayer( "ModLibsGeneral: Debug Display",
					debugDrawCallback,
					InterfaceScaleType.UI );
				layers.Insert( idx, debugLayer );

				var simpMsgLayer = new LegacyGameInterfaceLayer( "ModLibsGeneral: Simple Message",
					simpMsgDrawCallback,
					InterfaceScaleType.UI );
				layers.Insert( idx, simpMsgLayer );
			}
		}
	}
}
