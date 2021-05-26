using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;
using ModLibsGeneral.Libraries.World;


namespace ModLibsGeneral.Internals.NetProtocols {
	[Serializable]
	class WorldDataProtocol : SimplePacketPayload { //NetIOClientPayload {
		public int HalfDays;
		public bool HasObsoletedWorldId;
		public string ObsoletedWorldId;



		////////////////

		public WorldDataProtocol() {
			this.HalfDays = WorldStateLibraries.GetElapsedHalfDays();
		}


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			throw new NotImplementedException();
		}

		public override void ReceiveOnClient() {
			ModContent.GetInstance<WorldStateLibraries>().HalfDaysElapsed = this.HalfDays;
		}
	}
}