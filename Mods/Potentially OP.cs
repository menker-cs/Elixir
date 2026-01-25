using Elixir.Utilities;
using Photon.Pun;
using System;
using UnityEngine;
using static Photon.Pun.PhotonNetwork;

namespace Elixir.Mods
{
    public class Potentially_OP
    {
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
        private static float time = 0f;
        private static float delay = 0.2f;
        public static void PBSpamBalloons()
        {
            if (!PhotonNetwork.IsMasterClient || GorillaPaintbrawlManager.instance == null) return;
            GorillaPaintbrawlManager gpm = (GorillaPaintbrawlManager)GorillaGameManager.instance;

            if (UnityEngine.Time.time - time >= delay)
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
    }
}