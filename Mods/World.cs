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
        public static void ToggleILavaYou(bool t)
        {
            GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/ILavaYou_ForestArt_Prefab/").SetActive(t);
            GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/ILavaYou_PrefabV/").SetActive(t);
        }
        public static void ToggleSnow(bool t)
        {
            GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/Environment/WeatherDayNight/snow").SetActive(t);
            GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/Environment/WeatherDayNight/snow").transform.position = new Vector3(-55.2344f, 58.7391f, -56.9323f);
            GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/Environment/WeatherDayNight/snow/snow partic").SetActive(t);
        }

        public static bool whip = true;

        public static void SpawnHoverboard(Vector3 position, Quaternion rotation, Vector3 velocity, Color col, RpcTarget rpc)
        {
            FreeHoverboardManager.instance.photonView.RPC("DropBoard_RPC", rpc, new object[]
                {
                     whip,
                     BitPackUtils.PackWorldPosForNetwork(position),
                     BitPackUtils.PackQuaternionForNetwork(rotation),
                     BitPackUtils.PackWorldPosForNetwork(velocity),
                     BitPackUtils.PackWorldPosForNetwork(Vector3.zero),
                     BitPackUtils.PackColorForNetwork(col)
                });
            whip = !whip;
            Potentially_OP.RPCProtection();
        }

        public static Color RandomColor
        {
            get
            {
                return new Color(
                    UnityEngine.Random.value,
                    UnityEngine.Random.value,
                    UnityEngine.Random.value
                );
            }
        }

        static float ina;

        public static void SpawnHoverboard()
        {
            GTPlayer.Instance.SetHoverAllowed(true);
            FreeHoverboardManager.instance.SendDropBoardRPC(GorillaTagger.Instance.rightHandTransform.position, Quaternion.identity, Vector3.zero, Vector3.zero, RandomColor);
        }

        public static void HoverboardShoot(float speed)
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                if (Time.time > ina)
                {
                    ina = Time.time + 0.9f / speed;
                    SpawnHoverboard(
                       GorillaTagger.Instance.rightHandTransform.transform.position,
                       GorillaTagger.Instance.rightHandTransform.rotation,
                       GorillaTagger.Instance.rightHandTransform.forward * 10 * speed,
                       RandomColor,
                       RpcTarget.All
                    );
                }
            }
            if (ControllerInputPoller.instance.leftGrab)
            {
                if (Time.time > ina)
                {
                    ina = Time.time + 0.9f / speed;
                    SpawnHoverboard(
                       GorillaTagger.Instance.leftHandTransform.transform.position,
                       GorillaTagger.Instance.leftHandTransform.rotation,
                       GorillaTagger.Instance.leftHandTransform.forward * 10 * speed,
                       RandomColor,
                       RpcTarget.All
                    );
                }
            }
        }

        public static void HoverboardGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                VRRig.LocalRig.enabled = false;
                VRRig.LocalRig.transform.position = GunTemplate.spherepointer.transform.position + new Vector3(0,1.5f);
                if(Time.time > ina)
                {
                    ina = Time.time + 0.5f;
                    SpawnHoverboard(
                        GunTemplate.spherepointer.transform.position,
                        Quaternion.identity,
                        Vector3.zero,
                        RandomColor,
                        RpcTarget.All
                    );
                }
            }, false, () =>
            {
                VRRig.LocalRig.enabled = true;
            });
        }

        public static void HoverboardSpam(float delay)
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                if (Time.time > ina)
                {
                    ina = Time.time + delay;
                    SpawnHoverboard(
                       GorillaTagger.Instance.rightHandTransform.transform.position,
                       GorillaTagger.Instance.rightHandTransform.rotation,
                       Vector3.zero,
                       RandomColor,
                       RpcTarget.All
                    );
                }
                ;
            }
            if (ControllerInputPoller.instance.leftGrab)
            {
                if (Time.time > ina)
                {
                    ina = Time.time + delay;
                    SpawnHoverboard(
                       GorillaTagger.Instance.leftHandTransform.transform.position,
                       GorillaTagger.Instance.leftHandTransform.rotation,
                       Vector3.zero,
                       RandomColor,
                       RpcTarget.All
                    );
                }
            }
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
                tmp.fontSize = 0.9f;
                tmp.fontStyle = FontStyles.Bold;
                tmp.characterSpacing = 1f;
                tmp.alignment = TextAlignmentOptions.Center;
                tmp.color = ColorLib.DarkPurple;
                tmp.font = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdBodyText").GetComponent<TextMeshPro>().font;
            }

            tmp.text =
                GradientText.MakeAnimatedGradient(ColorLib.ClrToHex(Magenta), ColorLib.ClrToHex(Purple), "Elixir Menu", Time.time) + "\n" +
                $"<size=1.15>Status: " + GradientText.MakeAnimatedGradient(ColorLib.ClrToHex(Magenta), ColorLib.ClrToHex(Purple), Variables.Status, Time.time) + "\n" +
                $"VERSION: " + GradientText.MakeAnimatedGradient(ColorLib.ClrToHex(Magenta), ColorLib.ClrToHex(Purple), PluginInfo.Version, Time.time) + "</size>\n" +
                $"<size=0.75>Made By " + GradientText.MakeAnimatedGradient(ColorLib.ClrToHex(Menker2), ColorLib.ClrToHex(Menker), "Menker • Cosmic • Moe", Time.time);

            StumpText.transform.position = new Vector3(-66.8087f, 12.1808f, -82.5265f);
            StumpText.transform.LookAt(Camera.main.transform);
            StumpText.transform.Rotate(0f, 180f, 0f);
        }

        public static void MakeTMPRainbow(TMPro.TMP_Text tmp)
        {
            int n = 6;
            TMPro.VertexGradient vg = new TMPro.VertexGradient();

            float baseHue = Mathf.Repeat(Time.time * 0.5f, 1f);

            vg.topLeft = Color.HSVToRGB(Mathf.Repeat(baseHue + 0f / (n - 1), 1f), 1f, 1f);
            vg.topRight = Color.HSVToRGB(Mathf.Repeat(baseHue + 1f / (n - 1), 1f), 1f, 1f);
            vg.bottomLeft = Color.HSVToRGB(Mathf.Repeat(baseHue + 2f / (n - 1), 1f), 1f, 1f);
            vg.bottomRight = Color.HSVToRGB(Mathf.Repeat(baseHue + 3f / (n - 1), 1f), 1f, 1f);

            tmp.enableVertexGradient = true;
            tmp.colorGradient = vg;
        }

        public static void STUMPY()
        {
            UnityEngine.Object.Destroy(StumpText);
        }
    }
}
