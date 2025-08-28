using Newtonsoft.Json;
using SPTBattleAmbience.Data.Enum;
using SPTBattleAmbience.Utility;
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
            ETimeRestriction currentTimeRestriction = Utils.GetCurrentTimeRestriction();
            List<AmbienceEventConfig> validConfigs = [];

            foreach (AmbienceEventConfig config in EventConfigs.Values)
            {
                ETimeRestriction configTimeRestriction = config.TimeRestriction;

                if (configTimeRestriction == currentTimeRestriction || configTimeRestriction == ETimeRestriction.Always)
                {
                    validConfigs.Add(config);
                }
            }

            if (validConfigs.Count == 0)
            {
                return null;
            }

            return validConfigs.PickRandom();
        }
    }
}
