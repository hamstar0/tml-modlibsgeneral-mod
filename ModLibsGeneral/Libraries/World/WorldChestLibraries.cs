using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using ModLibsCore.Libraries.Debug;
using Terraria;


namespace ModLibsGeneral.Libraries.World {
	/// <summary>
	/// Assorted static library functions pertaining to the current world's chests.
	/// </summary>
	public partial class WorldChestLibraries {
		/// <summary>
		/// Implants the current item(s) from a given chest type, in a given area (if specified).
		/// </summary>
		/// <param name="fillDef"></param>
		/// <param name="chestDef"></param>
		/// <param name="within"></param>
		/// <returns>List of modified chests.</returns>
		public static IList<Chest> AddToWorldChests(
					ChestFillDefinition fillDef,
					ChestTypeDefinition chestDef = new ChestTypeDefinition(),
					Rectangle? within=null ) {
			var modifiedChestIndexes = new List<Chest>();
			IEnumerable<Chest> matchingChests = chestDef.GetMatchingWorldChests( within );
			
			foreach( Chest chest in matchingChests ) {
				(bool isModified, bool completed) status = fillDef.Fill( chest );

				if( status.isModified ) {
					modifiedChestIndexes.Add( chest );
				}
			}

			return modifiedChestIndexes;
		}


		/// <summary>
		/// Removes the current item(s) from a given chest type, in a given area (if specified).
		/// </summary>
		/// <param name="fillDef"></param>
		/// <param name="chestDef"></param>
		/// <param name="within"></param>
		/// <returns>List of modified chests.</returns>
		public static IList<Chest> RemoveFromWorldChests(
					ChestFillDefinition fillDef,
					ChestTypeDefinition chestDef = new ChestTypeDefinition(),
					Rectangle? within = null ) {
			var modifiedChestIndexes = new List<Chest>();
			IEnumerable<Chest> chests = chestDef.GetMatchingWorldChests( within );
/*LogLibraries.Log( "RemoveFromWorldChests - CHEST: "+chestDef.ToString()
	+", FOUND: "+chests.Count()
	+", WITHIN: "+(within?.ToString() ?? "-")
	+", FILL: "+fillDef.ToString() );*/

			foreach( Chest chest in chests ) {
				(bool isModified, bool completed) status = fillDef.Unfill( chest );
				if( status.isModified ) {
					modifiedChestIndexes.Add( chest );
				}
			}

			return modifiedChestIndexes;
		}
	}
}
