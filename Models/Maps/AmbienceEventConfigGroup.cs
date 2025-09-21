using Newtonsoft.Json;
using SPTBattleAmbience.Data.Enum;
using SPTBattleAmbience.Utility;
using System.Collections.Generic;
using UnityEngine;

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

            if (useWeight)
            {
                float totalWeight = 0f;

                foreach (AmbienceEventConfig config in validConfigs)
                {
                    totalWeight += config.Weight;
                }

                // never happens because weight defaults to 1, but better safe than sorry
                if (totalWeight == 0)
                {
                    return validConfigs.PickRandom();
                }
                
                float roll = Random.Range(0f, totalWeight);
                float cumulativeWeight = 0f;
                
                foreach (AmbienceEventConfig config in validConfigs)
                {
                    cumulativeWeight += config.Weight;
                    if (roll <= cumulativeWeight)
                    {
                        return config;
                    }
                }
            }

            return validConfigs.PickRandom();
        }
    }
}
