using UnityEngine;
using static Elixir.Utilities.Variables;
using static Elixir.Utilities.ColorLib;
using static Elixir.Mods.Categories.Settings;
using Photon.Pun;
using Object = UnityEngine.Object;
using Photon.Realtime;
using HarmonyLib;
using Elixir.Utilities;
using GorillaLocomotion;
using TMPro;
//using System.Drawing;

namespace Elixir.Mods.Categories
{
    public class Visuals
    {
        public static void Shadows(bool b)
        {
            GameLightingManager.instance.SetCustomDynamicLightingEnabled(b);
        }
        public static void ESP()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    vrrig.mainSkin.material.shader = Shader.Find("GUI/Text Shader");
                    vrrig.mainSkin.material.color = GetESPColor(vrrig);
                }
            }
        }
        public static void DisableESP()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    vrrig.mainSkin.material.shader = Shader.Find("GorillaTag/UberShader");
                }
            }
        }
        public static void BallESP()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    GameObject ESPBall = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    ESPBall.transform.position = vrrig.transform.position;
                    UnityEngine.Object.Destroy(ESPBall.GetComponent<SphereCollider>());
                    ESPBall.transform.localScale = new Vector3(0.5f, 0.5f, 0f);
                    ESPBall.transform.LookAt(GorillaTagger.Instance.headCollider.transform.position);

                    UnityEngine.Color color1 = GetESPColor(vrrig);
                    color1.a = 0.5f;
                    ESPBall.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                    ESPBall.GetComponent<Renderer>().material.color = color1;

                    UnityEngine.Object.Destroy(ESPBall, Time.deltaTime);
                }
            }
        }
        public static void Wireframe(bool d)
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    if (!d)
                    {
                        GameObject ESPBox = new GameObject("Line");
                        LineRenderer line = ESPBox.AddComponent<LineRenderer>();

                        line.startWidth = 0.035f;
                        line.endWidth = 0.035f;

                        UnityEngine.Color color1 = GetESPColor(vrrig);
                        color1.a = 0.5f;
                        line.startColor = color1;
                        line.endColor = color1;
                        line.material.shader = Shader.Find("GUI/Text Shader");

                        Vector3 head = GorillaTagger.Instance.headCollider.transform.position;
                        Vector3 toHead = (head - vrrig.transform.position).normalized;
                        Vector3 right = Vector3.Cross(toHead, Vector3.up).normalized;
                        Vector3 up = Vector3.Cross(right, toHead).normalized;

                        line.positionCount = 5;
                        line.SetPosition(0, vrrig.transform.position + (right + up) * 0.5f);
                        line.SetPosition(1, vrrig.transform.position + (right - up) * 0.5f);
                        line.SetPosition(2, vrrig.transform.position + (-right - up) * 0.5f);
                        line.SetPosition(3, vrrig.transform.position + (-right + up) * 0.5f);
                        line.SetPosition(4, vrrig.transform.position + (right + up) * 0.5f);

                        UnityEngine.Object.Destroy(ESPBox, Time.deltaTime);
                    }
                    else
                    {
                        GameObject ESPBox = new GameObject("Line");
                        LineRenderer line = ESPBox.AddComponent<LineRenderer>();

                        line.startWidth = 0.035f;
                        line.endWidth = 0.035f;

                        UnityEngine.Color color1 = GetESPColor(vrrig);
                        color1.a = 0.5f;
                        line.startColor = color1;
                        line.endColor = color1;
                        line.material.shader = Shader.Find("GUI/Text Shader");

                        line.positionCount = 16;
                        Vector3 offset = vrrig.transform.position;
                        line.SetPosition(0, offset + new Vector3(-0.5f, -0.5f, -0.5f));
                        line.SetPosition(1, offset + new Vector3(0.5f, -0.5f, -0.5f));
                        line.SetPosition(2, offset + new Vector3(0.5f, 0.5f, -0.5f));
                        line.SetPosition(3, offset + new Vector3(-0.5f, 0.5f, -0.5f));
                        line.SetPosition(4, offset + new Vector3(-0.5f, -0.5f, -0.5f));

                        line.SetPosition(5, offset + new Vector3(-0.5f, -0.5f, 0.5f));
                        line.SetPosition(6, offset + new Vector3(0.5f, -0.5f, 0.5f));
                        line.SetPosition(7, offset + new Vector3(0.5f, 0.5f, 0.5f));
                        line.SetPosition(8, offset + new Vector3(-0.5f, 0.5f, 0.5f));
                        line.SetPosition(9, offset + new Vector3(-0.5f, -0.5f, 0.5f));

                        line.SetPosition(10, offset + new Vector3(0.5f, -0.5f, 0.5f));
                        line.SetPosition(11, offset + new Vector3(0.5f, -0.5f, -0.5f));
                        line.SetPosition(12, offset + new Vector3(0.5f, 0.5f, -0.5f));
                        line.SetPosition(13, offset + new Vector3(0.5f, 0.5f, 0.5f)); 
                        line.SetPosition(14, offset + new Vector3(-0.5f, 0.5f, 0.5f));
                        line.SetPosition(15, offset + new Vector3(-0.5f, 0.5f, -0.5f));

                        UnityEngine.Object.Destroy(ESPBox, Time.deltaTime);
                    }
                }
            }
        }
        public static void CSGO()
        {
            Wireframe(false);
            Tracers();
        }
        public static void BoxESP(bool d)
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    if (!d)
                    {
                        GameObject ESPBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        ESPBox.transform.position = vrrig.transform.position;
                        UnityEngine.Object.Destroy(ESPBox.GetComponent<BoxCollider>());
                        ESPBox.transform.localScale = new Vector3(0.5f, 1f, 0.5f);
                        ESPBox.transform.LookAt(GorillaTagger.Instance.headCollider.transform.position);
                        UnityEngine.Color color1 = GetESPColor(vrrig);
                        color1.a = 0.5f;
                        ESPBox.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                        ESPBox.GetComponent<Renderer>().material.color = color1;
                        UnityEngine.Object.Destroy(ESPBox, Time.deltaTime);
                    }
                    else
                    {
                        GameObject ESPBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        ESPBox.transform.position = vrrig.transform.position;
                        UnityEngine.Object.Destroy(ESPBox.GetComponent<BoxCollider>());
                        ESPBox.transform.localScale = new Vector3(0.5f, 1f, 0.5f);
                        ESPBox.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                        UnityEngine.Color color1 = GetESPColor(vrrig);
                        color1.a = 0.5f;
                        ESPBox.GetComponent<Renderer>().material.color = color1;
                        UnityEngine.Object.Destroy(ESPBox, Time.deltaTime);
                    }
                }
            }
        }
        public static void Tracers()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    GameObject line = new GameObject("Line");
                    LineRenderer Line = line.AddComponent<LineRenderer>();
                    Line.SetPosition(0, GetTracerPosition());
                    Line.SetPosition(1, vrrig.transform.position);
                    Line.startWidth = 0.0225f;
                    Line.endWidth = 0.0225f;
                    UnityEngine.Color color1 = GetESPColor(vrrig);
                    color1.a = 0.5f;
                    Line.startColor = color1;
                    Line.endColor = color1;
                    Line.material.shader = Shader.Find("GUI/Text Shader");

                    UnityEngine.Object.Destroy(line, Time.deltaTime);
                }
            }
        }
        public static void DistanceESP()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig == null || vrrig == GorillaTagger.Instance.offlineVRRig) continue;

                GameObject distance = new GameObject($"{vrrig.name}'s Distance");
                TextMeshPro textMeshPro = distance.AddComponent<TextMeshPro>();

                textMeshPro.fontSize = 3.5f;
                textMeshPro.alignment = TextAlignmentOptions.Center;
                textMeshPro.color = RGB.color;

                textMeshPro.font = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdBodyText").GetComponent<TextMeshPro>().font;

                distance.transform.position = vrrig.headMesh.transform.position + new Vector3(0f, 0.65f, 0f);
                distance.transform.LookAt(Camera.main.transform);
                distance.transform.Rotate(0, 180, 0);

                float distanceTovrrig = Vector3.Distance(GorillaLocomotion.GTPlayer.Instance.headCollider.transform.position, vrrig.transform.position);
                textMeshPro.text = $"{Mathf.RoundToInt(distanceTovrrig)}m";

                GameObject.Destroy(distance, Time.deltaTime);
            }
        }
        public static void Nametags()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig == GorillaTagger.Instance.offlineVRRig) continue;

                GameObject name = new GameObject($"{vrrig.name}'s Nametag");
                TextMeshPro textMeshPro = name.AddComponent<TextMeshPro>();

                textMeshPro.color = RGB.color;
                textMeshPro.material.shader = Shader.Find("GUI/Text Shader");
                textMeshPro.fontSize = 3.5f;
                textMeshPro.fontStyle = FontStyles.Normal;
                textMeshPro.alignment = TextAlignmentOptions.Center;
                textMeshPro.text = RigManager.GetPlayerFromVRRig(vrrig).NickName;
                textMeshPro.font = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdBodyText").GetComponent<TextMeshPro>().font;

                name.transform.position = vrrig.headMesh.transform.position + new Vector3(0f, 0.90f, 0f);
                name.transform.LookAt(Camera.main.transform);
                name.transform.Rotate(0, 180, 0);

                GameObject.Destroy(name, Time.deltaTime);
            }
        }
        public static void AdvNametags()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    GameObject advName = new GameObject("Text");
                    TextMeshPro textMeshPro = advName.AddComponent<TextMeshPro>();

                    textMeshPro.fontSize = 2f;
                    textMeshPro.fontStyle = FontStyles.Normal;
                    textMeshPro.alignment = TextAlignmentOptions.Center;
                    textMeshPro.color = Color.white;
                    textMeshPro.font = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdBodyText").GetComponent<TextMeshPro>().font;

                    advName.transform.position = vrrig.headMesh.transform.position + new Vector3(0f, 1.2f, 0f);
                    advName.transform.LookAt(Camera.main.transform);
                    advName.transform.Rotate(0, 180, 0);

                    int fpps = Traverse.Create(vrrig).Field("fps").GetValue<int>();
                    string fpsColor = fpps < 45 ? "red" : (fpps > 80 ? "green" : "orange");

                    textMeshPro.text =
                        $"<color={hexColor}>{vrrig.OwningNetPlayer.NickName}</color>\n" +
                        $"ID: <color={hexColor}>{vrrig.Creator.UserId}</color>\n" +
                        $"Is Master: <color={hexColor}>{IsUserMaster(vrrig)}</color>\n" +
                        $"Platform: <color={hexColor}>{VrrigPlatform(vrrig)}</color>\n" +
                        $"Actor Number: <color={hexColor}>{vrrig.Creator.ActorNumber}</color>\n" +
                        $"Tagged: <color={hexColor}>{RigIsInfected(vrrig)}</color>\n" +
                        $"FPS: <color={fpsColor}>{fpps}</color>";

                    GameObject.Destroy(advName, Time.deltaTime);
                }
            }
        }
        public static void InfoDisplay()
        {
            int fps = (Time.deltaTime > 0) ? Mathf.RoundToInt(1.0f / Time.deltaTime) : 0;

            GameObject info = new GameObject("InfoDisplay TMP");
            TextMeshPro textMeshPro = info.AddComponent<TextMeshPro>();

            string fpsColor = fps > 80 ? "green" : "orange";
            string fpsColor2 = fps < 45 ? "red" : fpsColor;

            textMeshPro.text =
                $"Name: <color={hexColor}>{PhotonNetwork.LocalPlayer.NickName}</color>\n" +
                $"ID: <color={hexColor}>{PhotonNetwork.LocalPlayer.UserId}</color>\n" +
                $"Master Client: <color={hexColor}>{PhotonNetwork.MasterClient}</color>\n" +
                $"Actor Number: <color={hexColor}>{PhotonNetwork.LocalPlayer.ActorNumber}</color>\n" +
                $"Tagged: <color={hexColor}>{IAmInfected}</color>\n" +
                $"FPS: <color={fpsColor2}>{fps}</color>";

            textMeshPro.color = Color.white;
            textMeshPro.font = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdBodyText").GetComponent<TextMeshPro>().font;
            textMeshPro.fontSize = 6.5f;
            textMeshPro.alignment = TextAlignmentOptions.Right;
            textMeshPro.richText = true;

            Transform head = GTPlayer.Instance.headCollider.transform;
            info.transform.position = head.position + head.forward * 2f + head.up * 0.5f - head.right * 0.5f;
            info.transform.forward = head.forward;
            info.transform.localScale = Vector3.one * 0.1f;

            Object.Destroy(info, Time.deltaTime);
        }
        public static void SnakeESP()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    GameObject trailObject = new GameObject("PlayerTrail");
                    trailObject.transform.position = vrrig.transform.position;
                    trailObject.transform.SetParent(vrrig.transform);
                    TrailRenderer trailRenderer = trailObject.AddComponent<TrailRenderer>();
                    trailRenderer.material = new Material(Shader.Find("Unlit/Color"));
                    Color color1 = GetESPColor(vrrig);
                    color1.a = 0.5f;
                    trailRenderer.time = 2f;
                    trailRenderer.startWidth = 0.2f;
                    trailRenderer.endWidth = 0f;
                    trailRenderer.material.color = color1;
                    trailRenderer.autodestruct = true;
                    GameObject.Destroy(trailObject, trailRenderer.time + 0.5f);
                }
            }
        }
        public static void Beacons()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig)
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
        public static void EnableSkeleton()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    vrrig.skeleton.UpdateColor(vrrig.playerColor);
                    vrrig.skeleton.renderer.sharedMaterial.shader = Shader.Find("GUI/Text Shader");
                    vrrig.skeleton.renderer.sharedMaterial.color = vrrig.playerColor;
                    vrrig.skeleton.enabled = true;
                    vrrig.skeleton.renderer.enabled = true;
                }
            }
        }
        public static void DisableSkeleton()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    vrrig.skeleton.enabled = false;
                    vrrig.skeleton.renderer.enabled = false;
                }
            }
        }
        static Color GetESPColor(VRRig vrrig)
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

