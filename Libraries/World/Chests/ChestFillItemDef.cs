using System;
using Terraria;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.Items.Attributes;


namespace ModLibsGeneral.Libraries.World.Chests {
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
			int stack = WorldGen.genRand.Next( this.MinQuantity, this.MaxQuantity + 1 );

			var item = new Item();
			item.SetDefaults( this.ItemType, true );
			item.stack = stack;

			return item;
		}


		////////////////

		public override string ToString() {
			return "ChestFillItem - "+ItemNameAttributeLibraries.GetQualifiedName(this.ItemType)
				+" "+this.MinQuantity+"-"+this.MaxQuantity+"qt";
		}
	}
}
