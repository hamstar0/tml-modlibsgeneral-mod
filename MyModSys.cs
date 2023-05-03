using System;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.TModLoader;
using ModLibsGeneral.Libraries.World;
using ModLibsGeneral.Services.Messages.Simple;


namespace ModLibsGeneral {
	/// @private
	partial class ModLibsGeneralModSystem : ModSystem {
		public static bool MouseInterface { get; private set; }



		////////////////

		public override void PostUpdateEverything()/* tModPorter Note: Removed. Use ModSystem.PostUpdateEverything */ {
			MouseInterface = Main.LocalPlayer.mouseInterface;

			if( LoadLibraries.IsWorldBeingPlayed() ) {
				ModContent.GetInstance<WorldStateLibraries>().UpdateUponWorldBeingPlayed();

				SimpleMessage.UpdateMessage();
			}
		}
	}
}
