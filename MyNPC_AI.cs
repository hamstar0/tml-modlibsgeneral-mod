using System;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Services.EntityControls;


namespace ModLibsGeneral {
	/// @private
	partial class ModLibsGeneralNPC : GlobalNPC {
		public override bool PreAI( NPC npc ) {
			EntityControls.ApplyFakeTarget( npc, this.FakeTarget, this.FakeTargetPosition );

			if( this.LockedAI0.HasValue ) {
				npc.ai[0] = this.LockedAI0.Value;
			}
			if( this.LockedAI1.HasValue ) {
				npc.ai[1] = this.LockedAI1.Value;
			}
			if( this.LockedAI2.HasValue ) {
				npc.ai[2] = this.LockedAI2.Value;
			}
			if( this.LockedAI3.HasValue ) {
				npc.ai[3] = this.LockedAI3.Value;
			}

			return base.PreAI( npc );
		}

		public override void PostAI( NPC npc ) {
			EntityControls.RevertFakeTarget( npc, this.FakeTarget, this.FakeTargetPosition );
			base.PostAI( npc );
		}
	}
}
