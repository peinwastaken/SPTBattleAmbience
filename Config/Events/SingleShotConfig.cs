using BepInEx.Configuration;
using SPTBattleAmbience.Data.Enum;
using SPTBattleAmbience.Utility;

namespace SPTBattleAmbience.Config.Events
{
    public class SingleShotConfig(ConfigFile config)
        : BattleSoundConfigBase(config, new BattleSoundConfigStruct
        {
            SoundEvent = ESoundEvent.SingleShot,
            ConfigOrder = 1,
            ConfigCategory = Category.SingleShot,
            IsGunshotEvent = true,
            MinimumBattleSoundTypes = 1,
            MaximumBattleSoundTypes = 2,
            MinimumTimeToNextAmbience = 30f,
            MaximumTimeToNextAmbience = 60f,
            MinimumSoundCount = 1,
            MaximumSoundCount = 2,
            MinimumSoundGap = 2f,
            MaximumSoundGap = 5f,
            MinimumVolume = 0.2f,
            MaximumVolume = 0.4f
        })
    {
    }
}
