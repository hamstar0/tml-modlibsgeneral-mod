using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.DotNET.Extensions;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.Items.Attributes;
using ModLibsGeneral.Libraries.Tiles;


namespace ModLibsGeneral.Libraries.World {
	/// <summary>
	/// Definition for how to add filling to a given chest.
	/// </summary>
	public partial struct ChestFillDefinition {
		/// <summary>
		/// Any of these items are evaluated to decide on placement.
		/// </summary>
		public (float Weight, ChestFillItemDefinition ItemDef)[] Any;
		/// <summary>
		/// Each of the following are added until `PercentChance`.
		/// </summary>
		public ChestFillItemDefinition[] All;
		/// <summary>
		/// Chance any or all of this chest's fill definition are avoided.
		/// </summary>
		public float PercentChance;



		////////////////

		/// <summary></summary>
		/// <param name="any"></param>
		/// <param name="all"></param>
		/// <param name="percentChance"></param>
		public ChestFillDefinition(
					(float Weight, ChestFillItemDefinition ItemDef)[] any,
					ChestFillItemDefinition[] all,
					float percentChance=1f ) {
			this.Any = any;
			this.All = all;
			this.PercentChance = percentChance;
		}
		
		/// <summary></summary>
		/// <param name="any"></param>
		/// <param name="percentChance"></param>
		public ChestFillDefinition(
					(float Weight, ChestFillItemDefinition ItemDef)[] any,
					float percentChance=1f ) {
			this.Any = any;
			this.All = new ChestFillItemDefinition[ 0 ];
			this.PercentChance = percentChance;
		}

		/// <summary></summary>
		/// <param name="all"></param>
		/// <param name="percentChance"></param>
		public ChestFillDefinition(
					ChestFillItemDefinition[] all,
					float percentChance=1f ) {
			this.Any = new (float, ChestFillItemDefinition)[ 0 ];
			this.All = all;
			this.PercentChance = percentChance;
		}

		/// <summary></summary>
		/// <param name="single"></param>
		/// <param name="percentChance"></param>
		public ChestFillDefinition(
					ChestFillItemDefinition single,
					float percentChance=1f ) {
			this.Any = new (float, ChestFillItemDefinition)[ 0 ];
			this.All = new ChestFillItemDefinition[] { single };
			this.PercentChance = percentChance;
		}


		////////////////

		public override string ToString() {
			return this.ToString( "\n " );
		}

		public string ToString( string delim ) {
			string str = "ChestFill - %:" + this.PercentChance;
			if( this.Any.Length >= 1 ) {
				str += delim+" Any: "+this.Any
					.Select( def=>def.ItemDef.ToString()+":"+def.Weight )
					.ToStringJoined(", "+delim);
			}
			if( this.All.Length >= 1 ) {
				str += delim+" All: "+this.All
					.ToStringJoined(", "+delim);
			}

			return str;
		}
	}




	/// <summary></summary>
	public struct ChestFillItemDefinition {
		/// <summary></summary>
		public int ItemType;
		/// <summary></summary>
		public int MinQuantity;
		/// <summary></summary>
		public int MaxQuantity;



		////////////////

		/// <summary></summary>
		public ChestFillItemDefinition( int itemType, int min, int max ) {
			this.ItemType = itemType;
			this.MinQuantity = min;
			this.MaxQuantity = max;
		}

		/// <summary></summary>
		public ChestFillItemDefinition( int itemType ) {
			this.ItemType = itemType;
			this.MinQuantity = 1;
			this.MaxQuantity = 1;
		}


		////////////////

		/// <summary></summary>
		public Item CreateItem() {
			var item = new Item();
			item.SetDefaults( this.ItemType, true );
			item.stack = WorldGen.genRand.Next( this.MinQuantity, this.MaxQuantity );
			return item;
		}


		////////////////

		public override string ToString() {
			return "ChestFillItem - "+ItemNameAttributeLibraries.GetQualifiedName(this.ItemType)
				+" "+this.MinQuantity+"-"+this.MaxQuantity+"qt";
		}
	}




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
			if( tile?.active() != true ) {	// TODO: Check if even a chest?
				return false;
			}

			// If no specific chests are defined, allow any chest
			if( this.AnyOfTiles == null || this.AnyOfTiles.Length == 0 ) {
				return true;
			}

			if( this.AnyUndergroundChest ) {
				if( tileY > WorldLocationLibraries.DirtLayerBottomTileY ) {
					return true;
				}
			}

			foreach( (int? tileType, int? frame) in this.AnyOfTiles ) {
				if( tileType.HasValue ) {
					if( tile.type != tileType.Value ) {
						return false;
					}
				}

				if( frame.HasValue ) {
					if( (tile.frameX / 36) == frame.Value ) {
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

				if( !this.Validate( chest.x, chest.y ) ) {
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
