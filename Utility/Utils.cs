using SPTBattleAmbience.Data.Enum;
using UnityEngine;

namespace SPTBattleAmbience.Utility
{
    public static class Utils
    {
        public static Vector3 RandomVector => new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;

        public static ELocation GetLocationEnum(string mapId)
        {
            return mapId.ToLower() switch
            {
                "bigmap" => ELocation.Customs,
                "factory4_day" or "factory4_night" => ELocation.Factory,
                "interchange" => ELocation.Interchange,
                "lighthouse" => ELocation.Lighthouse,
                "rezervbase" => ELocation.Reserve,
                "shoreline" => ELocation.Shoreline,
                "tarkovstreets" => ELocation.Streets,
                "sandbox" or "sandbox_high" => ELocation.GroundZero,
                "laboratory" => ELocation.Labs,
                "woods" => ELocation.Woods,
                _ => ELocation.Unknown,
            };
        }

        public static Vector3 GetVectorWithAngleOffset(Vector3 vector, float angleRange)
        {
            float angle = Mathf.Atan2(vector.z, vector.x);
            float offset = Mathf.Deg2Rad * Random.Range(-angleRange, angleRange);
            float newAngle = angle + offset;

            return new Vector3(Mathf.Cos(newAngle), vector.y, Mathf.Sin(newAngle)).normalized;
        }
    }
}
