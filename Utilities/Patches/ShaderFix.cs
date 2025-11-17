using HarmonyLib;
using UnityEngine;
using static Oculus.Interaction.Context;

namespace MistMenu.Patches.Internal
{
    [HarmonyPatch(typeof(GameObject), "CreatePrimitive")]
    public class ShaderFix : MonoBehaviour
    {
        private static void Postfix(GameObject __result)
        {
            __result.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
            __result.GetComponent<Renderer>().material.color = Color.black;
        }
    }
    
}