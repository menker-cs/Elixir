using BepInEx;
using HarmonyLib;
using static Elixir.Initialization.PluginInfo;

namespace Elixir.Initialization
{
    [BepInPlugin(menuGUID, menuName, menuVersion)]
    public class BepInExInitializer : BaseUnityPlugin
    {
        public static BepInEx.Logging.ManualLogSource? LoggerInstance;

        void Awake()
        {
            LoggerInstance = Logger;
            new Harmony(menuGUID).PatchAll();
        }
    }
}
