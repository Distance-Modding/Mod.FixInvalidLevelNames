using Reactor.API.Configuration;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Distance.FixInvalidLevelNames
{
	public class ConfigurationLogic : MonoBehaviour
	{
		#region Properties

		private const string SanitizeEnabled_ID = "config.enabled";
		public bool SanitizeEnabled
		{
			get => Get<bool>(SanitizeEnabled_ID);
			set => Set(SanitizeEnabled_ID, value);
		}

		private const string SanitizeReservedNames_ID = "config.sanitize_reserved_names";
		public bool SanitizeReservedNames
		{
			get => Get<bool>(SanitizeReservedNames_ID);
			set => Set(SanitizeReservedNames_ID, value);
		}

		private const string SanitizeTrailingNames_ID = "config.sanitize_trailing_names";
		public bool SanitizeTrailingNames
		{
			get => Get<bool>(SanitizeTrailingNames_ID);
			set => Set(SanitizeTrailingNames_ID, value);
		}

		private const string SanitizeLongNames_ID = "config.sanitize_long_names";
		public bool SanitizeLongNames
		{
			get => Get<bool>(SanitizeLongNames_ID);
			set => Set(SanitizeLongNames_ID, value);
		}

		private const string MaxLevelNameLength_ID = "config.max_level_name_length";
		public int MaxLevelNameLength
		{
			get => Get<int>(MaxLevelNameLength_ID);
			set => Set(MaxLevelNameLength_ID, value);
		}

		// Useful info to let the user know what levels are affected.
		private const string InfoSanitizedLeaderboardFolderNames_ID = "info.sanitized_foldernames";
		public Dictionary<string, string> InfoSanitizedLeaderboardFolderNames
		{
			get => Convert(InfoSanitizedLeaderboardFolderNames_ID, new Dictionary<string, string>(), overwriteNull: true);
		}

		#endregion

		#region Helpers

		public bool AddSanitizedLeaderboardsFolderForLevel(string absoluteLevelPath, string sanitizedFolderName)
		{
			absoluteLevelPath = absoluteLevelPath.Replace('\\', '/').ToLowerInvariant();
			if (!InfoSanitizedLeaderboardFolderNames.ContainsKey(absoluteLevelPath))
			{
				InfoSanitizedLeaderboardFolderNames[absoluteLevelPath] = sanitizedFolderName;
				Save();
				return true;
			}
			return false;
		}

		#endregion

		internal Settings Config;

		public event Action<ConfigurationLogic> OnChanged;

		private void Load()
		{
			Config = new Settings("Config");
		}

		public void Awake()
		{
			Load();

			// Assign default settings (if not already assigned).
			Get(SanitizeEnabled_ID, true);
			Get(SanitizeReservedNames_ID, true);
			Get(SanitizeTrailingNames_ID, true);
			Get(SanitizeLongNames_ID, true);
			Get(MaxLevelNameLength_ID, 125); // Leaves room for a Windows username that's up to about 23 characters long.

			// Save settings, and any defaults that may have been added.
			Save();
		}

		public T Get<T>(string key, T @default = default)
		{
			return Config.GetOrCreate(key, @default);
		}

		public void Set<T>(string key, T value)
		{
			Config[key] = value;
			Save();
		}

		public T Convert<T>(string key, T @default = default, bool overwriteNull = false)
		{
			// Assign the object back after conversion, this allows for deep nested settings
			//  that can be preserved and updated without reassigning to the root property.
			var value = Config.GetOrCreate(key, @default);
			if (overwriteNull && value == null)
			{
				value = @default;
			}
			Config[key] = value;
			return value;
		}

		public void Save()
		{
			Config?.Save();
			OnChanged?.Invoke(this);
		}
	}
}
