using HarmonyLib;
using Hidden.Utilities.Notifs;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine;
using static Hidden.Menu.Main;

namespace Hidden.Utilities.Notifs
{
    [HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnPlayerEnteredRoom")]
    internal class JoinPatch : MonoBehaviour
    {
        private static void Prefix(Player newPlayer)
        {
            if (newPlayer != oldnewplayer)
            {
                NotificationLib.SendNotification("<color=white>[</color><color=green>Player Joined</color><color=white>] </color><color=white>Name: " + newPlayer.NickName + "</color>");
                oldnewplayer = newPlayer;
            }
        }

        private static Player oldnewplayer;
    }
}