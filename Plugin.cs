using BepInEx;
using Elixir.Management;
using Elixir.Utilities;
using Photon.Pun;
using System;
using TMPro;
using UnityEngine;
using static Elixir.Management.Menu;
using static Elixir.Patches.HarmonyPatches;
using static Elixir.Utilities.ColorLib;

namespace Elixir
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public void OnEnable() => ApplyHarmonyPatches();

        public void OnDisable() => RemoveHarmonyPatches();

        public void Start() => Menu.Start();

        static int fps;
        public void Update()
        {
            Menu.Update();

            #region Boards
            fps = (Time.deltaTime > 0) ? Mathf.RoundToInt(1 / Time.deltaTime) : 0;
            try
            {
                GameObject goop = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/SpectralGooPile (combined by EdMeshCombiner)");
                if (goop != null && computer != null && wallMonitor != null)
                {
                    computer.material = goop.GetComponent<Renderer>().material;
                    wallMonitor.material = goop.GetComponent<Renderer>().material;
                    ChangeBoardMaterial("Environment Objects/LocalObjects_Prefab/TreeRoom", "UnityTempFile", 4, goop.GetComponent<Renderer>().material, ref originalMat1!);
                }

                #region MOTD
                if (motdHeading == null || motdBody == null) return;
                motdHeading.SetText(GradientText.MakeAnimatedGradient(ColorLib.ClrToHex(Magenta), ColorLib.ClrToHex(Purple), $"Elixir | V{Elixir.PluginInfo.Version}", Time.time) + $"<color={hexColor1}>\n--------------------------------------------</color>"); motdHeading.color = Pink;
                motdBody.color = Pink;
                motdBody.SetText($"" +
                    $"\nThank You For Using Elixir!\n\n" +
                    $"Status: <color={hexColor1}>{Variables.Status}</color>\n" +
                    $"Current User: <color={hexColor1}>{PhotonNetwork.LocalPlayer.NickName.ToUpper()}</color> \n" +
                    $"Current Ping: <color={hexColor1}>{PhotonNetwork.GetPing().ToString().ToUpper()}</color>\n" +
                    $"Current FPS: <color={hexColor1}>{fps}</color> \n" +
                    $"Current Room: <color={hexColor1}>{(PhotonNetwork.InRoom ? PhotonNetwork.CurrentRoom.Name.ToUpper() : "Not Connected To A Room")} </color> \n\n" +
                    $"<color={hexColor1}>I Hope You Enjoy The Menu</color> \n" +
                    $"Made by <color={hexColor1}>Menker</color>");

                motdBody.alignment = TextAlignmentOptions.Top;
                #endregion

                #region COC
                if (cocHeading == null || cocBody == null) return;
                cocHeading.SetText(GradientText.MakeAnimatedGradient(ColorLib.ClrToHex(Magenta), ColorLib.ClrToHex(Purple), "Menu Meanings", Time.time) + $"<color={hexColor1}>\n-----------------------------</color>");
                cocHeading.color = Pink;

                cocBody.color = Pink;
                cocBody.SetText($"\n[D?] - Maybe Detected \n[NW] - Not Working\n[U] - Use\n[P] - Primary\n[S] - Secondary\n[G] - Grip\n[T] - Trigger\n[W?] - Maybe Working\n[B] - Buggy\n\nIf A Mod Has No Symbol It Is Probably Because I Forgot To Put One");
                cocBody.alignment = TextAlignmentOptions.Top;
                #endregion

                if (gameModeText == null) return;
                gameModeText.SetText("Elixir");
                gameModeText.color = RGB.color;
            }
            catch (NullReferenceException ex)
            {
                UnityEngine.Debug.LogError($"NullReferenceException: {ex.Message}\nStack Trace: {ex.StackTrace}");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"Unexpected error: {ex.Message}\nStack Trace: {ex.StackTrace}");
            }

            #endregion
            UpdateClr();
        }

    }
}
