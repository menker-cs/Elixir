using Elixir.Notifications;
using HarmonyLib;
using Photon.Pun;
using System;
using System.Collections;
using System.Net.Http;
using UnityEngine;

namespace Elixir.Utilities
{
    public class Variables : MonoBehaviourPunCallbacks
    {
        public static bool vCounter = true;
        public static bool disconnect = true;
        public static bool alphabet = false;
        public static bool tips = true;
        public static float lastFPSTime = 0f;
        public static int fps;
        public static int Mat;
        public static bool InPcCondition;

        public static void Placeholder() { }

        // --- Utility Methods ---
        public static void Outline(GameObject obj, Color clr)
        {
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(gameObject.GetComponent<BoxCollider>());
            gameObject.transform.parent = obj.transform;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.localPosition = obj.transform.localPosition;
            gameObject.transform.localScale = obj.transform.localScale + new Vector3(-0.01f, 0.0145f, 0.0145f);
            gameObject.GetComponent<Renderer>().material.color = clr;
        }
        public static void Trail(GameObject obj, Color clr, Color clr2)
        {
            GameObject trailObject = new GameObject("trail");
            trailObject.transform.position = obj.transform.position;
            trailObject.transform.SetParent(obj.transform);
            TrailRenderer trailRenderer = trailObject.AddComponent<TrailRenderer>();
            trailRenderer.material = new Material(Shader.Find("Unlit/Color")) { color = clr };
            trailRenderer.time = 0.5f;
            trailRenderer.startWidth = 0.025f;
            trailRenderer.endWidth = 0f;
            trailRenderer.startColor = clr;
            trailRenderer.endColor = clr2;
        }
        public static void IsModded()
        {
            if (!PhotonNetwork.IsConnected)
            {
                NotificationLib.SendNotification("<color=blue>Room</color> : You are not connected to a room.");
                return;
            }

            string message = GetHTMode().Contains("MODDED") ? "<color=blue>Room</color> : You are in a modded lobby." : "<color=blue>Room</color> : You are not in a modded lobby.";
            NotificationLib.SendNotification(message);
        }

        public static string GetHTMode()
        {
            if (PhotonNetwork.CurrentRoom == null || !PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("gameMode"))
            {
                return "ERROR";
            }
            return PhotonNetwork.CurrentRoom.CustomProperties["gameMode"].ToString();
        }

        public static Photon.Realtime.Player NetPlayerToPhotonPlayer(NetPlayer netPlayer) => netPlayer.GetPlayerRef();

        public static NetPlayer PhotonPlayerToNetPlayer(Photon.Realtime.Player player)
        {
            VRRig rig = GorillaGameManager.instance.FindPlayerVRRig(player);
            return rig != null ? rig.Creator : null!;
        }

        public static NetPlayer GetNetPlayerFromRig(VRRig vrrig) => vrrig.Creator;

        public static VRRig GetRigFromNetPlayer(NetPlayer netPlayer) => GorillaGameManager.instance.FindPlayerVRRig(netPlayer);

        public static PhotonView GetPhotonViewFromRig(VRRig vrrig)
        {
            return (PhotonView)Traverse.Create(vrrig).Field("photonView").GetValue();
        }

        public static PhotonView GetPhotonViewFromNetPlayer(NetPlayer netPlayer) => GetPhotonViewFromRig(GorillaGameManager.instance.FindPlayerVRRig(netPlayer));

        public static bool GetGamemode(string gamemodeName)
        {
            return GorillaGameManager.instance.GameModeName().ToLower().Contains(gamemodeName.ToLower());
        }

        public static bool IsOtherPlayer(VRRig rig) => rig != null && rig != GorillaTagger.Instance.offlineVRRig && !rig.isOfflineVRRig && !rig.isMyPlayer;

        public static bool IAmInfected => GorillaTagger.Instance.offlineVRRig != null && RigIsInfected(GorillaTagger.Instance.offlineVRRig);

        public static bool RigIsInfected(VRRig vrrig)
        {
            string materialName = vrrig.mainSkin.material.name;
            return materialName.Contains("fected") || materialName.Contains("It");
        }
        public static string VrrigPlatform(VRRig rig)
        {
            if ((rig.cosmeticSet?.ToString() ?? "").Contains("S. FIRST LOGIN"))
                return "STEAM";
            else if ((rig.cosmeticSet?.ToString() ?? "").Contains("FIRST LOGIN") || rig.Creator.GetPlayerRef().CustomProperties.Count >= 2)
                return "PC";

            return "QUEST";
        }
                public static void IsMasterCheck()
        {
            if (!PhotonNetwork.IsConnected)
            {
                NotificationLib.SendNotification("<color=blue>Room</color> : You are not connected to a room.");
                return;
            }

            NotificationLib.SendNotification(PhotonNetwork.IsMasterClient ? "<color=blue>Room</color> : You are master." : "<color=blue>Room</color> : " + PhotonNetwork.MasterClient + "is master client");
        }
        public static void UseGravity(bool useGravity)
        {
            GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().useGravity = useGravity;
        }
        public static IEnumerator Chase(VRRig target)
        {
            Transform myRig = GorillaTagger.Instance!.offlineVRRig.transform;
            while (Vector3.Distance(myRig.position, target!.transform.position) > 0.1f)
            {
                myRig.position = Vector3.MoveTowards(myRig.position, target.transform.position, Time.deltaTime * 1f);
                yield return null;
            }
            GorillaTagger.Instance.offlineVRRig.enabled = true;
        }
        public static void DoNoclip(bool b)
        {
            foreach (MeshCollider collider in Resources.FindObjectsOfTypeAll<MeshCollider>())
            {
                if (b)
                {
                    collider.enabled = false;
                }
                else
                {
                    collider.enabled = true;
                }
            }
        }
        public static bool InLobby() => PhotonNetwork.InLobby;
        public static bool IsMaster() => PhotonNetwork.IsMasterClient;
        public static bool IsUserMaster(VRRig rig)
        {
            return rig.OwningNetPlayer.IsMasterClient;
        }
        public static Vector3 Orbit(Transform transform, int speed)
        {
            return transform.position + new Vector3(MathF.Cos((float)Time.frameCount / speed), 1f, MathF.Sin((float)Time.frameCount / speed));
        }
        public static Vector3 RandomPos(Transform transform, float range)
        {
            return transform.position + new Vector3(UnityEngine.Random.Range(-range, range), UnityEngine.Random.Range(-range, range), UnityEngine.Random.Range(-range, range));
        }
        public static string GetStatus(string url) 
        {
            HttpClient client = new HttpClient();
            string stringg = client.GetStringAsync(url).Result;
            return stringg;
        }
        public static string Status = GetStatus("https://raw.githubusercontent.com/menker-cs/Elixir/refs/heads/main/status.txt");
    }
}

