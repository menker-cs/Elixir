using BepInEx;
using GorillaLocomotion;
using Elixir.Utilities;
using Elixir.Utilities.Notifs;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static Elixir.Utilities.GunTemplate;
using static Elixir.Utilities.Variables;

namespace Elixir.Mods.Categories
{
    public class Playerr
    {
        public static void RigGun()
        {
            if (taggerInstance == null) return;

            GunTemplate.StartBothGuns(() =>
            {
                taggerInstance.offlineVRRig.enabled = false;

                taggerInstance.offlineVRRig.transform.position = GunTemplate.spherepointer!.transform.position + new Vector3(0f, 1f, 0f);
            }, false);
            {
                taggerInstance.offlineVRRig.enabled = true;
            }
        }

        #region Advantage
        // TY Cha
        public static void TagPlayer(VRRig plr)
        {
            if (taggerInstance == null) return;

            taggerInstance.offlineVRRig.enabled = false;
            taggerInstance.offlineVRRig.transform.SetPositionAndRotation(plr.transform.position + new Vector3(0f, -0.25f, 0f), plr.transform.rotation);

            PhotonNetwork.SendAllOutgoingCommands();

            MethodInfo method = typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic);
            method?.Invoke(null, Array.Empty<object>());

            PhotonView photonView = GameObject.Find("Player Objects/RigCache/Network Parent/GameMode(Clone)").GetPhotonView();
            if (photonView)
            {
                photonView.RPC("RPC_ReportTag", RpcTarget.All, new object[]
                {
                    plr.Creator.ActorNumber
                });
            }

            taggerInstance.offlineVRRig.enabled = true;
            PhotonNetwork.SendAllOutgoingCommands();

            MethodInfo method2 = typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic);
            method2?.Invoke(null, Array.Empty<object>());

            MethodInfo method3 = typeof(PhotonView).GetMethod("OnSerialize", BindingFlags.Instance | BindingFlags.NonPublic);
            method3?.Invoke(photonView, new object[2]);

