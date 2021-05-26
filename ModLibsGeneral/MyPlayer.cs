using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;
using ModLibsGeneral.Internals.NetProtocols;


namespace ModLibsGeneral {
	class ModLibsGeneralPlayer : ModPlayer {
		public override void SyncPlayer( int toWho, int fromWho, bool newPlayer ) {
			if( Main.netMode == NetmodeID.Server ) {
				if( toWho != -1 ) {
					SimplePacket.SendToClient( new WorldDataProtocol(), toWho, fromWho );
				}
			}
		}
	}
}