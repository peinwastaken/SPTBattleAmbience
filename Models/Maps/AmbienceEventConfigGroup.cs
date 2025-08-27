using Newtonsoft.Json;
using System.Collections.Generic;

namespace SPTBattleAmbience.Models.Maps
{
    public class AmbienceEventConfigGroup
    {
        [JsonProperty("category")]
        public string Category = "weapons";

        [JsonProperty("weight")]
        public int Weight = 1;

        [JsonProperty("eventConfigs")]
        public Dictionary<string, AmbienceEventConfig> EventConfigs = [];

        public AmbienceEventConfig GetRandomEventConfig(bool useWeight = false)
        {
            return EventConfigs.Values.PickRandom();
        }
    }
}
