namespace SPTBattleAmbience.Utility
{
    public class Category
    {
        public static string General = "General";
        public static string SingleShot = "Single Shot";
        public static string FireFight = "Regular Firefight";
        public static string IntenseFirefight = "Intense Firefight";
        public static string Artillery = "Artillery";

        public static string Customs = "Map Settings - Customs";
        public static string Factory = "Map Settings - Factory";
        public static string Shoreline = "Map Settings - Shoreline";
        public static string Woods = "Map Settings - Woods";
        public static string Interchange = "Map Settings - Interchange";
        public static string Reserve = "Map Settings - Reserve";
        public static string Labs = "Map Settings - Labs";
        public static string Streets = "Map Settings - Streets";
        public static string GroundZero = "Map Settings - Ground Zero";
        public static string Lighthouse = "Map Settings - Lighthouse";

        public static string Format(int order, string category) => $"{order.ToString("D2")}. {category}";
    }
}
