using HarmonyLib;
using Elixir.Utilities.Notifs;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine;
using Elixir.Notifications;

namespace Elixir.Utilities.Notifs
{
    [HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnPlayerLeftRoom")]
    internal class LeavePatch : MonoBehaviour
    {
        private static void Prefix(Player otherPlayer)
        {
            if (otherPlayer != PhotonNetwork.LocalPlayer && otherPlayer != a)
            {
                NotificationLib.SendNotification("[<color=red>Player Left</color>] Name: " + otherPlayer.NickName);
                a = otherPlayer;
            }
        }

        private static Player? a;
    }
}