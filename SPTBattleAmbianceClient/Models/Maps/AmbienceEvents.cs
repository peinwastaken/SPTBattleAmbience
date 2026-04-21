using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace BattleAmbienceClient.Models.Maps
{
    public class AmbienceEvents
    {
        [JsonProperty("ambienceEvents")]
        public Dictionary<string, AmbienceEventConfigGroup> AmbienceEventGroups = [];
    }
}
