using Comfort.Common;
using EFT;
using PeinRecoilRework.Helpers;
using SPTBattleAmbience.Config;
using SPTBattleAmbience.Config.General;
using SPTBattleAmbience.Controllers;
using SPTBattleAmbience.Data;
using SPTBattleAmbience.Helpers;
using SPTBattleAmbience.Models;
using SPTBattleAmbience.Models.Maps;
using SPTBattleAmbience.Models.Sounds;
using SPTBattleAmbience.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SPTBattleAmbience.Managers;

public class AmbienceManager
{
    public AmbienceEventConfigGroup EventConfigGroup = null;
    public AmbienceEventConfig NextAmbienceEvent = null;
    public float TimeSinceLastEvent = 0f;
    public float NextEventTime = 0f;
        
    public void ChooseNextAmbience(float cooldownMultiplier = 1f, bool raidJustStarted = false)
    {
        bool useWeight = GeneralConfig.UseEventConfigWeights.Value;
            
        NextAmbienceEvent = EventConfigGroup.GetRandomEventConfig(useWeight);
        TimeSinceLastEvent = 0f;
            
        if (NextAmbienceEvent == null)
        {
            DebugLogger.LogWarning($"couldnt get random event for category {EventConfigGroup.Category} . skipping...");
            TimeSinceLastEvent = 0f;
            NextEventTime = 60f;
            return;
        }
            
        if (raidJustStarted)
        {
            NextEventTime = Random.Range(NextAmbienceEvent.MinimumTimeFromRaidStart, NextAmbienceEvent.MaximumTimeFromRaidStart) * cooldownMultiplier;
        }
        else
        {
            NextEventTime = Random.Range(NextAmbienceEvent.MinimumTimeToNextAmbience, NextAmbienceEvent.MaximumTimeToNextAmbience) * cooldownMultiplier;
        }

        DebugLogger.LogWarning($"Picked next ambience event: {NextAmbienceEvent.Name}");
    }
        
    public void Update(float dt)
    {
        TimeSinceLastEvent += dt;

        if (TimeSinceLastEvent > NextEventTime)
        {
            TriggerAmbience();
        }
    }
        
    private bool TryPrepareAmbience(
        out BattleSoundSequence sequence,
        out MapConfigBase mapConfig,
        out Vector3 soundSpawnPoint,
        out int rolloff)
    {
        sequence = null;
        mapConfig = null;
        soundSpawnPoint = Vector3.zero;
        rolloff = GeneralConfig.AmbientRolloff.Value;

        if (NextAmbienceEvent == null)
        {
            DebugLogger.LogWarning("NextAmbienceEvent is null. Skipping and waiting...");
            TimeSinceLastEvent = 0f;
            NextEventTime = 60f;
            return false;
        }

        Player mainPlayer = GameWorldHelper.GetLocalPlayer();
        string mapId = GameWorldHelper.GetCurrentMapId();
        mapConfig = ConfigHelper.GetMapConfig(mapId);

        if (mapConfig == null || !mapConfig.EnableEvents.Value)
        {
            DebugLogger.LogWarning($"Map {mapId} does not exist in the config or does not have events enabled.");
            TimeSinceLastEvent = 0f;
            NextEventTime = Random.Range(60, 120);
            return false;
        }

        soundSpawnPoint = CalculateSpawnPoint(mainPlayer, mapConfig, NextAmbienceEvent);
        rolloff = NextAmbienceEvent.SoundRolloff > 0 ? NextAmbienceEvent.SoundRolloff : GeneralConfig.AmbientRolloff.Value;
        sequence = GenerateSequence(NextAmbienceEvent);

        return true;
    }
        
        
    public Vector3 CalculateSpawnPoint(Player player, MapConfigBase mapConfig, AmbienceEventConfig ambienceEvent)
    {
        var ambienceController = BattleAmbienceController.Instance;
        
        // if were headless handle spawn point through fika sync plugin
        if (!player || FikaData.IsHeadless)
        {
            return Vector3.zero;
        }
        
        // if map uses sound zones then pick one of those to play audio in
        if (ambienceController.UseZones)
        {
            List<SoundZoneController> soundZones = ambienceController.GetSoundZones();
            SoundZoneController soundZone = soundZones.PickRandom();
            Vector3 pos = soundZone.PickRandomPoint().WithY(0);
            return pos;
        }
         
        // if map doesnt use sound zones then use player direction from center
        if (mapConfig.UsePlayerDirection.Value && ambienceEvent.UsePlayerDirection)
        {
            Vector3 mapCenter = mapConfig.MapCenter.Value;
            float mapRadius = mapConfig.MapRadius.Value;
            Vector3 dirToPlayerFlat = (player.Position - mapCenter).WithY(0).normalized;
            Vector3 soundSpawnDir = ModUtils.GetVectorWithAngleOffset(dirToPlayerFlat, 30f);
            return mapCenter + soundSpawnDir * mapRadius;
        }
        else
        {
            float soundDistance = ambienceEvent.SoundDistance > 0 ? ambienceEvent.SoundDistance : Random.Range(100, 500);
            return player.Position + ModUtils.RandomVector.WithY(0) * soundDistance;
        }
    }

