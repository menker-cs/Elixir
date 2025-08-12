using HarmonyLib;
using Elixir.Utilities.Notifs;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine;
using static Elixir.Menu.Main;

namespace Elixir.Utilities.Notifs
{
    [HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnPlayerLeftRoom")]
    internal class LeavePatch : MonoBehaviour
    {
        private static void Prefix(Player otherPlayer)
        {
            if (otherPlayer != PhotonNetwork.LocalPlayer && otherPlayer != a)
            {
                NotificationLib.SendNotification("<color=white>[</color><color=red>Player Left</color><color=white>]</color> <color=white>Name: " + otherPlayer.NickName + "</color>");
                a = otherPlayer;
            }
        }

        private static Player a;
    }
}