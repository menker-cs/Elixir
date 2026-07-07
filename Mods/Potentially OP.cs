using BepInEx;
using Elixir.Patches;
using Elixir.Utilities;
using ExitGames.Client.Photon;
using GorillaLocomotion.Climbing;
using GorillaTagScripts;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static Elixir.Utilities.ControllerInputLibrary;
using static Elixir.Utilities.Variables;
using static OVRColocationSession;
using static Photon.Pun.PhotonNetwork;
using Time = UnityEngine.Time;

namespace Elixir.Mods
{
    public class Potentially_OP
    {
        #region Paintbrawl
        public static void PBKillAll()
        {
            if (!PhotonNetwork.IsMasterClient || GorillaPaintbrawlManager.instance == null) return;

            foreach (var vrrig in VRRigCache.ActiveRigs)
            {
                NetPlayer player = vrrig.OwningNetPlayer;
                GorillaPaintbrawlManager.instance.HitPlayer(player);
            }
        }
        public static void PBKillGun()
        {
            if (!PhotonNetwork.IsMasterClient || GorillaPaintbrawlManager.instance == null) return;

            GunTemplate.StartBothGuns(() =>
            {
                NetPlayer player = GunTemplate.LockedPlayer.OwningNetPlayer;
                GorillaPaintbrawlManager.instance.HitPlayer(player);
            }, true);
        }
        public static void PBInfLives(bool enable)
        {
            if (!PhotonNetwork.IsMasterClient || GorillaPaintbrawlManager.instance == null) return;
            GorillaPaintbrawlManager gpm = (GorillaPaintbrawlManager)GorillaGameManager.instance;
            gpm.playerLives[LocalPlayer.ActorNumber] = 9999;
        }
        public static void PBGiveInfLives()
        {
            if (!PhotonNetwork.IsMasterClient || GorillaPaintbrawlManager.instance == null) return;

            GunTemplate.StartBothGuns(() =>
            {
                GorillaPaintbrawlManager gpm = (GorillaPaintbrawlManager)GorillaGameManager.instance;
                NetPlayer player = GunTemplate.LockedPlayer.OwningNetPlayer;
                gpm.playerLives[player.ActorNumber] = 9999;
            }, true);
        }
        public static void PBRevAll()
        {
            if (!PhotonNetwork.IsMasterClient || GorillaPaintbrawlManager.instance == null) return;
            GorillaPaintbrawlManager gpm = (GorillaPaintbrawlManager)GorillaGameManager.instance;

            foreach (var vrrig in VRRigCache.ActiveRigs)
            {
                NetPlayer player = vrrig.OwningNetPlayer;
                if (gpm.playerLives[player.ActorNumber] > 0) gpm.playerLives[player.ActorNumber] = 1;
            }
        }
        public static void PBRevGun()
        {
            if (!PhotonNetwork.IsMasterClient || GorillaPaintbrawlManager.instance == null) return;
            GorillaPaintbrawlManager gpm = (GorillaPaintbrawlManager)GorillaGameManager.instance;

            GunTemplate.StartBothGuns(() =>
            {
                NetPlayer player = GunTemplate.LockedPlayer.OwningNetPlayer;
                gpm.playerLives[player.ActorNumber] = 1;
            }, true);
        }
        private static float bTime = 0f;
        private static float delay = 0.2f;
        public static void PBSpamBalloons()
        {
            if (!PhotonNetwork.IsMasterClient || GorillaPaintbrawlManager.instance == null) return;
            GorillaPaintbrawlManager gpm = (GorillaPaintbrawlManager)GorillaGameManager.instance;

            if (UnityEngine.Time.time - bTime >= delay)
            {
                foreach (var vrrig in VRRigCache.ActiveRigs)
                {
                    NetPlayer player = vrrig.OwningNetPlayer;


                    gpm.playerLives[player.ActorNumber] = UnityEngine.Random.Range(0, 4);

                    time = UnityEngine.Time.time;
                }
            }
        }

