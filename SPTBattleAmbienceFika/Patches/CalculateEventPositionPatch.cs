using Comfort.Common;
using Fika.Core.Main.Players;
using Fika.Core.Networking;
using HarmonyLib;
using SPT.Reflection.Patching;
using SPTBattleAmbience.Config;
using SPTBattleAmbience.Controllers;
using SPTBattleAmbience.Helpers;
using SPTBattleAmbience.Managers;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using SPTBattleAmbience.Utility;

namespace SPTBattleAmbienceFika.Patches;

public class CalculateEventPositionPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return AccessTools.Method(typeof(AmbienceManager), nameof(AmbienceManager.CalculateSpawnPoint));
    }

    [PatchPrefix]
    private static bool PatchPostfix(AmbienceManager __instance, ref Vector3 __result)
    {
        if (FikaData.IsServer && !BattleAmbienceController.Instance.UseZones)
        {
            IFikaNetworkManager server = Singleton<IFikaNetworkManager>.Instance;
            List<FikaPlayer> players = server.CoopHandler.HumanPlayers;
            
            string mapId = GameWorldHelper.GetCurrentMapId();
            MapConfigBase mapConfig = ConfigHelper.GetMapConfig(mapId);

            Vector3 averageDir = mapConfig.MapCenter.Value;
            foreach (FikaPlayer player in players)
            {
                if (player != null)
                {
                    averageDir += player.Position;
                }
            }
            averageDir = (averageDir / players.Count).normalized;
            averageDir.y = 0;

            Vector3 mapCenter = mapConfig.MapCenter.Value;
            float mapSize = mapConfig.MapRadius.Value;
            Vector3 pos = mapCenter + averageDir * mapSize;
            
            __result = ModUtils.GetVectorWithAngleOffset(pos, 25f, false);
            
            return false;
        }
        
        return true;
    }
}
