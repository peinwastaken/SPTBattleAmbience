using Fika.Core.Main.Utils;
using HarmonyLib;
using PeinRecoilRework.Helpers;
using SPT.Reflection.Patching;
using SPTBattleAmbience.Controllers;
using SPTBattleAmbience.Helpers;
using System.Reflection;
using UnityEngine;

namespace SPTBattleAmbienceFika.Patches;

public class AmbienceControllerGameStartedPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return AccessTools.Method(typeof(BattleAmbienceController), nameof(BattleAmbienceController.OnGameStarted));
    }

    [PatchPrefix]
    private static bool PatchPrefix(BattleAmbienceController __instance)
    {
        // if were client destroy and ambience managers
        if (!FikaBackendUtils.IsServer)
        {
            DebugLogger.LogInfo("Destroying BattleAmbienceController");
            Object.Destroy(__instance);
            return false;
        }

        return true;
    }
}
