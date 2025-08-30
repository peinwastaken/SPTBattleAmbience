using Comfort.Common;
using EFT;
using PeinRecoilRework.Helpers;
using SPTBattleAmbience.Config;
using SPTBattleAmbience.Config.General;
using SPTBattleAmbience.Controllers;
using SPTBattleAmbience.Helpers;
using SPTBattleAmbience.Models;
using SPTBattleAmbience.Models.Maps;
using SPTBattleAmbience.Models.Sounds;
using SPTBattleAmbience.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SPTBattleAmbience.Managers
{
    public class AmbienceManager
    {
        public AmbienceEventConfigGroup EventConfigGroup = null;
        public AmbienceEventConfig NextAmbienceEvent = null;
        public float TimeSinceLastEvent = 0f;
        public float NextEventTime = 0f;
        
        public void ChooseNextAmbience(float cooldownMultiplier = 1f, bool raidJustStarted = false)
        {
            NextAmbienceEvent = EventConfigGroup.GetRandomEventConfig();
            TimeSinceLastEvent = 0f;
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

        public void TriggerAmbience()
        {
            if (NextAmbienceEvent == null)
            {
                DebugLogger.LogError("NextAmbienceEvent is null. Skipping and waiting...");
                TimeSinceLastEvent = 0f;
                NextEventTime = 60f;
                return;
            }
            
            Player mainPlayer = GameWorldHelper.GetLocalPlayer();
            string mapId = GameWorldHelper.GetCurrentMapId();
            MapConfigBase mapConfig = ConfigHelper.GetMapConfig(mapId);

            if (mapConfig == null || !mapConfig.EnableEvents.Value)
            {
                DebugLogger.LogWarning($"Map {mapId} does not exist in the config or does not have events enabled.");
                TimeSinceLastEvent = 0f;
                NextEventTime = Random.Range(60, 120);
                return;
            }
            
            Vector3 soundSpawnPoint;
            if (mapConfig.UsePlayerDirection.Value && NextAmbienceEvent.UsePlayerDirection)
            {
                Vector3 mapCenter = mapConfig.MapCenter.Value;
                float mapRadius = mapConfig.MapRadius.Value;
                Vector3 dirToPlayerFlat = (mainPlayer.Position - mapCenter).WithY(0).normalized;
                Vector3 soundSpawnDir = Utils.GetVectorWithAngleOffset(dirToPlayerFlat, 30f);
                soundSpawnPoint = mapCenter + soundSpawnDir * mapRadius;
            }
            else
            {
                float soundDistance = NextAmbienceEvent.SoundDistance > 0 ? NextAmbienceEvent.SoundDistance : Random.Range(100, 500);

                soundSpawnPoint = mainPlayer.Position + Utils.RandomVector.WithY(0) * soundDistance;
            }

            int rolloff = NextAmbienceEvent.SoundRolloff > 0 ? NextAmbienceEvent.SoundRolloff : GeneralConfig.AmbientRolloff.Value;

            DebugLogger.LogWarning($"Triggering ambience for map: {mapId} | Event id: {NextAmbienceEvent.Name} | Position {soundSpawnPoint}");

            BattleSoundSequence sequence = GenerateSequence(NextAmbienceEvent);

            BattleAmbienceController.Instance.StartCoroutine(PerformAmbience(sequence, mapConfig, soundSpawnPoint, rolloff));

            ChooseNextAmbience(mapConfig.AmbienceEventCooldownMultiplier.Value);
        }

        private IEnumerator PerformAmbience(BattleSoundSequence sequence, MapConfigBase mapConfig, Vector3 position, int rolloff)
        {
            DebugLogger.LogWarning("Starting ambience sequence");

            float mapVolumeMult = Random.Range(mapConfig.MinVolumeMultiplier.Value, mapConfig.MaxVolumeMultiplier.Value);
            float globalMult = GeneralConfig.GlobalAmbientVolumeMult.Value;
            float volume = Random.Range(NextAmbienceEvent.MinimumVolume, NextAmbienceEvent.MaximumVolume) * globalMult * mapVolumeMult;

            DebugLogger.LogWarning($"Calculated ambience volume: {volume} (Map Mult: {mapVolumeMult}, Global Mult: {globalMult})");

            foreach (KeyValuePair<AudioClip, float> clipInfo in sequence.AudioClips)
            {
                DebugLogger.LogWarning($"Playing ambience clip: {clipInfo.Key.name} and waiting for: {clipInfo.Value} seconds.");

                Singleton<BetterAudio>.Instance.PlayAtPoint(
                    position,
                    clipInfo.Key,
                    GeneralConfig.AudioSourceGroup.Value,
                    rolloff,
                    volume,
                    GeneralConfig.OcclusionTestMode.Value,
                    null,
                    true,
                    true
                );

                yield return new WaitForSeconds(clipInfo.Value);
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
            DebugLogger.LogWarning($"allSoundTypes: {Utils.StringifyArray(typesArray)} | length: {typesArray.Length}");

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
            DebugLogger.LogWarning($"SoundTypes: {Utils.StringifyArray(soundTypes)}");
            DebugLogger.LogWarning($"minSounds: {minSounds} | maxSounds: {maxSounds}");
            DebugLogger.LogWarning($"minGap: {minGap} | maxGap: {maxGap}");

            List<AudioClip> availableClips = [];
            List<KeyValuePair<AudioClip, float>> selectedClips = [];

            foreach (string soundType in soundTypes)
            {
                if (soundTypes.ContainsKeyword(soundType))
                {
                    Dictionary<string, AudioClip> audioClipDict = AmbientHelper.AmbientSoundCategories[category]?.SoundTypes[soundType]?.AudioClips;
                    if (audioClipDict == null) continue;

                    List<AudioClip> ambienceClips = new List<AudioClip>();

                    foreach (KeyValuePair<string, AudioClip> kvp in audioClipDict)
                    {
                        ambienceClips.Add(kvp.Value);
                    }

                    availableClips.AddRange(ambienceClips);
                }
            }

            int soundCount = Random.Range(minSounds, maxSounds + 1);

            for (int i = 0; i < soundCount; i++)
            {
                if (availableClips.Count == 0) break;

                AudioClip clip = availableClips[Random.Range(0, availableClips.Count)];
                float timeToNextClip = Random.Range(minGap, maxGap);

                selectedClips.Add(new KeyValuePair<AudioClip, float>(clip, timeToNextClip));
            }

            return new BattleSoundSequence()
            {
                AudioClips = selectedClips
            };
        }
    }
}
