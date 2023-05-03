using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;
using ModLibsCore.Classes.DataStructures;


namespace ModLibsGeneral.Libraries.Buffs {
	/// @private
	public partial class BuffAttributesLibraries : ModSystem {
		private ReadOnlyDictionaryOfSets<string, int> _NamesToIds = null;


		////////////////

		public override void PostSetupContent() {
			this.PopulateNames();
		}


		////////////////

		internal void PopulateNames() {
			var dict = new Dictionary<string, ISet<int>>();

			for( int i = 1; i < BuffLoader.BuffCount; i++ ) {
				string name = BuffAttributesLibraries.GetBuffDisplayName( i );

				if( dict.ContainsKey( name ) ) {
					dict[name].Add( i );
				} else {
					dict[name] = new HashSet<int>() { i };
				}
			}

			this._NamesToIds = new ReadOnlyDictionaryOfSets<string, int>( dict );
		}
	}
}
