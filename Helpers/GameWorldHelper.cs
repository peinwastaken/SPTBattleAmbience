using Comfort.Common;
using EFT;

namespace SPTBattleAmbience.Helpers
{
    public static class GameWorldHelper
    {
        public static Player GetLocalPlayer()
        {
            return Singleton<GameWorld>.Instance.MainPlayer;
        }

        public static string GetCurrentMapId()
        {
            string mapId = Singleton<GameWorld>.Instance.LocationId.ToLower();

            return mapId;
        }
    }
}
