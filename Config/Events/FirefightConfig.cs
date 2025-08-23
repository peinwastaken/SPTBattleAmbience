using BepInEx.Configuration;
using SPTBattleAmbience.Data.Enum;
using SPTBattleAmbience.Utility;

namespace SPTBattleAmbience.Config.Events
{
    public class FirefightConfig(ConfigFile config)
        : BattleSoundConfigBase(config, new BattleSoundConfigStruct
        {
            SoundEvent = ESoundEvent.Firefight,
            ConfigOrder = 2,
            ConfigCategory = Category.FireFight,
            IsGunshotEvent = true,
            MinimumBattleSoundTypes = 1,
            MaximumBattleSoundTypes = 2,
            MinimumTimeToNextAmbience = 60f,
            MaximumTimeToNextAmbience = 240f,
            MinimumSoundCount = 3,
            MaximumSoundCount = 8,
            MinimumSoundGap = 0.5f,
            MaximumSoundGap = 3f,
            MinimumVolume = 0.3f,
            MaximumVolume = 0.5f
        })
    {
    }
}
