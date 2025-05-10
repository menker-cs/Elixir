using System;
using System.Collections.Generic;
using System.Text;
using GorillaLocomotion;
using static Hidden.Menu.Main;
using static Hidden.Utilities.Variables;
using static Hidden.Utilities.ColorLib;
using static Hidden.Menu.ButtonHandler;
using static Hidden.Mods.ModButtons;
using static Hidden.Mods.Categories.Settings;
using UnityEngine;
using BepInEx;
using UnityEngine.InputSystem;
using GorillaNetworking;
using TMPro;
using Photon.Pun;
using Hidden.Utilities.Notifs;


namespace Hidden.Mods.Categories
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
            NotificationLib.SendNotification("Unlocks Competetive Cue");
        }
        public static void EnableILavaYou()
        {
            GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/ILavaYou_ForestArt_Prefab/").SetActive(true);
            GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/ILavaYou_PrefabV/").SetActive(true);
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
    }
}