            PhotonNetwork.NetworkingClient.LoadBalancingPeer.SendAcksOnly();
        }


        private static readonly Dictionary<int, float> lastTaggedTime = new Dictionary<int, float>();
        private static Photon.Realtime.Player? player;
        public static float Delay;
        public static void TagAura()
        {
            if (taggerInstance == null) return;

            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != taggerInstance.offlineVRRig && (Vector3.Distance(taggerInstance.leftHandTransform.position, vrrig.headMesh.transform.position) < 4f || Vector3.Distance(taggerInstance.rightHandTransform.position, vrrig.headMesh.transform.position) < 4f))
                {
                    PhotonView photonView = GameObject.Find("Player Objects/RigCache/Network Parent/GameMode(Clone)").GetPhotonView();
                    if (photonView)
                    {
                        photonView.RPC("RPC_ReportTag", RpcTarget.All, new object[]
                        {
                            vrrig.Creator.ActorNumber
                        });
                    }
                }
            }
        }
        public static void TagAll()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                int actorNumber = vrrig.Creator.ActorNumber;
                player = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber, false);
                if (player != null && vrrig.Creator.ActorNumber == actorNumber)
                {
                    float time = Time.time;
                    if (!lastTaggedTime.ContainsKey(actorNumber) || time - lastTaggedTime[actorNumber] >= 0.5f)
                    {
                        TagPlayer(vrrig);
                        lastTaggedTime[actorNumber] = time;
                    }
                }
            }
        }
        public static void TagGun()
        {
            GunTemplate.StartBothGuns(delegate
            {
                if (GunTemplate.LockedPlayer != null && PhotonNetwork.InRoom)
                {
                    VRRig LockedPlayer = GunTemplate.LockedPlayer;
                    int actorNumber = LockedPlayer.Creator.ActorNumber;
                    player = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber, false);
                    if (player != null && LockedPlayer.Creator.ActorNumber == actorNumber)
                    {
                        float time = Time.time;
                        if (!lastTaggedTime.ContainsKey(actorNumber) || time - lastTaggedTime[actorNumber] >= 5f)
                        {
                            TagPlayer(LockedPlayer);
                            lastTaggedTime[actorNumber] = time;
                        }
                    }
                }
            }, true);
        }
        public static void TagSelf()
        {
            if (taggerInstance == null) return;

            if (!IAmInfected)
            {
                if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.2f | UnityInput.Current.GetKey(KeyCode.T))
                {
                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        if (RigIsInfected(vrrig))
                        {
                            taggerInstance.offlineVRRig.enabled = false;
                            taggerInstance.offlineVRRig.transform.position = vrrig.rightHandTransform.position;
                            taggerInstance.myVRRig.transform.position = vrrig.rightHandTransform.position;
                            break;
                        }
                    }
                }
                else
                {
                    taggerInstance.offlineVRRig.enabled = true;
                }
            }
            else
            {
                taggerInstance.offlineVRRig.enabled = true;
                NotificationLib.SendNotification("<color=white>[</color><color=blue>Tag Self:</color><color=white>] You are already tagged</color>");
            }
        }
        #endregion

        private static bool isOn = false;
        private static bool wasButtonPressed = false;
        public static void GhostMonke()
        {
            if (taggerInstance == null) return;

            bool isButtonCurrentlyPressed = ControllerInputPoller.instance.rightControllerPrimaryButton;
            if (!wasButtonPressed && isButtonCurrentlyPressed | UnityInput.Current.GetKey(KeyCode.P))
            {
                isOn = !isOn;
            }

            wasButtonPressed = isButtonCurrentlyPressed;

            if (isOn)
            {
                taggerInstance.offlineVRRig.enabled = false;
            }
            else
            {
                taggerInstance.offlineVRRig.enabled = true;
            }
        }

        public static void InvisibleMonke()
        {
            if (taggerInstance == null) return;

            bool isButtonCurrentlyPressed = ControllerInputPoller.instance.rightControllerPrimaryButton;
            if (!wasButtonPressed && isButtonCurrentlyPressed | UnityInput.Current.GetKey(KeyCode.P))
            {
                isOn = !isOn;
            }

            wasButtonPressed = isButtonCurrentlyPressed;

            if (isOn)
            {
                taggerInstance.offlineVRRig.enabled = false;
                taggerInstance.offlineVRRig.transform.position = new Vector3(999f, 999f, 999f);
            }
            else
            {
                taggerInstance.offlineVRRig.enabled = true;
            }
        }
        public static void Spaz()
        {
            if (taggerInstance == null) return;

            taggerInstance.offlineVRRig.head.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360));
            taggerInstance.offlineVRRig.leftHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360));
            taggerInstance.offlineVRRig.rightHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360));

            taggerInstance.offlineVRRig.head.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 180), (float)UnityEngine.Random.Range(0, 180));
            taggerInstance.offlineVRRig.leftHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 180), (float)UnityEngine.Random.Range(0, 180));
            taggerInstance.offlineVRRig.rightHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 180), (float)UnityEngine.Random.Range(0, 180));
        }
        public static void SpazHands()
        {
            if (taggerInstance == null) return;

            taggerInstance.offlineVRRig.leftHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360));
            taggerInstance.offlineVRRig.rightHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360));

            taggerInstance.offlineVRRig.leftHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 180), (float)UnityEngine.Random.Range(0, 180));
            taggerInstance.offlineVRRig.rightHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 180), (float)UnityEngine.Random.Range(0, 180));
        }
        public static void FixHead()
        {
            if (taggerInstance == null) return;

            taggerInstance.offlineVRRig.head.trackingRotationOffset.y = 0f;
            taggerInstance.offlineVRRig.head.trackingRotationOffset.x = 0f;
            taggerInstance.offlineVRRig.head.trackingRotationOffset.z = 0f;
        }
        public static void BackwardsHead()
        {
            if (taggerInstance == null) return;

            taggerInstance.offlineVRRig.head.trackingRotationOffset.y = 180f;
        }
        public static void SnapNeck()
        {
            if (taggerInstance == null) return;

            taggerInstance.offlineVRRig.head.trackingRotationOffset.y = 90f;
        }
        public static void UpsidedownHead()
        {
            if (taggerInstance == null) return;

            taggerInstance.offlineVRRig.head.trackingRotationOffset.z = 180f;
        }
        public static void LongArms(float length)
        {
            if (taggerInstance == null) return;

            taggerInstance.transform.localScale = new Vector3(length, length, length);
        }
        public static void FixArms()
        {
            if (taggerInstance == null) return;

            taggerInstance.transform.localScale = Vector3.one;
        }
        public static void GrabRig()
        {
            if (taggerInstance == null) return;

            if (ControllerInputPoller.instance.rightGrab | UnityInput.Current.GetKey(KeyCode.G))
            {
                taggerInstance.offlineVRRig.enabled = false;
                taggerInstance.offlineVRRig.transform.position = taggerInstance.rightHandTransform.position;
                taggerInstance.offlineVRRig.transform.rotation = taggerInstance.rightHandTransform.rotation;
            }
            else
            {
                taggerInstance.offlineVRRig.enabled = true;
            }
        }
        public static void FreezeRig()
        {
            if (taggerInstance == null) return;

            if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.1f | UnityInput.Current.GetKey(KeyCode.T))
            {
                taggerInstance.offlineVRRig.enabled = false;
                taggerInstance.offlineVRRig.transform.position = taggerInstance.headCollider.transform.position;
                taggerInstance.offlineVRRig.transform.rotation = taggerInstance.headCollider.transform.rotation;
            }
            else
            {
                taggerInstance.offlineVRRig.enabled = true;
            }
        }
        public static void HeadSpinx()
        {
            if (taggerInstance == null) return;

            VRMap head = taggerInstance.offlineVRRig.head;
            head.trackingRotationOffset.x += 15f;
        }
        public static void HeadSpiny()
        {
            if (taggerInstance == null) return;

            VRMap head = taggerInstance.offlineVRRig.head;
            head.trackingRotationOffset.y += 15f;
        }
        public static void HeadSpinz()
        {
            if (taggerInstance == null) return;

            VRMap head = taggerInstance.offlineVRRig.head;
            head.trackingRotationOffset.z += 15f;
        }
        public static void AnnoyPlayerGun()
        {
            if (taggerInstance == null) return;

            GunTemplate.StartBothGuns(() =>
            {
                taggerInstance.offlineVRRig.enabled = false;
                taggerInstance.offlineVRRig.transform.position = Annoy(LockedPlayer!.transform, 1.25f);
            }, true);
            {
                taggerInstance.offlineVRRig.enabled = true;
            }
        }
        public static void FakeLag()
        {
            if (taggerInstance == null) return;

            if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                if (Time.time - nigTime >= delay)
                {
                    lag = !lag;
                    taggerInstance.offlineVRRig.enabled = !lag;

                    nigTime = Time.time;
                }
            }
            else
            {
                taggerInstance.offlineVRRig.enabled = true;
                lag = false;
            }
        }
        public static void Bees()
        {
            if (taggerInstance == null) return;

            if (PhotonNetwork.InRoom)
            {
                if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
                {
                    if (Time.time - nigTime >= delay)
                    {
                        taggerInstance.offlineVRRig.enabled = false;
                        taggerInstance.offlineVRRig.transform.position = RigManager.GetRandomVRRig(false).transform.position + new Vector3(0f, 2f, 0f);

                        nigTime = Time.time;
                    }
                }
                else
                {
                    taggerInstance.offlineVRRig.enabled = true;
                }
            }
            else
            {
                taggerInstance.offlineVRRig.enabled = true;
            }
        }
        public static void FBees()
        {
            if (taggerInstance == null) return;

            if (PhotonNetwork.InRoom)
            {
                if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
                {
                    taggerInstance.offlineVRRig.enabled = false;
                    taggerInstance.offlineVRRig.transform.position = RigManager.GetRandomVRRig(false).transform.position + new Vector3(0f, 2f, 0f);
                }
                else
                {
                    taggerInstance.offlineVRRig.enabled = true;
                }
            }
            else
            {
                taggerInstance.offlineVRRig.enabled = true;
            }
        }
        public static void OrbitPGun()
        {
            if (taggerInstance == null) return;

            GunTemplate.StartBothGuns(() =>
            {
                taggerInstance.offlineVRRig.enabled = false;
                taggerInstance.offlineVRRig.transform.position = Orbit(LockedPlayer!.transform, 15);
                taggerInstance.offlineVRRig.transform.LookAt(LockedPlayer.transform);
            }, true);
            {
                taggerInstance.offlineVRRig.enabled = true;
                taggerInstance.offlineVRRig.transform.rotation = Quaternion.identity;
            }
        }
        public static void OrbitGun()
        {
            if (taggerInstance == null) return;

            GunTemplate.StartBothGuns(() =>
            {
                taggerInstance.offlineVRRig.enabled = false;
                taggerInstance.offlineVRRig.transform.position = Orbit(spherepointer!.transform, 15);
                taggerInstance.offlineVRRig.transform.LookAt(spherepointer.transform);
            }, false);
            {
                taggerInstance.offlineVRRig.enabled = true;
                taggerInstance.offlineVRRig.transform.rotation = Quaternion.identity;
            }
        }
        public static void GrabGun()
        {
            if (taggerInstance == null) return;

            GunTemplate.StartBothGuns(() =>
            {
                taggerInstance.offlineVRRig.enabled = false;
                taggerInstance.offlineVRRig.transform.position = LockedPlayer!.rightHandTransform.position;
                taggerInstance.offlineVRRig.transform.rotation = LockedPlayer.rightHandTransform.rotation;
            }, true);
            {
                taggerInstance.offlineVRRig.enabled = true;
            }
        }
        public static void AnnoySelf()
        {
            if (taggerInstance == null) return;

            if (Inputs.rightGrip() || UnityInput.Current.GetKey(KeyCode.G))
            {
                taggerInstance.offlineVRRig.enabled = false;
                taggerInstance.offlineVRRig.transform.position = Annoy(taggerInstance.headCollider.transform, 1.25f);
            }
            else
            {
                taggerInstance.offlineVRRig.enabled = true;
            }
        }
        public static void OrbitSelf()
        {
            if (taggerInstance == null) return;

            if (Inputs.rightGrip() || UnityInput.Current.GetKey(KeyCode.G))
            {
                taggerInstance.offlineVRRig.enabled = false;
                taggerInstance.offlineVRRig.transform.position = Orbit(taggerInstance.headCollider.transform, 15);
                taggerInstance.offlineVRRig.transform.LookAt(taggerInstance.headCollider.transform);
            }
            else
            {
                taggerInstance.offlineVRRig.enabled = true;
                taggerInstance.offlineVRRig.transform.rotation = Quaternion.identity;
            }
        }
        public static void ChaseGun()
        {
            if (taggerInstance == null) return;

            GunTemplate.StartBothGuns(() =>
            {
                taggerInstance.offlineVRRig.enabled = false;
                taggerInstance.StartCoroutine(Chase());

            }, true);
            {
                taggerInstance.offlineVRRig.enabled = true;
            }
        }
        private static IEnumerator Chase()
        {
            Transform myRig = taggerInstance!.offlineVRRig.transform;
            while (Vector3.Distance(myRig.position, LockedPlayer!.transform.position) > 0.1f)
            {
                myRig.position = Vector3.MoveTowards(myRig.position, LockedPlayer.transform.position, Time.deltaTime * 1f);
                yield return null;
            }
            taggerInstance.offlineVRRig.enabled = true;
        }
        public static void QuestScore(int score)
        {
            if (taggerInstance == null) return;

            if (Time.time > lvlDelay)
            {
                lvlDelay = Time.time + 1f;
                taggerInstance.offlineVRRig.SetQuestScore(score);
            }
        }
        public static void SexGun()
        {
            if (taggerInstance == null) return;

            GunTemplate.StartBothGuns(() =>
            {
                taggerInstance.offlineVRRig.enabled = false;

                taggerInstance.offlineVRRig.transform.position = LockedPlayer!.transform.position + (LockedPlayer.transform.forward * -(0.4f + (Mathf.Sin(Time.frameCount / 10f) * 0.1f)));
                taggerInstance.offlineVRRig.transform.rotation = LockedPlayer.transform.rotation;

                taggerInstance.offlineVRRig.leftHand.rigTarget.transform.position = (LockedPlayer.transform.position + LockedPlayer.transform.right * -0.15f) + LockedPlayer.transform.up * -0.3f;
                taggerInstance.offlineVRRig.leftHand.rigTarget.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

                taggerInstance.offlineVRRig.rightHand.rigTarget.transform.position = (LockedPlayer.transform.position + LockedPlayer.transform.right * 0.15f) + LockedPlayer.transform.up * -0.3f;
                taggerInstance.offlineVRRig.rightHand.rigTarget.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }, true);
            {
                taggerInstance.offlineVRRig.enabled = true;
            }
        }
        public static void HeadGun()
        {
            if (taggerInstance == null) return;

            GunTemplate.StartBothGuns(() =>
            {
                taggerInstance.offlineVRRig.enabled = false;

                taggerInstance.offlineVRRig.transform.position = LockedPlayer!.transform.position + (LockedPlayer.transform.forward * (0.4f + (Mathf.Sin(Time.frameCount / 6f) * 0.1f))) + (LockedPlayer.transform.up * -0.4f);
                taggerInstance.offlineVRRig.transform.rotation = LockedPlayer.transform.rotation * Quaternion.Euler(0f, 180f, 0f);

                taggerInstance.offlineVRRig.leftHand.rigTarget.transform.position = (LockedPlayer.transform.position + LockedPlayer.transform.right * 0.15f) + LockedPlayer.transform.up * -0.3f;
                taggerInstance.offlineVRRig.leftHand.rigTarget.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

                taggerInstance.offlineVRRig.rightHand.rigTarget.transform.position = (LockedPlayer.transform.position + LockedPlayer.transform.right * -0.15f) + LockedPlayer.transform.up * -0.3f;
                taggerInstance.offlineVRRig.rightHand.rigTarget.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

            }, true);
            {
                taggerInstance.offlineVRRig.enabled = true;
            }
        }
        public static void GetHeadGun()
        {
            if (taggerInstance == null) return;

            GunTemplate.StartBothGuns(() =>
            {
                taggerInstance.offlineVRRig.enabled = false;

                taggerInstance.offlineVRRig.transform.position = LockedPlayer!.transform.position + (LockedPlayer.transform.forward * (0.4f + (Mathf.Sin(Time.frameCount / 6f) * 0.1f))) + (LockedPlayer.transform.up * 0.4f);
                taggerInstance.offlineVRRig.transform.rotation = LockedPlayer.transform.rotation * Quaternion.Euler(0f, 180f, 0f);

                taggerInstance.offlineVRRig.leftHand.rigTarget.transform.position = (LockedPlayer.transform.position + LockedPlayer.transform.right * 0.15f) + LockedPlayer.transform.up * 0.15f;
                taggerInstance.offlineVRRig.leftHand.rigTarget.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

                taggerInstance.offlineVRRig.rightHand.rigTarget.transform.position = (LockedPlayer.transform.position + LockedPlayer.transform.right * -0.15f) + LockedPlayer.transform.up * 0.15f;
                taggerInstance.offlineVRRig.rightHand.rigTarget.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

            }, true);
            {
                taggerInstance.offlineVRRig.enabled = true;
            }
        }
        public static void Spin()
        {
            if (taggerInstance == null) return;

            if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                taggerInstance.offlineVRRig.enabled = false;
                taggerInstance.offlineVRRig.transform.rotation = Quaternion.Euler(taggerInstance.offlineVRRig.transform.rotation.eulerAngles + new Vector3(0f, 500f * Time.deltaTime, 0f));
            }
            else
            {
                taggerInstance.offlineVRRig.enabled = true;
            }
        }
        public static void Tpose()
        {
            if (taggerInstance == null) return;

            if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                taggerInstance.offlineVRRig.enabled = false;

                taggerInstance.offlineVRRig.head.rigTarget.transform.rotation = taggerInstance.bodyCollider.transform.rotation;
                taggerInstance.offlineVRRig.rightHand.rigTarget.transform.rotation = taggerInstance.bodyCollider.transform.rotation;
                taggerInstance.offlineVRRig.leftHand.rigTarget.transform.rotation = taggerInstance.bodyCollider.transform.rotation;
                taggerInstance.offlineVRRig.leftHand.rigTarget.transform.position = taggerInstance.offlineVRRig.transform.position + taggerInstance.offlineVRRig.transform.right * 1.5f;
                taggerInstance.offlineVRRig.rightHand.rigTarget.transform.position = taggerInstance.offlineVRRig.transform.position + taggerInstance.offlineVRRig.transform.right * -1.5f;
            }
            else
            {
                taggerInstance.offlineVRRig.enabled = true;
            }
        }
        public static void Ascend()
        {
            if (taggerInstance == null) return;

            if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                taggerInstance.offlineVRRig.enabled = false;
                taggerInstance.offlineVRRig.transform.position += new Vector3(0f, 5f * Time.deltaTime, 0f);
            }
            else
            {
                taggerInstance.offlineVRRig.enabled = true;
            }
        }
        public static void Helicopter()
        {
            if (taggerInstance == null) return;

            if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                taggerInstance.offlineVRRig.enabled = false;
                Ascend();
                Spin();
                Tpose();
            }
            else
            {
                taggerInstance.offlineVRRig.enabled = true;
            }
        }

        private static float lvlDelay;

        private static float nigTime =  0f;
        private static readonly float delay = 0.37f;
        private static bool lag = false;
    }
}