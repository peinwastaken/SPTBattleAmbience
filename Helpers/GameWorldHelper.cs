using Comfort.Common;
using EFT;

namespace SPTBattleAmbience.Helpers
{
    public class GameWorldHelper
    {
        public static Player GetLocalPlayer()
        {
            return Singleton<GameWorld>.Instance.MainPlayer;
        }

        public static string GetCurrentMapId()
        {
            string mapId = GetLocalPlayer()?.Location.ToLower();

            return mapId;
        }
    }
}
