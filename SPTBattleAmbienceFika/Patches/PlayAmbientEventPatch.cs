using Comfort.Common;
using Fika.Core.Networking;
using Fika.Core.Networking.LiteNetLib;
using HarmonyLib;
using SPT.Reflection.Patching;
using SPTBattleAmbience.Data;
using SPTBattleAmbience.Helpers;
using SPTBattleAmbienceFika.Packets;
using System.Reflection;
using UnityEngine;

namespace SPTBattleAmbienceFika.Patches;

public class PlayAmbientEventPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return AccessTools.Method(typeof(AmbientHelper), nameof(AmbientHelper.PlayAmbienceSound));
    }

    [PatchPostfix]
    private static void PatchPostfix(Vector3 position, ClipInfo clipInfo, int rolloff, float volume)
    {
        if (FikaData.IsServer)
        {
            AmbienceEventPacket packet = new AmbienceEventPacket
            {
                SoundType = clipInfo.SoundType,
                SoundCategory = clipInfo.Category,
                ClipName = clipInfo.ClipName,
                Position = position,
                Volume = volume,
                Rolloff = rolloff,
            };
            
            Singleton<IFikaNetworkManager>.Instance.SendData(ref packet, DeliveryMethod.ReliableUnordered, true);
        }
    } 
}
