using BepInEx.Configuration;
using SPTBattleAmbience.Utility;
using UnityEngine;

namespace SPTBattleAmbience.Config
{
    public class MapConfigBase
    {
        public MapConfigStruct MapDefaultData;
        public ConfigEntry<bool> EnableEvents { get; set; }
        public ConfigEntry<float> MinVolumeMultiplier { get; set; }
        public ConfigEntry<float> MaxVolumeMultiplier { get; set; }
        public ConfigEntry<float> AmbienceEventCooldownMultiplier { get; set; }
        public ConfigEntry<Vector3> MapCenter { get; set; }
        public ConfigEntry<float> MapRadius { get; set; }
        public ConfigEntry<bool> UsePlayerDirection { get; set; }

        public MapConfigBase(ConfigFile config, MapConfigStruct configValues)
        {
            string category = Category.Format(configValues.ConfigOrder, configValues.ConfigCategory);

            EnableEvents = config.Bind(category, "Enable Events", configValues.EnableEvents, new ConfigDescription("Enable battle sound events on this map?", null, new ConfigurationManagerAttributes { Order = 1000 }));
            MinVolumeMultiplier = config.Bind(category, "Min Volume Multiplier", configValues.MinVolumeMultiplier, new ConfigDescription("Minimum volume multiplier for battle sounds on this map.", null, new ConfigurationManagerAttributes { Order = 990 }));
            MaxVolumeMultiplier = config.Bind(category, "Max Volume Multiplier", configValues.MaxVolumeMultiplier, new ConfigDescription("Maximum volume multiplier for battle sounds on this map.", null, new ConfigurationManagerAttributes { Order = 980 }));
            AmbienceEventCooldownMultiplier = config.Bind(category, "Ambience Event Cooldown Multiplier", configValues.AmbienceEventCooldownMultiplier, new ConfigDescription("Cooldown multiplier for ambience events on this map. Higher values increase the average time between ambience events.", null, new ConfigurationManagerAttributes { Order = 970 }));
            MapCenter = config.Bind(category, "Map Center", configValues.MapCenter, new ConfigDescription("Center point of the map for spawning battle sounds.", null, new ConfigurationManagerAttributes { Order = 960, IsAdvanced = true }));
            MapRadius = config.Bind(category, "Map Radius", configValues.MapRadius, new ConfigDescription("Radius from the map center to spawn battle sounds.", null, new ConfigurationManagerAttributes { Order = 950, IsAdvanced = true }));
            UsePlayerDirection = config.Bind(category, "Use Player Direction", configValues.UsePlayerDirection, new ConfigDescription("If enabled, battle sounds will spawn in the rough direction the player is facing relative to the center of the map. If disabled, battle sounds will spawn randomly around the player.", null, new ConfigurationManagerAttributes { Order = 940, IsAdvanced = true }));

            MapDefaultData = configValues;
        }
    }
}
