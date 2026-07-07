using GorillaTag.Gravity;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Elixir.Patches
{
    internal class MiscPatches
    {
        [HarmonyPatch(typeof(PlanetZone), nameof(PlanetZone.GetGravityVectorAtPoint))]
        public class PlanetZoneOverride
        {
            public static bool overide = false;
            public static Vector3 overideV = Vector3.up;

            public static bool Prefix(ref Vector3 __result)
            {
                if (!overide)
                    return true;

                __result = overideV;
                return false;
            }
        }
    }
}
