using Newtonsoft.Json;
using UnityEngine;

namespace SPTBattleAmbience.Models.Maps;

public class SoundZoneEntry
{
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("position")]
    public Vector3 Position { get; set; }
    
    [JsonProperty("rotation")]
    public Quaternion Rotation { get; set; }
    
    [JsonProperty("scale")]
    public Vector3 Scale { get; set; }
}
