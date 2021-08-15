using System;
using System.Linq;
using Terraria;
using ModLibsCore.Libraries.Debug;


namespace ModLibsGeneral.Libraries.World.Chests {
	/// <summary>
	/// Definition for how to add filling to a given chest.
	/// </summary>
	public partial struct ChestFillDefinition {
		/// <summary>
		/// Attempts to fill a given chest according to the current specifications.
		/// </summary>
		/// <param name="chest"></param>
		/// <returns></returns>
		public (bool IsModified, bool Completed) Fill( Chest chest ) {
			return this.FillWith( chest, false );
		}


		////////////////

		/// <summary>
		/// Attempts to remove from a given chest with the scenario to encounter.
		/// </summary>
		/// <param name="chest"></param>
		/// <returns></returns>
		public (bool IsModified, bool Completed) Unfill( Chest chest ) {
			return this.FillWith( chest, true );
		}


		////////////////

		private (bool IsModified, bool Completed) FillWith( Chest chest, bool removeOnly ) {
			if( WorldGen.genRand.NextFloat() >= this.PercentChance ) {
				return (false, true);
			}

			bool isModified = false;
			ChestFillDefinition self = this;

			float maxWeight = this.Any
				.Select( kv => kv.Weight )
				.Sum();
			float weightVal = maxWeight * WorldGen.genRand.NextFloat();

			float countedWeight = 0f;
			foreach( (float weight, ChestFillItemDefinition def) in this.Any ) {
				countedWeight += weight;
				if( countedWeight < weightVal ) {
					continue;
				}

				if( removeOnly ) {
					if( !this.RemoveNextFoundItem( chest, def ) ) {
						return (isModified, false);
					}
				} else {
					if( !this.AddNextItem( chest, def ) ) {
						return (isModified, false);
					}
				}
				isModified = true;
				break;
			}

			foreach( ChestFillItemDefinition def in this.All ) {
				if( removeOnly ) {
					if( !this.RemoveNextFoundItem( chest, def ) ) {
						return (isModified, false);
					}
				} else {
					if( !this.AddNextItem( chest, def ) ) {
						return (isModified, false);
					}
				}
				isModified = true;
			}
			
			return (isModified, true);
		}


		////////////////

		private bool AddNextItem( Chest chest, ChestFillItemDefinition def ) {
			int slot = this.GetEmptySlotIdx( chest );
			if( slot == -1 ) {
				return false;
			}

			chest.item[slot] = def.CreateItem();
			return true;
		}

		private bool RemoveNextFoundItem( Chest chest, ChestFillItemDefinition def ) {
			int slot = this.GetNonEmptySlotIdx( chest, def.ItemType );
			if( slot == -1 ) {
				return false;
			}

			Item item = chest.item[slot];
			if( item.stack < def.MinQuantity || item.stack > def.MaxQuantity ) {
				return false;
			}

			chest.item[slot] = new Item();
			return true;
		}

		////////////////

		private int GetEmptySlotIdx( Chest chest ) {
			for( int i = 0; i < chest.item.Length; i++ ) {
				Item item = chest.item[i];

				if( item?.IsAir != false ) {
					return i;
				}
			}

			return -1;
		}

		private int GetNonEmptySlotIdx( Chest chest, int itemType = -1 ) {
			for( int i = 0; i < chest.item.Length; i++ ) {
				Item item = chest.item[i];

				if( item?.IsAir == false ) {
					if( itemType == -1 || item.type == itemType ) {
						return i;
					}
				}
			}

			return -1;
		}
	}
}
