using System.Collections.Generic;
using UnityEngine;

namespace BattleAmbienceClient.Models.Sounds
{
    public class AmbientSounds
    {
        public Dictionary<string, AudioClip> AudioClips;

        public AudioClip GetRandomAudioClip()
        {
            return AudioClips.PickRandom().Value;
        }
    }
}
