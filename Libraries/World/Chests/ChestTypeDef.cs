using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.DotNET.Extensions;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.Tiles;


namespace ModLibsGeneral.Libraries.World.Chests {
	/// <summary></summary>
	public struct ChestTypeDefinition {
		/// <summary>See `TileFrameLibraries.VanillaChestTypeNamesByFrame` (value is `chestTile.frameX / 36`).</summary>
		public (int? TileType, int? TileFrame)[] AnyOfTiles;

		public bool AnyUndergroundChest;



		////////////////

		/// <summary></summary>
		/// <param name="anyOfTiles"></param>
		/// <param name="alsoUndergroundChests"></param>
		/// <param name="alsoDungeonAndTempleChests"></param>
		public ChestTypeDefinition(
					(int? tileType, int? tileFrame)[] anyOfTiles,
					bool alsoUndergroundChests=false,
					bool alsoDungeonAndTempleChests=false ) {
			this.AnyOfTiles = anyOfTiles;
			this.AnyUndergroundChest = alsoUndergroundChests;

			var addTiles = new List<(int? tileType, int? tileFrame)>( anyOfTiles );

			foreach( (string name, int frame) in TileFrameLibraries.VanillaChestFramesByTypeName ) {
				switch( name ) {
				case "Chest":
					break;
				//case "Locked Gold Chest":
				case "Locked Shadow Chest":
				case "Lihzahrd Chest":
				case "Locked Jungle Chest":
				case "Locked Corruption Chest":
				case "Locked Crimson Chest":
				case "Locked Hallowed Chest":
				case "Locked Frozen Chest":
				case "Locked Green Dungeon Chest":
				case "Locked Pink Dungeon Chest":
				case "Locked Blue Dungeon Chest":
					if( alsoDungeonAndTempleChests ) {
						addTiles.Add( (null, frame) );
					}
					break;
				default:
					addTiles.Add( (null, frame) );
					break;
				}
			}

			this.AnyOfTiles = addTiles.ToArray();
		}

		/// <summary></summary>
		/// <param name="tileType"></param>
		/// <param name="tileFrame"></param>
		public ChestTypeDefinition( int? tileType, int? tileFrame ) {
			this.AnyOfTiles = new (int?, int?)[] {
				(tileType, tileFrame)
			};
			this.AnyUndergroundChest = false;
		}


		////////////////
		
		/// <summary>
		/// Validates if the given coordinates represent a valid chest. 
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <returns></returns>
		public bool Validate( int tileX, int tileY ) {
			if( !WorldGen.InWorld(tileX, tileY) ) {
				throw new ModLibsException( "Tile OoB: "+tileX+", "+tileY );
			}

			Tile tile = Main.tile[tileX, tileY];
			if( tile?.active() != true ) {
				return false;
			}

			// If no specific chests are defined, allow any chest
			if( this.AnyOfTiles == null || this.AnyOfTiles.Length == 0 ) {
				return true;	// TODO: Check if even a chest?
			}

			if( this.AnyUndergroundChest ) {
				if( tileY > WorldLocationLibraries.DirtLayerBottomTileY ) {
					return true;
				}
			}

			foreach( (int? tileType, int? frame) in this.AnyOfTiles ) {
				if( tileType.HasValue ) {
					if( tile.TileType != tileType.Value ) {
						return false;
					}
				}

				if( frame.HasValue ) {
					if( (tile.TileFrameX / 36) == frame.Value ) {
						return true;
					}
				}
			}

			return false;
		}


		////////////////

		/// <summary></summary>
		/// <param name="within">Optional tile coordinate area.</param>
		/// <returns></returns>
		public IEnumerable<Chest> GetMatchingWorldChests( Rectangle? within = null ) {
			foreach( Chest chest in Main.chest ) {
				if( chest == null ) {
					continue;
				}
				if( within.HasValue ) {
					if( !within.Value.Contains( chest.x, chest.y ) ) {
						continue;
					}
				} else if( chest.x <= 0 || chest.y <= 0 ) {
					continue;
				}

				if( !this.Validate(chest.x, chest.y) ) {
					continue;
				}

				yield return chest;
			}
		}


		////////////////

		/// <summary></summary>
		/// <returns></returns>
		public override string ToString() {
			string tiles = this.AnyOfTiles != null
				? this.AnyOfTiles?
					.Select( t => "("+(t.TileType?.ToString() ?? "null")+", "+(t.TileFrame?.ToString() ?? "null")+")" )
					.ToStringJoined(", ")
				: "any";
			return "Chest types: "+tiles+" (any ug? "+this.AnyUndergroundChest+")";
		}
	}
}
