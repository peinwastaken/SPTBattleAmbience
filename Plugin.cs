using BepInEx;
using BepInEx.Logging;
using PeinRecoilRework.Helpers;
using SPTBattleAmbience.Config.General;
using SPTBattleAmbience.Controllers;
using SPTBattleAmbience.Data.Enum;
using SPTBattleAmbience.Helpers;
using SPTBattleAmbience.Models;
using SPTBattleAmbience.Patches;
using System;
using System.IO;

namespace SPTBattleAmbience
{
    [BepInPlugin("com.pein.battleambience", "SPTBattleAmbience", "1.1.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static new ManualLogSource Logger;

        private void Awake()
        {
            // Plugin startup logic
            Logger = base.Logger;
            DebugLogger.Logger = Logger;

            ConfigHelper.Initialize(Config);
            BattleAmbienceController.Initialize();

            LoadBattleSounds();

            new GameStartedPatch().Enable();
            new OnGameEndedPatch().Enable();
        }

        private void Update()
        {
            if (GeneralConfig.EnableDebug.Value == true && GeneralConfig.PlayAmbientShortcut.Value.IsDown())
            {
                DebugLogger.LogWarning("Playing ambient battle sounds...");
                BattleAmbienceController.Instance.TriggerAmbience();
            }
        }

        public static void LoadBattleSounds()
        {
            BattleAmbienceController ambienceController = BattleAmbienceController.Instance;
            string[] soundDirs = FileHelper.ReadDirectories(FileHelper.SoundsPath);

            foreach (string soundDir in soundDirs)
            {
                string dirName = Path.GetFileName(soundDir);
                EBattleSoundType soundType = Enum.TryParse(dirName, true, out EBattleSoundType parsedType) ? parsedType : EBattleSoundType.Undefined;
                BattleSound battleSound = new BattleSound(soundType, soundDir);

                ambienceController.AddBattleSound(soundType, battleSound);
            }
        }
    }
}
