using PeinRecoilRework.Helpers;
using SPTBattleAmbience.Config;
using SPTBattleAmbience.Data;
using SPTBattleAmbience.Helpers;
using SPTBattleAmbience.Managers;
using SPTBattleAmbience.Models.Maps;
using System.Collections.Generic;
using UnityEngine;

namespace SPTBattleAmbience.Controllers;

public class BattleAmbienceController : MonoBehaviour
{
    public static BattleAmbienceController Instance { get; private set; }
        
    public List<AmbienceManager> AmbienceManagers { get; private set; }
    public List<SoundZoneController> SoundZones { get; private set; }

    public bool UseZones;

    private bool _gameStarted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnGameStarted()
    {
        AmbienceManagers = new List<AmbienceManager>();
        SoundZones = new List<SoundZoneController>();

        string mapId = GameWorldHelper.GetCurrentMapId();
        AmbientHelper.MapAmbienceEvents.TryGetValue(mapId, out AmbienceEvents mapEvents);
        MapConfigBase mapConfig = ConfigHelper.GetMapConfig(mapId);

        if (mapEvents == null)
        {
            DebugLogger.LogWarning($"Ambient events not found for map {mapId}");
            return;
        }

        // load ambience groups
        foreach (KeyValuePair<string, AmbienceEventConfigGroup> kvp in mapEvents.AmbienceEventGroups)
        {
            AmbienceManager ambienceTimer = new AmbienceManager();
            ambienceTimer.EventConfigGroup = kvp.Value;
            ambienceTimer.ChooseNextAmbience(1f, true);
            AmbienceManagers.Add(ambienceTimer);
        }
        
        // load sound zones
        if (mapConfig.UseSoundZones.Value)
        {
            foreach (SoundZoneEntry soundZone in mapEvents.SoundZones)
            {
                var newZone = new GameObject(soundZone.Name);
                newZone.transform.position = soundZone.Position;
                newZone.transform.rotation = soundZone.Rotation;
                newZone.transform.localScale = soundZone.Scale;
                var controller = newZone.AddComponent<SoundZoneController>();
                SoundZones.Add(controller);
            }

            UseZones = SoundZones.Count > 0;
        }
        
        _gameStarted = true;
    }

    public void OnDestroy()
    {
        if(AmbienceManagers == null)
        {
            DebugLogger.LogWarning("Ambient Managers List Null");
            return;
        }
            
        AmbienceManagers.Clear();
        StopAllCoroutines();

        _gameStarted = false;
    }

    private void Update()
    {
        if (!_gameStarted) return;

        float dt = Time.deltaTime;

        foreach (AmbienceManager manager in AmbienceManagers)
        {
            manager.Update(dt);
        }
    }
    
    public List<SoundZoneController> GetSoundZones()
    {
        return SoundZones;
    }
}