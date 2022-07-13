using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Data;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using System;
using System.Reflection;
using UnityEngine;

namespace Distance.FixInvalidLevelNames
{
	/// <summary>
	/// The mod's main class containing its entry point
	/// </summary>
	[ModEntryPoint("com.github.trigger-segfault/Distance.FixInvalidLevelNames")]
	public sealed class Mod : MonoBehaviour
	{
		public const string Name = "FixInvalidLevelNames";
		public const string FullName = "Distance." + Name;
		public const string FriendlyName = "Fix Invalid Level Names";


		public static Mod Instance { get; private set; }

		public IManager Manager { get; private set; }

		public Log Logger { get; private set; }

		public ConfigurationLogic Config { get; private set; }

		/// <summary>
		/// Method called as soon as the mod is loaded.
		/// WARNING:	Do not load asset bundles/textures in this function
		///				The unity assets systems are not yet loaded when this
		///				function is called. Loading assets here can lead to
		///				unpredictable behaviour and crashes!
		/// </summary>
		public void Initialize(IManager manager)
		{
			// Do not destroy the current game object when loading a new scene
			DontDestroyOnLoad(this);

			Instance = this;
			Manager = manager;

			Logger = LogManager.GetForCurrentAssembly();
			Logger.Info(Mod.Name + ": Initializing...");

			Config = this.gameObject.AddComponent<ConfigurationLogic>();

			try
			{
				// Never ever EVER use this!!!
				// It's the same as below (with `GetCallingAssembly`) wrapped around a silent catch-all.
				//RuntimePatcher.AutoPatch();

				RuntimePatcher.HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
			}
			catch (Exception ex)
			{
				Logger.Error(Mod.Name + ": Error during Harmony.PatchAll()");
				Logger.Exception(ex);
				throw;
			}

			try
			{
				CreateSettingsMenu();
			}
			catch (Exception ex)
			{
				Logger.Error(Mod.Name + ": Error during CreateSettingsMenu()");
				Logger.Exception(ex);
				throw;
			}

			Logger.Info(Mod.Name + ": Initialized!");
		}

		private void CreateSettingsMenu()
		{
			MenuTree settingsMenu = new MenuTree("menu.mod." + Mod.Name.ToLower(), Mod.FriendlyName);

			settingsMenu.CheckBox(MenuDisplayMode.MainMenu,
				"setting:sanitize_enabled",
				"ENABLE THIS FIX",
				() => Config.SanitizeEnabled,
				(value) => Config.SanitizeEnabled = value,
				"Enables sanitization for level names (affects Workshop Local Leaderboards level folder names only).");

			settingsMenu.IntegerSlider(MenuDisplayMode.MainMenu,
				"setting:max_level_name_length",
				"NAME LENGTH THRESHOLD",
				() => Config.MaxLevelNameLength,
				(value) => Config.MaxLevelNameLength = value,
				70,  // 70  leaves room for a Windows username up to 78 characters long.
				145, // 145 leaves room for a Windows username up to 3 characters long.
				125, // 125 leaves room for a Windows username up to 23 characters long.
				"Max name length that doesn't need to be sanitized." +
				" 125 allows for a Windows username up to about 23 characters long." +
				"\n[FF0000]WARNING:[-] Sanitized names can't be accessed without this mod, so avoid lowering this too much unless needed.");

			Menus.AddNew(MenuDisplayMode.MainMenu, settingsMenu,
				Mod.FriendlyName.ToUpper(),
				"Settings for correcting Workshop level names that cause problems on Windows.");
		}
	}
}
