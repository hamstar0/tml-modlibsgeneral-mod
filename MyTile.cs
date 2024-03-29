﻿using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.TModLoader;
using ModLibsGeneral.Services.Hooks.ExtendedHooks;


namespace ModLibsGeneral {
	class ModLibsGeneralTile : GlobalTile {
		public override void KillTile( int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem ) {
//ModContent.GetInstance<ModLibsGeneralMod>().Logger.Info( "KillTile1 "+type+" ("+i+","+j+") - "
//	+"fail:"+fail+", effectOnly:"+effectOnly+", noItem:"+noItem);
			var eth = TmlLibraries.SafelyGetInstance<ExtendedTileHooks>();

			if( (!Main.gameMenu && Main.netMode != NetmodeID.Server) || Main.netMode == NetmodeID.Server ) {
				eth.CallKillTileHooks( i, j, type, ref fail, ref effectOnly, ref noItem );
				eth.CallKillMultiTileHooks( i, j, type );
			}
			// why was CallKillMultiTileHooks here instead, previously?
		}
	}



	class ModLibsGeneralWall : GlobalWall {
		public override void KillWall( int i, int j, int type, ref bool fail ) {
			if( (!Main.gameMenu && Main.netMode == NetmodeID.Server) || Main.netMode == NetmodeID.Server ) {
				var eth = TmlLibraries.SafelyGetInstance<ExtendedTileHooks>();
				eth.CallKillWallHooks( i, j, type, ref fail );
			}
		}
	}
}
