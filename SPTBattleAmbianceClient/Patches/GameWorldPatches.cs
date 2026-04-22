using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;
using SPTBattleAmbience.Controllers;
using SPTBattleAmbience.Helpers;
using System.Reflection;

namespace SPTBattleAmbience.Patches;

public class GameStartedPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return AccessTools.Method(typeof(GameWorld), nameof(GameWorld.OnGameStarted));
    }

    [PatchPrefix]
    private static void PatchPrefix(GameWorld __instance)
    {
        // force reload audioclips because otherwise they become null
        AmbientHelper.LoadAmbientSoundCategories();
        
        if (BattleAmbienceController.Instance == null)
        {
            BattleAmbienceController ambienceController = __instance.gameObject.AddComponent<BattleAmbienceController>();
            ambienceController.OnGameStarted();
        }
    }
}