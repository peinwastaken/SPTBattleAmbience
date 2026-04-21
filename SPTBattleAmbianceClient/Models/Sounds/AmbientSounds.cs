using System.Collections.Generic;
using UnityEngine;

namespace SPTBattleAmbience.Models.Sounds
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
