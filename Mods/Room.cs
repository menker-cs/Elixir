using BepInEx;
using Elixir.Notifications;
using Elixir.Utilities;
using Elixir.Utilities.Notifs;
using GorillaGameModes;
using GorillaNetworking;
using Photon.Pun;
using Photon.Realtime;
using System.Threading.Tasks;
using UnityEngine;
using static Elixir.Utilities.Variables;

namespace Elixir.Mods.Categories
{
    public class Room : MonoBehaviourPunCallbacks
    {
        public static void QuitGTAG()
        {
            Application.Quit();
        }

        public static void Disconnect()
        {
            PhotonNetwork.Disconnect();
        }
        public static void JoinRandomPublic()
        {
            GorillaNetworkJoinTrigger trigger = PhotonNetworkController.Instance.currentJoinTrigger ?? GorillaComputer.instance.GetJoinTriggerForZone("forest");
            PhotonNetworkController.Instance.AttemptToJoinPublicRoom(trigger, GorillaNetworking.JoinType.Solo);
        }
        public static void PrimaryDisconnect()
        {
            if (ControllerInputPoller.instance.rightControllerPrimaryButton | UnityInput.Current.GetKey(KeyCode.F))
            {
                PhotonNetwork.Disconnect();
            }
        }
        public static void Servers(string svr)
        {
            PhotonNetwork.ConnectToRegion(svr);
        }
        public static void JoinRoom(string RoomCode)
        {
            PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(RoomCode, GorillaNetworking.JoinType.Solo);
        }
        public static string roomCode;
        public static void Reconnect()
        {
            roomCode = PhotonNetwork.CurrentRoom.Name;
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.Disconnect();
            }
            Task.Delay(1500).ContinueWith(delegate (Task _)
            {
                PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(roomCode, 0);
            });
        }
        public static void DisableNetworkTriggers()
        {
            GameObject.Find("Environment Objects/TriggerZones_Prefab/JoinRoomTriggers_Prefab").SetActive(false);
        }
        public static void EnableNetworkTriggers()
        {
            GameObject.Find("Environment Objects/TriggerZones_Prefab/JoinRoomTriggers_Prefab").SetActive(true);
        }
        public static void ReportAll()
        {
            GorillaPlayerScoreboardLine[] Board = UnityEngine.Object.FindObjectsOfType<GorillaPlayerScoreboardLine>();
            foreach (GorillaPlayerScoreboardLine report in Board)
            {
                if (report.linePlayer != null)
                {
                    report.PressButton(true, GorillaPlayerLineButton.ButtonType.HateSpeech);
                }
            }
        }
        public static void MuteAll()
        {
            GorillaPlayerScoreboardLine[] Board = UnityEngine.Object.FindObjectsOfType<GorillaPlayerScoreboardLine>();
            foreach (GorillaPlayerScoreboardLine mute in Board)
            {
                if (mute.linePlayer != null)
                {
                    mute.PressButton(true, GorillaPlayerLineButton.ButtonType.Mute);
                    mute.muteButton.isOn = true;
                    mute.muteButton.UpdateColor();
                }
            }
        }
        public static void MuteGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                GorillaPlayerScoreboardLine[] Board = UnityEngine.Object.FindObjectsOfType<GorillaPlayerScoreboardLine>();
                foreach (GorillaPlayerScoreboardLine mute in Board)
                {
                    if (mute.linePlayer == GunTemplate.LockedPlayer!.OwningNetPlayer)
                    {
                        mute.PressButton(true, GorillaPlayerLineButton.ButtonType.Mute);
                        mute.muteButton.isOn = true;
                        mute.muteButton.UpdateColor();
                    }
                }
            }, true);
        }
        public static void ReportGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                GorillaPlayerScoreboardLine[] Board = UnityEngine.Object.FindObjectsOfType<GorillaPlayerScoreboardLine>();
                foreach (GorillaPlayerScoreboardLine report in Board)
                {
                    if (report.linePlayer == GunTemplate.LockedPlayer!.OwningNetPlayer)
                    {
                        report.PressButton(true, GorillaPlayerLineButton.ButtonType.HateSpeech);
                    }
                }
            }, true);
        }
        public static void SetGamemode(GameModeType gameModeType)
        {
            // creds to @meep670 for this!
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.Disconnect();
                GorillaComputer.instance.SetGameModeWithoutButton(gameModeType.ToString());
            }
            else
            {
                GorillaComputer.instance.SetGameModeWithoutButton(gameModeType.ToString());
            }
        }
        #region nigport
        public static bool reconnectReport = false;
        public static void AntiReport()
        {
            if (Settings.VisReportBool)
            {
                if (PhotonNetwork.InRoom)
                {
                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        if (vrrig != GorillaTagger.Instance.offlineVRRig)
                        {
                            Vector3 rHand = vrrig.rightHandTransform.position;
                            Vector3 lHand = vrrig.leftHandTransform.position;
                            rHand = vrrig.rightHandTransform.position + vrrig.rightHandTransform.forward * 0.125f;
                            lHand = vrrig.leftHandTransform.position + vrrig.leftHandTransform.forward * 0.125f;
                            float range = 0.4f;
                            foreach (GorillaPlayerScoreboardLine gorillaPlayerScoreboardLine in GorillaScoreboardTotalUpdater.allScoreboardLines)
                            {
                                if (gorillaPlayerScoreboardLine.linePlayer == NetworkSystem.Instance.LocalPlayer)
                                {
                                    Vector3 reportButton = gorillaPlayerScoreboardLine.reportButton.gameObject.transform.position + new Vector3(0f, 0.001f, 0.0004f);
                                    VisualizeAntiReport(reportButton, range);
                                    if (Vector3.Distance(reportButton, lHand) < range)
                                    {
                                        NotificationLib.SendNotification("<color=blue>Anti-Report:</color> : " + vrrig.playerText1.text + " Attempted to Report You!");
                                        if (reconnectReport)
                                            Reconnect();
                                        else
                                            Disconnect();
                                    }
                                    if (Vector3.Distance(reportButton, rHand) < range)
                                    {
                                        NotificationLib.SendNotification("<color=blue>Anti-Report</color> : " + vrrig.playerText1.text + " Attempted to <color=red>Report</color> You!");
                                        if (reconnectReport)
                                            Reconnect();
                                        else
                                            Disconnect();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (PhotonNetwork.InRoom)
                {
                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        if (vrrig != GorillaTagger.Instance.offlineVRRig)
                        {
                            Vector3 rHand = vrrig.rightHandTransform.position;
                            Vector3 lHand = vrrig.leftHandTransform.position;
                            rHand = vrrig.rightHandTransform.position + vrrig.rightHandTransform.forward * 0.125f;
                            lHand = vrrig.leftHandTransform.position + vrrig.leftHandTransform.forward * 0.125f;
                            float range = 0.4f;
                            foreach (GorillaPlayerScoreboardLine gorillaPlayerScoreboardLine in GorillaScoreboardTotalUpdater.allScoreboardLines)
                            {
                                if (gorillaPlayerScoreboardLine.linePlayer == NetworkSystem.Instance.LocalPlayer)
                                {
                                    Vector3 reportButton = gorillaPlayerScoreboardLine.reportButton.gameObject.transform.position + new Vector3(0f, 0.001f, 0.0004f);
                                    if (Vector3.Distance(reportButton, lHand) < range)
                                    {
                                        if (reconnectReport)
                                            Reconnect();
                                        else
                                            Disconnect();
                                    }
                                    if (Vector3.Distance(reportButton, rHand) < range)
                                    {
                                        if (reconnectReport)
                                            Reconnect();
                                        else
                                            Disconnect();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public static void StrictAntiReport()
        {
            if (Settings.VisReportBool)
            {
                if (PhotonNetwork.InRoom)
                {
                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        if (vrrig != GorillaTagger.Instance.offlineVRRig)
                        {
                            Vector3 rHand = vrrig.rightHandTransform.position;
                            Vector3 lHand = vrrig.leftHandTransform.position;
                            rHand = vrrig.rightHandTransform.position + vrrig.rightHandTransform.forward * 0.125f;
                            lHand = vrrig.leftHandTransform.position + vrrig.leftHandTransform.forward * 0.125f;
                            float range = 1.1f;
                            foreach (GorillaPlayerScoreboardLine gorillaPlayerScoreboardLine in GorillaScoreboardTotalUpdater.allScoreboardLines)
                            {
                                if (gorillaPlayerScoreboardLine.linePlayer == NetworkSystem.Instance.LocalPlayer)
                                {
                                    Vector3 reportButton = gorillaPlayerScoreboardLine.reportButton.gameObject.transform.position + new Vector3(0f, 0.001f, 0.0004f);
                                    VisualizeAntiReport(reportButton, range);
                                    if (Vector3.Distance(reportButton, lHand) < range)
                                    {
                                        //NotificationLib.SendNotification("<color=blue>Anti-Report:</color> : " + vrrig.playerText1.text + " Attempted to Report You!");
                                        Disconnect();
                                    }
                                    if (Vector3.Distance(reportButton, rHand) < range)
                                    {
                                        //NotificationLib.SendNotification("<color=blue>Anti-Report</color> : " + vrrig.playerText1.text + " Attempted to <color=red>Report</color> You!");
                                        Disconnect();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (PhotonNetwork.InRoom)
                {
                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        if (vrrig != GorillaTagger.Instance.offlineVRRig)
                        {
                            Vector3 rHand = vrrig.rightHandTransform.position;
                            Vector3 lHand = vrrig.leftHandTransform.position;
                            rHand = vrrig.rightHandTransform.position + vrrig.rightHandTransform.forward * 0.125f;
                            lHand = vrrig.leftHandTransform.position + vrrig.leftHandTransform.forward * 0.125f;
                            float range = 0.4f;
                            foreach (GorillaPlayerScoreboardLine gorillaPlayerScoreboardLine in GorillaScoreboardTotalUpdater.allScoreboardLines)
                            {
                                if (gorillaPlayerScoreboardLine.linePlayer == NetworkSystem.Instance.LocalPlayer)
                                {
                                    Vector3 reportButton = gorillaPlayerScoreboardLine.reportButton.gameObject.transform.position + new Vector3(0f, 0.001f, 0.0004f);
                                    if (Vector3.Distance(reportButton, lHand) < range)
                                    {
                                        //NotificationLib.SendNotification("<color=blue>Anti-Report:</color> : " + vrrig.playerText1.text + " Attempted to Report You!");
                                        Disconnect();
                                    }
                                    if (Vector3.Distance(reportButton, rHand) < range)
                                    {
                                        //NotificationLib.SendNotification("<color=blue>Anti-Report</color> : " + vrrig.playerText1.text + " Attempted to <color=red>Report</color> You!");
                                        Disconnect();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public static void VisualizeAntiReport(Vector3 position, float range)
        {
            GameObject report = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            UnityEngine.Object.Destroy(report.GetComponent<Collider>());
            UnityEngine.Object.Destroy(report.GetComponent<Rigidbody>());
            report.transform.position = position;
            report.transform.localScale = new Vector3(range, range, range);
            report.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
            Color c = ColorLib.MenuMat[0].color;
            c.a = 0.1f;
            report.GetComponent<Renderer>().material.color = c;
            UnityEngine.Object.Destroy(report, Time.deltaTime);

        }
        #endregion
    }
}