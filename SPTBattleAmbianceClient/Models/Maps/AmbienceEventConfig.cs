using Newtonsoft.Json;
using SPTBattleAmbience.Data.Enum;

namespace SPTBattleAmbience.Models.Maps
{
    public class AmbienceEventConfig
    {
        [JsonIgnore]
        public string Name { get; set; }

        [JsonProperty("usePlayerDirection")]
        public bool UsePlayerDirection { get; set; } = true;

        [JsonProperty("soundDistance")]
        public float SoundDistance { get; set; } = 0;

        [JsonProperty("soundRolloff")]
        public int SoundRolloff { get; set; } = 0;

        [JsonProperty("soundTypes")]
        public string[] SoundTypes { get; set; } = [];

        [JsonProperty("weight")]
        public int Weight { get; set; } = 1;

        [JsonProperty("minimumSoundTypes")]
        public int MinimumSoundTypes { get; set; } = 1;

        [JsonProperty("maximumSoundTypes")]
        public int MaximumSoundTypes { get; set; } = 3;

        [JsonProperty("minimumSoundCount")]
        public int MinimumSoundCount { get; set; } = 1;

        [JsonProperty("maximumSoundCount")]
        public int MaximumSoundCount { get; set; } = 5;

        [JsonProperty("minimumSoundGap")]
        public float MinimumSoundGap { get; set; } = 0.5f;

        [JsonProperty("maximumSoundGap")]
        public float MaximumSoundGap { get; set; } = 2f;

        [JsonProperty("minimumVolume")]
        public float MinimumVolume { get; set; } = 0.7f;

        [JsonProperty("maximumVolume")]
        public float MaximumVolume { get; set; } = 1f;

        [JsonProperty("minimumTimeToNextAmbience")]
        public float MinimumTimeToNextAmbience { get; set; } = 30f;

        [JsonProperty("maximumTimeToNextAmbience")]
        public float MaximumTimeToNextAmbience { get; set; } = 240f;

        [JsonProperty("minimumTimeFromRaidStart")]
        public float MinimumTimeFromRaidStart { get; set; } = 15f;

        [JsonProperty("maximumTimeFromRaidStart")]
        public float MaximumTimeFromRaidStart { get; set; } = 120f;

        [JsonProperty("ambientDelayTimeMult")]
        public float AmbientDelayTimeMult { get; set; } = 1f;

        [JsonProperty("timeRestriction")]
        public ETimeRestriction TimeRestriction { get; set; } = ETimeRestriction.Always;
    }
}
