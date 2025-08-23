using SPTBattleAmbience.Data.Enum;
using UnityEngine;
using System;

namespace SPTBattleAmbience.Helpers
{
    public static class BattleSoundHelper
    {
        public static EBattleSoundType[] GunShotTypes = new EBattleSoundType[]
        {
            EBattleSoundType.AK74,
            EBattleSoundType.Scar,
            EBattleSoundType.Adar,
            EBattleSoundType.HK,
            EBattleSoundType.M60,
            EBattleSoundType.AUG,
        };

        public static bool IsGunShotType(EBattleSoundType type)
        {
            foreach (EBattleSoundType gunType in GunShotTypes)
            {
                if (type == gunType)
                {
                    return true;
                }
            }

            return false;
        }

        public static ESoundEvent GetRandomSoundEvent()
        {
            Array values = Enum.GetValues(typeof(ESoundEvent));
            return (ESoundEvent)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        }
    }
}
