﻿using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ObjectData;
using Terraria.ModLoader;
using ModLibsCore.Libraries.TModLoader;
using ModLibsCore.Services.Timers;


namespace ModLibsGeneral.Services.Hooks.ExtendedHooks {
	/// <summary>
	/// Supplies custom tModLoader-like, delegate-based hooks for tile-relevant functions not currently available in
	/// tModLoader.
	/// </summary>
	public partial class ExtendedTileHooks : ILoadable {
		private static object MyLock = new object();



		////////////////

		/// <summary>
		/// Represents a GlobalTile.KillTile hook binding.
		/// </summary>
		/// <param name="i"></param>
		/// <param name="j"></param>
		/// <param name="type"></param>
		/// <param name="fail"></param>
		/// <param name="effectOnly"></param>
		/// <param name="noItem"></param>
		/// <param name="nonGameplay">Indicates this hook is being called for tile kills not directly from player actions (typically custom functions).</param>
		public delegate void KillTileDelegate(
			int i,
			int j,
			int type,
			ref bool fail,
			ref bool effectOnly,
			ref bool noItem,
			bool nonGameplay );

		/// <summary>
		/// Represents a GlobalWall.KillWall hook binding.
		/// </summary>
		/// <param name="i"></param>
		/// <param name="j"></param>
		/// <param name="type"></param>
		/// <param name="fail"></param>
		/// <param name="nonGameplay">Indicates this hook is being called for tile kills not directly from player actions (typically custom functions).</param>
		public delegate void KillWallDelegate( int i, int j, int type, ref bool fail, bool nonGameplay );

		/// <summary>
		/// Represents a GlobalTile.KillTile hook binding specifically for multi-tiles.
		/// </summary>
		/// <param name="i"></param>
		/// <param name="j"></param>
		/// <param name="type"></param>
		/// <param name="nonGameplay">Indicates this hook is being called for tile kills not directly from player actions (typically custom functions).</param>
		public delegate void KillMultiTileDelegate( int i, int j, int type, bool nonGameplay );



		////////////////

		private Func<bool> OnTick;

		private ISet<KillTileDelegate> OnKillTileHooks = new HashSet<KillTileDelegate>();
		private ISet<KillWallDelegate> OnKillWallHooks = new HashSet<KillWallDelegate>();
		private ISet<KillMultiTileDelegate> OnKillMultiTileHooks = new HashSet<KillMultiTileDelegate>();

		private ISet<int> CheckedTiles = new HashSet<int>();
		private ISet<int> CheckedWalls = new HashSet<int>();

		private Func<bool> KillTileSkipCondition = null;



		////////////////

		private ExtendedTileHooks() { }

		////

		/// @private
		void ILoadable.Load( Mod mod ) {
			this.OnTick = Timers.MainOnTickGet();
			Main.OnTickForInternalCodeOnly += ExtendedTileHooks._Update;
		}

		/// @private
		void ILoadable.Unload() {
			Main.OnTickForInternalCodeOnly -= ExtendedTileHooks._Update;
		}


		////////////////

		private static void _Update() {	// <- seems to help avoid Mystery Bugs(TM)
			var eth = TmlLibraries.SafelyGetInstance<ExtendedTileHooks>();
			eth.Update();
		}

		private void Update() {
			if( !this.OnTick() ) {
				return;
			}

			this.CheckedTiles.Clear();
			this.CheckedWalls.Clear();
		}


		////////////////

		internal void CallKillTileHooks( int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem ) {
			int tileToCheck = ( i << 16 ) + j;

			// Important stack overflow failsafe:
			if( this.CheckedTiles.Contains( tileToCheck ) ) {
				return;
			}

			IEnumerable<KillTileDelegate> hooks;
			lock( ExtendedTileHooks.MyLock ) {
				hooks = this.OnKillTileHooks.ToArray();
			}

			foreach( KillTileDelegate deleg in hooks ) {
				deleg.Invoke(
					i: i,
					j: j,
					type: type,
					fail: ref fail,
					effectOnly: ref effectOnly,
					noItem: ref noItem,
					nonGameplay: this.KillTileSkipCondition?.Invoke() ?? false
				);
			}

			this.CheckedTiles.Add( tileToCheck );
		}

		internal void CallKillWallHooks( int i, int j, int type, ref bool fail ) {
			int wallToCheck = ( i << 16 ) + j;

			// Important stack overflow failsafe:
			if( this.CheckedWalls.Contains( wallToCheck ) ) {
				return;
			}

			IEnumerable<KillWallDelegate> hooks;
			lock( ExtendedTileHooks.MyLock ) {
				hooks = this.OnKillWallHooks.ToArray();
			}

			foreach( KillWallDelegate deleg in this.OnKillWallHooks ) {
				deleg.Invoke( i, j, type, ref fail, this.KillTileSkipCondition?.Invoke() ?? false );
			}

			this.CheckedWalls.Add( wallToCheck );
		}

		////

		internal bool CanCallKillMultiTileHooks( int i, int j ) {
			Tile tile = Main.tile[i, j];
			if( !tile.HasTile ) {
				return false;
			}
			
			TileObjectData data = TileObjectData.GetTileData( tile );
			if( data == null || (data.Width == 0 || data.Height == 0) ) {
				return false;
			}

			int frameXOffset = tile.TileFrameX;
			int frameYOffset = tile.TileFrameY;

			if( data.StyleHorizontal ) {
				int frameWidth = data.CoordinateWidth + data.CoordinatePadding;
				frameXOffset = tile.TileFrameX % ( data.Width * frameWidth );
			} else {
				int frameHeight = data.CoordinateHeights[0] + data.CoordinatePadding;
				frameYOffset = tile.TileFrameY % ( data.Height * frameHeight );
			}

			if( frameXOffset != 0 || frameYOffset != 0 ) {
				return false;
			}

			return true;
		}

		internal void CallKillMultiTileHooks( int i, int j, int type ) {
			if( !this.CanCallKillMultiTileHooks(i, j) ) {
				return;
			}

			IEnumerable<KillMultiTileDelegate> hooks;
			lock( ExtendedTileHooks.MyLock ) {
				hooks = this.OnKillMultiTileHooks.ToArray();
			}

			foreach( KillMultiTileDelegate deleg in hooks ) {
				deleg.Invoke( i, j, type, this.KillTileSkipCondition?.Invoke() ?? false );
			}
		}
	}
}
