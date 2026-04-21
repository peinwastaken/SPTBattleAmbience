using Comfort.Common;
using EFT;

namespace SPTBattleAmbience.Helpers;

public class GameWorldHelper
{
    public static Player GetLocalPlayer()
    {
        if (FikaGlobals.IsHeadless)
        {
            return null;
        }
            
        return Singleton<GameWorld>.Instance.MainPlayer;
    }

    public static string GetCurrentMapId()
    {
        string mapId = Singleton<GameWorld>.Instance.LocationId;

        return mapId.ToLowerInvariant();
    }
}