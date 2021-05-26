using System;
using Terraria;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Services.Network.SimplePacket;


namespace ModLibsGeneral.Internals.NetProtocols {
	[Serializable]
	class ItemNoGrabProtocol : SimplePacketPayload {
		public static void SendToServer( int itemWho, int noGrabDelayAmt ) {
			var protocol = new ItemNoGrabProtocol( itemWho, noGrabDelayAmt );
			SimplePacket.SendToServer( protocol );
		}



		////////////////

		public int ItemWho;
		public int NoGrabDelayAmt;



		////////////////

		public ItemNoGrabProtocol() { }

		private ItemNoGrabProtocol( int itemWho, int noGrabDelayAmt ) {
			this.ItemWho = itemWho;
			this.NoGrabDelayAmt = noGrabDelayAmt;
		}


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			Item item = Main.item[this.ItemWho];
			if( item == null /*|| !item.active*/ ) {
				//throw new ModLibsException( "!ModLibraries.ItemNoGrabProtocol.ReceiveWithServer - Invalid item indexed at "+this.ItemWho );
				throw new ModLibsException( "Invalid item indexed at " + this.ItemWho );
			}

			item.noGrabDelay = this.NoGrabDelayAmt;
			item.ownIgnore = fromWho;
			item.ownTime = this.NoGrabDelayAmt;
		}

		public override void ReceiveOnClient() {
			throw new ModLibsException( "Not implemented" );
		}
	}
}