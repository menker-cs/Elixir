using GorillaLocomotion;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elixir.Patches
{
    internal class GrabPatches
    {
        [HarmonyPatch(typeof(GTPlayer), nameof(GTPlayer.TakeMyHand_ProcessMovement))]
        public class GrabPatch
        {
            public static bool enabled;

            public static bool Prefix(GTPlayer __instance) => !enabled;
        }
    }
}
