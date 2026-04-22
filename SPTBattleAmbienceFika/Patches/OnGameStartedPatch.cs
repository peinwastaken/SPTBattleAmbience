using Comfort.Common;
using EFT;
using Fika.Core.Main.Utils;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using SPTBattleAmbience.Helpers;

namespace SPTBattleAmbienceFika.Patches;

public class OnGameStartedPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return AccessTools.Method(typeof(GameWorld), nameof(GameWorld.OnGameStarted));
    }

    [PatchPrefix]
    private static void PatchPrefix()
    {
        bool isServer = FikaBackendUtils.IsServer;
        bool isHeadless = FikaBackendUtils.IsHeadless;
        
        FikaData.IsHeadless = isHeadless;
        FikaData.IsServer = isServer;
    }
}
