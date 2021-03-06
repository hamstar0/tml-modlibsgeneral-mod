using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;


namespace ModLibsGeneral.Libraries.Audio {
	/// <summary>
	/// Assorted static library functions pertaining to sounds.
	/// </summary>
	public class SoundLibraries : ILoadable {
		/// <summary>
		/// Gets volume and pan data for a sound that would play at a given point.
		/// </summary>
		/// <param name="worldX"></param>
		/// <param name="worldY"></param>
		/// <returns></returns>
		public static (float Volume, float Pan) GetSoundDataFromSource( int worldX, int worldY ) {
			var maxRange = new Rectangle(
				(int)( Main.screenPosition.X - (Main.screenWidth * 2) ),
				(int)( Main.screenPosition.Y - (Main.screenHeight * 2) ),
				Main.screenWidth * 5,
				Main.screenHeight * 5 );
			var source = new Rectangle( worldX, worldY, 1, 1 );

			Vector2 screenCenter = new Vector2(
				Main.screenPosition.X + (float)Main.screenWidth * 0.5f,
				Main.screenPosition.Y + (float)Main.screenHeight * 0.5f );

			if( !source.Intersects(maxRange) ) {
				return (0, 0);
			}

			float pan = (float)(worldX - screenCenter.X) / (float)(Main.screenWidth / 2);
			float distX = (float)worldX - screenCenter.X;
			float distY = (float)worldY - screenCenter.Y;
			float dist = (float)Math.Sqrt( (distX * distX) + (distY * distY) );
			float vol = 1f - (dist / ((float)Main.screenWidth * 1.5f));

			pan = MathHelper.Clamp( pan, -1f, 1f );
			vol = Math.Max( vol, 0f );

			return (vol, pan);
		}


		////////////////

		/// <summary>
		/// A more flexible variant of `Main.PlaySound` to allow adjusting volume and position, in particular for
		/// currently-playing sounds.
		/// </summary>
		/// <param name="mod"></param>
		/// <param name="soundPath"></param>
		/// <param name="position"></param>
		/// <param name="volume"></param>
		public static void PlaySound( Mod mod, string soundPath, Vector2 position, float volume = 1f ) {
			if( Main.netMode == NetmodeID.Server ) { return; }

			SoundStyle sound;
			var sndLibs = ModContent.GetInstance<SoundLibraries>();

			if( sndLibs.Sounds.ContainsKey( soundPath ) ) {
				sound = sndLibs.Sounds[soundPath];
				sndLibs.Sounds[soundPath] = sound.WithVolumeScale( volume );
			} else {
				try {
					sound = mod.GetLegacySoundSlot(
						Terraria.ModLoader.SoundType.Custom,
						"Sounds/Custom/" + soundPath
					).WithVolume( volume );

					sndLibs.Sounds[soundPath] = sound;
				} catch( Exception e ) {
					throw new ModLibsException( "Sound load issue.", e );
				}
			}

			SoundEngine.PlaySound( sound, position );
		}

		/// <summary>
		/// A more flexible variant of `Main.PlaySound` to allow adjusting volume and position.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="sound"></param>
		/// <param name="position"></param>
		/// <param name="volume"></param>
		public static void PlaySound( string name, SoundStyle sound, Vector2 position, float volume = 1f ) {
			if( Main.netMode == NetmodeID.Server ) { return; }

			var sndLibs = ModContent.GetInstance<SoundLibraries>();

			if( sndLibs.Sounds.ContainsKey(name) ) {
				sound = sndLibs.Sounds[name];
			}

			try {
				sound = sound.WithVolumeScale( volume );
				sndLibs.Sounds[ name ] = sound;
			} catch( Exception e ) {
				throw new ModLibsException( "Sound load issue.", e );
			}

			SoundEngine.PlaySound( sound, position );
		}



		////////////////

		private IDictionary<string, SoundStyle> Sounds = new Dictionary<string, SoundStyle>();



		////////////////

		void ILoadable.Load( Mod mod ) { }
		void ILoadable.Unload() { }
	}
}
