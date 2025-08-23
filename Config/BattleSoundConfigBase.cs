using BepInEx.Configuration;
using SPTBattleAmbience.Utility;

namespace SPTBattleAmbience.Config
{
    public abstract class BattleSoundConfigBase
    {
        public ConfigEntry<bool> IsGunshotEvent { get; set; }
        public ConfigEntry<int> MinimumBattleSoundTypes { get; set; }
        public ConfigEntry<int> MaximumBattleSoundTypes { get; set; }
        public ConfigEntry<float> MinimumTimeToNextAmbience { get; set; }
        public ConfigEntry<float> MaximumTimeToNextAmbience { get; set; }
        public ConfigEntry<int> MinimumSoundCount { get; set; }
        public ConfigEntry<int> MaximumSoundCount { get; set; }
        public ConfigEntry<float> MinimumSoundGap { get; set; }
        public ConfigEntry<float> MaximumSoundGap { get; set; }
        public ConfigEntry<float> MinimumVolume { get; set; }
        public ConfigEntry<float> MaximumVolume { get; set; }

        public BattleSoundConfigBase(ConfigFile config, BattleSoundConfigStruct configValues)
        {
            string category = Category.Format(configValues.ConfigOrder, configValues.ConfigCategory);

            IsGunshotEvent = config.Bind(category, "Is Gunshot Event", configValues.IsGunshotEvent, new ConfigDescription("Is this a gunshot event?", null, new ConfigurationManagerAttributes { Order = 1000, IsAdvanced = true }));
            MinimumBattleSoundTypes = config.Bind(category, "Minimum Battle Sound Types", configValues.MinimumBattleSoundTypes, new ConfigDescription("Minimum number of different sound types to play per ambience event", null, new ConfigurationManagerAttributes { Order = 990 }));
            MaximumBattleSoundTypes = config.Bind(category, "Maximum Battle Sound Types", configValues.MaximumBattleSoundTypes, new ConfigDescription("Maximum number of different sound types to play per ambience event", null, new ConfigurationManagerAttributes { Order = 980 }));
            MinimumTimeToNextAmbience = config.Bind(category, "Minimum Time To Next Ambience", configValues.MinimumTimeToNextAmbience, new ConfigDescription("Minimum time in seconds before the next ambience event can play", null, new ConfigurationManagerAttributes { Order = 970 }));
            MaximumTimeToNextAmbience = config.Bind(category, "Maximum Time To Next Ambience", configValues.MaximumTimeToNextAmbience, new ConfigDescription("Maximum time in seconds before the next ambience event can play", null, new ConfigurationManagerAttributes { Order = 960 }));
            MinimumSoundCount = config.Bind(category, "Minimum Sound Count", configValues.MinimumSoundCount, new ConfigDescription("Minimum number of sounds to play per ambience event", null, new ConfigurationManagerAttributes { Order = 950 }));
            MaximumSoundCount = config.Bind(category, "Maximum Sound Count", configValues.MaximumSoundCount, new ConfigDescription("Maximum number of sounds to play per ambience event", null, new ConfigurationManagerAttributes { Order = 940 }));
            MinimumSoundGap = config.Bind(category, "Minimum Sound Gap", configValues.MinimumSoundGap, new ConfigDescription("Minimum time in seconds between each sound played during an ambience event", null, new ConfigurationManagerAttributes { Order = 930 }));
            MaximumSoundGap = config.Bind(category, "Maximum Sound Gap", configValues.MaximumSoundGap, new ConfigDescription("Maximum time in seconds between each sound played during an ambience event", null, new ConfigurationManagerAttributes { Order = 920 }));
            MinimumVolume = config.Bind(category, "Minimum Volume", configValues.MinimumVolume, new ConfigDescription("Minimum volume for each sound played during an ambience event", null, new ConfigurationManagerAttributes { Order = 910 }));
            MaximumVolume = config.Bind(category, "Maximum Volume", configValues.MaximumVolume, new ConfigDescription("Maximum volume for each sound played during an ambience event", null, new ConfigurationManagerAttributes { Order = 900 }));
        }
    }
}
