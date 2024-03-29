﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Distance.FixInvalidLevelNames
{
	public static class Sanitizer
	{
		public const string InvalidFolder = ".invalid";

		/// <summary>
		/// Reasons a file name was sanitized.
		/// </summary>
		[Flags]
		public enum InvalidReason
		{
			Valid = 0,
			// 260 character limit can break Steam Cloud syncing.
			TooLong = 0x1,
			// Trailing whitespace or period (prefix period is allowed). This includes names consisting only of whitespace and/or periods.
			Trailing = 0x2,
			// Name is reserved by windows (even with an extension).
			// This includes the names '.' and '..'
			Reserved = 0x4,
		}

		/// <summary>
		/// List of name prefixes that are reserved for use on Windows.
		/// </summary>
		/// <see href="https://docs.microsoft.com/en-us/windows/win32/fileio/naming-a-file"/>
		private static readonly HashSet<string> reservedNames = new HashSet<string>(
			new string[] { ".", "..", "CON", "PRN", "AUX", "NUL" }
			.Concat(Enumerable.Range(1, 9).Select((x) => $"COM{x}")) // COM1 - COM9
			.Concat(Enumerable.Range(1, 9).Select((x) => $"LPT{x}")) // LPT1 - LPT9
			);

		/// <summary>
		/// Tests if a folder/file name is invalid, and returns why.
		/// </summary>
		public static bool IsInvalidName(string name, int? maxLength, out InvalidReason reason)
		{
			reason = InvalidReason.Valid;

			// Check reserved names:
			if (reservedNames.Contains(name.ToUpperInvariant()))
			{
				reason |= InvalidReason.Reserved;
			}
			// Check reserved names (without any extension whatsoever):
			// e.g. a file name of "COM1.myfolder.hello" would be invalid, even though removing the extension once wouldn't match a reserved name.
			int dotIndex = name.IndexOf('.');
			if (dotIndex != -1 && reservedNames.Contains(name.Substring(0, dotIndex).ToUpperInvariant()))
			{
				reason |= InvalidReason.Reserved;
			}

			// Can't start or end with whitespace, and can't end with a period:

			if (name.Trim().Length != name.Length ||
				name.TrimEnd('.').Length != name.Length)
			{
				reason |= InvalidReason.Trailing;
			}

			// Names that are too long will mess with Steam cloud sync and such:
			if (!maxLength.HasValue)
			{
				maxLength = 260;
			}
			if (maxLength.HasValue && name.Length > maxLength.Value)
			{
				reason |= InvalidReason.TooLong;
			}

			return reason != InvalidReason.Valid;
		}

		public static string SanitizeLeaderboardsFolderForLevel(string levelFolderName, string absoluteLevelPath)
		{
			if (Mod.Instance.Config.SanitizeEnabled &&
				Sanitizer.IsInvalidName(levelFolderName, Mod.Instance.Config.MaxLevelNameLength, out InvalidReason reason))
			{
				LevelInfo levelInfo = G.Sys.LevelSets_.GetLevelInfo(absoluteLevelPath);
				switch (levelInfo.levelType_)
				{
				case LevelType.My:
					// Not handled
					break;

				case LevelType.Workshop:
					if (G.Sys.SteamworksManager_.UGC_.TryGetWorkshopLevelData(levelInfo.relativePath_, out WorkshopLevelInfo ugcLevelData))
					{
						string newFolderName = Path.Combine(Sanitizer.InvalidFolder, ugcLevelData.levelID_.ToString(CultureInfo.InvariantCulture));
						if (Mod.Instance.Config.AddSanitizedLeaderboardsFolderForLevel(absoluteLevelPath, newFolderName))
						{
							Mod.Instance.Logger.Info($"First-time sanitization for level: {absoluteLevelPath}");
						}
						return newFolderName;
					}
					else
					{
						// Not handled
					}
					break;
				}
			}

			return levelFolderName;
		}
	}
}
