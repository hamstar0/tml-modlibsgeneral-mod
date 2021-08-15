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
//if( !removeOnly ) LogLibraries.LogOnce("1a "+this.PercentChance);
				return (false, false);
			}
//if( !removeOnly ) LogLibraries.LogOnce("1b "+this.PercentChance);

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

				if( !this.EditNextItem(chest, def, removeOnly) ) {
//if( !removeOnly ) LogLibraries.LogOnce("2 "+isModified);
					return (isModified, false);
				}
				isModified = true;
				break;
			}

			foreach( ChestFillItemDefinition def in this.All ) {
				if( !this.EditNextItem(chest, def, removeOnly) ) {
//if( !removeOnly ) LogLibraries.LogOnce("3 "+isModified);
					return (isModified, false);
				}
				isModified = true;
			}
			
//if( !removeOnly ) LogLibraries.LogOnce("4 "+isModified);
			return (isModified, true);
		}


		////////////////

		private bool EditNextItem( Chest chest, ChestFillItemDefinition def, bool removeOnly ) {
			int findItemType = removeOnly
				? -1
				: def.ItemType;

			int slot = removeOnly
				? this.GetNonEmptySlotIdx( chest, findItemType )
				: this.GetEmptySlotIdx( chest );
			if( slot == -1 ) {
				return false;
			}

			chest.item[slot] = removeOnly
				? new Item()
				: def.CreateItem();

			return true;
		}

		////

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
