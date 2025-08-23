using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;
using SPTBattleAmbience.Controllers;
using System.Reflection;

namespace SPTBattleAmbience.Patches
{
    public class OnGameEndedPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(Player), nameof(Player.OnGameSessionEnd));
        }

        [PatchPostfix]
        private static void PatchPostfix()
        {
            BattleAmbienceController.Instance.OnGameEnded();
        }
    }
}
