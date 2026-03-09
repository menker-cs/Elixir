using Elixir.Notifications;
using ExitGames.Client.Photon;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
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
        public static bool autoLoadPrefs;
        public static string Status = GetString("https://raw.githubusercontent.com/menker-cs/Elixir/refs/heads/main/status.txt");

        public static void Placeholder() { }
        public static string GetString(string url)
        {
            HttpClient client = new HttpClient();
            string stringg = client.GetStringAsync(url).Result;
            return stringg;
        }

        #region GameObject Stuff & Vector3
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
            trailRenderer.material.color = clr;
            trailRenderer.time = 0.5f;
            trailRenderer.startWidth = 0.025f;
            trailRenderer.endWidth = 0f;
            trailRenderer.startColor = clr;
            trailRenderer.endColor = clr2;
        }
        public static Vector3 Orbit(Transform transform, int speed)
        {
            return transform.position + new Vector3(MathF.Cos((float)Time.frameCount / speed), 1f, MathF.Sin((float)Time.frameCount / speed));
        }
        public static Vector3 RandomPos(Transform transform, float range)
        {
            return transform.position + new Vector3(UnityEngine.Random.Range(-range, range), UnityEngine.Random.Range(-range, range), UnityEngine.Random.Range(-range, range));
        }
        #endregion

        #region Rig Stuff
        public static VRRig GetVRRigFromPlayer(NetPlayer p)
        {
            return GorillaGameManager.instance.FindPlayerVRRig(p);
        }
        public static Player NetPlayerToPlayer(NetPlayer p)
        {
            return p.GetPlayerRef();
        }
        public static VRRig GetRandomVRRig(bool includeSelf)
        {
            VRRig random = VRRigCache.ActiveRigs[UnityEngine.Random.Range(0, VRRigCache.ActiveRigs.Count - 1)];
            if (includeSelf)
            {
                return random;
            }
            else
            {
                if (random != GorillaTagger.Instance.offlineVRRig)
                {
                    return random;
                }
                else
                {
                    return GetRandomVRRig(includeSelf);
                }
            }
        }
        public static NetworkView GetNetworkViewFromVRRig(VRRig p)
        {
            return (NetworkView)Traverse.Create(p).Field("netView").GetValue();
        }
        public static PhotonView GetPhotonViewFromVRRig(VRRig p)
        {
            NetworkView? view = Traverse.Create(p).Field("netView").GetValue() as NetworkView;
            return RigManager.NetView2PhotonView(view);
        }
        public static PhotonView NetView2PhotonView(NetworkView? view)
        {
            bool flag = view == null;
            PhotonView? result;
            if (flag)
            {
                Debug.Log("null netview passed to converter");
                result = null;
            }
            else
            {
                result = view?.GetView;
            }
            return result!;

        }
        public static VRRig GetOwnVRRig()
        {
            return GorillaTagger.Instance.offlineVRRig;
        }
        public static NetPlayer GetNetPlayerFromVRRig(VRRig p)
        {
            return RigManager.ToNetPlayer(RigManager.GetPhotonViewFromVRRig(p).Owner);
        }
        public static NetPlayer ToNetPlayer(Player player)
        {
            foreach (NetPlayer netPlayer in NetworkSystem.Instance.AllNetPlayers)
            {
                bool flag = netPlayer.GetPlayerRef() == player;
                if (flag)
                {
                    return netPlayer;
                }
            }
            return null!;
        }
        public static VRRig GetClosestVRRig()
        {
            float num = float.MaxValue;
            VRRig? outRig = null;
            foreach (VRRig vrrig in VRRigCache.ActiveRigs)
            {
                if (Vector3.Distance(GorillaTagger.Instance.bodyCollider.transform.position, vrrig.transform.position) < num)
                {
                    num = Vector3.Distance(GorillaTagger.Instance.bodyCollider.transform.position, vrrig.transform.position);
                    outRig = vrrig;
                }
            }
            return outRig!;
        }
        public static Player GetRandomPlayer(bool includeSelf)
        {
            if (includeSelf)
            {
                return PhotonNetwork.PlayerList[UnityEngine.Random.Range(0, PhotonNetwork.PlayerList.Length - 1)];
            }
            else
            {
                return PhotonNetwork.PlayerListOthers[UnityEngine.Random.Range(0, PhotonNetwork.PlayerListOthers.Length - 1)];
            }
        }
        public static Player GetPlayerFromVRRig(VRRig p)
        {
            return GetPhotonViewFromVRRig(p).Owner;
        }
        public static Player GetPlayerFromID(string id)
        {
            Photon.Realtime.Player? found = null;
            foreach (Photon.Realtime.Player target in PhotonNetwork.PlayerList)
            {
                if (target.UserId == id)
                {
                    found = target;
                    break;
                }
            }
            return found!;
        }
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
        public static bool IsUserMaster(VRRig rig)
        {
            return rig.OwningNetPlayer.IsMasterClient;
        }
        #endregion

        #region My Rig stuff
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
        #endregion

        #region RPC Flush Stuff
        public static void RPCFlush()
        {
            PhotonNetwork.RemoveRPCs(PhotonNetwork.LocalPlayer);
            MonkeAgent.instance.rpcCallLimit = int.MaxValue;
            PhotonNetwork.RemoveBufferedRPCs(GorillaTagger.Instance.myVRRig.ViewID, null, null);
            PhotonNetwork.OpCleanActorRpcBuffer(PhotonNetwork.LocalPlayer.ActorNumber);
            PhotonNetwork.OpCleanRpcBuffer(GorillaTagger.Instance.myVRRig.GetView);
        }
        public static void RPC2()
        {
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable
            {
                [0] = GorillaTagger.Instance.myVRRig.ViewID
            };
            PhotonNetwork.NetworkingClient.OpRaiseEvent(200, hashtable, new RaiseEventOptions
            {
                CachingOption = (EventCaching)6,
                TargetActors = new int[]
                {
            PhotonNetwork.LocalPlayer.ActorNumber
                }
            }, SendOptions.SendReliable);
        }
        #endregion
    }
}

