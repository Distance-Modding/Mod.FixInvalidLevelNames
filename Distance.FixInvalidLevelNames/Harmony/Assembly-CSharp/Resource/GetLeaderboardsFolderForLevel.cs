using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Distance.FixInvalidLevelNames.Harmony
{
	[HarmonyPatch(typeof(Resource), nameof(Resource.GetLeaderboardsFolderForLevel))]
	internal static class Resource__GetLeaderboardsFolderForLevel
	{
		[HarmonyTranspiler]
		internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			Mod.Instance.Logger.Info("Transpiling...");
			// VISUAL:
			// Sanitize name after getting file (folder) name without extension.
			//string fileNameWithoutExtension = Resource.GetFileNameWithoutExtension(absoluteLevelPath);
			//fileNameWithoutExtension = Sanitizer.SanitizeLeaderboardsFolderForLevel(fileNameWithoutExtension);

			var codes = new List<CodeInstruction>(instructions);
			for (int i = 0; i < codes.Count; i++)
			{
				if (codes[i].opcode == OpCodes.Call && ((MethodInfo)codes[i].operand).Name == "GetFileNameWithoutExtension")
				{
					Mod.Instance.Logger.Info($"call GetFileNameWithoutExtension @ {i}");

					// After:   call Resource.GetFileNameWithoutExtension
					// Insert:  ldarg.0
					// Insert:  call SanitizeLeaderboardsFolderForLevel
					codes.InsertRange(i + 1, new CodeInstruction[]
					{
						new CodeInstruction(OpCodes.Ldarg_0, null),
						new CodeInstruction(OpCodes.Call, typeof(Sanitizer).GetMethod(nameof(Sanitizer.SanitizeLeaderboardsFolderForLevel))),
					});

					break;
				}
			}
			return codes.AsEnumerable();
		}
	}
}
