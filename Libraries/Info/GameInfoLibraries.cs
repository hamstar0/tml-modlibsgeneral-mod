﻿using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.NPCs;
using ModLibsGeneral.Libraries.Players;


namespace ModLibsGeneral.Libraries.Info {
	/// <summary>
	/// Assorted static library functions pertaining to game information.
	/// </summary>
	public partial class GameInfoLibraries {
		/// <summary>
		/// Gets the current world's (vanilla) progress stage in human-readable form.
		/// </summary>
		/// <returns></returns>
		public static string GetVanillaProgress() {
			if( NPC.downedMoonlord ) {
				return "Post Moon Lord";
			}
			if( NPC.LunarApocalypseIsUp ) {
				return "Lunar apocalypse";
			}
			if( NPC.downedGolemBoss ) {
				return "Post Golem";
			}
			if( NPC.downedPlantBoss ) {
				return "Post Plantera";
			}
			if( NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 ) {
				return "Post Mech Bosses";
			}
			if( Main.hardMode ) {
				return "Hard mode";
			}
			if( NPC.downedBoss2 ) {
				if( WorldGen.crimson ) {
					return "Post Brain of Cthulhu";
				} else {
					return "Post Eater of Worlds";
				}
			}
			if( NPC.downedBoss3 ) {
				return "Post Skeletron";
			}
			if( NPC.downedQueenBee ) {
				return "Post Queen Bee";
			}
			if( NPC.downedBoss1 || NPC.downedSlimeKing ) {
				return "Boss killing begun";
			}
			if( NPC.downedGoblins ) {
				return "Goblins killed";
			}
			return "Start of game";
		}
		
		/// <summary>
		/// Gets a list of all (vanilla) bosses killed in a human-readable form.
		/// </summary>
		/// <returns></returns>
		public static IList<string> GetVanillaProgressList() {
			var list = new List<string>();

			if( NPC.downedBoss1 ) { list.Add( "Eye of Cthulhu killed" ); }
			if( NPC.downedSlimeKing ) { list.Add( "King Slime killed" ); }
			if( NPC.downedQueenBee ) { list.Add( "Queen Bee killed" ); }
			if( NPC.downedBoss2 && !WorldGen.crimson ) { list.Add( "Eater of Worlds killed" ); }
			if( NPC.downedBoss2 && WorldGen.crimson ) { list.Add( "Brain of Cthulhu killed" ); }
			if( NPC.downedBoss3 ) { list.Add( "Skeletron killed" ); }
			if( Main.hardMode ) { list.Add( "Wall of Flesh killed" ); }
			if( NPC.downedMechBoss1 ) { list.Add( "Destroyer killed" ); }
			if( NPC.downedMechBoss2 ) { list.Add( "Twins killed" ); }
			if( NPC.downedMechBoss3 ) { list.Add( "Skeletron Prime killed" ); }
			if( NPC.downedPlantBoss ) { list.Add( "Plantera killed" ); }
			if( NPC.downedGolemBoss ) { list.Add( "Golem killed" ); }
			if( NPC.downedFishron ) { list.Add( "Duke Fishron killed" ); }
			if( NPC.downedAncientCultist ) { list.Add( "Ancient Cultist killed" ); }
			if( NPC.downedMoonlord ) { list.Add( "Moon Lord killed" ); }

			return list;
		}


		/// <summary>
		/// Gets a list of currently active (vanilla) world events.
		/// </summary>
		/// <returns></returns>
		public static IList<string> GetCurrentVanillaEvents() {
			VanillaEventFlag flags = NPCInvasionLibraries.GetCurrentEventTypeSet();

			IList<string> outList = new List<string>();

			if( ( flags & VanillaEventFlag.Goblins ) > 0 ) {
				flags -= VanillaEventFlag.Goblins;
				outList.Add( "Goblins" );
			}
			if( ( flags & VanillaEventFlag.FrostLegion ) > 0 ) {
				flags -= VanillaEventFlag.FrostLegion;
				outList.Add( "Frost Legion" );
			}
			if( ( flags & VanillaEventFlag.Pirates ) > 0 ) {
				flags -= VanillaEventFlag.Pirates;
				outList.Add( "Pirates" );
			}
			if( ( flags & VanillaEventFlag.Martians ) > 0 ) {
				flags -= VanillaEventFlag.Martians;
				outList.Add( "Martians" );
			}
			if( ( flags & VanillaEventFlag.BloodMoon ) > 0 ) {
				flags -= VanillaEventFlag.BloodMoon;
				outList.Add( "Blood Moon" );
			}
			if( ( flags & VanillaEventFlag.SlimeRain ) > 0 ) {
				flags -= VanillaEventFlag.SlimeRain;
				outList.Add( "Slime Rain" );
			}
			if( ( flags & VanillaEventFlag.Sandstorm ) > 0 ) {
				flags -= VanillaEventFlag.Sandstorm;
				outList.Add( "Sandstorm" );
			}
			if( ( flags & VanillaEventFlag.SolarEclipse ) > 0 ) {
				flags -= VanillaEventFlag.SolarEclipse;
				outList.Add( "Solar Eclipse" );
			}
			if( ( flags & VanillaEventFlag.PumpkinMoon ) > 0 ) {
				flags -= VanillaEventFlag.PumpkinMoon;
				outList.Add( "Pumpkin Moon" );
			}
			if( ( flags & VanillaEventFlag.FrostMoon ) > 0 ) {
				flags -= VanillaEventFlag.FrostMoon;
				outList.Add( "Frost Moon" );
			}
			if( ( flags & VanillaEventFlag.LunarApocalypse ) > 0 ) {
				flags -= VanillaEventFlag.LunarApocalypse;
				outList.Add( "Lunar Apocalypse" );
			}

			if( outList.Count == 0 ) {
				outList.Add( "Normal" );
			}

			return outList;
		}