        public static void PBRestart()
        {
            if (!PhotonNetwork.IsMasterClient || GorillaPaintbrawlManager.instance == null) return;
            GorillaPaintbrawlManager gpm = (GorillaPaintbrawlManager)GorillaGameManager.instance;

            gpm.BattleEnd();
            gpm.StartBattle();
        }
        public static void PBTeamBattle()
        {
            if (!PhotonNetwork.IsMasterClient || GorillaPaintbrawlManager.instance == null) return;
            GorillaPaintbrawlManager gpm = (GorillaPaintbrawlManager)GorillaGameManager.instance;
            gpm.teamBattle = true;
        }
        #endregion
        #region Guardian

        public static void GuardianFling(VRRig target, Vector3 force)
        {
            var guardian = (GorillaGuardianManager)GorillaGameManager.instance;

            if (!guardian.IsPlayerGuardian(NetworkSystem.Instance.LocalPlayer))
                return;

            if (force.sqrMagnitude > 20f)
                force = force.normalized * 20f;

            var view = target.netView;
            var owner = target.Creator;

            view.SendRPC("GrabbedByPlayer", owner, true, false, false);
            view.SendRPC("DroppedByPlayer", owner, force);
        }

        public static GTDoor CityDoor => GameObject.Find("Environment Objects/LocalObjects_Prefab/CityToBasement/DungeonEntrance/DungeonDoor_Prefab").GetComponent<GTDoor>();
        public static bool OpenDoor()
        {
            var door = CityDoor;
            if (door == null) return false;
            try
            {
                door.GetComponent<PhotonView>().RPC("ChangeDoorState", RpcTarget.AllViaServer, new object[] { GTDoor.DoorState.Opening });
                door.GetComponent<PhotonView>().RPC("ChangeDoorState", RpcTarget.AllViaServer, new object[] { GTDoor.DoorState.OpeningWaitingOnRPC });
                return true;
            }
            catch { return false; }
        }
        static float ina;
        public static void CityDoorNoiseSpammer()
        {
            try
            {
                if (ControllerInputPoller.instance.rightGrab)
                {
                    if (Time.time > ina)
                    {
                        OpenDoor();
                        ina = Time.time + 1;
                    }

                    var door = CityDoor;
                    door.GetComponent<PhotonView>().RPC("ChangeDoorState", RpcTarget.AllViaServer, new object[] { GTDoor.DoorState.HeldOpen });
                    door.GetComponent<PhotonView>().RPC("ChangeDoorState", RpcTarget.AllViaServer, new object[] { GTDoor.DoorState.Closing });
                    Potentially_OP.RPCProtection();
                }
                return;
            }
            catch { }
        }

