using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace SPTBattleAmbience.Models.Maps
{
    public class AmbienceEvents
    {
        [JsonProperty("ambienceEvents")]
        public Dictionary<string, AmbienceEventConfigGroup> AmbienceEventGroups = [];

        public AmbienceEventConfigGroup GetRandomEventGroup(bool useWeight = false)
        {
            if (!useWeight)
            {
                return AmbienceEventGroups.Values.PickRandom();
            }
            else
            {
                return AmbienceEventGroups.Values.PickRandom();
            }
        }
    }
}
