using BepInEx;
using GorillaLocomotion;
using Hidden.Utilities;
using Hidden.Utilities.Notifs;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static Hidden.Utilities.GunTemplate;
using static Hidden.Utilities.Variables;

namespace Hidden.Mods.Categories
{
    public class Playerr
    {
        public static void RigGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;

                GorillaTagger.Instance.offlineVRRig.transform.position = GunTemplate.spherepointer.transform.position + new Vector3(0f, 1f, 0f);
            }, false);
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        #region Advantage
        // TY Cha
        public static void TagPlayer(VRRig plr)
        {
            GorillaTagger.Instance.offlineVRRig.enabled = false;
            GorillaTagger.Instance.offlineVRRig.transform.SetPositionAndRotation(plr.transform.position + new Vector3(0f, -0.25f, 0f), plr.transform.rotation);

            PhotonNetwork.SendAllOutgoingCommands();

            MethodInfo method = typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic);
            if (method != null)
            {
                method.Invoke(null, Array.Empty<object>());
            }

            PhotonView photonView = GameObject.Find("Player Objects/RigCache/Network Parent/GameMode(Clone)").GetPhotonView();
            if (photonView != null)
            {
                photonView.RPC("RPC_ReportTag", RpcTarget.All, new object[]
                {
                    plr.Creator.ActorNumber
                });
            }

            GorillaTagger.Instance.offlineVRRig.enabled = true;
            PhotonNetwork.SendAllOutgoingCommands();

            MethodInfo method2 = typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic);
            if (method2 != null)
            {
                method2.Invoke(null, Array.Empty<object>());
            }

            MethodInfo method3 = typeof(PhotonView).GetMethod("OnSerialize", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method3 != null)
            {
                method3.Invoke(photonView, new object[2]);
            }

            PhotonNetwork.NetworkingClient.LoadBalancingPeer.SendAcksOnly();
        }


        private static Dictionary<int, float> lastTaggedTime = new Dictionary<int, float>();
        private static Photon.Realtime.Player player;
        public static float Delay;
        public static void TagAura()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig && (Vector3.Distance(GorillaTagger.Instance.leftHandTransform.position, vrrig.headMesh.transform.position) < 4f || Vector3.Distance(GorillaTagger.Instance.rightHandTransform.position, vrrig.headMesh.transform.position) < 4f))
                {
                    PhotonView photonView = GameObject.Find("Player Objects/RigCache/Network Parent/GameMode(Clone)").GetPhotonView();
                    if (photonView != null)
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
            if (!IAmInfected)
            {
                if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.2f | UnityInput.Current.GetKey(KeyCode.T))
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
                }
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
                NotificationLib.SendNotification("<color=white>[</color><color=blue>Tag Self:</color><color=white>] You are already tagged</color>");
            }
        }
        #endregion

        private static bool isOn = false;
        private static bool wasButtonPressed = false;
        public static void GhostMonke()
        {
            bool isButtonCurrentlyPressed = pollerInstance.rightControllerPrimaryButton;
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
            bool isButtonCurrentlyPressed = pollerInstance.rightControllerPrimaryButton;
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
            GorillaTagger.Instance.offlineVRRig.head.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360));
            GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360));
            GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360));

            GorillaTagger.Instance.offlineVRRig.head.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 180), (float)UnityEngine.Random.Range(0, 180));
            GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 180), (float)UnityEngine.Random.Range(0, 180));
            GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 180), (float)UnityEngine.Random.Range(0, 180));
        }
        public static void SpazHands()
        {
            GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360));
            GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360));

            GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 180), (float)UnityEngine.Random.Range(0, 180));
            GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.eulerAngles = new Vector3((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 180), (float)UnityEngine.Random.Range(0, 180));
        }
        public static void FixHead()
        {
            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.y = 0f;
            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.x = 0f;
            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.z = 0f;
        }
        public static void BackwardsHead()
        {
            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.y = 180f;
        }
        public static void SnapNeck()
        {
            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.y = 90f;
        }
        public static void UpsidedownHead()
        {
            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.z = 180f;
        }
        public static void LongArms(float length)
        {
            GorillaTagger.Instance.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }
        public static void FixArms()
        {
            GorillaTagger.Instance.transform.localScale = Vector3.one;
        }
        public static void GrabRig()
        {
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
            VRMap head = taggerInstance.offlineVRRig.head;
            head.trackingRotationOffset.x += 15f;
        }
        public static void HeadSpiny()
        {
            VRMap head = taggerInstance.offlineVRRig.head;
            head.trackingRotationOffset.y += 15f;
        }
        public static void HeadSpinz()
        {
            VRMap head = taggerInstance.offlineVRRig.head;
            head.trackingRotationOffset.z += 15f;
        }
        public static void AnnoyPlayerGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = Annoy(LockedPlayer.transform, 1.25f);
            }, true);
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void FakeLag()
        {
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
            GunTemplate.StartBothGuns(() =>
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = Orbit(LockedPlayer.transform, 15);
                GorillaTagger.Instance.offlineVRRig.transform.LookAt(LockedPlayer.transform);
            }, true);
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
                GorillaTagger.Instance.offlineVRRig.transform.rotation = Quaternion.identity;
            }
        }
        public static void OrbitGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = Orbit(spherepointer.transform, 15);
                GorillaTagger.Instance.offlineVRRig.transform.LookAt(spherepointer.transform);
            }, false);
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
                GorillaTagger.Instance.offlineVRRig.transform.rotation = Quaternion.identity;
            }
        }
        public static void GrabGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = LockedPlayer.rightHandTransform.position;
                GorillaTagger.Instance.offlineVRRig.transform.rotation = LockedPlayer.rightHandTransform.rotation;
            }, true);
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void AnnoySelf()
        {
            if (Inputs.rightGrip() || UnityInput.Current.GetKey(KeyCode.G))
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = Annoy(GorillaTagger.Instance.headCollider.transform, 1.25f);
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void OrbitSelf()
        {
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
            GunTemplate.StartBothGuns(() =>
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.StartCoroutine(Chase());

            }, true);
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        private static IEnumerator Chase()
        {
            Transform myRig = GorillaTagger.Instance.offlineVRRig.transform;
            while (Vector3.Distance(myRig.position, LockedPlayer.transform.position) > 0.1f)
            {
                myRig.position = Vector3.MoveTowards(myRig.position, LockedPlayer.transform.position, Time.deltaTime * 1f);
                yield return null;
            }
            GorillaTagger.Instance.offlineVRRig.enabled = true;
        }
        public static void QuestScore(int score)
        {
            if (Time.time > lvlDelay)
            {
                lvlDelay = Time.time + 1f;
                GorillaTagger.Instance.offlineVRRig.SetQuestScore(score);
            }
        }
        public static void SexGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;

                GorillaTagger.Instance.offlineVRRig.transform.position = LockedPlayer.transform.position + (LockedPlayer.transform.forward * -(0.4f + (Mathf.Sin(Time.frameCount / 10f) * 0.1f)));
                GorillaTagger.Instance.offlineVRRig.transform.rotation = LockedPlayer.transform.rotation;

                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = (LockedPlayer.transform.position + LockedPlayer.transform.right * -0.15f) + LockedPlayer.transform.up * -0.3f;
                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = (LockedPlayer.transform.position + LockedPlayer.transform.right * 0.15f) + LockedPlayer.transform.up * -0.3f;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }, true);
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void HeadGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;

                GorillaTagger.Instance.offlineVRRig.transform.position = LockedPlayer.transform.position + (LockedPlayer.transform.forward * (0.4f + (Mathf.Sin(Time.frameCount / 6f) * 0.1f))) + (LockedPlayer.transform.up * -0.4f);
                GorillaTagger.Instance.offlineVRRig.transform.rotation = LockedPlayer.transform.rotation * Quaternion.Euler(0f, 180f, 0f);

                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = (LockedPlayer.transform.position + LockedPlayer.transform.right * 0.15f) + LockedPlayer.transform.up * -0.3f;
                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = (LockedPlayer.transform.position + LockedPlayer.transform.right * -0.15f) + LockedPlayer.transform.up * -0.3f;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

            }, true);
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void GetHeadGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;

                GorillaTagger.Instance.offlineVRRig.transform.position = LockedPlayer.transform.position + (LockedPlayer.transform.forward * (0.4f + (Mathf.Sin(Time.frameCount / 6f) * 0.1f))) + (LockedPlayer.transform.up * 0.4f);
                GorillaTagger.Instance.offlineVRRig.transform.rotation = LockedPlayer.transform.rotation * Quaternion.Euler(0f, 180f, 0f);

                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = (LockedPlayer.transform.position + LockedPlayer.transform.right * 0.15f) + LockedPlayer.transform.up * 0.15f;
                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = (LockedPlayer.transform.position + LockedPlayer.transform.right * -0.15f) + LockedPlayer.transform.up * 0.15f;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

            }, true);
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        private static float lvlDelay;

        private static float nigTime =  0f;
        private static float delay = 0.37f;
        private static bool lag = false;







        // patched
        public static void Fling(VRRig rig, Vector3 velocity)
        {
            RigManager.GetNetworkViewFromVRRig(GorillaTagger.Instance.offlineVRRig).SendRPC(
                "DroppedByPlayer",
                RigManager.GetPlayerFromVRRig(rig),
                new object[] { velocity }
            );
        }
    }
}