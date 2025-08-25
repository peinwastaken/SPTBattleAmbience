using Comfort.Common;
using EFT;
using PeinRecoilRework.Helpers;
using SPTBattleAmbience.Utility;
using SPTBattleAmbience.Config;
using SPTBattleAmbience.Config.General;
using SPTBattleAmbience.Data.Enum;
using SPTBattleAmbience.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SPTBattleAmbience.Helpers;

namespace SPTBattleAmbience.Controllers
{
    public class BattleAmbienceController : MonoBehaviour
    {
        public static BattleAmbienceController Instance { get; private set; }
        public Dictionary<EBattleSoundType, BattleSound> BattleSounds = new Dictionary<EBattleSoundType, BattleSound>();

        public bool IsPlayingAmbience = false;

        private float _nextAmbienceTime = 0f;
        private float _timeSinceLastAmbience = 0f;
        private bool _gameStarted = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            _nextAmbienceTime = 9999;
        }

        public void OnGameStarted()
        {
            _timeSinceLastAmbience = 0f;
            _nextAmbienceTime = Random.Range(20, 150);

            _gameStarted = true;
        }

        public void OnGameEnded()
        {
            IsPlayingAmbience = false;
            _timeSinceLastAmbience = 0f;

            _gameStarted = false;
        }

        public void Update()
        {
            if (_gameStarted)
            {
                _timeSinceLastAmbience += Time.deltaTime;

                if (_timeSinceLastAmbience >= _nextAmbienceTime)
                {
                    TriggerAmbience();
                }
            }
        }

        public static void Initialize()
        {
            if (Instance == null)
            {
                GameObject controllerObject = new GameObject("BattleAmbienceController");
                Instance = controllerObject.AddComponent<BattleAmbienceController>();

                DontDestroyOnLoad(controllerObject);
            }
        }

        public void AddBattleSound(EBattleSoundType soundType, BattleSound sound)
        {
            BattleSounds[soundType] = sound;
        }

        public void TriggerAmbience()
        {
            if (IsPlayingAmbience) return;

            Player mainPlayer = Singleton<GameWorld>.Instance.MainPlayer;
            ELocation currentLocation = Utils.GetLocationEnum(mainPlayer.Location);
            MapConfigBase mapConfig = ConfigHelper.GetMapConfig(currentLocation);

            if (mapConfig == null || !mapConfig.EnableEvents.Value)
            {
                DebugLogger.LogWarning($"Map {currentLocation} does not exist in the config or does not have events enabled. Skipping ambience event.");
                _timeSinceLastAmbience = 0f;
                _nextAmbienceTime = Random.Range(60, 120);
                return;
            }

            Vector3 soundSpawnPoint;
            if (mapConfig.UsePlayerDirection.Value)
            {
                Vector3 mapCenter = mapConfig.MapCenter.Value;
                float mapRadius = mapConfig.MapRadius.Value;
                Vector3 dirToPlayerFlat = (mainPlayer.Position - mapCenter).WithY(0).normalized;
                Vector3 soundSpawnDir = Utils.GetVectorWithAngleOffset(dirToPlayerFlat, 30f);
                soundSpawnPoint = mapCenter + soundSpawnDir * mapRadius;
            }
            else
            {
                soundSpawnPoint = mainPlayer.Position + Utils.RandomVector.WithY(0) * 30f;
            }

            DebugLogger.LogWarning($"Triggering ambience for map: {currentLocation} | Position {soundSpawnPoint}");

            ESoundEvent soundEvent = BattleSoundHelper.GetRandomSoundEvent();
            BattleSoundConfigBase soundConfig = ConfigHelper.GetSoundConfig(soundEvent);
            bool isGunshotEvent = soundConfig.IsGunshotEvent.Value;
            EBattleSoundType[] soundTypes = isGunshotEvent ? BattleSoundHelper.GunShotTypes : new EBattleSoundType[] { EBattleSoundType.Artillery };

            _timeSinceLastAmbience = 0f;
            _nextAmbienceTime = Random.Range(soundConfig.MinimumTimeToNextAmbience.Value, soundConfig.MaximumTimeToNextAmbience.Value) * mapConfig.AmbienceEventCooldownMultiplier.Value;
            IsPlayingAmbience = true;

            BattleSoundSequence sequence = GenerateSequence(
                soundTypes,
                soundConfig.MinimumSoundCount.Value,
                soundConfig.MaximumSoundCount.Value,
                soundConfig.MinimumSoundGap.Value,
                soundConfig.MaximumSoundGap.Value
            );

            StartCoroutine(PerformAmbience(sequence, mapConfig, soundSpawnPoint, soundConfig.MinimumVolume.Value, soundConfig.MaximumVolume.Value));
        }

        public BattleSoundSequence GenerateSequence(EBattleSoundType[] soundTypes, int minClips, int maxClips, float minGap, float maxGap)
        {
            int clipCount = Random.Range(minClips, maxClips + 1);

            BattleSoundSequence sequence = new BattleSoundSequence();

            List<KeyValuePair<AudioClip, float>> selectedClips = [];

            // get all available clips from the specified sound types
            List<AudioClip> availableClips = new List<AudioClip>();
            foreach (EBattleSoundType type in soundTypes)
            {
                if (BattleSounds.ContainsKey(type))
                {
                    availableClips.AddRange(BattleSounds[type].AudioClips.Values);
                }
            }

            // randomly select clips and assign time offsets until we reach the desired count of clips
            for (int i = 0; i < clipCount; i++)
            {
                if (availableClips.Count == 0) break;

                AudioClip clip = availableClips[Random.Range(0, availableClips.Count)];
                float timeToNextClip = Random.Range(minGap, maxGap);

                selectedClips.Add(new KeyValuePair<AudioClip, float>(clip, timeToNextClip));
            }

            sequence.AudioClips = selectedClips;

            return sequence;
        }

        private IEnumerator PerformAmbience(BattleSoundSequence sequence, MapConfigBase mapConfig, Vector3 position, float minVolume, float maxVolume)
        {
            DebugLogger.LogWarning("Starting ambience coroutine.");

            float mapVolumeMult = Random.Range(mapConfig.MinVolumeMultiplier.Value, mapConfig.MaxVolumeMultiplier.Value);
            float globalMult = GeneralConfig.GlobalAmbientVolumeMult.Value;
            float volume = Random.Range(minVolume, maxVolume) * globalMult * mapVolumeMult;

            DebugLogger.LogWarning($"Calculated ambience volume: {volume} (Map Mult: {mapVolumeMult}, Global Mult: {globalMult})");

            foreach (KeyValuePair<AudioClip, float> clipInfo in sequence.AudioClips)
            {
                DebugLogger.LogWarning($"Playing ambience clip: {clipInfo.Key.name} and waiting for: {clipInfo.Value} seconds.");

                Singleton<BetterAudio>.Instance.PlayAtPoint(
                    position,
                    clipInfo.Key,
                    GeneralConfig.AudioSourceGroup.Value,
                    GeneralConfig.AmbientRolloff.Value,
                    volume,
                    GeneralConfig.OcclusionTestMode.Value,
                    null,
                    true,
                    true
                );

                yield return new WaitForSeconds(clipInfo.Value);
            }

            IsPlayingAmbience = false;
        }
    }
}
