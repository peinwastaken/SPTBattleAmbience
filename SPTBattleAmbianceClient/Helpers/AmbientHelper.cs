using Comfort.Common;
using Newtonsoft.Json;
using PeinRecoilRework.Helpers;
using SPTBattleAmbience.Config.General;
using SPTBattleAmbience.Data;
using SPTBattleAmbience.Models.Maps;
using SPTBattleAmbience.Models.Sounds;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SPTBattleAmbience.Helpers;

public static class AmbientHelper
{
    // configs/maps/mapname.json
    public static Dictionary<string, AmbienceEvents> MapAmbienceEvents = [];

    // sounds/category/soundtype/sound.wav
    public static Dictionary<string, AmbientSoundCategory> AmbientSoundCategories = [];
    
    public static Dictionary<string, ClipInfo> LoadedAudioClips = [];

    public static string ClipInfoPath(string category, string soundType, string fileName) => $"{category}/{soundType}/{fileName}";

    public static ClipInfo GetClipInfo(string category, string soundType, string fileName)
    {
        LoadedAudioClips.TryGetValue(ClipInfoPath(category, soundType, fileName), out ClipInfo clipInfo);
        return clipInfo;
    }
    
    public static AmbienceEvents GetMapAmbienceEvents(string mapName)
    {
        MapAmbienceEvents.TryGetValue(mapName, out AmbienceEvents ambienceEvents);
        return ambienceEvents;
    }

    public static void PlayAmbienceSound(Vector3 position, ClipInfo clipInfo, int rolloff, float volume)
    {
        if (clipInfo == null || !clipInfo.AudioClip)
        {
            DebugLogger.LogError($"Could not play ambience event at position {position}");
            return;
        }
        
        DebugLogger.LogInfo($"playing sound: {clipInfo.ClipName} at position {position}");
            
        Singleton<BetterAudio>.Instance.PlayAtPoint(
            position,
            clipInfo.AudioClip,
            GeneralConfig.AudioSourceGroup.Value,
            rolloff,
            volume,
            GeneralConfig.OcclusionTestMode.Value,
            null,
            true,
            true
        );
    }
    
    public static async void LoadAmbientSoundCategories()
    {
        try
        {
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

                    foreach ((string name, AudioClip clip) in soundClips)
                    {
                        LoadedAudioClips[ClipInfoPath(categoryName, soundTypeName, name)] = new ClipInfo
                        {
                            SoundType = soundTypeName,
                            Category = categoryName,
                            ClipName = name,
                            AudioClip = clip
                        };
                    }

                    soundCategory.SoundTypes[soundTypeName] = ambientSounds;
                }

                AmbientSoundCategories[categoryName] = soundCategory;
                DebugLogger.LogWarning($"Finished loading category: {categoryName}");
            }
        }
        catch (Exception e)
        {
            DebugLogger.LogError(e.Message);
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

            MapAmbienceEvents[mapName] = config;
        }
    }
}