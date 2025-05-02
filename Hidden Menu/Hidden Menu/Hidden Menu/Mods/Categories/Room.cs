using GorillaNetworking;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using static Hidden.Utilities.NotificationLib;
using static Hidden.Utilities.Variables;
using static Hidden.Menu.Main;
using Hidden.Utilities;
using System.Diagnostics;
using Valve.VR;
using Cinemachine;
using HarmonyLib;
using System.Reflection;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fusion;
using UnityEngine.XR;
using Photon.Realtime;
using ExitGames.Client.Photon;
using BepInEx;

namespace Hidden.Mods.Categories
{
    public class Room : MonoBehaviourPunCallbacks
    {
        public static string roomCode;

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
            if (PhotonNetwork.InRoom)
            {
                UnityEngine.Debug.LogWarning("<color=blue>Photon</color> : Already connected to a room.");
                NotificationLib.SendNotification("<color=blue>Photon</color> : Already connected to a room.");
                return;
            }

            string currentMap = DetectCurrentMap();
            if (currentMap == null)
            {
                UnityEngine.Debug.LogError("<color=blue>Photon</color> : Unable to detect the current map.");
                NotificationLib.SendNotification("<color=blue>Photon</color> : Unable to detect the current map.");
                return;
            }

            string path = GetPathForGameMode(currentMap);
            if (path == null)
            {
                UnityEngine.Debug.LogError($"<color=blue>Photon</color> : No valid path found for map: {currentMap}.");
                NotificationLib.SendNotification($"<color=blue>Photon</color> : No valid path found for map: {currentMap}.");
                return;
            }

            GorillaNetworkJoinTrigger joinTrigger = GameObject.Find(path)?.GetComponent<GorillaNetworkJoinTrigger>();
            if (joinTrigger == null)
            {
                UnityEngine.Debug.LogError($"<color=blue>Photon</color> : Join trigger not found for path: {path}.");
                NotificationLib.SendNotification($"<color=blue>Photon</color> : Join trigger not found for path: {path}.");
                return;
            }

            PhotonNetworkController.Instance.AttemptToJoinPublicRoom(joinTrigger, JoinType.Solo);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            if (returnCode == ErrorCode.GameFull)
            {
                UnityEngine.Debug.LogWarning($"OnJoinRoomFailed : Failed to join room '{roomCode}'. Reason: Is Full.");
                NotificationLib.SendNotification($"<color=red>Error</color> : Failed to join room '{roomCode}'. Reason: Is Full.");
            }
            else
            {
                UnityEngine.Debug.LogWarning($"OnJoinRoomFailed: Failed to join room '{roomCode}'. Reason: {message}.");
                NotificationLib.SendNotification($"<color=red>Error</color>: Failed to join room '{roomCode}'. Reason: {message}.");
            }
        }
        public static void PrimaryDisconnect()
        {
            if (ControllerInputPoller.instance.rightControllerPrimaryButton | UnityInput.Current.GetKey(KeyCode.F))
            {
                PhotonNetwork.Disconnect();
            }
        }
        public static void JoinRoom(string RoomCode)
        {
            PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(RoomCode, JoinType.Solo);
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
            foreach (GorillaPlayerScoreboardLine Report in Board)
            {
                if (Report.linePlayer != null)
                {
                    Report.PressButton(true, GorillaPlayerLineButton.ButtonType.HateSpeech);
                }
            }
        }
        public static void MuteAll()
        {
            GorillaPlayerScoreboardLine[] Board = UnityEngine.Object.FindObjectsOfType<GorillaPlayerScoreboardLine>();
            foreach (GorillaPlayerScoreboardLine Mute in Board)
            {
                if (Mute.linePlayer != null)
                {
                    Mute.PressButton(true, GorillaPlayerLineButton.ButtonType.Mute);
                    Mute.muteButton.isOn = true;
                    Mute.muteButton.UpdateColor();
                }
            }
        }
        #region nigport
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
                                        Disconnect();
                                    }
                                    if (Vector3.Distance(reportButton, rHand) < range)
                                    {
                                        NotificationLib.SendNotification("<color=blue>Anti-Report</color> : " + vrrig.playerText1.text + " Attempted to <color=red>Report</color> You!");
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
                                        NotificationLib.SendNotification("<color=blue>Anti-Report:</color> : " + vrrig.playerText1.text + " Attempted to Report You!");
                                        Disconnect();
                                    }
                                    if (Vector3.Distance(reportButton, rHand) < range)
                                    {
                                        NotificationLib.SendNotification("<color=blue>Anti-Report</color> : " + vrrig.playerText1.text + " Attempted to <color=red>Report</color> You!");
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
                                        NotificationLib.SendNotification("<color=blue>Anti-Report:</color> : " + vrrig.playerText1.text + " Attempted to Report You!");
                                        Disconnect();
                                    }
                                    if (Vector3.Distance(reportButton, rHand) < range)
                                    {
                                        NotificationLib.SendNotification("<color=blue>Anti-Report</color> : " + vrrig.playerText1.text + " Attempted to <color=red>Report</color> You!");
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
                                        NotificationLib.SendNotification("<color=blue>Anti-Report:</color> : " + vrrig.playerText1.text + " Attempted to Report You!");
                                        Disconnect();
                                    }
                                    if (Vector3.Distance(reportButton, rHand) < range)
                                    {
                                        NotificationLib.SendNotification("<color=blue>Anti-Report</color> : " + vrrig.playerText1.text + " Attempted to <color=red>Report</color> You!");
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
            UnityEngine.Object.Destroy(report, Time.deltaTime);
            UnityEngine.Object.Destroy(report.GetComponent<Collider>());
            UnityEngine.Object.Destroy(report.GetComponent<Rigidbody>());
            report.transform.position = position;
            report.transform.localScale = new Vector3(range, range, range);
            report.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
            Color c = MenuColor;
            c.a = 0.1f;
            report.GetComponent<Renderer>().material.color = c;
        }
        #endregion
    }
}