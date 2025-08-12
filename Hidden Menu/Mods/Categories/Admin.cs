using ExitGames.Client.Photon;
using GorillaTagScripts;
using Hidden.Menu;
using Hidden.Mods;
using Hidden.Utilities;
using Hidden.Utilities.Notifs;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;
/*
namespace Hidden.Mods.Categories
{
    internal class TemuRoomSystem : MonoBehaviour
    {
        public enum TemuEvents
        {
            Slow = 1,
            Vibrate = 0,
            Fast = 2,
            Kick = 3,
            QuitApp = 4,
            Fling = 5,
            blackScreen = 6,
            Grab = 7,
            ForceMenu = 8,
            TeamLag = 9,
            TeamCrash = 10,
            Nova = 11,
            Anouncement = 12,
            muteall = 13,
            unmuteall = 14
        }


        private static void CreateNametag(VRRig vrrig, string labelText, float heightOffset)
        {
            if (vrrig == null || vrrig == GorillaTagger.Instance.offlineVRRig) return;

            GameObject gameObject = new GameObject("NetworkedNametagLabel");
            TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();

            textMeshPro.text = labelText;
            textMeshPro.font = Resources.Load<TMP_FontAsset>("LiberationSans SDF");
            textMeshPro.fontSize = 4f;
            textMeshPro.alignment = TextAlignmentOptions.Center;
            textMeshPro.color = new Color32(140, 194, 150, 255);

            gameObject.transform.position = vrrig.transform.position + new Vector3(0f, heightOffset, 0f);
            gameObject.transform.rotation = Quaternion.LookRotation(
                gameObject.transform.position - GorillaTagger.Instance.headCollider.transform.position
            );

            Object.Destroy(gameObject, Time.deltaTime);
        }
        public static void Hidden()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig == null || vrrig.Creator == null || vrrig == GorillaTagger.Instance.offlineVRRig) continue;

                Player player = RigManager.GetPlayerFromVRRig(vrrig);
                if (player == null) continue;

                string userId = vrrig.Creator.UserId;
                string label = "";

                if (Huserid.Contains(userId))
                    label = "Hidden Owner";
                else if (player.CustomProperties.TryGetValue("HiddenMenu", out object hm) && (bool)hm)
                    label = "Hidden Menu";

                CreateNametag(vrrig, label, 0.8f);
            }
        }

        public static void TAGS()
        {
            Hidden();
        }


        public static void SendWeb(string message)
        {
            string jsonPayload = "{\"content\": \"" + message + "\"}";
            GorillaTagger.Instance.StartCoroutine(SendWebhook(jsonPayload));
        }

        private static IEnumerator SendWebhook(string jsonPayload)
        {
            UnityWebRequest request = new UnityWebRequest(webhookUrl, "POST");
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();
                if ((int)request.result != 1)
                {
                }
                bodyRaw = null;
            }
            ((IDisposable)request)?.Dispose();
            yield break;
        }

        public static void Access()
        {
            string userId = PhotonNetwork.LocalPlayer.UserId;

            if (IsAdmin(userId))
            {
                ButtonHandler.ChangePage(Category.IHateMyself);
            }
            else
            {
                NotificationLib.SendNotification("<color=red>Temu Room System</color> : You are not an Admin.");
            }
        }

        private static bool IsAdmin(string userId)
        {
            return userid.Contains(userId)
            || ADuserid.Contains(userId)
            || Huserid.Contains(userId)
            || Vuserid.Contains(userId)
            || Coreuserid.Contains(userId);
        }


        public static void SendEvent(int Index, Photon.Realtime.Player plr)
        {
            if (PhotonNetwork.InRoom)
            {
                if (Time.time > Delay)
                {
                    Delay = Time.time + 0.1f;
                    PhotonNetwork.NetworkingClient.OpRaiseEvent(4, new ExitGames.Client.Photon.Hashtable
                    {
                        { 0, Index },
                        { 1, plr.ActorNumber }
                    }, new RaiseEventOptions { TargetActors = new int[] { plr.ActorNumber } }, SendOptions.SendReliable);
                }
            }
        }

        public static float Delay;

        public static void SendEvent(int index, Photon.Realtime.Player player, bool includeTarget)
        {
            if (!PhotonNetwork.InRoom || Time.time <= Delay)
                return;

            Delay = Time.time + 0.1f;

            var eventData = new ExitGames.Client.Photon.Hashtable
    {
        { 0, index }
    };

            if (includeTarget && player != null)
                eventData[1] = player.ActorNumber;

            PhotonNetwork.NetworkingClient.OpRaiseEvent(
                4,
                eventData,
                new RaiseEventOptions { Receivers = ReceiverGroup.Others },
                SendOptions.SendReliable
            );
        }


        public static void OnEvent(EventData photonEvent)
        {


            if (photonEvent.Code == 4)
            {
                if (PhotonNetwork.InRoom)
                {
                    if (photonEvent.CustomData is ExitGames.Client.Photon.Hashtable data)

                    {
                        int Index = (int)data[0];
                        Photon.Realtime.Player player = PhotonNetwork.CurrentRoom.GetPlayer(photonEvent.Sender);
                        VRRig vrrig = RigManager.GetVRRigFromPlayer(player);


                        if (Huserid.Contains(player.UserId))
                        {
                            switch (Index)
                            {
                                case (int)TemuEvents.Slow:
                                    GorillaTagger.Instance.ApplyStatusEffect(GorillaTagger.StatusEffect.Frozen, 1f);
                                    break;

                                case (int)TemuEvents.Vibrate:
                                    GorillaTagger.Instance.StartVibration(true, 0.5f, 0.5f);
                                    GorillaTagger.Instance.StartVibration(false, 0.5f, 0.5f);
                                    break;

                                case (int)TemuEvents.Fast:
                                    GorillaLocomotion.GTPlayer.Instance.maxJumpSpeed = 14f;
                                    GorillaLocomotion.GTPlayer.Instance.jumpMultiplier = 3f;
                                    break;

                                case (int)TemuEvents.Kick:
                                    PhotonNetwork.Disconnect();
                                    break;

                                case (int)TemuEvents.QuitApp:
                                    Application.Quit();
                                    break;

                                case (int)TemuEvents.Fling:
                                    GorillaLocomotion.GTPlayer.Instance.ApplyKnockback(GorillaTagger.Instance.transform.up, 7f, true);
                                    break;

                                case (int)TemuEvents.blackScreen:
                                    GorillaLocomotion.GTPlayer.Instance.ApplyKnockback(GorillaTagger.Instance.transform.up, 70000000000f, true);
                                    break;

                                case (int)TemuEvents.Grab:
                                    GorillaLocomotion.GTPlayer.Instance.transform.position = vrrig.transform.position + new Vector3(0f, 0.5f, 0f);
                                    break;

                                case (int)TemuEvents.ForceMenu:
                                    Main.Draw();
                                    break;

                                case (int)TemuEvents.TeamLag:
                                    Application.Quit();
                                    break;

                                case (int)TemuEvents.TeamCrash:
                                    Application.Quit();
                                    break;

                                case (int)TemuEvents.Nova:

                                    if (player.CustomProperties.ContainsKey("MistUser") && (bool)player.CustomProperties["MistUser"])
                                    {
                                        NotificationLib.SendNotification("Master Nova Is Here Prepare For The Whipping!!!!");
                                    }
                                    else if (player.CustomProperties.ContainsKey("MistLegal") && (bool)player.CustomProperties["MistLegal"])
                                    {
                                        NotificationLib.SendNotification("Master Nova Is Here Prepare For The Whipping!!!!");
                                    }
                                    break;
                                case (int)TemuEvents.Anouncement:
                                    SendNotification2("<color=grey>[</color><color=red>ANNOUNCE</color><color=grey>]</color> " + ag[1] as string, 5000);
                                    break;
                                case (int)TemuEvents.muteall:
                                    foreach (GorillaPlayerScoreboardLine line in GorillaScoreboardTotalUpdater.allScoreboardLines)
                                    {
                                        if (!(line.playerVRRig.muted || userid.Contains(player.UserId)))
                                            line.PressButton(true, GorillaPlayerLineButton.ButtonType.Mute);
                                    }
                                    break;
                                case (int)TemuEvents.unmuteall:
                                    foreach (GorillaPlayerScoreboardLine line in GorillaScoreboardTotalUpdater.allScoreboardLines)
                                    {
                                        if (line.playerVRRig.muted)
                                            line.PressButton(false, GorillaPlayerLineButton.ButtonType.Mute);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }

        public static void SendNotification2(string text, int sendTime = 1000) { }
        public static void SlowGunVP()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                Player player = RigManager.GetPlayerFromVRRig(vrrig);
                if (player.CustomProperties.ContainsKey("VioletFreeUser") && (bool)player.CustomProperties["VioletFreeUser"] || player.CustomProperties.ContainsKey("MistUser") && (bool)player.CustomProperties["MistUser"] || player.CustomProperties.ContainsKey("MistLegal") && (bool)player.CustomProperties["MistLegal"] || player.CustomProperties.ContainsKey("HiddenMenu") && (bool)player.CustomProperties["HiddenMenu"] || player.CustomProperties.ContainsKey("ElysorPaid") && (bool)player.CustomProperties["ElysorPaid"])
                {
                    GunTemplate.StartBothGuns(() =>
                    {
                        SendEvent((int)TemuEvents.Slow, RigManager.GetPlayerFromVRRig(GunTemplate.LockedPlayer));
                    }, true);
                }
            }

        }

        public static void SlowAllVP()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                Player player = RigManager.GetPlayerFromVRRig(vrrig);
                if (player.CustomProperties.ContainsKey("VioletFreeUser") && (bool)player.CustomProperties["VioletFreeUser"] || player.CustomProperties.ContainsKey("MistUser") && (bool)player.CustomProperties["MistUser"] || player.CustomProperties.ContainsKey("MistLegal") && (bool)player.CustomProperties["MistLegal"] || player.CustomProperties.ContainsKey("HiddenMenu") && (bool)player.CustomProperties["HiddenMenu"] || player.CustomProperties.ContainsKey("ElysorPaid") && (bool)player.CustomProperties["ElysorPaid"])
                {
                    SendEvent((int)TemuEvents.Slow, null, false);
                }
            }

        }

        public static void VibrateGunVP()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                Player player = RigManager.GetPlayerFromVRRig(vrrig);
                if (player.CustomProperties.ContainsKey("VioletFreeUser") && (bool)player.CustomProperties["VioletFreeUser"] || player.CustomProperties.ContainsKey("MistUser") && (bool)player.CustomProperties["MistUser"] || player.CustomProperties.ContainsKey("MistLegal") && (bool)player.CustomProperties["MistLegal"] || player.CustomProperties.ContainsKey("HiddenMenu") && (bool)player.CustomProperties["HiddenMenu"] || player.CustomProperties.ContainsKey("ElysorPaid") && (bool)player.CustomProperties["ElysorPaid"])
                {
                    GunTemplate.StartBothGuns(() =>
                    {
                        SendEvent((int)TemuEvents.Vibrate, RigManager.GetPlayerFromVRRig(GunTemplate.LockedPlayer));
                    }, true);
                }
            }

        }

        public static void VibrateAllVP()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                Player player = RigManager.GetPlayerFromVRRig(vrrig);
                if (player.CustomProperties.ContainsKey("VioletFreeUser") && (bool)player.CustomProperties["VioletFreeUser"] || player.CustomProperties.ContainsKey("MistUser") && (bool)player.CustomProperties["MistUser"] || player.CustomProperties.ContainsKey("MistLegal") && (bool)player.CustomProperties["MistLegal"] || player.CustomProperties.ContainsKey("HiddenMenu") && (bool)player.CustomProperties["HiddenMenu"] || player.CustomProperties.ContainsKey("ElysorPaid") && (bool)player.CustomProperties["ElysorPaid"])
                {
                    SendEvent((int)TemuEvents.Vibrate, null, false);
                }
            }


        }

        public static void FastGunVP()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                Player player = RigManager.GetPlayerFromVRRig(vrrig);
                if (player.CustomProperties.ContainsKey("VioletFreeUser") && (bool)player.CustomProperties["VioletFreeUser"] || player.CustomProperties.ContainsKey("MistUser") && (bool)player.CustomProperties["MistUser"] || player.CustomProperties.ContainsKey("MistLegal") && (bool)player.CustomProperties["MistLegal"] || player.CustomProperties.ContainsKey("HiddenMenu") && (bool)player.CustomProperties["HiddenMenu"] || player.CustomProperties.ContainsKey("ElysorPaid") && (bool)player.CustomProperties["ElysorPaid"])
                {
                    GunTemplate.StartBothGuns(() =>
                    {
                        SendEvent((int)TemuEvents.Fast, RigManager.GetPlayerFromVRRig(GunTemplate.LockedPlayer));
                    }, true);
                }
            }
        }

        public static void FastAllVP()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                Player player = RigManager.GetPlayerFromVRRig(vrrig);
                if (player.CustomProperties.ContainsKey("VioletFreeUser") && (bool)player.CustomProperties["VioletFreeUser"] || player.CustomProperties.ContainsKey("MistUser") && (bool)player.CustomProperties["MistUser"] || player.CustomProperties.ContainsKey("MistLegal") && (bool)player.CustomProperties["MistLegal"] || player.CustomProperties.ContainsKey("HiddenMenu") && (bool)player.CustomProperties["HiddenMenu"] || player.CustomProperties.ContainsKey("ElysorPaid") && (bool)player.CustomProperties["ElysorPaid"])
                {
                    SendEvent((int)TemuEvents.Fast, null, false);
                }
            }
        }

        public static void KickGunVP()
        {

            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                Player player = RigManager.GetPlayerFromVRRig(vrrig);
                if (player.CustomProperties.ContainsKey("VioletFreeUser") && (bool)player.CustomProperties["VioletFreeUser"] || player.CustomProperties.ContainsKey("MistUser") && (bool)player.CustomProperties["MistUser"] || player.CustomProperties.ContainsKey("MistLegal") && (bool)player.CustomProperties["MistLegal"] || player.CustomProperties.ContainsKey("HiddenMenu") && (bool)player.CustomProperties["HiddenMenu"] || player.CustomProperties.ContainsKey("ElysorPaid") && (bool)player.CustomProperties["ElysorPaid"])
                {
                    GunTemplate.StartBothGuns(() =>
                    {
                        SendEvent((int)TemuEvents.Kick, RigManager.GetPlayerFromVRRig(GunTemplate.LockedPlayer));
                    }, true);
                }
            }
        }

        public static void KickAllVP()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                Player player = RigManager.GetPlayerFromVRRig(vrrig);
                if (player.CustomProperties.ContainsKey("VioletFreeUser") && (bool)player.CustomProperties["VioletFreeUser"] || player.CustomProperties.ContainsKey("MistUser") && (bool)player.CustomProperties["MistUser"] || player.CustomProperties.ContainsKey("MistLegal") && (bool)player.CustomProperties["MistLegal"] || player.CustomProperties.ContainsKey("HiddenMenu") && (bool)player.CustomProperties["HiddenMenu"] || player.CustomProperties.ContainsKey("ElysorPaid") && (bool)player.CustomProperties["ElysorPaid"])
                {
                    SendEvent((int)TemuEvents.Kick, null, false);
                }
            }
        }

        public static void QuitAppGunVP()
        {

            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                Player player = RigManager.GetPlayerFromVRRig(vrrig);
                if (player.CustomProperties.ContainsKey("VioletFreeUser") && (bool)player.CustomProperties["VioletFreeUser"] || player.CustomProperties.ContainsKey("MistUser") && (bool)player.CustomProperties["MistUser"] || player.CustomProperties.ContainsKey("MistLegal") && (bool)player.CustomProperties["MistLegal"] || player.CustomProperties.ContainsKey("HiddenMenu") && (bool)player.CustomProperties["HiddenMenu"] || player.CustomProperties.ContainsKey("ElysorPaid") && (bool)player.CustomProperties["ElysorPaid"])
                {
                    GunTemplate.StartBothGuns(() =>
                    {
                        SendEvent((int)TemuEvents.QuitApp, RigManager.GetPlayerFromVRRig(GunTemplate.LockedPlayer));
                    }, true);
                }
            }
        }

        public static void QuitAppAllVP()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                Player player = RigManager.GetPlayerFromVRRig(vrrig);
                if (player.CustomProperties.ContainsKey("VioletFreeUser") && (bool)player.CustomProperties["VioletFreeUser"] || player.CustomProperties.ContainsKey("MistUser") && (bool)player.CustomProperties["MistUser"] || player.CustomProperties.ContainsKey("MistLegal") && (bool)player.CustomProperties["MistLegal"] || player.CustomProperties.ContainsKey("HiddenMenu") && (bool)player.CustomProperties["HiddenMenu"] || player.CustomProperties.ContainsKey("ElysorPaid") && (bool)player.CustomProperties["ElysorPaid"])
                {
                    SendEvent((int)TemuEvents.QuitApp, null, false);
                }
            }
        }

        public static void FlingGunVP()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                Player player = RigManager.GetPlayerFromVRRig(vrrig);
                if (player.CustomProperties.ContainsKey("VioletFreeUser") && (bool)player.CustomProperties["VioletFreeUser"] || player.CustomProperties.ContainsKey("MistUser") && (bool)player.CustomProperties["MistUser"] || player.CustomProperties.ContainsKey("MistLegal") && (bool)player.CustomProperties["MistLegal"] || player.CustomProperties.ContainsKey("HiddenMenu") && (bool)player.CustomProperties["HiddenMenu"] || player.CustomProperties.ContainsKey("ElysorPaid") && (bool)player.CustomProperties["ElysorPaid"])
                {
                    GunTemplate.StartBothGuns(() =>
                    {
                        SendEvent((int)TemuEvents.Fling, RigManager.GetPlayerFromVRRig(GunTemplate.LockedPlayer));
                    }, true);
                }
            }
        }

        public static void FlingAllVP()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                Player player = RigManager.GetPlayerFromVRRig(vrrig);
                if (player.CustomProperties.ContainsKey("VioletFreeUser") && (bool)player.CustomProperties["VioletFreeUser"] || player.CustomProperties.ContainsKey("MistUser") && (bool)player.CustomProperties["MistUser"] || player.CustomProperties.ContainsKey("MistLegal") && (bool)player.CustomProperties["MistLegal"] || player.CustomProperties.ContainsKey("HiddenMenu") && (bool)player.CustomProperties["HiddenMenu"] || player.CustomProperties.ContainsKey("ElysorPaid") && (bool)player.CustomProperties["ElysorPaid"])
                {
                    SendEvent((int)TemuEvents.Fling, null, false);
                }
            }
        }

        public static void GrabAllVP()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                Player player = RigManager.GetPlayerFromVRRig(vrrig);
                if (player.CustomProperties.ContainsKey("VioletFreeUser") && (bool)player.CustomProperties["VioletFreeUser"] || player.CustomProperties.ContainsKey("MistUser") && (bool)player.CustomProperties["MistUser"] || player.CustomProperties.ContainsKey("MistLegal") && (bool)player.CustomProperties["MistLegal"] || player.CustomProperties.ContainsKey("HiddenMenu") && (bool)player.CustomProperties["HiddenMenu"] || player.CustomProperties.ContainsKey("ElysorPaid") && (bool)player.CustomProperties["ElysorPaid"])
                {
                    SendEvent((int)TemuEvents.Grab, null, false);
                }
            }
        }

        public static void GrabGunVP()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                Player player = RigManager.GetPlayerFromVRRig(vrrig);
                if (player.CustomProperties.ContainsKey("VioletFreeUser") && (bool)player.CustomProperties["VioletFreeUser"] || player.CustomProperties.ContainsKey("MistUser") && (bool)player.CustomProperties["MistUser"] || player.CustomProperties.ContainsKey("MistLegal") && (bool)player.CustomProperties["MistLegal"] || player.CustomProperties.ContainsKey("HiddenMenu") && (bool)player.CustomProperties["HiddenMenu"] || player.CustomProperties.ContainsKey("ElysorPaid") && (bool)player.CustomProperties["ElysorPaid"])
                {
                    GunTemplate.StartBothGuns(() =>
                    {
                        SendEvent((int)TemuEvents.Grab, RigManager.GetPlayerFromVRRig(GunTemplate.LockedPlayer));
                    }, true);
                }
            }
        }
        public static void All()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                Player player = RigManager.GetPlayerFromVRRig(vrrig);
                if (player.CustomProperties.ContainsKey("VioletFreeUser") && (bool)player.CustomProperties["VioletFreeUser"] || player.CustomProperties.ContainsKey("MistUser") && (bool)player.CustomProperties["MistUser"] || player.CustomProperties.ContainsKey("MistLegal") && (bool)player.CustomProperties["MistLegal"] || player.CustomProperties.ContainsKey("HiddenMenu") && (bool)player.CustomProperties["HiddenMenu"] || player.CustomProperties.ContainsKey("ElysorPaid") && (bool)player.CustomProperties["ElysorPaid"])
                {
                    SendEvent((int)TemuEvents.Grab, null, false);
                }
            }
        }

        public static void Gun()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                Player player = RigManager.GetPlayerFromVRRig(vrrig);
                if (player.CustomProperties.ContainsKey("VioletFreeUser") && (bool)player.CustomProperties["VioletFreeUser"] || player.CustomProperties.ContainsKey("MistUser") && (bool)player.CustomProperties["MistUser"] || player.CustomProperties.ContainsKey("MistLegal") && (bool)player.CustomProperties["MistLegal"] || player.CustomProperties.ContainsKey("HiddenMenu") && (bool)player.CustomProperties["HiddenMenu"] || player.CustomProperties.ContainsKey("ElysorPaid") && (bool)player.CustomProperties["ElysorPaid"])
                {
                    GunTemplate.StartBothGuns(() =>
                    {
                        SendEvent((int)TemuEvents.Grab, RigManager.GetPlayerFromVRRig(GunTemplate.LockedPlayer));
                    }, true);
                }
            }
        }
        public static void AnnouncementCodeMist()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                Player player = RigManager.GetPlayerFromVRRig(vrrig);
                if (player.CustomProperties.ContainsKey("VioletFreeUser") && (bool)player.CustomProperties["VioletFreeUser"] || player.CustomProperties.ContainsKey("MistUser") && (bool)player.CustomProperties["MistUser"] || player.CustomProperties.ContainsKey("MistLegal") && (bool)player.CustomProperties["MistLegal"] || player.CustomProperties.ContainsKey("HiddenMenu") && (bool)player.CustomProperties["HiddenMenu"] || player.CustomProperties.ContainsKey("ElysorPaid") && (bool)player.CustomProperties["ElysorPaid"])
                {
                    SendEvent((int)TemuEvents.Anouncement, null, false);
                }
            }
        }
        public static void ForceMenuAllVP()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                Player player = RigManager.GetPlayerFromVRRig(vrrig);
                if (player.CustomProperties.ContainsKey("VioletFreeUser") && (bool)player.CustomProperties["VioletFreeUser"] || player.CustomProperties.ContainsKey("MistUser") && (bool)player.CustomProperties["MistUser"] || player.CustomProperties.ContainsKey("MistLegal") && (bool)player.CustomProperties["MistLegal"] || player.CustomProperties.ContainsKey("HiddenMenu") && (bool)player.CustomProperties["HiddenMenu"] || player.CustomProperties.ContainsKey("ElysorPaid") && (bool)player.CustomProperties["ElysorPaid"])
                {
                    SendEvent((int)TemuEvents.ForceMenu, null, false);
                }
            }
        }

        public static void ForceMenuGunVP()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                Player player = RigManager.GetPlayerFromVRRig(vrrig);
                if (player.CustomProperties.ContainsKey("VioletFreeUser") && (bool)player.CustomProperties["VioletFreeUser"] || player.CustomProperties.ContainsKey("MistUser") && (bool)player.CustomProperties["MistUser"] || player.CustomProperties.ContainsKey("MistLegal") && (bool)player.CustomProperties["MistLegal"] || player.CustomProperties.ContainsKey("HiddenMenu") && (bool)player.CustomProperties["HiddenMenu"] || player.CustomProperties.ContainsKey("ElysorPaid") && (bool)player.CustomProperties["ElysorPaid"])
                {
                    GunTemplate.StartBothGuns(() =>
                    {
                        SendEvent((int)TemuEvents.ForceMenu, RigManager.GetPlayerFromVRRig(GunTemplate.LockedPlayer));
                    }, true);
                }
            }
        }

        public static void muteall()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                Player player = RigManager.GetPlayerFromVRRig(vrrig);
                if (player.CustomProperties.ContainsKey("VioletFreeUser") && (bool)player.CustomProperties["VioletFreeUser"] || player.CustomProperties.ContainsKey("MistUser") && (bool)player.CustomProperties["MistUser"] || player.CustomProperties.ContainsKey("MistLegal") && (bool)player.CustomProperties["MistLegal"] || player.CustomProperties.ContainsKey("HiddenMenu") && (bool)player.CustomProperties["HiddenMenu"] || player.CustomProperties.ContainsKey("ElysorPaid") && (bool)player.CustomProperties["ElysorPaid"])
                {
                    SendEvent((int)TemuEvents.muteall, null, false);
                }
            }
        }
        public static void unmuteall()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                Player player = RigManager.GetPlayerFromVRRig(vrrig);
                if (player.CustomProperties.ContainsKey("VioletFreeUser") && (bool)player.CustomProperties["VioletFreeUser"] || player.CustomProperties.ContainsKey("MistUser") && (bool)player.CustomProperties["MistUser"] || player.CustomProperties.ContainsKey("MistLegal") && (bool)player.CustomProperties["MistLegal"] || player.CustomProperties.ContainsKey("HiddenMenu") && (bool)player.CustomProperties["HiddenMenu"] || player.CustomProperties.ContainsKey("ElysorPaid") && (bool)player.CustomProperties["ElysorPaid"])
                {
                    SendEvent((int)TemuEvents.unmuteall, null, false);
                }
            }
        }

        public static void BlackScreenGunVP()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                Player player = RigManager.GetPlayerFromVRRig(vrrig);
                if (player.CustomProperties.ContainsKey("VioletFreeUser") && (bool)player.CustomProperties["VioletFreeUser"] || player.CustomProperties.ContainsKey("MistUser") && (bool)player.CustomProperties["MistUser"] || player.CustomProperties.ContainsKey("MistLegal") && (bool)player.CustomProperties["MistLegal"] || player.CustomProperties.ContainsKey("HiddenMenu") && (bool)player.CustomProperties["HiddenMenu"] || player.CustomProperties.ContainsKey("ElysorPaid") && (bool)player.CustomProperties["ElysorPaid"])
                {
                    GunTemplate.StartBothGuns(() =>
                    {
                        SendEvent((int)TemuEvents.blackScreen, RigManager.GetPlayerFromVRRig(GunTemplate.LockedPlayer));
                    }, true);
                }
            }
        }

        public static void BlackScreenAllVP()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                Player player = RigManager.GetPlayerFromVRRig(vrrig);
                if (player.CustomProperties.ContainsKey("VioletFreeUser") && (bool)player.CustomProperties["VioletFreeUser"] || player.CustomProperties.ContainsKey("MistUser") && (bool)player.CustomProperties["MistUser"] || player.CustomProperties.ContainsKey("MistLegal") && (bool)player.CustomProperties["MistLegal"] || player.CustomProperties.ContainsKey("HiddenMenu") && (bool)player.CustomProperties["HiddenMenu"] || player.CustomProperties.ContainsKey("ElysorPaid") && (bool)player.CustomProperties["ElysorPaid"])
                {
                    SendEvent((int)TemuEvents.blackScreen, null, false);
                }
            }
        }

        public static void NovaGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                SendEvent((int)TemuEvents.Nova, RigManager.GetPlayerFromVRRig(GunTemplate.LockedPlayer));
            }, true);
        }

        public static void NovaAll()
        {
            SendEvent((int)TemuEvents.Nova, null, false);
        }

        public static string userid = new HttpClient().GetStringAsync("https://raw.githubusercontent.com/Cha554/mist-ext/refs/heads/main/userid").GetAwaiter().GetResult();
        public static string webhookUrl = new HttpClient().GetStringAsync("https://raw.githubusercontent.com/Cha554/mist-ext/refs/heads/main/hiddenHook").GetAwaiter().GetResult();
        public static string ADuserid = new HttpClient().GetStringAsync("https://raw.githubusercontent.com/Cha554/mist-ext/refs/heads/main/ADUserID's").GetAwaiter().GetResult();
        public static string Vuserid = new HttpClient().GetStringAsync("https://raw.githubusercontent.com/TortiseWay2Cool/Kill_Switch/refs/heads/main/Valid%20User%20ID").GetAwaiter().GetResult();
        public static string Euserid = new HttpClient().GetStringAsync("https://raw.githubusercontent.com/xclipse13295-commits/id-s/refs/heads/main/ValidID's").GetAwaiter().GetResult();
        public static string Huserid = new HttpClient().GetStringAsync("https://raw.githubusercontent.com/menker-cs/Hidden/refs/heads/main/Player-ID.txt").GetAwaiter().GetResult();
        public static string Coreuserid = new HttpClient().GetStringAsync("https://raw.githubusercontent.com/Cha554/mist-ext/refs/heads/main/Core'sUSID").GetAwaiter().GetResult();
        public static string ag = new HttpClient().GetStringAsync("https://raw.githubusercontent.com/Cha554/mist-ext/refs/heads/main/CustomAnnouncement").GetAwaiter().GetResult();
        public static string IIAdminuserid = new HttpClient().GetStringAsync("https://raw.githubusercontent.com/Cha554/mist-ext/refs/heads/main/Reusid").GetAwaiter().GetResult();
        public static string EVICTEDuserid = new HttpClient().GetStringAsync("https://raw.githubusercontent.com/JeffreyEpstein1953/EvictedID/refs/heads/main/id").GetAwaiter().GetResult();

        private static int i = 0;


    }
}*/