        public static void GuardianFlingGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                if (Time.time > flingDelay)
                {
                    flingDelay = Time.time + 0.12f;
                    GuardianFling(GunTemplate.LockedPlayer, new Vector3(0, 19f));
                    RPCProtection();
                }
            }, true);
        }

        static float flingDelay;
        public static void GuardianFlingAll()
        {
            if(Time.time > flingDelay)
            {
                flingDelay = Time.time + 0.12f;
                foreach (var rig in PlayerListOthers)
                {
                    GuardianFling(GetVRRigFromPlayer(rig), new Vector3(0, 19f));
                }
                RPCProtection();
            }
        }

        public static void GuardianAll()
        {
            if (!IsMasterClient)
                return;

            int playerIndex = 0;

            foreach (var zone in GorillaGuardianZoneManager.zoneManagers)
            {
                if (!zone.enabled)
                    continue;

                if (!zone.IsZoneValid())
                    continue;

                zone.SetGuardian(PlayerList[playerIndex]);
                playerIndex++;
            }
        }

        public static TappableGuardianIdol[]? g = null;

        public static TappableGuardianIdol[] GetGuradianRocks()
        {
            if (Time.time > time)
            {
                g = null;
                time = Time.time + 5f;
            }
            if (g == null)
            {
                g = UnityEngine.Object.FindObjectsOfType<TappableGuardianIdol>();
            }
            return g!;
        }
        private static float time = -1f;
        private static float guardianDelya;
        private static float grabDelya;
        public static void AlwaysGuardian()
        {
            if (!PhotonNetwork.InRoom) return;

            foreach (TappableGuardianIdol gRocks in GetGuradianRocks())
            {
                if (!gRocks.isChangingPositions)
                {
                    GorillaGuardianManager gm = (GorillaGuardianManager)GorillaGameManager.instance;
                    if (!gm.IsPlayerGuardian(NetworkSystem.Instance.LocalPlayer))
                    {
                        GorillaTagger.Instance.offlineVRRig.enabled = false;
                        GorillaTagger.Instance.offlineVRRig.transform.position = gRocks.transform.position;

                        GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = gRocks.transform.position;
                        GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = gRocks.transform.position;

                        if (Time.time > guardianDelya)
                        {
                            guardianDelya = Time.time + 0.01f;
                            gRocks.manager.photonView.RPC("SendOnTapRPC", RpcTarget.All, gRocks.tappableId, UnityEngine.Random.Range(0.2f, 0.4f));
                            RPCFlush();
                            RPC2();
                        }
                    }
                }
                else
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
            }
        }
        public static void GrabAll()
        {
            if (ControllerInputPoller.instance.rightGrab || Mouse.current.rightButton.isPressed)
            {
                if (Time.time > grabDelya)
                {
                    grabDelya = Time.time + 0.1f;
                    GorillaGuardianManager gm = (GorillaGuardianManager)GorillaGameManager.instance;
                    if (gm.IsPlayerGuardian(NetworkSystem.Instance.LocalPlayer))
                    {
                        foreach (VRRig l in VRRigCache.ActiveRigs)
                        {
                            RigManager.GetNetworkViewFromVRRig(l).SendRPC("GrabbedByPlayer", RpcTarget.Others, new object[] { true, false, false });
                            RPCFlush();
                            RPC2();
                        }
                    }
                }
            }
        }

        public static void GunAll()
        {
            GunTemplate.StartBothGuns(() =>
            {
                if (Time.time > grabDelya)
                {
                    grabDelya = Time.time + 0.1f;
                    GorillaGuardianManager gm = (GorillaGuardianManager)GorillaGameManager.instance;
                    if (gm.IsPlayerGuardian(NetworkSystem.Instance.LocalPlayer))
                    {
                        RigManager.GetNetworkViewFromVRRig(GunTemplate.LockedPlayer).SendRPC("GrabbedByPlayer", RigManager.GetNetPlayerFromVRRig(GunTemplate.LockedPlayer), new object[] { true, false, false });
                    }
                }
            }, true);
        }
        public static void UnGudian()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                foreach (GorillaGuardianZoneManager gorillaGuardianZoneManager in GorillaGuardianZoneManager.zoneManagers)
                {
                    GorillaGuardianManager gm = (GorillaGuardianManager)GorillaGameManager.instance;
                    {
                        bool enabled = gm.enabled;
                        if (enabled)
                        {
                            foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                            {
                                gm.EjectGuardian(RigManager.GetPlayerFromVRRig((vrrig)));
                            }
                        }
                    }
                }
            }
        }
        public static void Gudian()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                foreach (GorillaGuardianZoneManager gorillaGuardianZoneManager in GorillaGuardianZoneManager.zoneManagers)
                {
                    GorillaGuardianManager gm = (GorillaGuardianManager)GorillaGameManager.instance;
                    {
                        bool enabled = gm.enabled;
                        if (enabled)
                        {
                            gorillaGuardianZoneManager.SetGuardian(RigManager.GetNetPlayerFromVRRig(RigManager.GetOwnVRRig()));
                        }
                    }
                }
            }
        }
        #endregion

        #region Exploits


        public static void SpazElevator()
        {
            DestroyPhotonview(GRElevatorManager._instance.photonView);
        }

        public static void DestroyPhotonview(PhotonView view)
        {
            if(IsMasterClient)
            {
                var hash = new Hashtable
                {
                    [0] = view.ViewID
                };
                PhotonNetwork.RaiseEventInternal(204, hash, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
                RPCProtection();
            }
        }

        public static bool LocalGrabbingPlayer(VRRig vrrig)
        {
            if (!vrrig.isOfflineVRRig && !vrrig.isLocal)
            {
                TakeMyHand_HandLink leftLink = vrrig.leftHandLink;
                TakeMyHand_HandLink rightLink = vrrig.rightHandLink;

                if (leftLink.grabbedPlayer == NetworkSystem.Instance.LocalPlayer || rightLink.grabbedPlayer == NetworkSystem.Instance.LocalPlayer)
                    return true;
                else
                    return false;
            } else { return false; }
        }

        public static void FlingPlayerGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                {
                    if (LocalGrabbingPlayer(vrrig))
                    {
                        TakeMyHand_HandLink leftLink = vrrig.leftHandLink;
                        TakeMyHand_HandLink rightLink = vrrig.rightHandLink;

                        bool righthand = VRRig.LocalRig.rightHandLink.grabbedPlayer == RigManager.GetNetPlayerFromVRRig(vrrig);

                        if (righthand && RightTrigger())
                        {
                            VRRig.LocalRig.enabled = false;
                            VRRig.LocalRig.transform.position = VRRig.LocalRig.transform.position + GunTemplate.spherepointer.transform.position * 25f;
                        }

                        if (!righthand && LeftTrigger())
                        {
                            VRRig.LocalRig.enabled = false;
                            VRRig.LocalRig.transform.position = VRRig.LocalRig.transform.position + GunTemplate.spherepointer.transform.position * 25f;
                        }
                    }
                    else
                    {
                        VRRig.LocalRig.enabled = true;
                    }
                }
            }, false);
        }

        public static void TeleportPlayerGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                {
                    if (LocalGrabbingPlayer(vrrig))
                    {
                        TakeMyHand_HandLink leftLink = vrrig.leftHandLink;
                        TakeMyHand_HandLink rightLink = vrrig.rightHandLink;

                        bool righthand = VRRig.LocalRig.rightHandLink.grabbedPlayer == RigManager.GetNetPlayerFromVRRig(vrrig);

                        if (righthand && RightTrigger())
                        {
                            VRRig.LocalRig.enabled = false;
                            VRRig.LocalRig.transform.position = GunTemplate.spherepointer.transform.position;
                        }

                        if (!righthand && LeftTrigger())
                        {
                            VRRig.LocalRig.enabled = false;
                            VRRig.LocalRig.transform.position = GunTemplate.spherepointer.transform.position;
                        }
                    }
                    else
                    {
                        VRRig.LocalRig.enabled = true;
                    }
                }
            }, false);
        }

        public static void RPCProtection()
        {
            MonkeAgent.instance.rpcErrorMax = int.MaxValue;
            MonkeAgent.instance.rpcCallLimit = int.MaxValue;
            MonkeAgent.instance.logErrorMax = int.MaxValue;
            PhotonNetwork.MaxResendsBeforeDisconnect = int.MaxValue;
            PhotonNetwork.QuickResends = int.MaxValue;
            foreach (var value in MonkeAgent.instance.userRPCCalls.Values)
            {
                foreach (var value2 in value.Values)
                {
                    value2.RPCCalls = -int.MaxValue;
                }
            }
        }

        public static void FlushAllRpcs()
        {
            MonkeAgent.instance.rpcErrorMax = int.MaxValue;
            MonkeAgent.instance.rpcCallLimit = int.MaxValue;
            MonkeAgent.instance.logErrorMax = int.MaxValue;
            PhotonNetwork.MaxResendsBeforeDisconnect = int.MaxValue;
            PhotonNetwork.QuickResends = int.MaxValue;
            MonkeAgent.instance.RefreshRPCs();
            MonkeAgent.instance.OnPlayerLeftRoom(PhotonNetwork.LocalPlayer);
        }

        static float notiCooldown;
        static float kickCooldown;
        public static void PartyKickGun()
        {
            if (GorillaTagger.Instance.offlineVRRig.partyMemberStatus == VRRig.PartyMemberStatus.NotInLocalParty)
            {
                if (Time.time > notiCooldown)
                {
                    Notifications.NotificationLib.SendNotification("You are not in a party.");
                }
                return;
            }
            if (Time.time > kickCooldown)
            {
                kickCooldown = Time.time + 8.5f;
                Notifications.NotificationLib.SendNotification("Kicking Party Please Wait...");
                for (int i = 0; i < 3920; i++)
                    FriendshipGroupDetection.Instance.photonView.RPC("RequestPartyGameMode", RpcTarget.Others, "Infection");
            }
        }

        public enum LagOption
        {
            Lag,
            Stutter,
            Freeze
        }

        public static void LagOptionVoid(LagOption option, RaiseEventOptions raiseOptions)
        {
            switch(option)
            {
                case LagOption.Lag:
                    Lag(raiseOptions);
                    break;
                case LagOption.Stutter:
                    Stutter(raiseOptions);
                    break;
                case LagOption.Freeze:
                    Freeze(raiseOptions);
                    break;
            }
        }

        public static LagOption Current = LagOption.Lag;

        public static void SwitchCurrentLagType()
        {
            switch (Current)
            {
                case LagOption.Lag:
                    Current = LagOption.Stutter;
                    Notifications.NotificationLib.SendNotification("Lag Type: Stutter");
                    break;
                case LagOption.Stutter:
                    Current = LagOption.Freeze;
                    Notifications.NotificationLib.SendNotification("Lag Type: Freeze");
                    break;
                case LagOption.Freeze:
                    Current = LagOption.Lag;
                    Notifications.NotificationLib.SendNotification("Lag Type: Lag");
                    break;
            }
        }

        public static void LagGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                LagOptionVoid(Current, new RaiseEventOptions { TargetActors = new int[] { GunTemplate.LockedPlayer.Creator.ActorNumber } });
            }, true);
        }

        public static void LagAll()
        {
            LagOptionVoid(Current, new RaiseEventOptions { Receivers = ReceiverGroup.Others });
        }

        public static void LagAura()
        {
            if (GorillaTagger.Instance == null)
                return;

            if (!(Inputs.rightGrip() || Inputs.leftGrip() || UnityInput.Current.GetKey(KeyCode.G)))
                return;

            List<int> actors = new List<int>();

            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (rig == null || rig == GorillaTagger.Instance.offlineVRRig)
                    continue;

                float leftDist = Vector3.Distance(GorillaTagger.Instance.leftHandTransform.position, rig.headMesh.transform.position);
                float rightDist = Vector3.Distance(GorillaTagger.Instance.rightHandTransform.position, rig.headMesh.transform.position);

                if (leftDist < 4f || rightDist < 4f)
                    actors.Add(rig.Creator.ActorNumber);
            }

            if (actors.Count == 0)
                return;

            LagOptionVoid(Current, new RaiseEventOptions
            {
                TargetActors = actors.ToArray()
            });
        }

        static float LagDelay;

        static byte LagCode = 3;
        static Hashtable LagData = new Hashtable();

        public static void Lag(RaiseEventOptions options)
        {
            if (Time.time > LagDelay)
            {
                for (int i = 0; i < 150; i++)
                {
                    PhotonNetwork.NetworkingClient.OpRaiseEvent(LagCode, LagData, options, new SendOptions { DeliveryMode = DeliveryMode.UnreliableUnsequenced, Reliability = false, Encrypt = false });
                }
                LagDelay = Time.time + 0.4f;
                RPCProtection();
            }
        }

        public static void Stutter(RaiseEventOptions options)
        {
            if (Time.time > LagDelay)
            {
                for (int i = 0; i < 750; i++)
                {
                    PhotonNetwork.NetworkingClient.OpRaiseEvent(LagCode, LagData, options, new SendOptions { DeliveryMode = DeliveryMode.UnreliableUnsequenced, Reliability = false, Encrypt = false });
                }
                LagDelay = Time.time + 2f;
                RPCProtection();
            }
        }

        public static void Freeze(RaiseEventOptions options)
        {
            if (Time.time > LagDelay)
            {
                for (int i = 0; i < 3000; i++)
                {
                    PhotonNetwork.NetworkingClient.OpRaiseEvent(LagCode, LagData, options, new SendOptions { DeliveryMode = DeliveryMode.UnreliableUnsequenced, Reliability = false, Encrypt = false });
                }
                LagDelay = Time.time + 8f;
                RPCProtection();
            }
        }

        static float grabDelay;

        public static void GrabCrashGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                ForceGrab(GunTemplate.LockedPlayer, new Vector3(UnityEngine.Random.value < 0.5f ? -1 : 1 * 50000, 50000, UnityEngine.Random.value < 0.5f ? -1 : 1 * 50000));
            }, true, ()=> 
            {
                Grabbing = false;
                VRRig.LocalRig.BreakHandLinks();
                VRRig.LocalRig.enabled = true;
            });
        }

        public static bool IsGrabbable(VRRig rig) =>
            rig != null && (rig.leftHandLink.CanBeGrabbed() || rig.rightHandLink.CanBeGrabbed());

        public static bool Grabbing
        {
            set
            {
                GrabPatches.GrabPatch.enabled = value;

                if (!value && !VRRig.LocalRig.enabled)
                {
                    VRRig.LocalRig.enabled = true;
                }
            }
        }

        public static bool ForceGrab(VRRig targetRig, Vector3 destination)
        {
            if (targetRig == null || targetRig.isLocal)
                return false;

            if (!IsGrabbable(targetRig))
            {
                Grabbing = false;
                VRRig.LocalRig.BreakHandLinks();
                VRRig.LocalRig.enabled = true;
                return false;
            }

            Grabbing = true;

            VRRig.LocalRig.enabled = false;
            VRRig.LocalRig.transform.position = destination;

            bool useLeft = targetRig.leftHandLink.CanBeGrabbed();

            TakeMyHand_HandLink targetHand;
            TakeMyHand_HandLink localHand;

            if (useLeft)
            {
                targetHand = targetRig.leftHandLink;
                localHand = VRRig.LocalRig.leftHandLink;
            }
            else
            {
                targetHand = targetRig.rightHandLink;
                localHand = VRRig.LocalRig.rightHandLink;
            }

            if (targetHand.grabbedPlayer == NetworkSystem.Instance.LocalPlayer)
            {
                return false;
            }

            if (grabDelay == 0f)
            {
                grabDelay = targetHand.rejectGrabsUntilTimestamp > Time.time
                    ? targetHand.rejectGrabsUntilTimestamp
                    : Time.time + 1f;
            }

            if (Time.time <= grabDelay)
                return false;

            VRRig.LocalRig.transform.position = targetRig.syncPos;
            localHand.TentacleTryCreateLink(targetHand);

            grabDelay = targetHand.rejectGrabsUntilTimestamp > Time.time
                ? targetHand.rejectGrabsUntilTimestamp
                : Time.time + 1f;

            return false;
        }




        private static float lastBreak = 0f;
        public static void BreakPlayerGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                if (Time.time > lastBreak)
                {
                    lastBreak = Time.time + 1f;
                    PhotonNetwork.OpRemoveCompleteCacheOfPlayer((NetPlayerToPlayer(GetPlayerFromVRRig(GunTemplate.LockedPlayer)).ActorNumber));
                }
            }, true);
        }

        public static void BreakAllPlayers()
        {
            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            {
                PhotonNetwork.OpRemoveCompleteCacheOfPlayer(player.ActorNumber);
            }
        }

        public static void BreakOtherPlayers()
        {
            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerListOthers)
            {
                PhotonNetwork.OpRemoveCompleteCacheOfPlayer(player.ActorNumber);
            }
        }
        #endregion
    }
}