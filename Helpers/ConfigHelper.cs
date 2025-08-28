using BepInEx.Configuration;
using PeinRecoilRework.Helpers;
using SPTBattleAmbience.Config;
using SPTBattleAmbience.Config.General;
using SPTBattleAmbience.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace SPTBattleAmbience.Helpers
{
    public static class ConfigHelper
    {
        public static Dictionary<string, MapConfigBase> MapConfigs { get; private set; } = new Dictionary<string, MapConfigBase>();

        public static void Initialize(ConfigFile config)
        {
            GeneralConfig.Bind(config);
            InitializeMapConfigs(config);
        }

        private static void InitializeMapConfigs(ConfigFile config)
        {
            MapConfigs["bigmap"] = new MapConfigBase(config, new MapConfigStruct
            {
                ConfigOrder = 5,
                ConfigCategory = Category.Customs,
                MinVolumeMultiplier = 0.7f,
                MaxVolumeMultiplier = 1f,
                AmbienceEventCooldownMultiplier = 1f,
                EnableEvents = true,
                MapCenter = new Vector3(190f, 0, -15f),
                MapRadius = 700f
            });

            MapConfigs["factory4_day"] = new MapConfigBase(config, new MapConfigStruct
            {
                ConfigOrder = 6,
                ConfigCategory = Category.Factory,
                MinVolumeMultiplier = 1f,
                MaxVolumeMultiplier = 1.5f,
                AmbienceEventCooldownMultiplier = 0.8f,
                EnableEvents = true,
                MapCenter = new Vector3(20f, 0, 5f),
                MapRadius = 300f
            });
            MapConfigs["factory4_night"] = MapConfigs["factory4_day"];

            MapConfigs["woods"] = new MapConfigBase(config, new MapConfigStruct
            {
                ConfigOrder = 7,
                ConfigCategory = Category.Woods,
                MinVolumeMultiplier = 0.7f,
                MaxVolumeMultiplier = 1f,
                AmbienceEventCooldownMultiplier = 1.3f,
                EnableEvents = true,
                MapCenter = new Vector3(-30f, 0, -200f),
                MapRadius = 1100f
            });

            MapConfigs["shoreline"] = new MapConfigBase(config, new MapConfigStruct
            {
                ConfigOrder = 8,
                ConfigCategory = Category.Shoreline,
                MinVolumeMultiplier = 0.7f,
                MaxVolumeMultiplier = 1f,
                AmbienceEventCooldownMultiplier = 1f,
                EnableEvents = true,
                MapCenter = new Vector3(-215f, 0, 165f),
                MapRadius = 1000f
            });

            MapConfigs["interchange"] = new MapConfigBase(config, new MapConfigStruct
            {
                ConfigOrder = 9,
                ConfigCategory = Category.Interchange,
                MinVolumeMultiplier = 0.7f,
                MaxVolumeMultiplier = 1f,
                AmbienceEventCooldownMultiplier = 0.8f,
                EnableEvents = true,
                MapCenter = new Vector3(95f, 0, -55f),
                MapRadius = 650f
            });

            MapConfigs["lighthouse"] = new MapConfigBase(config, new MapConfigStruct
            {
                ConfigOrder = 10,
                ConfigCategory = Category.Lighthouse,
                MinVolumeMultiplier = 0.7f,
                MaxVolumeMultiplier = 1f,
                AmbienceEventCooldownMultiplier = 0.9f,
                EnableEvents = true,
                MapCenter = new Vector3(-15, 0, -185),
                MapRadius = 1200f
            });

            MapConfigs["rezervbase"] = new MapConfigBase(config, new MapConfigStruct
            {
                ConfigOrder = 11,
                ConfigCategory = Category.Reserve,
                MinVolumeMultiplier = 0.7f,
                MaxVolumeMultiplier = 1f,
                AmbienceEventCooldownMultiplier = 1f,
                EnableEvents = true,
                MapCenter = new Vector3(-30f, 0, -25f),
                MapRadius = 600f
            });

            MapConfigs["streets"] = new MapConfigBase(config, new MapConfigStruct
            {
                ConfigOrder = 12,
                ConfigCategory = Category.Streets,
                MinVolumeMultiplier = 0.7f,
                MaxVolumeMultiplier = 1f,
                AmbienceEventCooldownMultiplier = 0.6f,
                EnableEvents = true,
                MapCenter = new Vector3(30f, 0, 155f),
                MapRadius = 700f
            });

            MapConfigs["sandbox"] = new MapConfigBase(config, new MapConfigStruct
            {
                ConfigOrder = 13,
                ConfigCategory = Category.GroundZero,
                MinVolumeMultiplier = 0.7f,
                MaxVolumeMultiplier = 1f,
                AmbienceEventCooldownMultiplier = 0.5f,
                EnableEvents = true,
                MapCenter = new Vector3(0, 0, 0),
                MapRadius = 800f
            });
            MapConfigs["sandbox_high"] = MapConfigs["sandbox"];

            MapConfigs["laboratory"] = new MapConfigBase(config, new MapConfigStruct
            {
                ConfigOrder = 14,
                ConfigCategory = Category.Labs,
                MinVolumeMultiplier = 0.7f,
                MaxVolumeMultiplier = 1f,
                AmbienceEventCooldownMultiplier = 1f,
                EnableEvents = false,
                MapCenter = new Vector3(0, 0, 0),
                MapRadius = 1000f
            });

            DebugLogger.LogWarning($"Initialized {MapConfigs.Count} map configs");
        }

        public static MapConfigBase GetMapConfig(string mapId)
        {
            MapConfigs.TryGetValue(mapId.ToLower(), out MapConfigBase config);

            if (config == null)
            {
                Plugin.Logger.LogError("Couldnt find map config for location: " + mapId);
            }

            return config;
        }
    }
}
