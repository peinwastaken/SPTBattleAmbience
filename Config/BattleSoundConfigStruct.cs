using SPTBattleAmbience.Data.Enum;

namespace SPTBattleAmbience.Config
{
    public struct BattleSoundConfigStruct
    {
        public ESoundEvent SoundEvent;
        public int ConfigOrder;
        public string ConfigCategory;
        public bool IsGunshotEvent;
        public int MinimumBattleSoundTypes;
        public int MaximumBattleSoundTypes;
        public float MinimumTimeToNextAmbience;
        public float MaximumTimeToNextAmbience;
        public int MinimumSoundCount;
        public int MaximumSoundCount;
        public float MinimumSoundGap;
        public float MaximumSoundGap;
        public float MinimumVolume;
        public float MaximumVolume;
    }
}
