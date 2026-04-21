using BepInEx.Configuration;
using UnityEngine;
using static BetterAudio;

namespace BattleAmbienceClient.Config.General
{
    public static class GeneralConfig
    {
        public static ConfigEntry<bool> EnableDebug { get; set; }
        public static ConfigEntry<float> GlobalAmbientVolumeMult { get; set; }
        public static ConfigEntry<float> GlobalAmbientCooldownMult { get; set; }
        public static ConfigEntry<bool> UseEventConfigWeights { get; set; }
        public static ConfigEntry<int> AmbientRolloff { get; set; }
        public static ConfigEntry<EOcclusionTest> OcclusionTestMode { get; set; }
        public static ConfigEntry<AudioSourceGroupType> AudioSourceGroup { get; set; }
        public static ConfigEntry<KeyboardShortcut> PlayAmbientShortcut { get; set; }

        public static void Bind(ConfigFile config)
        {
            string category = "00. General Settings";

            EnableDebug = config.Bind(category, "Enable Debug Logging", false, new ConfigDescription("Enables debug logging to the BepInEx console", null, new ConfigurationManagerAttributes { Order = 1000, IsAdvanced = true }));
            GlobalAmbientVolumeMult = config.Bind(category, "Global Ambient Volume Multiplier", 1f, new ConfigDescription("Global volume multiplier for all ambient battle sounds", null, new ConfigurationManagerAttributes { Order = 990 }));
            GlobalAmbientCooldownMult = config.Bind(category, "Global Ambient Cooldown Multiplier", 1f, new ConfigDescription("Global cooldown multiplier for all ambient battle sounds", null, new ConfigurationManagerAttributes { Order = 986 }));
            UseEventConfigWeights = config.Bind(category, "Use Event Weights", true, new ConfigDescription("Use assigned weights when selecting random events", null, new ConfigurationManagerAttributes { Order = 985, IsAdvanced = true }));
            AmbientRolloff = config.Bind(category, "Ambient Rolloff Distance", 1000, new ConfigDescription("Default rolloff distance for ambient battle sounds", null, new ConfigurationManagerAttributes { Order = 980 }));
            OcclusionTestMode = config.Bind(category, "Occlusion Test Mode", EOcclusionTest.ContinuousPropagated, new ConfigDescription("Occlusion test mode for ambient battle sounds. Use either ContinuousPropagated or Fast.", null, new ConfigurationManagerAttributes { Order = 978, IsAdvanced = true }));
            AudioSourceGroup = config.Bind(category, "Audio Source Group", AudioSourceGroupType.Gunshots, new ConfigDescription("Audio source group for ambient battle sounds", null, new ConfigurationManagerAttributes { Order = 976, IsAdvanced = true }));
            PlayAmbientShortcut = config.Bind(category, "Play Ambient Shortcut", new KeyboardShortcut(KeyCode.N), new ConfigDescription("Debug keybind to immediately play battle ambience", null, new ConfigurationManagerAttributes { Order = 970, IsAdvanced = true }));
        }
    }
}
