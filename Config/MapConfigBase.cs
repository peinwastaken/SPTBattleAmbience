using BepInEx.Configuration;
using SPTBattleAmbience.Utility;

namespace SPTBattleAmbience.Config
{
    public class MapConfigBase
    {
        public ConfigEntry<bool> EnableEvents { get; set; }
        public ConfigEntry<float> MinVolumeMultiplier { get; set; }
        public ConfigEntry<float> MaxVolumeMultiplier { get; set; }
        public ConfigEntry<float> AmbienceEventCooldownMultiplier { get; set; }

        public MapConfigBase(ConfigFile config, MapConfigStruct configValues)
        {
            string category = Category.Format(configValues.ConfigOrder, configValues.ConfigCategory);

            EnableEvents = config.Bind(category, "Enable Events", configValues.EnableEvents, new ConfigDescription("Enable battle sound events on this map?", null, new ConfigurationManagerAttributes { Order = 1000 }));
            MinVolumeMultiplier = config.Bind(category, "Min Volume Multiplier", configValues.MinVolumeMultiplier, new ConfigDescription("Minimum volume multiplier for battle sounds on this map.", null, new ConfigurationManagerAttributes { Order = 990 }));
            MaxVolumeMultiplier = config.Bind(category, "Max Volume Multiplier", configValues.MaxVolumeMultiplier, new ConfigDescription("Maximum volume multiplier for battle sounds on this map.", null, new ConfigurationManagerAttributes { Order = 980 }));
            AmbienceEventCooldownMultiplier = config.Bind(category, "Ambience Event Cooldown Multiplier", configValues.AmbienceEventCooldownMultiplier, new ConfigDescription("Cooldown multiplier for ambience events on this map. Higher values increase the average time between ambience events.", null, new ConfigurationManagerAttributes { Order = 970 }));
        }
    }
}
