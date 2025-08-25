using BepInEx.Configuration;
using PeinRecoilRework.Helpers;
using SPTBattleAmbience.Config;
using SPTBattleAmbience.Config.Events;
using SPTBattleAmbience.Config.General;
using SPTBattleAmbience.Data.Enum;
using SPTBattleAmbience.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace SPTBattleAmbience.Helpers
{
    public static class ConfigHelper
    {
        public static Dictionary<ESoundEvent, BattleSoundConfigBase> SoundEventConfigs = new Dictionary<ESoundEvent, BattleSoundConfigBase>();
        public static Dictionary<ELocation, MapConfigBase> MapConfigs = new Dictionary<ELocation, MapConfigBase>();

        public static void Initialize(ConfigFile config)
        {
            GeneralConfig.Bind(config);
            InitializeMapConfigs(config);
            InitializeEventConfigs(config);
        }

        private static void InitializeEventConfigs(ConfigFile config)
        {
            SoundEventConfigs[ESoundEvent.SingleShot] = new SingleShotConfig(config);
            SoundEventConfigs[ESoundEvent.Firefight] = new FirefightConfig(config);
            SoundEventConfigs[ESoundEvent.IntenseFirefight] = new IntenseFirefightConfig(config);
            SoundEventConfigs[ESoundEvent.Artillery] = new ArtilleryEventConfig(config);
        }

        private static void InitializeMapConfigs(ConfigFile config)
        {
            MapConfigs[ELocation.Customs] = new MapConfigBase(config, new MapConfigStruct
            {
                Location = ELocation.Customs,
                ConfigOrder = 5,
                ConfigCategory = Category.Customs,
                MinVolumeMultiplier = 0.7f,
                MaxVolumeMultiplier = 1f,
                AmbienceEventCooldownMultiplier = 1f,
                EnableEvents = true,
                MapCenter = new Vector3(190f, 0, -15f),
                MapRadius = 700f,
                UsePlayerDirection = true,
            });

            MapConfigs[ELocation.Factory] = new MapConfigBase(config, new MapConfigStruct
            {
                Location = ELocation.Factory,
                ConfigOrder = 6,
                ConfigCategory = Category.Factory,
                MinVolumeMultiplier = 1f,
                MaxVolumeMultiplier = 1.5f,
                AmbienceEventCooldownMultiplier = 0.8f,
                EnableEvents = true,
                MapCenter = new Vector3(20f, 0, 5f),
                MapRadius = 300f,
                UsePlayerDirection = true,
            });

            MapConfigs[ELocation.Woods] = new MapConfigBase(config, new MapConfigStruct
            {
                Location = ELocation.Woods,
                ConfigOrder = 7,
                ConfigCategory = Category.Woods,
                MinVolumeMultiplier = 0.7f,
                MaxVolumeMultiplier = 1f,
                AmbienceEventCooldownMultiplier = 1.3f,
                EnableEvents = true,
                MapCenter = new Vector3(-30f, 0, -200f),
                MapRadius = 1100f,
                UsePlayerDirection = true,
            });

            MapConfigs[ELocation.Shoreline] = new MapConfigBase(config, new MapConfigStruct
            {
                Location = ELocation.Shoreline,
                ConfigOrder = 8,
                ConfigCategory = Category.Shoreline,
                MinVolumeMultiplier = 0.7f,
                MaxVolumeMultiplier = 1f,
                AmbienceEventCooldownMultiplier = 1f,
                EnableEvents = true,
                MapCenter = new Vector3(-215f, 0, 165f),
                MapRadius = 1000f,
                UsePlayerDirection = true,
            });

            MapConfigs[ELocation.Interchange] = new MapConfigBase(config, new MapConfigStruct
            {
                Location = ELocation.Interchange,
                ConfigOrder = 9,
                ConfigCategory = Category.Interchange,
                MinVolumeMultiplier = 0.7f,
                MaxVolumeMultiplier = 1f,
                AmbienceEventCooldownMultiplier = 0.8f,
                EnableEvents = true,
                MapCenter = new Vector3(95f, 0, -55f),
                MapRadius = 650f,
                UsePlayerDirection = true,
            });

            MapConfigs[ELocation.Lighthouse] = new MapConfigBase(config, new MapConfigStruct
            {
                Location = ELocation.Lighthouse,
                ConfigOrder = 10,
                ConfigCategory = Category.Lighthouse,
                MinVolumeMultiplier = 0.7f,
                MaxVolumeMultiplier = 1f,
                AmbienceEventCooldownMultiplier = 0.9f,
                EnableEvents = true,
                MapCenter = new Vector3(-15, 0, -185),
                MapRadius = 1200f,
                UsePlayerDirection = true,
            });

            MapConfigs[ELocation.Reserve] = new MapConfigBase(config, new MapConfigStruct
            {
                Location = ELocation.Reserve,
                ConfigOrder = 11,
                ConfigCategory = Category.Reserve,
                MinVolumeMultiplier = 0.7f,
                MaxVolumeMultiplier = 1f,
                AmbienceEventCooldownMultiplier = 1f,
                EnableEvents = true,
                MapCenter = new Vector3(-30f, 0, -25f),
                MapRadius = 600f,
                UsePlayerDirection = true,
            });

            MapConfigs[ELocation.Streets] = new MapConfigBase(config, new MapConfigStruct
            {
                Location = ELocation.Streets,
                ConfigOrder = 12,
                ConfigCategory = Category.Streets,
                MinVolumeMultiplier = 0.7f,
                MaxVolumeMultiplier = 1f,
                AmbienceEventCooldownMultiplier = 0.6f,
                EnableEvents = true,
                MapCenter = new Vector3(30f, 0, 155f),
                MapRadius = 700f,
                UsePlayerDirection = true,
            });

            MapConfigs[ELocation.GroundZero] = new MapConfigBase(config, new MapConfigStruct
            {
                Location = ELocation.GroundZero,
                ConfigOrder = 13,
                ConfigCategory = Category.GroundZero,
                MinVolumeMultiplier = 0.7f,
                MaxVolumeMultiplier = 1f,
                AmbienceEventCooldownMultiplier = 0.5f,
                EnableEvents = true,
                MapCenter = new Vector3(0, 0, 0),
                MapRadius = 800f,
                UsePlayerDirection = true,
            });

            MapConfigs[ELocation.Labs] = new MapConfigBase(config, new MapConfigStruct
            {
                Location = ELocation.Labs,
                ConfigOrder = 14,
                ConfigCategory = Category.Labs,
                MinVolumeMultiplier = 0.7f,
                MaxVolumeMultiplier = 1f,
                AmbienceEventCooldownMultiplier = 1f,
                EnableEvents = false,
                MapCenter = new Vector3(0, 0, 0),
                MapRadius = 1000f,
                UsePlayerDirection = false,
            });

            DebugLogger.LogWarning($"Initialized {MapConfigs.Count} map configs");
        }

        public static BattleSoundConfigBase GetSoundConfig(ESoundEvent soundEvent)
        {
            SoundEventConfigs.TryGetValue(soundEvent, out BattleSoundConfigBase config);
            return config;
        }

        public static MapConfigBase GetMapConfig(ELocation location)
        {
            MapConfigs.TryGetValue(location, out MapConfigBase config);

            if (config == null)
            {
                Plugin.Logger.LogError("Couldnt find map config for location: " + location);
            }

            return config;
        }
    }
}
