using BattleAmbienceClient.Controllers;
using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;

namespace BattleAmbienceClient.Patches
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
            if (BattleAmbienceController.Instance == null)
            {
                Plugin.CreateAmbienceController();
            }
            
            BattleAmbienceController.Instance?.OnGameStarted();
        }
    }
}