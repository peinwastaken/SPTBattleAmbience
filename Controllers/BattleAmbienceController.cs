using PeinRecoilRework.Helpers;
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
            
            AmbienceManagers = new List<AmbienceManager>();
        }

        public void OnGameStarted()
        {
            // force reload audioclips because otherwise they become null
            Plugin.LoadAmbientSoundCategories();

            string mapId = GameWorldHelper.GetCurrentMapId();
            AmbienceEvents mapEvents = AmbientHelper.MapAmbienceEvents[mapId];

            if (mapEvents == null)
            {
                DebugLogger.LogWarning($"Ambient events not found for map ${mapId}");
                return;
            }

            foreach (KeyValuePair<string, AmbienceEventConfigGroup> kvp in mapEvents.AmbienceEventGroups)
            {
                AmbienceManager ambienceTimer = new AmbienceManager();
                ambienceTimer.EventConfigGroup = kvp.Value;
                ambienceTimer.ChooseNextAmbience();

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
