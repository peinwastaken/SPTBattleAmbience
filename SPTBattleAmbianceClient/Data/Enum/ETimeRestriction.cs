using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BattleAmbienceClient.Data.Enum
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ETimeRestriction
    {
        Always,
        Day,
        Night
    }
}
