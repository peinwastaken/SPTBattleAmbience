using SPTBattleAmbience.Data.Enum;
using UnityEngine;

namespace SPTBattleAmbience.Config
{
    public struct MapConfigStruct
    {
        public ELocation Location;
        public int ConfigOrder;
        public string ConfigCategory;
        public bool EnableEvents;
        public float AmbienceEventCooldownMultiplier;
        public float MinVolumeMultiplier;
        public float MaxVolumeMultiplier;
        public Vector3 MapCenter;
        public float MapRadius;
        public bool UsePlayerDirection;
    }
}
