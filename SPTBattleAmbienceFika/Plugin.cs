using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using Comfort.Common;
using Fika.Core.Modding;
using Fika.Core.Modding.Events;
using Fika.Core.Networking;
using PeinRecoilRework.Helpers;
using SPT.Reflection.Patching;
using SPTBattleAmbience.Data;
using SPTBattleAmbience.Helpers;
using SPTBattleAmbienceFika.Packets;
using System.Collections.Generic;

namespace SPTBattleAmbienceFika;

[BepInPlugin("com.pein.battleambiencefika", "BattleAmbienceFikaSync", "1.0.2")]
[BepInDependency("com.fika.core")]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        
        PatchManager patchManager = new PatchManager(this, true);
        patchManager.EnablePatches();
        
        FikaEventDispatcher.SubscribeEvent<FikaNetworkManagerCreatedEvent>(RegisterPackets);

        Dictionary<string, PluginInfo> plugins = Chainloader.PluginInfos;
        FikaData.IsHeadless = plugins.ContainsKey("com.fika.headless");
    }

    private void RegisterPackets(FikaNetworkManagerCreatedEvent fikaNetworkManagerCreatedEvent)
    {
        IFikaNetworkManager manager = Singleton<IFikaNetworkManager>.Instance;
        
        manager.RegisterPacket<AmbienceEventPacket>(OnAmbienceEventPacketReceived);
    }

    private void OnAmbienceEventPacketReceived(AmbienceEventPacket packet)
    {
        ClipInfo clipInfo = AmbientHelper.GetClipInfo(packet.SoundCategory, packet.SoundType, packet.ClipName);
        
        DebugLogger.LogInfo($"Received {packet}");
        
        AmbientHelper.PlayAmbienceSound(packet.Position, clipInfo, packet.Rolloff, packet.Volume);
    }
}
