using BepInEx;
using Elixir.Management;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using static Elixir.Utilities.ColorLib;

namespace Elixir
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public void OnEnable() => HarmonyPatches.Patch(true);

        public void OnDisable() => HarmonyPatches.Patch(false);

        public void Start() => Menu.Start();

        public void Update()
        {
            Menu.Update();
        }
        
        //public void Awake() => Menu.Awake();
    }
}
