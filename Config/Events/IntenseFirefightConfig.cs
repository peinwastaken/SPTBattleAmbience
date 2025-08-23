using BepInEx.Configuration;
using SPTBattleAmbience.Data.Enum;
using SPTBattleAmbience.Utility;

namespace SPTBattleAmbience.Config.Events
{
    public class IntenseFirefightConfig(ConfigFile config)
        : BattleSoundConfigBase(config, new BattleSoundConfigStruct
        {
            SoundEvent = ESoundEvent.IntenseFirefight,
            ConfigOrder = 3,
            ConfigCategory = Category.IntenseFirefight,
            IsGunshotEvent = true,
            MinimumBattleSoundTypes = 2,
            MaximumBattleSoundTypes = 4,
            MinimumTimeToNextAmbience = 180f,
            MaximumTimeToNextAmbience = 300f,
            MinimumSoundCount = 10,
            MaximumSoundCount = 35,
            MinimumSoundGap = 0.5f,
            MaximumSoundGap = 4f,
            MinimumVolume = 0.3f,
            MaximumVolume = 0.7f
        })
    {
    }
}
