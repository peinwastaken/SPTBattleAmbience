using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;
using SPTBattleAmbience.Controllers;
using System.Reflection;

namespace SPTBattleAmbience.Patches
{
    public class GameStartedPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(GameWorld), nameof(GameWorld.OnGameStarted));
        }

        [PatchPrefix]
        private static void PatchPrefix()
        {
            Plugin.LoadBattleSounds();
            BattleAmbienceController.Instance.OnGameStarted();
        }
    }
}