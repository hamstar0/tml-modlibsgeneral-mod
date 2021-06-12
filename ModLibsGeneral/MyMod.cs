using System;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.DotNET.Extensions;
using ModLibsCore.Libraries.TModLoader;
using ModLibsGeneral.Libraries.World;
using ModLibsGeneral.Libraries.Recipes;


namespace ModLibsGeneral {
	/// @private
	partial class ModLibsGeneralMod : Mod {
		public static ModLibsGeneralMod Instance { get; private set; }



		////////////////
		
		public bool MouseInterface { get; private set; }



		////////////////

		public ModLibsGeneralMod() {
			ModLibsGeneralMod.Instance = this;
		}


		public override void Load() {
		}

		////

		public override void Unload() {
			try {
				LogLibraries.Alert( "Unloading mod..." );
			} catch { }
			
			ModLibsGeneralMod.Instance = null;
		}


		////////////////

		public override void AddRecipeGroups() {
			foreach( (string grpName, RecipeGroup grp) in RecipeCommonGroupsLibraries.Groups ) {
				RecipeGroup.RegisterGroup( grpName, grp );
			}
		}


		////////////////

		public override void PostUpdateEverything() {
			this.MouseInterface = Main.LocalPlayer.mouseInterface;

			if( LoadLibraries.IsWorldBeingPlayed() ) {
				ModContent.GetInstance<WorldStateLibraries>().UpdateUponWorldBeingPlayed();
			}
		}


		////////////////

		//public override void UpdateMusic( ref int music ) { //, ref MusicPriority priority
		//	this.MusicLibraries.UpdateMusic();
		//}
	}
}
