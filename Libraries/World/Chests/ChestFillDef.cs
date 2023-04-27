using System;
using System.Linq;
using Terraria;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.DotNET.Extensions;
using ModLibsCore.Libraries.Debug;


namespace ModLibsGeneral.Libraries.World.Chests {
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
}
