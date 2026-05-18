using Elixir.Utilities;
using GorillaLocomotion;
using HarmonyLib;
using Photon.Pun;
using System.Linq;
using TMPro;
using UnityEngine;
using static Elixir.Mods.Categories.Settings;
using static Elixir.Utilities.ColorLib;
using static Elixir.Utilities.Variables;
using static Mono.Security.X509.X520;
using Object = UnityEngine.Object;
//using System.Drawing;

namespace Elixir.Mods.Categories
{
    public class Visuals
    {
        public static void Shadows(bool b)
        {
            GameLightingManager.instance.SetCustomDynamicLightingEnabled(b);
        }
        public static void Chams(bool toggle)
        {
            if (!PhotonNetwork.InRoom)
                return;

            if (toggle)
            {
                foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig && vrrig != null)
                    {
                        vrrig.mainSkin.material.shader = Shader.Find("GUI/Text Shader");
                        vrrig.mainSkin.material.color = GetESPColor(vrrig);
                    }
                }
            }
            else
            {
                foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig)
                    {
                        vrrig.mainSkin.material.shader = Shader.Find("GorillaTag/UberShader");
                    }
                }
            }
        }

        public static void BoxESP(bool toggle)
        {
            if (!PhotonNetwork.InRoom)
                return;

            if (toggle)
            {
                foreach (Photon.Realtime.Player players in PhotonNetwork.PlayerList)
                {
                    VRRig vrrig = GorillaGameManager.instance.FindPlayerVRRig(players);
                    if (!vrrig.isOfflineVRRig && !vrrig.isMyPlayer && !vrrig.mainSkin.material.name.Contains("fected"))
                    {
                        if (!vrrig.isOfflineVRRig && !vrrig.isMyPlayer)
                        {
                            if (!vrrig.gameObject.GetComponent<ESPBox>())
                            {
                                vrrig.gameObject.AddComponent<ESPBox>();
                            }
                            else
                            {
                                Material thing = new Material(Shader.Find("GUI/Text Shader"));
                                thing.color = Color.green;
                                ESPBox component = vrrig.GetComponent<ESPBox>();

                                component.topSide.GetComponent<MeshRenderer>().material.color = new Color32(85, 214, 73, 105);
                                component.bottomSide.GetComponent<MeshRenderer>().material.color = new Color32(85, 214, 73, 105);
                                component.leftSide.GetComponent<MeshRenderer>().material.color = new Color32(85, 214, 73, 105);
                                component.rightSide.GetComponent<MeshRenderer>().material.color = new Color32(85, 214, 73, 105);

                                component.rightSide.SetActive(true);
                                component.leftSide.SetActive(true);
                                component.bottomSide.SetActive(true);
                                component.topSide.SetActive(true);
                            }
                        }

                    }
                    else
                    {
                        if (!vrrig.isOfflineVRRig && !vrrig.isMyPlayer)
                        {
                            if (!vrrig.gameObject.GetComponent<ESPBox>())
                            {
                                vrrig.gameObject.AddComponent<ESPBox>();
                            }
                            else
                            {
                                Material thing = new Material(Shader.Find("GUI/Text Shader"));
                                thing.color = Color.red;
                                ESPBox component = vrrig.GetComponent<ESPBox>();

                                component.topSide.GetComponent<MeshRenderer>().material.color = Color.red;
                                component.bottomSide.GetComponent<MeshRenderer>().material.color = Color.red;
                                component.leftSide.GetComponent<MeshRenderer>().material.color = Color.red;
                                component.rightSide.GetComponent<MeshRenderer>().material.color = Color.red;

                                component.rightSide.SetActive(true);
                                component.leftSide.SetActive(true);
                                component.bottomSide.SetActive(true);
                                component.topSide.SetActive(true);
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (Photon.Realtime.Player players in PhotonNetwork.PlayerList)
                {
                    VRRig vrrig = GorillaGameManager.instance.FindPlayerVRRig(players);
                    if (!vrrig.isOfflineVRRig && !vrrig.isMyPlayer)
                    {
                        if (vrrig.gameObject.GetComponent<ESPBox>())
                        {
                            ESPBox component = vrrig.gameObject.GetComponent<ESPBox>();

                            GameObject.Destroy(component.rightSide);
                            GameObject.Destroy(component.leftSide);
                            GameObject.Destroy(component.bottomSide);
                            GameObject.Destroy(component.topSide);

                            GameObject.Destroy(vrrig.gameObject.GetComponent<ESPBox>());
                        }
                    }
                }
            }
        }

        public static void Tracers(bool toggle)
        {
            if (!PhotonNetwork.InRoom)
                return;

            if (toggle)
            {
                foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig && vrrig != null)
                    {
                        if (!vrrig.transform.Find("Line"))
                        {
                            GameObject line = new GameObject("Line");
                            line.transform.SetParent(vrrig.transform, false);

                            LineRenderer Line = line.AddComponent<LineRenderer>();
                            Line.startWidth = 0.0225f;
                            Line.endWidth = 0.0225f;
                            Line.material.shader = Shader.Find("GUI/Text Shader");
                        }

                        GameObject linee = vrrig.transform.Find("Line").gameObject;
                        LineRenderer liner = linee.GetComponent<LineRenderer>();

                        liner.SetPosition(0, GetTracerPosition());
                        liner.SetPosition(1, vrrig.transform.position);

                        UnityEngine.Color color1 = GetESPColor(vrrig);
                        color1.a = 0.5f;

                        liner.startColor = color1;
                        liner.endColor = color1;
                    }

                    if (vrrig == null)
                    {
                        if (vrrig.transform.Find("Line"))
                        {
                            GameObject.Destroy(vrrig.transform.Find("Line").gameObject);
                        }
                    }
                }
            } else
            {
                foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig && vrrig != null)
                    {
                        if (vrrig.transform.Find("Line"))
                        {
                            GameObject.Destroy(vrrig.transform.Find("Line").gameObject);
                        }
                    }

                    if (vrrig == null)
                    {
                        if (vrrig.transform.Find("NameLinetag"))
                        {
                            GameObject.Destroy(vrrig.transform.Find("Line").gameObject);
                        }
                    }
                }
            }
        }

        public static void DistanceNametags(bool toggle)
        {
            if (!PhotonNetwork.InRoom)
                return;

            if (toggle)
            {
                foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig && vrrig != null)
                    {
                        if (!vrrig.transform.Find("Nametag"))
                        {
                            GameObject nametag = new GameObject("Nametag");
                            nametag.transform.SetParent(vrrig.transform, false);
                            TextMeshPro textMeshPro = nametag.AddComponent<TextMeshPro>();

                            textMeshPro.fontSize = 2.7f;
                            textMeshPro.fontStyle = FontStyles.Normal;
                            textMeshPro.alignment = TextAlignmentOptions.Center;
                            textMeshPro.font = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdBodyText").GetComponent<TextMeshPro>().font;
                        }

                        GameObject name = vrrig.transform.Find("Nametag").gameObject;

                        TextMeshPro tmp = name.GetComponent<TextMeshPro>();
                        float distanceTovrrig = Vector3.Distance(GorillaLocomotion.GTPlayer.Instance.headCollider.transform.position, vrrig.transform.position);
                        tmp.text = GradientText.MakeAnimatedGradient(ColorLib.ClrToHex(Magenta), ColorLib.ClrToHex(Purple), $"{Mathf.RoundToInt(distanceTovrrig)}M", Time.time);

                        name.transform.position = vrrig.headMesh.transform.position + new Vector3(0f, 0.90f, 0f);
                        name.transform.LookAt(Camera.main.transform);
                        name.transform.Rotate(0, 180, 0);
                    }

                    if (vrrig == null)
                    {
                        if (vrrig.transform.Find("Nametag"))
                        {
                            GameObject.Destroy(vrrig.transform.Find("Nametag").gameObject);
                        }
                    }
                }
            }
            else
            {
                foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig && vrrig != null)
                    {
                        if (vrrig.transform.Find("Nametag"))
                        {
                            GameObject.Destroy(vrrig.transform.Find("Nametag").gameObject);
                        }
                    }

                    if (vrrig == null)
                    {
                        if (vrrig.transform.Find("Nametag"))
                        {
                            GameObject.Destroy(vrrig.transform.Find("Nametag").gameObject);
                        }
                    }
                }
            }
        }

        public static void Nametags(bool toggle)
        {
            if (!PhotonNetwork.InRoom)
                return;

            if (toggle)
            {
                foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig && vrrig != null)
                    {
                        if (!vrrig.transform.Find("Nametag"))
                        {
                            GameObject nametag = new GameObject("Nametag");
                            nametag.transform.SetParent(vrrig.transform, false);
                            TextMeshPro textMeshPro = nametag.AddComponent<TextMeshPro>();

                            textMeshPro.fontSize = 1.4f;
                            textMeshPro.fontStyle = FontStyles.Normal;
                            textMeshPro.alignment = TextAlignmentOptions.Center;
                            textMeshPro.font = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdBodyText").GetComponent<TextMeshPro>().font;
                        }

                        GameObject name = vrrig.transform.Find("Nametag").gameObject;

                        TextMeshPro tmp = name.GetComponent<TextMeshPro>();
                        tmp.text = GradientText.MakeAnimatedGradient(ColorLib.ClrToHex(Magenta), ColorLib.ClrToHex(Purple), RigManager.GetPlayerFromVRRig(vrrig).NickName, Time.time);

                        name.transform.position = vrrig.headMesh.transform.position + new Vector3(0f, 0.90f, 0f);
                        name.transform.LookAt(Camera.main.transform);
                        name.transform.Rotate(0, 180, 0);
                    }

                    if (vrrig == null)
                    {
                        if (vrrig.transform.Find("Nametag"))
                        {
                            GameObject.Destroy(vrrig.transform.Find("Nametag").gameObject);
                        }
                    }
                }
            } else
            {
                foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig && vrrig != null)
                    {
                        if (vrrig.transform.Find("Nametag"))
                        {
                            GameObject.Destroy(vrrig.transform.Find("Nametag").gameObject);
                        }
                    }

                    if (vrrig == null)
                    {
                        if (vrrig.transform.Find("Nametag"))
                        {
                            GameObject.Destroy(vrrig.transform.Find("Nametag").gameObject);
                        }
                    }
                }
            }
        }

        public static void MenuNametag(bool toggle)
        {
            if (!PhotonNetwork.InRoom)
                return;

            if (toggle)
            {
                foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig && vrrig != null)
                    {
                        Photon.Realtime.Player player = RigManager.GetPlayerFromVRRig(vrrig);
                        if (player.CustomProperties.ContainsKey("Elixir"))
                        {
                            if (!vrrig.transform.Find("Nametag"))
                            {
                                GameObject nametag = new GameObject("Nametag");
                                nametag.transform.SetParent(vrrig.transform, false);
                                TextMeshPro textMeshPro = nametag.AddComponent<TextMeshPro>();

                                textMeshPro.fontSize = 1.4f;
                                textMeshPro.fontStyle = FontStyles.Normal;
                                textMeshPro.alignment = TextAlignmentOptions.Center;
                                textMeshPro.font = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdBodyText").GetComponent<TextMeshPro>().font;
                            }

                            GameObject name = vrrig.transform.Find("Nametag").gameObject;

                            TextMeshPro tmp = name.GetComponent<TextMeshPro>();

                            tmp.text = GradientText.MakeAnimatedGradient(ColorLib.ClrToHex(Magenta), ColorLib.ClrToHex(Purple), "Elixir User", Time.time);

                            name.transform.position = vrrig.headMesh.transform.position + new Vector3(0f, 0.90f, 0f);
                            name.transform.LookAt(Camera.main.transform);
                            name.transform.Rotate(0, 180, 0);
                        }
                    }

                    if (vrrig == null)
                    {
                        Photon.Realtime.Player player = RigManager.GetPlayerFromVRRig(vrrig);
                        if (player.CustomProperties.ContainsKey("Elixir"))
                        {
                            if (vrrig.transform.Find("Nametag"))
                            {
                                GameObject.Destroy(vrrig.transform.Find("Nametag").gameObject);
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig && vrrig != null)
                    {
                        Photon.Realtime.Player player = RigManager.GetPlayerFromVRRig(vrrig);
                        if (player.CustomProperties.ContainsKey("Elixir"))
                        {
                            if (vrrig.transform.Find("Nametag"))
                            {
                                GameObject.Destroy(vrrig.transform.Find("Nametag").gameObject);
                            }
                        }
                    }

                    if (vrrig == null)
                    {
                        Photon.Realtime.Player player = RigManager.GetPlayerFromVRRig(vrrig);
                        if (player.CustomProperties.ContainsKey("Elixir"))
                        {
                            if (vrrig.transform.Find("Nametag"))
                            {
                                GameObject.Destroy(vrrig.transform.Find("Nametag").gameObject);
                            }
                        }
                    }
                }
            }

            if (!PhotonNetwork.InRoom)
                return;

            foreach (VRRig vrrig in VRRigCache.ActiveRigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig && vrrig != null)
                {
                    Photon.Realtime.Player player = RigManager.GetPlayerFromVRRig(vrrig);

                    if (player.CustomProperties.ContainsKey("Elixir"))
                    {
                        GameObject name = new GameObject($"{vrrig.name}'s Nametag");
                        TextMeshPro textMeshPro = name.AddComponent<TextMeshPro>();

                        textMeshPro.fontSize = 3.5f;
                        textMeshPro.fontStyle = FontStyles.Normal;
                        textMeshPro.alignment = TextAlignmentOptions.Center;
                        textMeshPro.text = GradientText.MakeAnimatedGradient(ColorLib.ClrToHex(Magenta), ColorLib.ClrToHex(Purple), "Elixir User", Time.time);
                        textMeshPro.font = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdBodyText").GetComponent<TextMeshPro>().font;

                        name.transform.position = vrrig.headMesh.transform.position + new Vector3(0f, 1.15f, 0f);
                        name.transform.LookAt(Camera.main.transform);
                        name.transform.Rotate(0, 180, 0);

                        GameObject.Destroy(name, Time.deltaTime);
                    }
                }
            }
        }

        public static void BoneESP(bool toggle)
        {
            if (toggle)
            {
                Material material = new Material(Shader.Find("GUI/Text Shader"));
                material.color = Color.green;
                Material materialnew = new Material(Shader.Find("GUI/Text Shader"));
                materialnew.color = Color.red;

                foreach (Photon.Realtime.Player players in PhotonNetwork.PlayerList)
                {
                    VRRig vrrig = GorillaGameManager.instance.FindPlayerVRRig(players);
                    if (!vrrig.isOfflineVRRig && !vrrig.isMyPlayer && !vrrig.mainSkin.material.name.Contains("fected"))
                    {
                        if (!vrrig.isOfflineVRRig && !vrrig.isMyPlayer)
                        {
                            if (!vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>())
                            {
                                vrrig.head.rigTarget.gameObject.AddComponent<LineRenderer>();
                            }
                            vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().endWidth = 0.025f;
                            vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().startWidth = 0.025f;
                            vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().material = material;
                            vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().SetPosition(0, vrrig.head.rigTarget.transform.position + new Vector3(0f, 0.16f, 0f));
                            vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().SetPosition(1, vrrig.head.rigTarget.transform.position - new Vector3(0f, 0.4f, 0f));
                            for (int i = 0; i < bones.Count<int>(); i += 2)
                            {
                                if (!vrrig.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>())
                                {
                                    vrrig.mainSkin.bones[bones[i]].gameObject.AddComponent<LineRenderer>();
                                }
                                vrrig.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>().endWidth = 0.025f;
                                vrrig.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>().startWidth = 0.025f;
                                vrrig.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>().material = material;
                                vrrig.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>().SetPosition(0, vrrig.mainSkin.bones[bones[i]].position);
                                vrrig.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>().SetPosition(1, vrrig.mainSkin.bones[bones[i + 1]].position);
                            }
                        }
                    }
                    else
                    {
                        if (!vrrig.isOfflineVRRig && !vrrig.isMyPlayer)
                        {
                            if (!vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>())
                            {
                                vrrig.head.rigTarget.gameObject.AddComponent<LineRenderer>();
                            }
                            vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().endWidth = 0.025f;
                            vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().startWidth = 0.025f;
                            vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().material = materialnew;
                            vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().SetPosition(0, vrrig.head.rigTarget.transform.position + new Vector3(0f, 0.16f, 0f));
                            vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().SetPosition(1, vrrig.head.rigTarget.transform.position - new Vector3(0f, 0.4f, 0f));
                            for (int i = 0; i < bones.Count<int>(); i += 2)
                            {
                                if (!vrrig.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>())
                                {
                                    vrrig.mainSkin.bones[bones[i]].gameObject.AddComponent<LineRenderer>();
                                }
                                vrrig.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>().endWidth = 0.025f;
                                vrrig.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>().startWidth = 0.025f;
                                vrrig.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>().material = materialnew;
                                vrrig.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>().SetPosition(0, vrrig.mainSkin.bones[bones[i]].position);
                                vrrig.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>().SetPosition(1, vrrig.mainSkin.bones[bones[i + 1]].position);
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (Photon.Realtime.Player players in PhotonNetwork.PlayerList)
                {
                    VRRig vrrig2 = GorillaGameManager.instance.FindPlayerVRRig(players);
                    for (int j = 0; j < bones.Count<int>(); j += 2)
                    {
                        if (vrrig2.mainSkin.bones[bones[j]].gameObject.GetComponent<LineRenderer>())
                        {
                            UnityEngine.Object.Destroy(vrrig2.mainSkin.bones[bones[j]].gameObject.GetComponent<LineRenderer>());
                        }
                        if (vrrig2.head.rigTarget.gameObject.GetComponent<LineRenderer>())
                        {
                            UnityEngine.Object.Destroy(vrrig2.head.rigTarget.gameObject.GetComponent<LineRenderer>());
                        }
                    }
                }
            }
        }

        public static int[] bones = new int[]
        {
            4,
            3,
            5,
            4,
            19,
            18,
            20,
            19,
            3,
            18,
            21,
            20,
            22,
            21,
            25,
            21,
            29,
            21,
            31,
            29,
            27,
            25,
            24,
            22,
            6,
            5,
            7,
            6,
            10,
            6,
            14,
            6,
            16,
            14,
            12,
            10,
            9,
            7
        };

        public static void AdvNametags(bool toggle)
        {
            if (!PhotonNetwork.InRoom)
                return;

            if (toggle)
            {
                foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig && vrrig != null)
                    {
                        if (!vrrig.transform.Find("Nametag"))
                        {
                            GameObject nametag = new GameObject("Nametag");
                            nametag.transform.SetParent(vrrig.transform, false);
                            TextMeshPro textMeshPro = nametag.AddComponent<TextMeshPro>();

                            textMeshPro.fontSize = 1.4f;
                            textMeshPro.fontStyle = FontStyles.Normal;
                            textMeshPro.alignment = TextAlignmentOptions.Center;
                            textMeshPro.font = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdBodyText").GetComponent<TextMeshPro>().font;
                        }

                        GameObject name = vrrig.transform.Find("Nametag").gameObject;

                        TextMeshPro tmp = name.GetComponent<TextMeshPro>();
                        int fpps = Traverse.Create(vrrig).Field("fps").GetValue<int>();
                        string fpsColor = fpps < 45 ? "red" : (fpps > 80 ? "green" : "orange");

                        tmp.text =
                            $"{GradientText.MakeAnimatedGradient(ColorLib.ClrToHex(Magenta), ColorLib.ClrToHex(Purple), vrrig.OwningNetPlayer.NickName, Time.time)}\n" +
                            $"ID: {GradientText.MakeAnimatedGradient(ColorLib.ClrToHex(Magenta), ColorLib.ClrToHex(Purple), vrrig.Creator.UserId, Time.time)}\n" +
                            $"Is Master: {GradientText.MakeAnimatedGradient(ColorLib.ClrToHex(Magenta), ColorLib.ClrToHex(Purple), IsUserMaster(vrrig).ToString(), Time.time)}\n" +
                            $"Platform: {GradientText.MakeAnimatedGradient(ColorLib.ClrToHex(Magenta), ColorLib.ClrToHex(Purple), VrrigPlatform(vrrig), Time.time)}\n" +
                            $"Actor Number: {GradientText.MakeAnimatedGradient(ColorLib.ClrToHex(Magenta), ColorLib.ClrToHex(Purple), vrrig.Creator.ActorNumber.ToString(), Time.time)}\n" +
                            $"Tagged: {GradientText.MakeAnimatedGradient(ColorLib.ClrToHex(Magenta), ColorLib.ClrToHex(Purple), RigIsInfected(vrrig).ToString(), Time.time)}\n" +
                            $"FPS: <color={fpsColor}>{fpps}</color>"; ;

                        name.transform.position = vrrig.headMesh.transform.position + new Vector3(0f, 0.90f, 0f);
                        name.transform.LookAt(Camera.main.transform);
                        name.transform.Rotate(0, 180, 0);
                    }

                    if (vrrig == null)
                    {
                        if (vrrig.transform.Find("Nametag"))
                        {
                            GameObject.Destroy(vrrig.transform.Find("Nametag").gameObject);
                        }
                    }
                }
            }
            else
            {
                foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig && vrrig != null)
                    {
                        if (vrrig.transform.Find("Nametag"))
                        {
                            GameObject.Destroy(vrrig.transform.Find("Nametag").gameObject);
                        }
                    }

                    if (vrrig == null)
                    {
                        if (vrrig.transform.Find("Nametag"))
                        {
                            GameObject.Destroy(vrrig.transform.Find("Nametag").gameObject);
                        }
                    }
                }
            }
        }

        private static GameObject infoObj;
        private static TextMeshPro infoText;
        public static void InfoDisplay(bool toggle)
        {
            if (toggle)
            {
                if (infoObj == null && PhotonNetwork.InRoom)
                {
                    infoObj = new GameObject("InfoDisplay TMP");
                    infoObj.transform.localScale = Vector3.one * 0.1f;
                    infoObj.transform.parent = GTPlayer.Instance.headCollider.transform;

                    infoText = infoObj.AddComponent<TextMeshPro>();
                    infoText.color = Color.white;
                    infoText.font = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdBodyText").GetComponent<TextMeshPro>().font;
                    infoText.fontSize = 6.5f;
                    infoText.alignment = TextAlignmentOptions.Right;
                    infoText.richText = true;
                }

                if (!PhotonNetwork.InRoom)
                {
                    if (infoText != null)
                        GameObject.Destroy(infoText);

                    if (infoObj != null)
                        GameObject.Destroy(infoObj);
                }

                int fps = (Time.deltaTime > 0) ? Mathf.RoundToInt(1.0f / Time.deltaTime) : 0;

                string fpsColor = fps > 80 ? "green" : "orange";
                string fpsColor2 = fps < 45 ? "red" : fpsColor;

                infoText.text =
                    $"Name: {GradientText.MakeAnimatedGradient(ColorLib.ClrToHex(Magenta), ColorLib.ClrToHex(Purple), PhotonNetwork.LocalPlayer.NickName, Time.time)}\n" +
                    $"ID: {GradientText.MakeAnimatedGradient(ColorLib.ClrToHex(Magenta), ColorLib.ClrToHex(Purple), PhotonNetwork.LocalPlayer.UserId, Time.time)}\n" +
                    $"Master Client: {GradientText.MakeAnimatedGradient(ColorLib.ClrToHex(Magenta), ColorLib.ClrToHex(Purple), PhotonNetwork.MasterClient.ToString(), Time.time)}\n" +
                    $"Actor Number: {GradientText.MakeAnimatedGradient(ColorLib.ClrToHex(Magenta), ColorLib.ClrToHex(Purple), PhotonNetwork.LocalPlayer.ActorNumber.ToString(), Time.time)}\n" +
                    $"Tagged: {GradientText.MakeAnimatedGradient(ColorLib.ClrToHex(Magenta), ColorLib.ClrToHex(Purple), IAmInfected.ToString(), Time.time)}\n" +
                    $"FPS: <color={fpsColor2}>{fps}</color>";

                Transform head = GTPlayer.Instance.headCollider.transform;
                infoObj.transform.position = head.position + head.forward * 2f + head.up * 0.5f - head.right * 0.5f;
                infoObj.transform.forward = head.forward;
            } else
            {
                if (infoText != null)
                    GameObject.Destroy(infoText);

                if (infoObj != null)
                    GameObject.Destroy(infoObj);
            }
        }

        public static void SnakeESP(bool toggle)
        {
            if (!PhotonNetwork.InRoom)
                return;

            if (toggle)
            {
                foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig && vrrig != null)
                    {
                        if (!vrrig.transform.Find("PlayerTrail"))
                        {
                            GameObject trailObject = new GameObject("PlayerTrail");
                            trailObject.transform.SetParent(vrrig.transform, false);

                            TrailRenderer trailRenderer = trailObject.AddComponent<TrailRenderer>();
                            trailRenderer.material = new Material(Shader.Find("Unlit/Color"));
                            Color color1 = GetESPColor(vrrig);
                            color1.a = 0.5f;
                            trailRenderer.time = 2f;
                            trailRenderer.startWidth = 0.2f;
                            trailRenderer.endWidth = 0f;
                            trailRenderer.material.color = color1;
                            trailRenderer.autodestruct = true;
                        }

                        GameObject trailObj = vrrig.transform.Find("PlayerTrail").gameObject;
                        trailObj.transform.position = vrrig.transform.position;
                    }

                    if (vrrig == null)
                    {
                        if (vrrig.transform.Find("PlayerTrail"))
                        {
                            GameObject.Destroy(vrrig.transform.Find("PlayerTrail").gameObject);
                        }
                    }
                }
            } else
            {
                foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig && vrrig != null)
                    {
                        if (vrrig.transform.Find("PlayerTrail"))
                        {
                            GameObject.Destroy(vrrig.transform.Find("PlayerTrail").gameObject);
                        }
                    }

                    if (vrrig == null)
                    {
                        if (vrrig.transform.Find("PlayerTrail"))
                        {
                            GameObject.Destroy(vrrig.transform.Find("PlayerTrail").gameObject);
                        }
                    }
                }
            }
        }

        public static void Beacons()
        {
            if (!PhotonNetwork.InRoom)
                return;

            foreach (VRRig vrrig in VRRigCache.ActiveRigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig && vrrig != null)
                {
                    GameObject line = new GameObject("Line");
                    LineRenderer Line = line.AddComponent<LineRenderer>();

                    Vector3 rig = vrrig.transform.position;
                    Line.SetPositions(new Vector3[] { rig + new Vector3(0f, 1000f, 0f), rig - new Vector3(0f, 1000f, 0f) });
                    Line.startWidth = 0.07f;
                    Line.endWidth = 0.07f;
                    UnityEngine.Color color1 = GetESPColor(vrrig);
                    color1.a = 0.5f;
                    Line.startColor = color1;
                    Line.endColor = color1;
                    Line.material.shader = Shader.Find("GUI/Text Shader");

                    UnityEngine.Object.Destroy(line, Time.deltaTime);
                }
            }
        }
        public static void EntityESP(bool bat)
        {
            GameObject ESP = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            ESP.transform.position = bat ? Fun.Bat.transform.position : Fun.Bug.transform.position;
            UnityEngine.Object.Destroy(ESP.GetComponent<Collider>());
            ESP.transform.localScale = new Vector3(0.5f, 0.5f, 0f);
            ESP.transform.LookAt(GorillaTagger.Instance.headCollider.transform.position);
            ESP.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
            UnityEngine.Color color = RGB.color;
            color.a = 0.5f;
            ESP.GetComponent<Renderer>().material.color = color;
            UnityEngine.Object.Destroy(ESP, Time.deltaTime);
        }
        public static void Ignore()
        {
            GunTemplate.StartBothGuns(() =>
            {
                if (GunTemplate.LockedPlayer!.bodyRenderer.gameModeBodyType == GorillaBodyType.Default)
                {
                    GunTemplate.LockedPlayer.bodyRenderer.SetGameModeBodyType(GorillaBodyType.Invisible);
                    #region mute
                    GorillaPlayerScoreboardLine[] Board = UnityEngine.Object.FindObjectsOfType<GorillaPlayerScoreboardLine>();
                    foreach (GorillaPlayerScoreboardLine mute in Board)
                    {
                        if (mute.linePlayer == GunTemplate.LockedPlayer.OwningNetPlayer)
                        {
                            mute.PressButton(true, GorillaPlayerLineButton.ButtonType.Mute);
                            mute.muteButton.isOn = true;
                            mute.muteButton.UpdateColor();
                        }
                    }
                    #endregion
                }
            }, true);
        }

        public static void SkeletonEsp(bool toggle)
        {
            if (!PhotonNetwork.InRoom)
                return;

            if (toggle)
            {
                foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig && vrrig != null)
                    {
                        vrrig.skeleton.UpdateColor(vrrig.playerColor);
                        vrrig.skeleton.renderer.sharedMaterial.shader = Shader.Find("GUI/Text Shader");
                        vrrig.skeleton.renderer.sharedMaterial.color = vrrig.playerColor;
                        vrrig.skeleton.enabled = true;
                        vrrig.skeleton.renderer.enabled = true;
                    }
                }
            }
            else
            {
                foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig)
                    {
                        vrrig.skeleton.enabled = false;
                        vrrig.skeleton.renderer.enabled = false;
                    }
                }
            }
        }

        public static Color GetESPColor(VRRig vrrig)
        {
            return espSetting switch
            {
                1 => vrrig.mainSkin.material.name.Contains("fected") ? Color.red : Color.green,
                2 => vrrig.playerColor,
                3 => RGB.color,
                4 => ColorLib.MenuMat[0].color,
                _ => vrrig.mainSkin.material.name.Contains("fected") ? Color.red : Color.green,
            };
        }
        static Vector3 GetTracerPosition()
        {
            return tracePos switch
            {
                1 => GorillaTagger.Instance.rightHandTransform.position,
                2 => GorillaTagger.Instance.leftHandTransform.position,
                3 => GorillaTagger.Instance.bodyCollider.transform.position,
                4 => GorillaTagger.Instance.headCollider.transform.position,
                _ => GorillaTagger.Instance.rightHandTransform.position,
            };
        }
    }
}

