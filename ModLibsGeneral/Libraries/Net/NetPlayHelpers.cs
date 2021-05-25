using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;


namespace ModLibsGeneral.Libraries.Net {
	/// <summary>
	/// Assorted static "helper" functions pertaining to network play.
	/// </summary>
	public partial class NetPlayLibraries {
		/// <summary>
		/// Connects the current machine to a server to begin a game. Meant to be called from the main menu.
		/// </summary>
		/// <param name="ip"></param>
		/// <param name="port"></param>
		public static void JoinServer( string ip, int port ) {
			Main.autoPass = false;
			Netplay.ListenPort = port;
			Main.getIP = ip;
			Main.defaultIP = ip;
			if( Netplay.SetRemoteIP( ip ) ) {
				Main.menuMode = 10;
				Netplay.StartTcpClient();
			}
		}
	}
}
