using BepInEx;
using Elixir.Utilities;
using Elixir.Utilities.Notifs;
using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static Elixir.Mods.Categories.Settings;
using static Elixir.Utilities.ColorLib;
using static Elixir.Utilities.Variables;


namespace Elixir.Mods.Categories
{
    public class World
    {
        public static void DisableQuitBox()
        {
            GameObject.Find("QuitBox").SetActive(false);
        }
        public static void EnableQuitBox()
        {
            GameObject.Find("QuitBox").SetActive(true);
        }
        public static void SilentHandTaps()
        {
            GorillaTagger.Instance.handTapVolume = 0f;
        }
        public static void LoudHandTaps()
        {
            GorillaTagger.Instance.handTapVolume = 10f;
        }
        public static void UnlockComp()
        {
            GorillaComputer.instance.CompQueueUnlockButtonPress();
        }
        public static void EnableILavaYou()
        {
            GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/ILavaYou_ForestArt_Prefab/").SetActive(true);
            GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/ILavaYou_PrefabV/").SetActive(true);
        }
        public static void ToggleSnow(bool t)
        {
            GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/Environment/WeatherDayNight/snow").SetActive(t);
            GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/Environment/WeatherDayNight/snow").transform.position = new Vector3(-55.2344f, 58.7391f, -56.9323f);
            GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/Environment/WeatherDayNight/snow/snow partic").SetActive(t);
        }
        public static void DisableILavaYou()
        {
            GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/ILavaYou_ForestArt_Prefab/").SetActive(false);
            GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/ILavaYou_PrefabV/").SetActive(false);
        }
        public static void Rain()
        {
            for (int i = 1; i < BetterDayNightManager.instance.weatherCycle.Length; i++)
            {
                BetterDayNightManager.instance.weatherCycle[i] = BetterDayNightManager.WeatherType.Raining;
            }
        }
        public static void Rain1()
        {
            for (int i = 1; i < BetterDayNightManager.instance.weatherCycle.Length; i++)
            {
                BetterDayNightManager.instance.weatherCycle[i] = BetterDayNightManager.WeatherType.None;
            }
        }
        public static void NightTimeMod() 
        {
            BetterDayNightManager.instance.SetTimeOfDay(0); 
        }
        public static void DayTimeMod()
        {
            BetterDayNightManager.instance.SetTimeOfDay(3);
        }
        public static void idkTimeMod()
        {
            BetterDayNightManager.instance.SetTimeOfDay(4);
        }
        private static GameObject StumpText = new GameObject("Stump");
        public static void Stumpy()
        {
            if (StumpText == null)
            {
                StumpText = new GameObject("Stump");
            }

            TextMeshPro tmp = StumpText.GetComponent<TextMeshPro>();
            if (tmp == null)
            {
                tmp = StumpText.AddComponent<TextMeshPro>();
                tmp.fontSize = 2f;
                tmp.fontStyle = FontStyles.Bold;
                tmp.characterSpacing = 1f;
                tmp.alignment = TextAlignmentOptions.Center;
                tmp.color = ColorLib.DarkPurple;
                tmp.font = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdBodyText").GetComponent<TextMeshPro>().font;
            }

            tmp.text =
                GradientText.MakeAnimatedGradient(ColorLib.ClrToHex(Magenta), ColorLib.ClrToHex(Purple), "Elixir Menu", Time.time) + "\n" +
                $"<size=2>Status: " + GradientText.MakeAnimatedGradient(ColorLib.ClrToHex(Magenta), ColorLib.ClrToHex(Purple), "Undected", Time.time) + "\n" +
                $"VERSION: " + GradientText.MakeAnimatedGradient(ColorLib.ClrToHex(Magenta), ColorLib.ClrToHex(Purple), PluginInfo.Version, Time.time) + "</size>\n" +
                $"<size=1.5>Made By " + GradientText.MakeAnimatedGradient(ColorLib.ClrToHex(Menker2), ColorLib.ClrToHex(Menker), "Menker", Time.time);

            StumpText.transform.position = new Vector3(-66.8087f, 12.1808f, -82.5265f);
            StumpText.transform.LookAt(Camera.main.transform);
            StumpText.transform.Rotate(0f, 180f, 0f);
        }

        public static void STUMPY()
        {
            UnityEngine.Object.Destroy(StumpText);
        }
    }
}
