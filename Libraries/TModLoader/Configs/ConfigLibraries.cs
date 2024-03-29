﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.DotNET.Reflection;


namespace ModLibsGeneral.Libraries.TModLoader.Configs {
	/// <summary>
	/// Assorted static library functions pertaining to configs (ModConfig, primarily).
	/// </summary>
	public partial class ConfigLibraries {
		/// <summary>
		/// Syncs to everyone. Use with caution.
		/// </summary>
		/// <param name="config"></param>
		/// <returns>`false` if sync is diabled (client-only config).</returns>
		public static bool SyncConfig( ModConfig config ) {	//untested
			if( config.Mode != ConfigScope.ServerSide ) {
				return false;
			}

			string json = JsonConvert.SerializeObject( config, ConfigManager.serializerSettings );

			var requestChanges = (ModPacket)Activator.CreateInstance(
				typeof( ModPacket ),
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
				null,
				new object[] { MessageID.InGameChangeConfig },
				null
			);

			if( Main.netMode == NetmodeID.Server ) {
				requestChanges.Write( true );
				requestChanges.Write( "ConfigLibraries.SyncConfig syncing..." );
			}

			requestChanges.Write( config.Mod.Name );
			requestChanges.Write( config.Name );
			requestChanges.Write( json );

			requestChanges.Send();

			return true;
		}


		/// <summary>
		/// Combines one ModConfig instance into another, prioritizing non-default values of the latter.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="to">Config to push changed field/property values to. Any existing values are overridden, where changes found.</param>
		/// <param name="fro">Config to pull field/property values from. Only pulls non-default (changed) values.</param>
		public static void MergeConfigs<T>( T to, T fro ) where T : ModConfig {
			if( typeof(T).IsAbstract ) {
				throw new ModLibsException( "Cannot merge abstract class "+typeof(T).Name+" (did you mean "+to.GetType().Name+"?)" );
			}
			ConfigLibraries.MergeConfigsForType( typeof(T), to, fro );
		}

		internal static void MergeConfigsForType( Type configType, ModConfig toConfig, ModConfig froConfig ) {
			var defaultConfig = (ModConfig)Activator.CreateInstance(
				configType,
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
				null,
				new object[] { },
				null
			);
			if( defaultConfig == null ) {
				throw new ModLibsException( "Could generate default template for ModConfig "+configType.Name );
			}

			JsonConvert.PopulateObject( "{}", defaultConfig, ConfigManager.serializerSettings );

			object froVal, defaultVal;
			IEnumerable<MemberInfo> members = configType.GetMembers( BindingFlags.Public | BindingFlags.Instance );

			foreach( MemberInfo memb in members ) {
				if( memb.MemberType == MemberTypes.Property ) {
					var prop = (PropertyInfo)memb;

					if( !prop.CanWrite || !prop.CanRead ) {
						continue;
					}
					if( prop.GetCustomAttribute<JsonIgnoreAttribute>() != null ) {
						continue;
					}
				} else if( memb.MemberType != MemberTypes.Field ) {
					continue;
				}

				if( !ReflectionLibraries.Get(froConfig, memb.Name, out froVal) ) {
					throw new ModLibsException( "Could retrieve member "+memb.Name+" from 'fro' instance of "+configType.Name );
				}
				if( !ReflectionLibraries.Get(defaultConfig, memb.Name, out defaultVal) ) {
					throw new ModLibsException( "Could retrieve member "+memb.Name+" from template instance of "+configType.Name );
				}

				bool froValueIsDefault = froVal?.Equals(defaultVal)
					?? froVal == defaultVal;    // <- default is null
				bool froValueIsCollection = froVal is IEnumerable;
				bool froValueIsEmpty = froValueIsCollection
					&& (froVal == null || ((IEnumerable)froVal).Cast<object>().Count() == 0);
				
				if( (!froValueIsCollection && !froValueIsDefault) || (froValueIsCollection && !froValueIsEmpty) ) {
					if( !ReflectionLibraries.Set(toConfig, memb.Name, froVal) ) {
						throw new ModLibsException( "Could set merged field/property "+memb.Name+" for "+configType.Name );
					}
				}
			}
		}
	}
}
