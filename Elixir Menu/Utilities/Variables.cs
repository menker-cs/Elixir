using HarmonyLib;
using Elixir.Mods;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using static Elixir.Utilities.ColorLib;
using UnityEngine.Animations.Rigging;
using System.Collections;
using System.Text;
using UnityEngine.Networking;
using System;
using System.Net;
using Elixir.Utilities.Notifs;

namespace Elixir.Utilities
{
    public class Variables : MonoBehaviourPunCallbacks
    {
        // --- UI Variables ---
        public static GameObject? menuObj = null;
        public static GameObject? background = null;
        public static GameObject? canvasObj = null;
        public static GameObject? clickerObj = null;
        public static GameObject? PageButtons = null;
        public static GameObject? disconnectButton = null;
        public static GameObject? ModButton = null;
        public static Text? title;

        public static Font font = Font.CreateDynamicFontFromOSFont("MS Gothic", 16);

        // --- UI Colors ---
        public static Color ClickerColor = Color.black;
        public static Color TitleTextColor = White;
        public static Color ModsTextColor = Black;
        public static Color DisconnectButtonTextColor = White;
        public static Color BackToStartTextColor = Color.grey;
        public static Color PageButtonsTextColor = White;
        public static Color espColor = White;

        // --- Menu and Interaction Settings ---
        public static Category currentPage = Category.Home;
        public static int currentCategoryPage = 0;
        public static int ButtonsPerPage = 8;
        public static bool toggledisconnectButton = false;
        public static bool rightHandedMenu = false;
        public static bool outl = true;
        public static bool catsSwitch = true;
        public static bool toggleNotifications = true;
        public static bool PCMenuOpen = false;
        public static KeyCode PCMenuKey = KeyCode.LeftAlt;
        public static bool openMenu;
        public static bool menuOpen = false;
        public static bool InMenuCondition;
        public static bool grav = true;
        public static bool tip = true;
        public static bool vCounter = true;
        public static bool tracker = true;
        public static float lastFPSTime = 0f;
        public static int fps;
        public static int Mat;
        public static bool InPcCondition;

        // --- Player and Movement Variables ---
        public static GorillaLocomotion.GTPlayer? playerInstance;
        public static GorillaTagger? taggerInstance;
        public static ControllerInputPoller? pollerInstance = null;
        public static VRRig? vrrig = null;
        public static Material? vrrigMaterial = null;

        // --- Camera Variables ---
        public static GameObject? thirdPersonCamera;
        public static GameObject? shoulderCamera;
        public static GameObject? TransformCam = null;
        public static bool didThirdPerson = false;
        public static GameObject? cm;

        // --- Physics Variables ---
        public static Vector3 previousVelocity = Vector3.zero;

        public const float velocityThreshold = 0.05f;
        public static int Rotation = 1;

        public static void Placeholder() { }

        // --- Utility Methods ---
        public static void IsModded()
        {
            if (!PhotonNetwork.IsConnected)
            {
                NotificationLib.SendNotification("<color=blue>Room</color> : You are not connected to a room.");
                return;
            }

            string message = GetHTMode().Contains("MODDED") ? "<color=blue>Room</color> : You are in a modded lobby." : "<color=blue>Room</color> : You are not in a modded lobby.";
            NotificationLib.SendNotification(message);
        }

        public static string GetHTMode()
        {
            if (PhotonNetwork.CurrentRoom == null || !PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("gameMode"))
            {
                return "ERROR";
            }
            return PhotonNetwork.CurrentRoom.CustomProperties["gameMode"].ToString();
        }

        public static Photon.Realtime.Player NetPlayerToPhotonPlayer(NetPlayer netPlayer) => netPlayer.GetPlayerRef();

        public static NetPlayer PhotonPlayerToNetPlayer(Photon.Realtime.Player player)
        {
            VRRig rig = GorillaGameManager.instance.FindPlayerVRRig(player);
            return rig != null ? rig.Creator : null!;
        }

        public static NetPlayer GetNetPlayerFromRig(VRRig vrrig) => vrrig.Creator;

