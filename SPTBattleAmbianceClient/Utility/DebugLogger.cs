using BepInEx.Logging;
using SPTBattleAmbience.Config.General;

namespace PeinRecoilRework.Helpers
{
    public class DebugLogger
    {
        public static ManualLogSource Logger { get; set; }
        private static bool _isDebug => GeneralConfig.EnableDebug.Value;

        public static void Log(string message, LogLevel logLevel = LogLevel.Info)
        {
            if (!_isDebug) return;

            Logger?.Log(logLevel, message);
        }

        public static void LogInfo(string message)
        {
            Log(message, LogLevel.Info);
        }

        public static void LogWarning(string message)
        {
            Log(message, LogLevel.Warning);
        }

        public static void LogError(string message)
        {
            Log(message, LogLevel.Error);
        }
    }
}
