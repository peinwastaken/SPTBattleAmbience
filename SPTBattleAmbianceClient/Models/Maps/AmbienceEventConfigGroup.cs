using Newtonsoft.Json;
using PeinRecoilRework.Helpers;
using SPTBattleAmbience.Data.Enum;
using SPTBattleAmbience.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace SPTBattleAmbience.Models.Maps;

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
            DebugLogger.LogInfo(configTimeRestriction.ToString());

            if (configTimeRestriction == currentTimeRestriction || configTimeRestriction == ETimeRestriction.Always)
            {
                validConfigs.Add(config);
            }
        }

        if (validConfigs.Count == 0)
        {
            DebugLogger.LogWarning($"No ambient event found for current time restriction: {currentTimeRestriction}");
            return null;
        }

        if (useWeight)
        {
            Dictionary<AmbienceEventConfig, float> weighedEvents = [];
                
            foreach (AmbienceEventConfig config in validConfigs)
            {
                weighedEvents.Add(config, config.Weight);
            }

            return weighedEvents.PickRandomWeighed();
        }

        return validConfigs.PickRandom();
    }
}