        public static VRRig GetRigFromNetPlayer(NetPlayer netPlayer) => GorillaGameManager.instance.FindPlayerVRRig(netPlayer);

        public static PhotonView GetPhotonViewFromRig(VRRig vrrig)
        {
            return (PhotonView)Traverse.Create(vrrig).Field("photonView").GetValue();
        }

        public static PhotonView GetPhotonViewFromNetPlayer(NetPlayer netPlayer) => GetPhotonViewFromRig(GorillaGameManager.instance.FindPlayerVRRig(netPlayer));

        public static bool GetGamemode(string gamemodeName)
        {
            return GorillaGameManager.instance.GameModeName().ToLower().Contains(gamemodeName.ToLower());
        }

        public static bool IsOtherPlayer(VRRig rig) => rig != null && rig != GorillaTagger.Instance.offlineVRRig && !rig.isOfflineVRRig && !rig.isMyPlayer;

        public static bool IAmInfected => GorillaTagger.Instance.offlineVRRig != null && RigIsInfected(GorillaTagger.Instance.offlineVRRig);

        public static bool RigIsInfected(VRRig vrrig)
        {
            string materialName = vrrig.mainSkin.material.name;
            return materialName.Contains("fected") || materialName.Contains("It");
        }
        public static string VrrigPlatform(VRRig rig)
        {
            string concatStringOfCosmeticsAllowed = rig.concatStringOfCosmeticsAllowed;

            if (concatStringOfCosmeticsAllowed.Contains("S. FIRST LOGIN"))
                return "STEAM";
            else if (concatStringOfCosmeticsAllowed.Contains("FIRST LOGIN") || rig.Creator.GetPlayerRef().CustomProperties.Count >= 2)
                return "PC";

            return "QUEST";
        }
        public static void IsMasterCheck()
        {
            if (!PhotonNetwork.IsConnected)
            {
                NotificationLib.SendNotification("<color=blue>Room</color> : You are not connected to a room.");
                return;
            }

            NotificationLib.SendNotification(PhotonNetwork.IsMasterClient ? "<color=blue>Room</color> : You are master." : "<color=blue>Room</color> : " + PhotonNetwork.MasterClient + "is master client");
        }
        public static void UseGravity(bool useGravity)
        {
            GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().useGravity = useGravity;
        }
        public static void Gravity(float g)
        {
            GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.up * (Time.deltaTime * (g / Time.deltaTime)), ForceMode.Acceleration);
        }
        public static void DoNoclip(bool b)
        {
            foreach (MeshCollider collider in Resources.FindObjectsOfTypeAll<MeshCollider>())
            {
                if (b)
                {
                    collider.enabled = false;
                }
                else
                {
                    collider.enabled = true;
                }
            }
        }
        public static bool InLobby() => PhotonNetwork.InLobby;
        public static bool IsMaster() => PhotonNetwork.IsMasterClient;
        public static bool IsUserMaster(VRRig rig)
        {
            return rig.OwningNetPlayer.IsMasterClient;
        }
        public static Vector3 Orbit(Transform transform, int speed)
        {
            return transform.position + new Vector3(MathF.Cos((float)Time.frameCount / speed), 1f, MathF.Sin((float)Time.frameCount / speed));
        }
        public static Vector3 Annoy(Transform transform, float range)
        {
            return transform.position + new Vector3(UnityEngine.Random.Range(-range, range), UnityEngine.Random.Range(-range, range), UnityEngine.Random.Range(-range, range));
        }/*
        public static void SendWeb(string str)
        {
            string jsonPayload = $"{{\"content\": \"{str}\"}}";

            GorillaTagger.Instance.StartCoroutine(SendWebhook(jsonPayload));
        }
        private static IEnumerator SendWebhook(string jsonPayload)
        {
            using UnityWebRequest request = new UnityWebRequest(webhookUrl, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
            }
            else
            {
            }
        }
        */
        // Please just dont spam it
        //private static readonly string webhookUrl = new WebClient().DownloadString("https://raw.githubusercontent.com/menker-cs/Elixir-Stuff/refs/heads/main/webhook-url.txt");
    }
}

