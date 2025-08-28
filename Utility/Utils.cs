using SPTBattleAmbience.Data.Enum;
using SPTBattleAmbience.Helpers;
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

        public static Vector3 GetVectorWithAngleOffset(Vector3 vector, float angleRange)
        {
            float angle = Mathf.Atan2(vector.z, vector.x);
            float offset = Mathf.Deg2Rad * Random.Range(-angleRange, angleRange);
            float newAngle = angle + offset;

            return new Vector3(Mathf.Cos(newAngle), vector.y, Mathf.Sin(newAngle)).normalized;
        }

        public static string StringifyArray(object[] array)
        {
            return $"[{string.Join(", ", array)}]";
        }

        public static T[] GetRandomItems<T>(this T[] array, int count)
        {
            if (array == null || array.Length <= 1) return array;

            for (int currentIndex = array.Length - 1; currentIndex > 0; currentIndex--)
            {
                int randomIndex = Random.Range(0, currentIndex + 1);
                (array[randomIndex], array[currentIndex]) = (array[currentIndex], array[randomIndex]);
            }

            return array;
        }

        public static bool IsDayTime()
        {
            string mapId = GameWorldHelper.GetCurrentMapId();
            
            if (mapId.StartsWith("factory4"))
            {
                return mapId == "factory4_day";
            }

            float hour = TOD_Sky.Instance.Cycle.Hour;
            return hour >= 4 && hour <= 22;
        }

        public static ETimeRestriction GetCurrentTimeRestriction()
        {
            bool isDayTime = IsDayTime();

            return isDayTime ? ETimeRestriction.Day : ETimeRestriction.Night;
        }
    }
}
