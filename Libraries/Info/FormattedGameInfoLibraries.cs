﻿using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.DotNET;
using ModLibsCore.Libraries.DotNET.Formatting;
using ModLibsGeneral.Libraries.Items;
using ModLibsGeneral.Libraries.NPCs;
using ModLibsGeneral.Libraries.World;
using System.Threading;
using Terraria.Enums;

namespace ModLibsGeneral.Libraries.Info {
	/// <summary>
	/// Assorted static library functions pertaining to game information formatted for output
	/// </summary>
	public partial class FormattedGameInfoLibraries {
		/// <summary>
		/// Gets a list of assorted game data statistics, formatted for (markdown) output.
		/// </summary>
		/// <param name="mods">Mods to display in this list. Typically only the set of loaded mods (ModLoader.Mods`).</param>
		/// <returns></returns>
		public static IList<string> GetFormattedGameInfo( IEnumerable<Mod> mods ) {
			var list = new List<string>();

			var modsList = mods.OrderBy( m => m.Name )
				.SafeSelect( m => StringFormattingLibraries.SanitizeMarkdown(m.DisplayName) + " " + m.Version.ToString() )
				.ToArray();
			bool isDay = Main.dayTime;
			double timeOfDay = Main.time;
			int halfDays = WorldStateLibraries.GetElapsedHalfDays();
			string worldSize = WorldLibraries.GetSize().ToString();
			string[] worldProg = GameInfoLibraries.GetVanillaProgressList().ToArray();
			int activeItems = ItemLibraries.GetActive().Count;
			int activeNpcs = NPCLibraries.GetActive().Count;
			//string[] playerInfos = InfoLibraries.GetCurrentPlayerInfo().ToArray();
			//string[] playerEquips = InfoLibraries.GetCurrentPlayerEquipment().ToArray();
			int activePlayers = Main.CurrentFrameFlags.ActivePlayersCount;
			string netmode = Main.netMode == NetmodeID.SinglePlayer ? "single-player" : "multiplayer";
			bool autopause = Main.autoPause;
			bool autosave = Main.autoSave;
			int lighting = Lighting.LegacyEngine.Mode;
			int numCores = Environment.ProcessorCount;
			FrameSkipMode frameSkipMode = Main.FrameSkipMode;
			bool isMaximized = Main.screenMaximized;
			int windowWid = Main.screenWidth;
			int windowHei = Main.screenHeight;
			int qualityStyle = Main.qaStyle;
			bool bgOn = Main.BackgroundEnabled;
			bool childSafe = !ChildSafety.Disabled;
			float gameZoom = Main.GameZoomTarget;
			float uiZoom = Main.UIScale;

			list.Add( ModLoader.versionedName );
			list.Add( FormattedGameInfoLibraries.RenderMarkdownModTable( modsList ) );
			list.Add( FormattedGameInfoLibraries.RenderMarkdownPlayersTable() );

			for( int i=0; i<Main.player.Length; i++ ) {
				Player plr = Main.player[i];
				if( plr == null || !plr.active ) { continue; }

				list.Add( FormattedGameInfoLibraries.RenderMarkdownPlayerEquipsTable(plr) );
			}

			list.Add( "Is day: " + isDay + ", Time of day/night: " + timeOfDay + ", Elapsed half days: " + halfDays );  //+ ", Total time (seconds): " + Main._drawInterfaceGameTime.TotalGameTime.Seconds;
			list.Add( "World name: " + StringFormattingLibraries.SanitizeMarkdown(Main.worldName) + ", world size: " + worldSize );
			list.Add( "World progress: " + (worldProg.Length > 0 ? string.Join(", ", worldProg) : "none") );
			list.Add( "Items on ground: " + activeItems + ", Npcs active: " + activeNpcs );
			//list.Add( "Player info: " + string.Join( ", ", playerInfos ) );
			//list.Add( "Player equips: " + (playerEquips.Length > 0 ? string.Join(", ", playerEquips) : "none" ) );
			list.Add( "Player count: " + activePlayers + " (" + netmode + ")" );
			list.Add( "Autopause: " + autopause );
			list.Add( "Autosave: " + autosave );
			list.Add( "Lighting mode: " + lighting );
			list.Add( "Processor cores: " + numCores );
			list.Add( "Frame skip mode: " + frameSkipMode );
			list.Add( "Is screen maximized: " + isMaximized );
			list.Add( "Screen resolution: " + windowWid + " " + windowHei );
			list.Add( "Quality style: " + qualityStyle );
			list.Add( "Background on: " + bgOn );
			list.Add( "Child safety: " + childSafe );
			list.Add( "Game zoom: " + gameZoom );
			list.Add( "UI zoom: " + uiZoom );
			list.Add( "FrameworkVersion.Framework: " + Enum.GetName(typeof(Framework), FrameworkVersion.Framework) );
			list.Add( "FrameworkVersion.Version: " + FrameworkVersion.Version.ToString() );

			return list;
		}
	}
}
