﻿using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.DotNET;
using ModLibsCore.Libraries.DotNET.Formatting;
using System;
using System.Collections.Generic;
using Terraria;


namespace ModLibsGeneral.Libraries.Info {
	/// <summary>
	/// Assorted static library functions pertaining to game information formatted for output.
	/// </summary>
	public partial class FormattedGameInfoLibraries {
		/// <summary>
		/// Renders a list of mod names into a markdown table.
		/// </summary>
		/// <param name="mods"></param>
		/// <returns></returns>
		public static string RenderMarkdownModTable( string[] mods ) {
			int len = mods.Length;

			string output = "| Mods:  | - | - |";
			output += "\n| :--- | :--- | :--- |";

			for( int i = 0; i < len; i++ ) {
				output += '\n';
				output += "| `" + mods[i] + "` | ";
				output += (++i < len ? "`"+mods[i]+"`" : "-") + " | ";
				output += (++i < len ? "`"+mods[i]+"`" : "-") + " |";
			}

			return output;
		}


		////

		/// <summary>
		/// Renders a list of players into a markdown table.
		/// </summary>
		/// <returns></returns>
		public static string RenderMarkdownPlayersTable() {
			IDictionary<string, string> playerInfos = null;
			string columns = "";
			int cols = 0;

			for( int i=0; i<Main.player.Length; i++ ) {
				Player plr = Main.player[i];
				if( plr == null || !plr.active ) { continue; }

				playerInfos = GameInfoLibraries.GetPlayerInfo(plr);
				cols = playerInfos.Count > cols ? playerInfos.Count : cols;

				playerInfos["Name"] = StringFormattingLibraries.SanitizeMarkdown( playerInfos["Name"] );
				
				columns += "| " + string.Join(" | ", playerInfos.Values) + " |";
			}

			string header = "| " + string.Join(" | ", playerInfos.Keys) + " |";

			string subheader = "|";
			for( int i = 0; i < cols; i++ ) {
				subheader += " :--- |";
			}

			return header+"\n"+subheader+"\n"+columns;
		}


		/// <summary>
		/// Renders a list of players with their body equips into a markdown table.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static string RenderMarkdownPlayerEquipsTable( Player player ) {
			IDictionary<string, string> playerEquips = GameInfoLibraries.GetPlayerEquipment( player );
			int cols = playerEquips.Count;

			string playerLabel = "**Player "+ StringFormattingLibraries.SanitizeMarkdown(player.name)+"'s ("+player.whoAmI+") equipment:**";

			string equipsLabels = cols > 0 ? string.Join( " | ", playerEquips.Keys ) : "-";
			string header = "| " + equipsLabels + " |";

			string subheader = "|";
			if( cols > 0 ) {
				for( int i = 0; i < cols; i++ ) {
					subheader += " :--- |";
				}
			} else {
				subheader += " :--- |";
			}

			string equips = string.Join( " | ", playerEquips.Values.SafeSelect(e=> StringFormattingLibraries.SanitizeMarkdown(e)) );
			string equipsCols = cols > 0 ? equips : "-";
			string columns = "| " + equipsCols + " |";

			return playerLabel + "\n \n" + header + "\n" + subheader + "\n" + columns;
		}
	}
}
