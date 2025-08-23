using BepInEx.Configuration;
using SPTBattleAmbience.Data.Enum;
using SPTBattleAmbience.Utility;

namespace SPTBattleAmbience.Config.Events
{
    public class ArtilleryEventConfig(ConfigFile config)
        : BattleSoundConfigBase(config, new BattleSoundConfigStruct
        {
            SoundEvent = ESoundEvent.Artillery,
            ConfigOrder = 4,
            ConfigCategory = Category.Artillery,
            IsGunshotEvent = false,
            MinimumBattleSoundTypes = 2,
            MaximumBattleSoundTypes = 4,
            MinimumTimeToNextAmbience = 150f,
            MaximumTimeToNextAmbience = 250f,
            MinimumSoundCount = 3,
            MaximumSoundCount = 7,
            MinimumSoundGap = 5f,
            MaximumSoundGap = 8f,
            MinimumVolume = 0.4f,
            MaximumVolume = 0.8f
        })
    {
    }
}
