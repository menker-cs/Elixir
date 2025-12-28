using BepInEx;
using Elixir.Components;
using Elixir.Management;
using static Elixir.Patches.HarmonyPatches;

namespace Elixir
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public void OnEnable() => ApplyHarmonyPatches();

        public void OnDisable() => RemoveHarmonyPatches();

        public void Start() => Menu.Start();

        public void Update()
        {
            Menu.Update();
        }

    }
}
