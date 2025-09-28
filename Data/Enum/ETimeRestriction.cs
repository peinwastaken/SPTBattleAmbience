using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Security.Policy;

namespace SPTBattleAmbience.Data.Enum
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ETimeRestriction
    {
        Always,
        Day,
        Night
    }
}
