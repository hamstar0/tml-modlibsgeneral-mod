using System.Reflection;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;


namespace ModLibsGeneral.Libraries.ModHelpers {
	/// <summary>
	/// Assorted static library functions pertaining to Mod Helpers features.
	/// </summary>
	public partial class ModFeaturesLibraries {
		private static PropertyInfo GetGithubUserNameProp( Mod mod ) {
			return mod.GetType().GetProperty( "GithubUserName", BindingFlags.Static | BindingFlags.Public );
		}

		private static PropertyInfo GetGitubProjectNameProp( Mod mod ) {
			return mod.GetType().GetProperty( "GithubProjectName", BindingFlags.Static | BindingFlags.Public );
		}
		
		////

		private static bool DetectGithub( Mod mod ) {
			if( ModFeaturesLibraries.GetGithubUserNameProp( mod ) == null ) { return false; }
			if( ModFeaturesLibraries.GetGitubProjectNameProp( mod ) == null ) { return false; }
			return true;
		}


		////////////////

		/// <summary>
		/// Indicates if a given `Mod` references a github repo via. `GithubProjectName` and `GithubUserName` static properties.
		/// </summary>
		/// <param name="mod"></param>
		/// <returns></returns>
		public static bool HasGithub( Mod mod ) {
			var self = ModContent.GetInstance<ModFeaturesLibraries>();
			return self.GithubMods.ContainsKey( mod.Name );
		}

		////////////////

		/// <summary>
		/// Gets a mod's github user name, if defined.
		/// </summary>
		/// <param name="mod"></param>
		/// <returns></returns>
		public static string GetGithubUserName( Mod mod ) {
			var self = ModContent.GetInstance<ModFeaturesLibraries>();
			if( !self.GithubMods.ContainsKey( mod.Name ) ) { return null; }

			PropertyInfo gitUserProp = ModFeaturesLibraries.GetGithubUserNameProp( mod );
			return (string)gitUserProp.GetValue( null );
		}

		/// <summary>
		/// Gets a mod's github project (source code) name, if defined.
		/// </summary>
		/// <param name="mod"></param>
		/// <returns></returns>
		public static string GetGithubProjectName( Mod mod ) {
			var self = ModContent.GetInstance<ModFeaturesLibraries>();
			if( !self.GithubMods.ContainsKey( mod.Name ) ) { return null; }

			PropertyInfo gitProjProp = ModFeaturesLibraries.GetGitubProjectNameProp( mod );
			return (string)gitProjProp.GetValue( null );
		}
	}
}
