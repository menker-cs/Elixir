using Elixir.Notifications;
using Elixir.Utilities.Notifs;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine;

namespace Elixir.Utilities.Notifs
{
    [HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnPlayerEnteredRoom")]
    internal class JoinPatch : MonoBehaviour
    {
        private static void Prefix(Player newPlayer)
        {
            if (newPlayer != oldnewplayer)
            {
                NotificationLib.SendNotification("[<color=red>Player Left</color>]Name: " + oldnewplayer.NickName);
                oldnewplayer = newPlayer;
            }
        }

        private static Player? oldnewplayer;
    }
}