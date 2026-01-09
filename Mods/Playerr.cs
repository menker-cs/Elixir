using BepInEx;
using Elixir.Notifications;
using Elixir.Utilities;
using GorillaLocomotion;
using static GorillaLocomotion.GTPlayer;
using Photon.Pun;
using System;
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
            if (GorillaTagger.Instance == null) return;

            GunTemplate.StartBothGuns(() =>
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = GunTemplate.spherepointer!.transform.position + new Vector3(0f, 1f, 0f);
            }, false);

            if (GunTemplate.spherepointer == null || !GunTemplate.trigger)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        #region Advantage
        // TY Cha
        public static void TagPlayer(VRRig plr)
        {
            if (GorillaTagger.Instance == null) return;

            GorillaTagger.Instance.offlineVRRig.enabled = false;
            GorillaTagger.Instance.offlineVRRig.transform.SetPositionAndRotation(plr.transform.position + new Vector3(0f, -0.25f, 0f), plr.transform.rotation);

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

            GorillaTagger.Instance.offlineVRRig.enabled = true;
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
            if (GorillaTagger.Instance == null) return;

            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig && (Vector3.Distance(GorillaTagger.Instance.leftHandTransform.position, vrrig.headMesh.transform.position) < 4f || Vector3.Distance(GorillaTagger.Instance.rightHandTransform.position, vrrig.headMesh.transform.position) < 4f) && Inputs.rightGrip() || Inputs.leftGrip() || UnityInput.Current.GetKey(KeyCode.G))
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
            if (GorillaTagger.Instance == null) return;

            if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.2f | UnityInput.Current.GetKey(KeyCode.T) && !IAmInfected)
            {
                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    if (RigIsInfected(vrrig))
                    {
                        GorillaTagger.Instance.offlineVRRig.enabled = false;
                        GorillaTagger.Instance.offlineVRRig.transform.position = vrrig.rightHandTransform.position;
                        GorillaTagger.Instance.myVRRig.transform.position = vrrig.rightHandTransform.position;
                        break;
                    }
                }
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
                NotificationLib.SendNotification("<color=white>[</color>Tag Self:<color=white>]</color> You are already tagged");
            }
        }
        #endregion

        private static bool isOn = false;
        private static bool wasButtonPressed = false;
        public static void GhostMonke()
        {
            if (GorillaTagger.Instance == null) return;

            bool isButtonCurrentlyPressed = ControllerInputPoller.instance.rightControllerPrimaryButton;
            if (!wasButtonPressed && isButtonCurrentlyPressed | UnityInput.Current.GetKey(KeyCode.P))
            {
                isOn = !isOn;
            }

            wasButtonPressed = isButtonCurrentlyPressed;

            if (isOn)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void InvisibleMonke()
        {
            if (GorillaTagger.Instance == null) return;

            bool isButtonCurrentlyPressed = ControllerInputPoller.instance.rightControllerPrimaryButton;
            if (!wasButtonPressed && isButtonCurrentlyPressed | UnityInput.Current.GetKey(KeyCode.P))
            {
                isOn = !isOn;
            }

            wasButtonPressed = isButtonCurrentlyPressed;

            if (isOn)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(999f, 999f, 999f);
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void Spaz()
        {
            if (GorillaTagger.Instance == null) return;

            GorillaTagger.Instance.offlineVRRig.head.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360));
            GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360));
            GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360));

            GorillaTagger.Instance.offlineVRRig.head.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 180), (float)UnityEngine.Random.Range(0, 180));
            GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 180), (float)UnityEngine.Random.Range(0, 180));
            GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 180), (float)UnityEngine.Random.Range(0, 180));
        }
        public static void SpazHands()
        {
            if (GorillaTagger.Instance == null) return;

            GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360));
            GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360));

            GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 180), (float)UnityEngine.Random.Range(0, 180));
            GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 180), (float)UnityEngine.Random.Range(0, 180));
        }
        public static void FixHead()
        {
            if (GorillaTagger.Instance == null) return;

            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.y = 0f;
            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.x = 0f;
            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.z = 0f;
        }
        public static void BackwardsHead()
        {
            if (GorillaTagger.Instance == null) return;

            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.y = 180f;
        }
        public static void SnapNeck()
        {
            if (GorillaTagger.Instance == null) return;

            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.y = 90f;
        }
        public static void UpsidedownHead()
        {
            if (GorillaTagger.Instance == null) return;

            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.z = 180f;
        }
        public static void LongArms(float length)
        {
            if (GorillaTagger.Instance == null) return;

            GorillaTagger.Instance.transform.localScale = new Vector3(length, length, length);
        }

        static float customLength = 1f;
        public static void CustomArms()
        {
            if (GorillaTagger.Instance == null) return;

            if (Inputs.rightTrigger() || UnityInput.Current.GetKey(KeyCode.T))
            {
                customLength += 0.005f;
                if (customLength > 3f)
                    customLength = 3f;
            }

            if (Inputs.leftTrigger() || UnityInput.Current.GetKey(KeyCode.Y))
            {
                customLength -= 0.005f;
                if (customLength < 0.2f)
                    customLength = 0.2f;
            }

            GorillaTagger.Instance.transform.localScale = new Vector3(customLength, customLength, customLength);
        }
        static float customSize = 1f;
        public static void SizeChanger()
        {
            if (GorillaTagger.Instance == null) return;

            if (Inputs.rightTrigger() || UnityInput.Current.GetKey(KeyCode.T))
            {
                customSize += 0.005f;
                if (customSize > 3f)
                    customSize = 3f;
            }

            if (Inputs.leftTrigger() || UnityInput.Current.GetKey(KeyCode.Y))
            {
                customSize -= 0.005f;
                if (customSize < 0.2f)
                    customSize = 0.2f;
            }

            GorillaTagger.Instance.offlineVRRig.NativeScale = customSize;
            GorillaTagger.Instance.transform.localScale = new Vector3(customLength, customLength, customLength);
        }
        public static void FixBody()
        {
            if (GorillaTagger.Instance == null) return;

            GorillaTagger.Instance.transform.localScale = Vector3.one;
            GorillaTagger.Instance.offlineVRRig.NativeScale = 1f;
            customLength = 1f;
            customSize = 1f;
        }
        public static void GrabRig()
        {
            if (GorillaTagger.Instance == null) return;

            if (ControllerInputPoller.instance.rightGrab | UnityInput.Current.GetKey(KeyCode.G))
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                GorillaTagger.Instance.offlineVRRig.transform.rotation = GorillaTagger.Instance.rightHandTransform.rotation;
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void FreezeRig()
        {
            if (GorillaTagger.Instance == null) return;

            if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.1f | UnityInput.Current.GetKey(KeyCode.T))
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = GorillaTagger.Instance.headCollider.transform.position;
                GorillaTagger.Instance.offlineVRRig.transform.rotation = GorillaTagger.Instance.headCollider.transform.rotation;
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void HeadSpinx()
        {
            if (GorillaTagger.Instance == null) return;

            VRMap head = GorillaTagger.Instance.offlineVRRig.head;
            head.trackingRotationOffset.x += 15f;
        }
        public static void HeadSpiny()
        {
            if (GorillaTagger.Instance == null) return;

            VRMap head = GorillaTagger.Instance.offlineVRRig.head;
            head.trackingRotationOffset.y += 15f;
        }
        public static void HeadSpinz()
        {
            if (GorillaTagger.Instance == null) return;

            VRMap head = GorillaTagger.Instance.offlineVRRig.head;
            head.trackingRotationOffset.z += 15f;
        }
        public static void AnnoyPlayerGun()
        {
            if (GorillaTagger.Instance == null) return;

            GunTemplate.StartBothGuns(() =>
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = RandomPos(LockedPlayer!.transform, 1.25f);
            }, true);

            if (GunTemplate.spherepointer == null || !GunTemplate.trigger)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void FakeLag()
        {
            if (GorillaTagger.Instance == null) return;

            if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                if (Time.time - nigTime >= delay)
                {
                    lag = !lag;
                    GorillaTagger.Instance.offlineVRRig.enabled = !lag;

                    nigTime = Time.time;
                }
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
                lag = false;
            }
        }
        public static void Bees()
        {
            if (GorillaTagger.Instance == null) return;

            if (PhotonNetwork.InRoom)
            {
                if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
                {
                    if (Time.time - nigTime >= delay)
                    {
                        GorillaTagger.Instance.offlineVRRig.enabled = false;
                        GorillaTagger.Instance.offlineVRRig.transform.position = RigManager.GetRandomVRRig(false).transform.position + new Vector3(0f, 2f, 0f);

                        nigTime = Time.time;
                    }
                }
                else
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void FBees()
        {
            if (GorillaTagger.Instance == null) return;

            if (PhotonNetwork.InRoom)
            {
                if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = false;
                    GorillaTagger.Instance.offlineVRRig.transform.position = RigManager.GetRandomVRRig(false).transform.position + new Vector3(0f, 2f, 0f);
                }
                else
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void OrbitPGun()
        {
            if (GorillaTagger.Instance == null) return;

            GunTemplate.StartBothGuns(() =>
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = Orbit(LockedPlayer!.transform, 15);
                GorillaTagger.Instance.offlineVRRig.transform.LookAt(LockedPlayer.transform);
            }, true);

            if (GunTemplate.spherepointer == null || !GunTemplate.trigger)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
                GorillaTagger.Instance.offlineVRRig.transform.rotation = Quaternion.identity;
            }
        }
        public static void OrbitGun()
        {
            if (GorillaTagger.Instance == null) return;

            GunTemplate.StartBothGuns(() =>
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = Orbit(spherepointer!.transform, 15);
                GorillaTagger.Instance.offlineVRRig.transform.LookAt(spherepointer.transform);
            }, false);

            if (GunTemplate.spherepointer == null || !GunTemplate.trigger)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
                GorillaTagger.Instance.offlineVRRig.transform.rotation = Quaternion.identity;
            }
        }
        public static void GrabGun()
        {
            if (GorillaTagger.Instance == null) return;

            GunTemplate.StartBothGuns(() =>
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = LockedPlayer!.rightHandTransform.position;
                GorillaTagger.Instance.offlineVRRig.transform.rotation = LockedPlayer.rightHandTransform.rotation;
            }, true);

            if (GunTemplate.spherepointer == null || !GunTemplate.trigger)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void AnnoySelf()
        {
            if (GorillaTagger.Instance == null) return;

            if (Inputs.rightGrip() || UnityInput.Current.GetKey(KeyCode.G))
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = RandomPos(GorillaTagger.Instance.headCollider.transform, 1.25f);
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void OrbitSelf()
        {
            if (GorillaTagger.Instance == null) return;

            if (Inputs.rightGrip() || UnityInput.Current.GetKey(KeyCode.G))
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = Orbit(GorillaTagger.Instance.headCollider.transform, 15);
                GorillaTagger.Instance.offlineVRRig.transform.LookAt(GorillaTagger.Instance.headCollider.transform);
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
                GorillaTagger.Instance.offlineVRRig.transform.rotation = Quaternion.identity;
            }
        }
        public static void ChaseGun()
        {
            if (GorillaTagger.Instance == null) return;

            GunTemplate.StartBothGuns(() =>
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.StartCoroutine(Chase(LockedPlayer!));
            }, true);

            if (GunTemplate.spherepointer == null || !GunTemplate.trigger)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void QuestScore(int score)
        {
            if (GorillaTagger.Instance == null) return;

            if (Time.time > lvlDelay)
            {
                lvlDelay = Time.time + 1f;
                GorillaTagger.Instance.offlineVRRig.SetQuestScore(score);
            }
        }
        public static void SexGun()
        {
            if (GorillaTagger.Instance == null) return;

            GunTemplate.StartBothGuns(() =>
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;

                GorillaTagger.Instance.offlineVRRig.transform.position = LockedPlayer!.transform.position + (LockedPlayer.transform.forward * -(0.4f + (Mathf.Sin(Time.frameCount / 10f) * 0.1f)));
                GorillaTagger.Instance.offlineVRRig.transform.rotation = LockedPlayer.transform.rotation;

                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = (LockedPlayer.transform.position + LockedPlayer.transform.right * -0.15f) + LockedPlayer.transform.up * -0.3f;
                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = (LockedPlayer.transform.position + LockedPlayer.transform.right * 0.15f) + LockedPlayer.transform.up * -0.3f;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }, true);

            if (GunTemplate.spherepointer == null || !GunTemplate.trigger)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void HeadGun()
        {
            if (GorillaTagger.Instance == null) return;

            GunTemplate.StartBothGuns(() =>
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;

                GorillaTagger.Instance.offlineVRRig.transform.position = LockedPlayer!.transform.position + (LockedPlayer.transform.forward * (0.4f + (Mathf.Sin(Time.frameCount / 6f) * 0.1f))) + (LockedPlayer.transform.up * -0.4f);
                GorillaTagger.Instance.offlineVRRig.transform.rotation = LockedPlayer.transform.rotation * Quaternion.Euler(0f, 180f, 0f);

                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = (LockedPlayer.transform.position + LockedPlayer.transform.right * 0.15f) + LockedPlayer.transform.up * -0.3f;
                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = (LockedPlayer.transform.position + LockedPlayer.transform.right * -0.15f) + LockedPlayer.transform.up * -0.3f;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }, true);

            if (GunTemplate.spherepointer == null || !GunTemplate.trigger)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void GetHeadGun()
        {
            if (GorillaTagger.Instance == null) return;

            GunTemplate.StartBothGuns(() =>
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;

                GorillaTagger.Instance.offlineVRRig.transform.position = LockedPlayer!.transform.position + (LockedPlayer.transform.forward * (0.4f + (Mathf.Sin(Time.frameCount / 6f) * 0.1f))) + (LockedPlayer.transform.up * 0.4f);
                GorillaTagger.Instance.offlineVRRig.transform.rotation = LockedPlayer.transform.rotation * Quaternion.Euler(0f, 180f, 0f);

                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = (LockedPlayer.transform.position + LockedPlayer.transform.right * 0.15f) + LockedPlayer.transform.up * 0.15f;
                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = (LockedPlayer.transform.position + LockedPlayer.transform.right * -0.15f) + LockedPlayer.transform.up * 0.15f;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }, true);

            if (GunTemplate.spherepointer == null || !GunTemplate.trigger)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void Spin()
        {
            if (GorillaTagger.Instance == null) return;

            if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.rotation = Quaternion.Euler(GorillaTagger.Instance.offlineVRRig.transform.rotation.eulerAngles + new Vector3(0f, 500f * Time.deltaTime, 0f));
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void Tpose()
        {
            if (GorillaTagger.Instance == null) return;

            if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;

                GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + GorillaTagger.Instance.offlineVRRig.transform.right * 1.5f;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + GorillaTagger.Instance.offlineVRRig.transform.right * -1.5f;
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void Ascend()
        {
            if (GorillaTagger.Instance == null) return;

            if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position += new Vector3(0f, 5f * Time.deltaTime, 0f);
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void Helicopter()
        {
            if (GorillaTagger.Instance == null) return;

            if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                Ascend();
                Spin();
                Tpose();
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        private static float lvlDelay;

        private static float nigTime = 0f;
        private static readonly float delay = 0.37f;
        private static bool lag = false;
    }
}