using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.Pun;
using TMPro;
//using static Elixir.Utilities.NotificationLib;

namespace Volt.Patches
{
    internal class RoomPatches : MonoBehaviourPunCallbacks
    {
        public override void OnJoinedRoom()
        {
            if (!PhotonNetwork.InRoom)
                return;

            string photonRoomName = PhotonNetwork.CurrentRoom.Name;
            //SendNotification($"Joined Room: {photonRoomName}");
            //Webhook.Send($"{PhotonNetwork.NickName} Joined The Room {photonRoomName}");
            base.OnJoinedRoom();
        }

        public override void OnLeftRoom()
        {
            string photonRoomName = PhotonNetwork.CurrentRoom.Name;
            //SendNotification($"Left Room: {photonRoomName}");
            //menu.transform.Find("Canvas/Visual/RoomCode").GetComponent<TextMeshProUGUI>().text = "Room Code:";
            //Webhook.Send($"{PhotonNetwork.NickName} Left The Room {photonRoomName}");
            base.OnLeftRoom();
        }
    }
}
