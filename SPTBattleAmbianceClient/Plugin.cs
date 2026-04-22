using BepInEx;
using BepInEx.Logging;
using Comfort.Common;
using EFT;
using PeinRecoilRework.Helpers;
using SPTBattleAmbience.Config.General;
using SPTBattleAmbience.Controllers;
using SPTBattleAmbience.Helpers;
using SPTBattleAmbience.Patches;

namespace SPTBattleAmbience;

[BepInPlugin("com.pein.battleambience", "SPTBattleAmbience", "2.3.0")]
public class Plugin : BaseUnityPlugin
{
    public static new ManualLogSource Logger;

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        DebugLogger.Logger = Logger;

        ConfigHelper.Initialize(Config);
            
        AmbientHelper.LoadAmbientSoundCategories();
        AmbientHelper.LoadMapConfigs();

        new GameStartedPatch().Enable();
            
        FikaData.Initialize();
    }

    private void Update()
    {
        if (Singleton<GameWorld>.Instance && GeneralConfig.EnableDebug.Value && GeneralConfig.PlayAmbientShortcut.Value.IsDown())
        {
            BattleAmbienceController.Instance.AmbienceManagers.Random().TriggerAmbience();
        }
    }
}