		////

		/// <summary>
		/// Gets a formatted collection of basic player statistics.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static IDictionary<string, string> GetPlayerInfo( Player player ) {
			var dict = new Dictionary<string, string> {
				{ "Name", player.name },
				{ "Male", ""+player.Male },
				{ "Has Demon Heart", ""+player.extraAccessory },
				{ "Difficulty mode", ""+player.difficulty },
				{ "Life", player.statLife + " of " + player.statLifeMax2 },
				{ "Mana", player.statMana + " of " + player.statManaMax2 },
				{ "Defense", ""+player.statDefense }
			};

			if(player.statLifeMax != player.statLifeMax2 ) {
				dict["Life"] += " (" + player.statLifeMax + ")";
			}
			if(player.statManaMax != player.statManaMax2 ) {
				dict["Mana"] += " (" + player.statManaMax + ")";
			}
			
			return dict;
		}


		/// <summary>
		/// Gets a formatted collection of a player's body equipment.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static IDictionary<string, string> GetPlayerEquipment( Player player ) {
			var dict = new Dictionary<string, string>();
			int acc = 1;
			int van = 1;
			int unk = 1;

			for( int i = 0; i < player.armor.Length; i++ ) {
				string key;
				Item item = player.armor[i];
				if( item == null || item.IsAir ) { continue; }

				if( i == 0 ) {
					key = "Head";
				} else if( i == 1 ) {
					key = "Body";
				} else if( i == 2 ) {
					key = "Legs";
				} else if( PlayerItemLibraries.IsAccessorySlot( player, i ) ) {
					key = "Accessory "+acc;
					acc++;
				} else if( PlayerItemLibraries.IsVanitySlot( player, i ) ) {
					key = "Vanity "+van;
					van++;
				} else {
					key = "? "+unk;
					unk++;
				}

				dict[ key ] = item.HoverName;
			}

			return dict;
		}


		////////////////

		/// <summary>
		/// Gets the most recent error logs into a list.
		/// </summary>
		/// <param name="maxLines">Highest amount of lines of the log to return.</param>
		/// <returns></returns>
		public static IList<string> GetErrorLog( int maxLines ) {
			IList<string> lines = new List<string>();
			char sep = Path.DirectorySeparatorChar;
			string path = Path.Combine( Logging.LogDir, "client.log" );	//Main.SavePath + sep + "Logs" + sep + "client.log";

			if( !File.Exists( path ) ) {
				return new List<string> { "No error logs available." };
			}

			//using( var reader = new StreamReader( path ) ) {
			using( Stream stream = File.Open( path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite ) ) {
				using( var reader = new StreamReader( stream ) ) {
					int size = 1024;
					bool eof = false;

					do {
						lines = new List<string>( maxLines + 25 );

						if( reader.BaseStream.Length > size ) {
							reader.BaseStream.Seek( -size, SeekOrigin.End );
							size += 1024;
						} else {
							eof = true;
							reader.BaseStream.Seek( 0, SeekOrigin.Begin );
						}

						string line;
						while( (line = reader.ReadLine()) != null ) {
							lines.Add( line );
						}
					} while( lines.Count < maxLines && !eof );
				}
			}

			var fmtLines = new List<string>();

			if( lines.Count > maxLines ) {
				fmtLines.Add( "..." );
			}

			int maxCountedLines = Math.Min( maxLines, lines.Count );
			for( int i=0; i<maxCountedLines; i++ ) {
				if( i >= lines.Count ) {
					break;
				}

				string line = lines[ (lines.Count - maxCountedLines) + i ];
				fmtLines.Add( line.Trim() );
			}

			return fmtLines;
		}
	}
}
