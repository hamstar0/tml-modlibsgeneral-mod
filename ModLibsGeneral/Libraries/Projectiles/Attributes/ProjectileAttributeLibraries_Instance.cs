using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Classes.DataStructures;
using ModLibsCore.Classes.Loadable;


namespace ModLibsGeneral.Libraries.Projectiles.Attributes {
	/// <summary>
	/// Assorted static "helper" functions pertaining to gameplay attributes of NPCs.
	/// </summary>
	public partial class ProjectileAttributeLibraries : ILoadable {
		private ReadOnlyDictionaryOfSets<string, int> _DisplayNamesToIds = null;


		////////////////

		/// <summary>
		/// Table of NPC ids by qualified names.
		/// </summary>
		public static ReadOnlyDictionaryOfSets<string, int> DisplayNamesToIds =>
			ModContent.GetInstance<ProjectileAttributeLibraries>()._DisplayNamesToIds;



		////////////////

		internal ProjectileAttributeLibraries() { }

		/// @private
		void ILoadable.OnModsLoad() { }

		/// @private
		void ILoadable.OnModsUnload() { }

		/// @private
		void ILoadable.OnPostModsLoad() { }


		////////////////

		internal void PopulateNames() {
			var dict = new Dictionary<string, ISet<int>>();

			for( int i = 1; i < ProjectileLoader.ProjectileCount; i++ ) {
				string name = ProjectileAttributeLibraries.GetQualifiedName( i );

				if( dict.ContainsKey( name ) ) {
					dict[name].Add( i );
				} else {
					dict[name] = new HashSet<int>() { i };
				}
			}

			this._DisplayNamesToIds = new ReadOnlyDictionaryOfSets<string, int>( dict );
		}
	}
}
