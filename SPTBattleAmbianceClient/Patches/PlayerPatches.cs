using BattleAmbienceClient.Controllers;
using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;

namespace BattleAmbienceClient.Patches
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
            BattleAmbienceController.Instance?.OnGameEnded();
        }
    }
}
