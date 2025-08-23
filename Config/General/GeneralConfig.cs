using BepInEx.Configuration;
using UnityEngine;

namespace SPTBattleAmbience.Config.General
{
    public static class GeneralConfig
    {
        public static ConfigEntry<bool> EnableDebug { get; set; }
        public static ConfigEntry<float> GlobalAmbientVolumeMult { get; set; }
        public static ConfigEntry<int> AmbientRolloff { get; set; }
        public static ConfigEntry<KeyboardShortcut> PlayAmbientShortcut { get; set; }

        public static void Bind(ConfigFile config)
        {
            string category = "00. General Settings";

            EnableDebug = config.Bind(category, "Enable Debug Logging", false, new ConfigDescription("Enables debug logging to the BepInEx console", null, new ConfigurationManagerAttributes { Order = 1000, IsAdvanced = true }));
            GlobalAmbientVolumeMult = config.Bind(category, "Global Ambient Volume Multiplier", 1f, new ConfigDescription("Global volume multiplier for all ambient battle sounds", null, new ConfigurationManagerAttributes { Order = 990 }));
            AmbientRolloff = config.Bind(category, "Ambient Rolloff Distance", 10000, new ConfigDescription("Rolloff distance for ambient battle sounds", null, new ConfigurationManagerAttributes { Order = 980 }));
            PlayAmbientShortcut = config.Bind(category, "Play Ambient Shortcut", new KeyboardShortcut(KeyCode.N), new ConfigDescription("Debug keybind to immediately play battle ambience", null, new ConfigurationManagerAttributes { Order = 970, IsAdvanced = true }));
        }
    }
}
