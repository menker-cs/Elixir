using Elixir.Utilities;
using static Elixir.Utilities.Variables;
using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
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

            foreach (var vrrig in GorillaParent.instance.vrrigs)
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

            foreach (var vrrig in GorillaParent.instance.vrrigs)
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
                foreach (var vrrig in GorillaParent.instance.vrrigs)
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
                        foreach (VRRig l in GorillaParent.instance.vrrigs)
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
                            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
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
    }
}