    public void TriggerAmbience()
    {
        if (!TryPrepareAmbience(out var sequence, out var mapConfig, out var soundSpawnPoint, out var rolloff))
        {
            DebugLogger.LogWarning($"Failed to prepare ambience for category {EventConfigGroup.Category}");
            return;
        }

        DebugLogger.LogWarning($"Triggering ambience for map: {GameWorldHelper.GetCurrentMapId()} | Event id: {NextAmbienceEvent.Name} | Position {soundSpawnPoint}");

        BattleAmbienceController.Instance.StartCoroutine(PerformAmbience(sequence, mapConfig, soundSpawnPoint, rolloff));
            
        ChooseNextAmbience(mapConfig.AmbienceEventCooldownMultiplier.Value * GeneralConfig.GlobalAmbientCooldownMult.Value);
    }

    public IEnumerator PerformAmbience(BattleSoundSequence sequence, MapConfigBase mapConfig, Vector3 position, int rolloff)
    {
        DebugLogger.LogWarning("Starting ambience sequence");

        float mapVolumeMult = Random.Range(mapConfig.MinVolumeMultiplier.Value, mapConfig.MaxVolumeMultiplier.Value);
        float globalMult = GeneralConfig.GlobalAmbientVolumeMult.Value;
        float volume = Random.Range(NextAmbienceEvent.MinimumVolume, NextAmbienceEvent.MaximumVolume) * globalMult * mapVolumeMult;

        DebugLogger.LogWarning($"Calculated ambience volume: {volume} (Map Mult: {mapVolumeMult}, Global Mult: {globalMult})");

        foreach (BattleSoundEntry entry in sequence.AudioClips)
        {
            ClipInfo clipInfo = entry.ClipInfo;
            float timeToNextClip = entry.TimeToNextClip;
                
            DebugLogger.LogWarning($"Playing ambience clip: {clipInfo.AudioClip.name} and waiting for: {timeToNextClip} seconds.");

            AmbientHelper.PlayAmbienceSound(position, clipInfo, rolloff, volume);
                
            yield return new WaitForSeconds(timeToNextClip);
        }
    }

    private string[] GetAvailableSoundTypes(string category, AmbienceEventConfig eventConfig)
    {
        string[] configSoundTypes = eventConfig.SoundTypes;

        if (configSoundTypes.Length > 0)
        {
            return configSoundTypes;
        }

        int minTypes = eventConfig.MinimumSoundTypes;
        int maxTypes = eventConfig.MaximumSoundTypes;

        Dictionary<string, AmbientSounds> soundTypeDict = AmbientHelper.AmbientSoundCategories[category].SoundTypes;
        List<string> allSoundTypes = new List<string>();

        foreach (string soundType in soundTypeDict.Keys)
        {
            DebugLogger.LogWarning($"adding sound type {soundType}");
            allSoundTypes.Add(soundType);
        }

        string[] typesArray = allSoundTypes.ToArray();

        DebugLogger.LogWarning($"category: {category}");
        DebugLogger.LogWarning($"soundTypeDict length: {soundTypeDict.Count}");
        DebugLogger.LogWarning($"allSoundTypes: {ModUtils.StringifyArray(typesArray)} | length: {typesArray.Length}");

        int typeCount = Random.Range(minTypes, maxTypes + 1);
        if (typeCount >= typesArray.Length)
        {
            return typesArray;
        }

        return typesArray.GetRandomItems(typeCount);
    }

    private BattleSoundSequence GenerateSequence(AmbienceEventConfig eventConfig)
    {
        string category = EventConfigGroup.Category;
        string[] soundTypes = GetAvailableSoundTypes(category, eventConfig);
            
        int minSounds = eventConfig.MinimumSoundCount;
        int maxSounds = eventConfig.MaximumSoundCount;

        float minGap = eventConfig.MinimumSoundGap;
        float maxGap = eventConfig.MaximumSoundGap;

        DebugLogger.LogWarning("Generating sequence:");
        DebugLogger.LogWarning($"Category: {category}");
        DebugLogger.LogWarning($"SoundTypes: {ModUtils.StringifyArray(soundTypes)}");
        DebugLogger.LogWarning($"minSounds: {minSounds} | maxSounds: {maxSounds}");
        DebugLogger.LogWarning($"minGap: {minGap} | maxGap: {maxGap}");

        List<ClipInfo> availableClips = new();
        List<BattleSoundEntry> selectedClips = [];

        foreach (string soundType in soundTypes)
        {
            if (soundTypes.ContainsKeyword(soundType))
            {
                Dictionary<string, AudioClip> audioClipDict = AmbientHelper.AmbientSoundCategories[category]?.SoundTypes[soundType]?.AudioClips;
                if (audioClipDict == null) continue;

                foreach (KeyValuePair<string, AudioClip> kvp in audioClipDict)
                {
                    availableClips.Add(AmbientHelper.GetClipInfo(category, soundType, kvp.Key));
                }
            }
        }
            
        int soundCount = Random.Range(minSounds, maxSounds + 1);

        for (int i = 0; i < soundCount; i++)
        {
            if (availableClips.Count == 0) break;

            ClipInfo clipInfo = availableClips[Random.Range(0, availableClips.Count)];
            float timeToNextClip = Random.Range(minGap, maxGap);

            selectedClips.Add(new BattleSoundEntry
            {
                ClipInfo = clipInfo,
                TimeToNextClip = timeToNextClip
            });
        }

        return new BattleSoundSequence
        {
            AudioClips = selectedClips
        };
    }
}