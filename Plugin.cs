using BepInEx;
using BepInEx.Logging;
using Newtonsoft.Json;
using PeinRecoilRework.Helpers;
using SPTBattleAmbience.Config.General;
using SPTBattleAmbience.Controllers;
using SPTBattleAmbience.Helpers;
using SPTBattleAmbience.Models.Maps;
using SPTBattleAmbience.Models.Sounds;
using SPTBattleAmbience.Patches;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SPTBattleAmbience
{
    [BepInPlugin("com.pein.battleambience", "SPTBattleAmbience", "2.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static new ManualLogSource Logger;

        private void Awake()
        {
            // Plugin startup logic
            Logger = base.Logger;
            DebugLogger.Logger = Logger;

            ConfigHelper.Initialize(Config);

            CreateAmbienceController();
            LoadAmbientSoundCategories();
            LoadMapConfigs();

            new GameStartedPatch().Enable();
            new OnGameEndedPatch().Enable();
        }

        private void Update()
        {
            if (GeneralConfig.EnableDebug.Value == true && GeneralConfig.PlayAmbientShortcut.Value.IsDown())
            {
                BattleAmbienceController.Instance.AmbienceManagers.Random().TriggerAmbience();
            }
        }

        public void CreateAmbienceController()
        {
            GameObject gameObject = new GameObject("BattleAmbienceController");
            gameObject.AddComponent<BattleAmbienceController>();
            DontDestroyOnLoad(gameObject);
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

                AmbientHelper.MapAmbienceEvents[mapName] = config;
            }
        }
    }
}
