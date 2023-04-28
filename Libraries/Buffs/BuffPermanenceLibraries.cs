using System;
using System.Collections.Generic;
using System.Linq;
using ModLibsCore.Services.Network.SimplePacket;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ModLibsGeneral.Libraries.Buffs;

/// <summary>
/// Assorted static library functions pertaining to permanent buffs.
/// </summary>
public class BuffPermanenceLibraries {
	/// <summary>
	/// Adds a permanent buff to a player. Will persist across saves.
	/// </summary>
	/// <param name="player">Player to apply buff to.</param>
	/// <param name="buffId">ID of buff.</param>
	public static void AddPermanentBuff( Player player, int buffId ) {
		if( Main.netMode == NetmodeID.MultiplayerClient ) {
			throw new InvalidOperationException( "This function is not meant to be called on multiplayer clients." );
		}

		if( player.TryGetModPlayer( out BuffPermanencePlayer modPlayer ) ) {
			modPlayer.SetIsBuffPermanent( buffId, true );
		}
	}

	/// <summary>
	/// Removes a permanent buff from a player.
	/// </summary>
	/// <param name="player">Player to remove buff from.</param>
	/// <param name="buffId">ID of buff.</param>
	public static void RemovePermanentBuff( Player player, int buffId ) {
		if( Main.netMode == NetmodeID.MultiplayerClient ) {
			throw new InvalidOperationException( "This function is not meant to be called on multiplayer clients." );
		}

		if( player.TryGetModPlayer( out BuffPermanencePlayer modPlayer ) ) {
			modPlayer.SetIsBuffPermanent( buffId, false );
		}
	}
}

internal class BuffPermanencePlayer : ModPlayer {
	private const string SaveEntry = "perma_buffs";

	private HashSet<int> permanentBuffs;
	private bool needsSync;

	public override void PreUpdateBuffs() {
		if( this.needsSync && Main.netMode == NetmodeID.Server ) {
			SimplePacket.SendToClient( new BuffPermanenceSynchronizationPacket( (byte)this.Player.whoAmI, this.permanentBuffs?.ToArray() ) );

			this.needsSync = false;
		}

		if( this.permanentBuffs == null ) { return; }

		foreach( int buffId in permanentBuffs ) {
			this.Player.AddBuff( buffId, ushort.MaxValue, quiet: true );
		}
	}

	public override void SaveData( TagCompound tag ) {
		if( this.permanentBuffs?.Count > 0 ) {
			tag[SaveEntry] = this.permanentBuffs.ToArray();
		}
	}

	public override void LoadData( TagCompound tag ) {
		if( tag.TryGet( SaveEntry, out int[] list ) ) {
			this.SetPermanentBuffs( list );
		}
	}

	public void SetIsBuffPermanent( int buffId, bool value ) {
		if( value ) {
			this.needsSync |= ( this.permanentBuffs ??= new()).Add( buffId );
		} else {
			this.needsSync |= this.permanentBuffs?.Remove( buffId ) ?? false;
		}
	}

	public void SetPermanentBuffs(IEnumerable<int> buffIds) {
		if( buffIds != null ) {
			this.permanentBuffs = new( buffIds );
		} else {
			this.permanentBuffs?.Clear();
		}
	}
}

[Serializable]
internal class BuffPermanenceSynchronizationPacket : SimplePacketPayload {
	public byte PlayerId;
	public int[] PermanentBuffIds;

	public BuffPermanenceSynchronizationPacket( byte playerId, int[] permanentBuffIds ) {
		this.PlayerId = playerId;
		this.PermanentBuffIds = permanentBuffIds;
	}

	public override void ReceiveOnClient() {
		if( Main.player[ this.PlayerId ].TryGetModPlayer( out BuffPermanencePlayer modPlayer ) ) {
			modPlayer.SetPermanentBuffs( this.PermanentBuffIds );
		}
	}

	public override void ReceiveOnServer( int fromWho ) { }
}
