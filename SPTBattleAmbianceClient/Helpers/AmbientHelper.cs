using Comfort.Common;
using PeinRecoilRework.Helpers;
using SPTBattleAmbience.Config.General;
using SPTBattleAmbience.Data;
using SPTBattleAmbience.Models.Maps;
using SPTBattleAmbience.Models.Sounds;
using System.Collections.Generic;
using UnityEngine;

namespace SPTBattleAmbience.Helpers
{
    public class AmbientHelper
    {
        // assets/configs/maps/mapname.json
        public static Dictionary<string, AmbienceEvents> MapAmbienceEvents = [];

        // assets/sounds/category/soundtype/sound.wav
        public static Dictionary<string, AmbientSoundCategory> AmbientSoundCategories = [];

        public static AmbienceEvents GetMapAmbienceEvents(string mapName)
        {
            MapAmbienceEvents.TryGetValue(mapName, out AmbienceEvents ambienceEvents);
            return ambienceEvents;
        }

        public static void PlayAmbienceSound(Vector3 position, ClipInfo clipInfo, int rolloff, float volume)
        {
            DebugLogger.LogInfo($"playing sound: {clipInfo.ClipName} at position {position}");
            
            Singleton<BetterAudio>.Instance.PlayAtPoint(
                position,
                clipInfo.AudioClip,
                GeneralConfig.AudioSourceGroup.Value,
                rolloff,
                volume,
                GeneralConfig.OcclusionTestMode.Value,
                null,
                true,
                true
            );
        }
    }
}
