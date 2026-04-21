using BattleAmbienceClient.Config.General;
using BattleAmbienceClient.Controllers;
using BattleAmbienceClient.Helpers;
using BattleAmbienceClient.Models.Maps;
using BattleAmbienceClient.Models.Sounds;
using BattleAmbienceClient.Patches;
using BattleAmbienceClient.Utility;
using BepInEx;
using BepInEx.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BattleAmbienceClient
{
    [BepInPlugin("com.pein.battleambience", "SPTBattleAmbience", "2.2.1")]
    public class Plugin : BaseUnityPlugin
    {
        public static new ManualLogSource Logger;

        private void Awake()
        {
            // Plugin startup logic
            Logger = base.Logger;
            DebugLogger.Logger = Logger;

            ConfigHelper.Initialize(Config);
            
            LoadAmbientSoundCategories();
            LoadMapConfigs();

            new GameStartedPatch().Enable();
            new OnGameEndedPatch().Enable();
            
            FikaGlobals.Initialize();
        }

        private void Update()
        {
            if (GeneralConfig.EnableDebug.Value && GeneralConfig.PlayAmbientShortcut.Value.IsDown())
            {
                BattleAmbienceController.Instance.AmbienceManagers.Random().TriggerAmbience();
            }
        }

        public static void CreateAmbienceController()
        {
            GameObject controllerObject = new GameObject("BattleAmbienceController");
            controllerObject.AddComponent<BattleAmbienceController>();
            DontDestroyOnLoad(controllerObject);
        }

        public static async void LoadAmbientSoundCategories()
        {
            BattleAmbienceController ambienceController = BattleAmbienceController.Instance;
            string[] categoryPaths = FileHelper.ReadDirectories(FileHelper.SoundsPath);

            foreach (string categoryPath in categoryPaths)
            {
                string categoryName = Path.GetFileName(categoryPath);
                string[] soundTypePaths = FileHelper.ReadDirectories(categoryPath);
                DebugLogger.LogWarning($"Loading ambient sound category: {categoryName}");

                AmbientSoundCategory soundCategory = new AmbientSoundCategory();

                foreach (string soundTypePath in soundTypePaths)
                {
                    string soundTypeName = Path.GetFileName(soundTypePath);
                    Dictionary<string, AudioClip> soundClips = await FileHelper.LoadAudioClipsFromDirectory(soundTypePath);
                    DebugLogger.LogWarning($"Loaded sound type: {soundTypeName} with {soundClips.Count} clips");

                    AmbientSounds ambientSounds = new AmbientSounds()
                    {
                        AudioClips = soundClips
                    };

                    soundCategory.SoundTypes[soundTypeName] = ambientSounds;
                }

                AmbientHelper.AmbientSoundCategories[categoryName] = soundCategory;
                DebugLogger.LogWarning($"Finished loading category: {categoryName}");
            }
        }

        public static void LoadMapConfigs()
        {
            string[] mapConfigFiles = FileHelper.ReadFiles(FileHelper.MapConfigsPath, "*.json");

            foreach (string mapConfigFile in mapConfigFiles)
            {
                string mapName = Path.GetFileNameWithoutExtension(mapConfigFile);

                string jsonContent = File.ReadAllText(mapConfigFile);
                AmbienceEvents config = JsonConvert.DeserializeObject<AmbienceEvents>(jsonContent);

                foreach (AmbienceEventConfigGroup configGroup in config.AmbienceEventGroups.Values)
                {
                    foreach (KeyValuePair<string, AmbienceEventConfig> kvp in configGroup.EventConfigs)
                    {
                        kvp.Value.Name = kvp.Key;
                    }
                }

                AmbientHelper.MapAmbienceEvents[mapName] = config;
            }
        }
    }
}
