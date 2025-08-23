using PeinRecoilRework.Helpers;
using SPTBattleAmbience.Data.Enum;
using SPTBattleAmbience.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace SPTBattleAmbience.Models
{
    public class BattleSound
    {
        public EBattleSoundType Type { get; set; }
        public Dictionary<string, AudioClip> AudioClips { get; set; }

        public BattleSound(EBattleSoundType type, string audioPath, bool isGunshot = false)
        {
            DebugLogger.LogInfo($"Loading BattleSound of type {type} from path: {audioPath}");

            Type = type;
            AudioClips = new Dictionary<string, AudioClip>();

            FileHelper.LoadAudioClipsFromDirectory(audioPath, AudioClips);
        }

        public void AddAudioClip(string key, AudioClip clip)
        {
            AudioClips[key] = clip;
        }

        public AudioClip GetAudioClip(string key)
        {
            AudioClip clip;
            AudioClips.TryGetValue(key, out clip);
            return clip;
        }

        public AudioClip GetRandomAudioClip()
        {
            AudioClip clip = AudioClips.PickRandom().Value;
            return clip;
        }
    }
}
