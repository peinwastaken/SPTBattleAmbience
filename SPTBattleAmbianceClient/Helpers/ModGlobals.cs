using BepInEx;
using BepInEx.Bootstrap;
using System.Collections.Generic;

namespace SPTBattleAmbience.Helpers;

public static class FikaData
{
    public static bool IsFika = false;
    public static bool IsHeadless = false;
    public static bool IsServer = false;

    public static void Initialize()
    {
        Dictionary<string, PluginInfo> plugins = Chainloader.PluginInfos;
        IsFika = plugins.ContainsKey("com.fika.core");
        IsHeadless = plugins.ContainsKey("com.fika.headless");
        IsServer = false;
    }
}
