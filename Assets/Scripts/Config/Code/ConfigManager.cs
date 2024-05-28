namespace Config
{
    public class ConfigManager
    {
        public ConfigManager()
        {
            Newtonsoft.Json.Utilities.AotHelper.EnsureList<UIConfig>();
        }
        public static void LoadAllConfigsByAddressable(string directory)
        {
            Config.UIConfig.DeserializeByAddressable(directory);
        }
        public static void LoadAllConfigsByFile(string directory)
        {
            Config.UIConfig.DeserializeByFile(directory);
        }
        public static System.Collections.IEnumerator LoadAllConfigsByBundle(string directory, string subFolder)
        {
            yield return Config.UIConfig.DeserializeByBundle(directory, subFolder);
        }
    }
}
