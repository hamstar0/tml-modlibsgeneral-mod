﻿using System;
using Terraria.Graphics.Shaders;


namespace ModLibsGeneral.Libraries.Misc {
	/// <summary>
	/// Assorted static library functions pertaining to shaders.
	/// </summary>
	public class ShaderLibraries {
		/// <summary>
		/// Generalized getter for shader ids from item types.
		/// </summary>
		/// <param name="itemType"></param>
		/// <returns>-1 if no shader id exists for the given item.</returns>
		public static int GetShaderIdByDyeItemType( int itemType ) {
			int shaderId = (int)GameShaders.Armor.GetShaderIdFromItemId( itemType );
			if( shaderId > 0 ) {
				return shaderId;
			}
			shaderId = (int)GameShaders.Hair.GetShaderIdFromItemId( itemType );
			if( shaderId >= 0 ) {
				return shaderId;
			}
			return -1;
		}
	}
}
