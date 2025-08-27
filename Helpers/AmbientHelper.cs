using SPTBattleAmbience.Models.Maps;
using SPTBattleAmbience.Models.Sounds;
using System.Collections.Generic;

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
            if (MapAmbienceEvents.ContainsKey(mapName))
            {
                return MapAmbienceEvents[mapName];
            }

            return null;
        }
    }
}
