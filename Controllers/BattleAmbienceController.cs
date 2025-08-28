using PeinRecoilRework.Helpers;
using SPTBattleAmbience.Config;
using SPTBattleAmbience.Helpers;
using SPTBattleAmbience.Managers;
using SPTBattleAmbience.Models.Maps;
using System.Collections.Generic;
using UnityEngine;

namespace SPTBattleAmbience.Controllers
{
    public class BattleAmbienceController : MonoBehaviour
    {
        public static BattleAmbienceController Instance { get; private set; }
        
        public List<AmbienceManager> AmbienceManagers { get; private set; }

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

            // force reload audioclips because otherwise they become null
            Plugin.LoadAmbientSoundCategories();

            string mapId = GameWorldHelper.GetCurrentMapId();
            AmbientHelper.MapAmbienceEvents.TryGetValue(mapId, out AmbienceEvents mapEvents);

            if (mapEvents == null)
            {
                DebugLogger.LogWarning($"Ambient events not found for map {mapId}");
                return;
            }

            MapConfigBase mapConfig = ConfigHelper.GetMapConfig(mapId);

            foreach (KeyValuePair<string, AmbienceEventConfigGroup> kvp in mapEvents.AmbienceEventGroups)
            {
                AmbienceManager ambienceTimer = new AmbienceManager();
                ambienceTimer.EventConfigGroup = kvp.Value;
                ambienceTimer.ChooseNextAmbience(1f, true);

                AmbienceManagers.Add(ambienceTimer);
            }

            _gameStarted = true;
        }

        public void OnGameEnded()
        {
            AmbienceManagers.Clear();

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
    }
}
