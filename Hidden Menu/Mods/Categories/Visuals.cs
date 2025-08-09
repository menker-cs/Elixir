using UnityEngine;
using static Hidden.Utilities.Variables;
using static Hidden.Utilities.ColorLib;
using static Hidden.Mods.Categories.Settings;
using static Hidden.Menu.Main;
using Photon.Pun;
using Object = UnityEngine.Object;
using Photon.Realtime;
using HarmonyLib;
using Hidden.Utilities;
using GorillaLocomotion;
using TMPro;
//using System.Drawing;

namespace Hidden.Mods.Categories
{
    public class Visuals
    {
        public static void Shadows(bool b)
        {
            GameLightingManager.instance.SetCustomDynamicLightingEnabled(b);
        }
        public static void ESP()
        {
            // 1 = casual
            // 2 = infection
            // 3 = rainbow
            // 4 = menu color
            if (espSetting == 1)
            {
                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig)
                    {
                        if (vrrig.mainSkin.material.name.Contains("fected"))
                        {
                            vrrig.mainSkin.material.shader = Shader.Find("GUI/Text Shader");
                            vrrig.mainSkin.material.color = UnityEngine.Color.red;
                        }
                        else
                        {
                            vrrig.mainSkin.material.shader = Shader.Find("GUI/Text Shader");
                            vrrig.mainSkin.material.color = UnityEngine.Color.green;
                        }
                    }
                }
            }
            else if (espSetting == 2)
            {
                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig)
                    {
                        vrrig.mainSkin.material.shader = Shader.Find("GUI/Text Shader");
                        vrrig.mainSkin.material.color = vrrig.playerColor;
                    }
                }
            }
            else if (espSetting == 3)
            {
                GradientColorKey[] array = new GradientColorKey[7];
                array[0].color = UnityEngine.Color.red;
                array[0].time = 0f;
                array[1].color = UnityEngine.Color.yellow;
                array[1].time = 0.2f;
                array[2].color = UnityEngine.Color.green;
                array[2].time = 0.3f;
                array[3].color = UnityEngine.Color.cyan;
                array[3].time = 0.5f;
                array[4].color = UnityEngine.Color.blue;
                array[4].time = 0.6f;
                array[5].color = UnityEngine.Color.magenta;
                array[5].time = 0.8f;
                array[6].color = UnityEngine.Color.red;
                array[6].time = 1f;
                Gradient gradient = new Gradient();
                gradient.colorKeys = array;
                float num = Mathf.PingPong(Time.time / 2f, 1f);
                UnityEngine.Color color = gradient.Evaluate(num);

                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig)
                    {
                        vrrig.mainSkin.material.shader = Shader.Find("GUI/Text Shader");
                        vrrig.mainSkin.material.color = color;
                    }
                }
            }
            else if (espSetting == 4)
            {
                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig)
                    {
                        vrrig.mainSkin.material.shader = Shader.Find("GUI/Text Shader");
                        vrrig.mainSkin.material.color = ColorLib.MenuMat[Theme-1].color;
                    }
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
            // 1 = casual
            // 2 = infection
            // 3 = rainbow
            // 4 = menu color
            if (espSetting == 1)
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
                        if (vrrig.mainSkin.material.name.Contains("fected"))
                        {
                            UnityEngine.Color color = UnityEngine.Color.red;
                            color.a = 0.5f;
                            ESPBall.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                            ESPBall.GetComponent<Renderer>().material.color = color;
                        }
                        else
                        {
                            UnityEngine.Color color = UnityEngine.Color.green;
                            color.a = 0.5f;
                            ESPBall.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                            ESPBall.GetComponent<Renderer>().material.color = color;
                        }
                        UnityEngine.Object.Destroy(ESPBall, Time.deltaTime);
                    }
                }
            }
            else if (espSetting == 2)
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
                        ESPBall.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                        UnityEngine.Color color = vrrig.playerColor;
                        color.a = 0.5f;
                        ESPBall.GetComponent<Renderer>().material.color = color;
                        UnityEngine.Object.Destroy(ESPBall, Time.deltaTime);
                    }
                }
            }
            else if (espSetting == 3)
            {
                GradientColorKey[] array = new GradientColorKey[7];
                array[0].color = UnityEngine.Color.red;
                array[0].time = 0f;
                array[1].color = UnityEngine.Color.yellow;
                array[1].time = 0.2f;
                array[2].color = UnityEngine.Color.green;
                array[2].time = 0.3f;
                array[3].color = UnityEngine.Color.cyan;
                array[3].time = 0.5f;
                array[4].color = UnityEngine.Color.blue;
                array[4].time = 0.6f;
                array[5].color = UnityEngine.Color.magenta;
                array[5].time = 0.8f;
                array[6].color = UnityEngine.Color.red;
                array[6].time = 1f;
                Gradient gradient = new Gradient();
                gradient.colorKeys = array;
                float num = Mathf.PingPong(Time.time / 2f, 1f);
                UnityEngine.Color color = gradient.Evaluate(num);

                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig)
                    {
                        GameObject ESPBall = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        ESPBall.transform.position = vrrig.transform.position;
                        UnityEngine.Object.Destroy(ESPBall.GetComponent<SphereCollider>());
                        ESPBall.transform.localScale = new Vector3(0.5f, 0.5f, 0f);
                        ESPBall.transform.LookAt(GorillaTagger.Instance.headCollider.transform.position);
                        ESPBall.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                        color.a = 0.5f;
                        ESPBall.GetComponent<Renderer>().material.color = color;
                        UnityEngine.Object.Destroy(ESPBall, Time.deltaTime);
                    }
                }
            }
            else if (espSetting == 4)
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
                        ESPBall.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                        UnityEngine.Color color = ColorLib.MenuMat[Theme-1].color;
                        color.a = 0.5f;
                        ESPBall.GetComponent<Renderer>().material.color = color;
                        UnityEngine.Object.Destroy(ESPBall, Time.deltaTime);
                    }
                }
            }
        }
        public static void BoxESP(bool d)
        {
            if (d == false)
            {
                if (espSetting == 1)
                {
                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        if (vrrig != GorillaTagger.Instance.offlineVRRig)
                        {
                            GameObject ESPBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            ESPBox.transform.position = vrrig.transform.position;
                            UnityEngine.Object.Destroy(ESPBox.GetComponent<BoxCollider>());
                            ESPBox.transform.localScale = new Vector3(0.5f, 1f, 0f);
                            ESPBox.transform.LookAt(GorillaTagger.Instance.headCollider.transform.position);
                            if (vrrig.mainSkin.material.name.Contains("fected"))
                            {
                                UnityEngine.Color color = UnityEngine.Color.red;
                                color.a = 0.5f;
                                ESPBox.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                                ESPBox.GetComponent<Renderer>().material.color = color;
                            }
                            else
                            {
                                UnityEngine.Color color = UnityEngine.Color.green;
                                color.a = 0.5f;
                                ESPBox.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                                ESPBox.GetComponent<Renderer>().material.color = color;
                            }
                            UnityEngine.Object.Destroy(ESPBox, Time.deltaTime);
                        }
                    }
                }
                if (espSetting == 2)
                {
                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        if (vrrig != GorillaTagger.Instance.offlineVRRig)
                        {
                            GameObject ESPBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            ESPBox.transform.position = vrrig.transform.position;
                            UnityEngine.Object.Destroy(ESPBox.GetComponent<BoxCollider>());
                            ESPBox.transform.localScale = new Vector3(0.5f, 1f, 0f);
                            ESPBox.transform.LookAt(GorillaTagger.Instance.headCollider.transform.position);
                            ESPBox.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                            UnityEngine.Color color = vrrig.playerColor;
                            color.a = 0.5f;
                            ESPBox.GetComponent<Renderer>().material.color = color;
                            UnityEngine.Object.Destroy(ESPBox, Time.deltaTime);
                        }
                    }
                }
                if (espSetting == 3)
                {
                    GradientColorKey[] array = new GradientColorKey[7];
                    array[0].color = UnityEngine.Color.red;
                    array[0].time = 0f;
                    array[1].color = UnityEngine.Color.yellow;
                    array[1].time = 0.2f;
                    array[2].color = UnityEngine.Color.green;
                    array[2].time = 0.3f;
                    array[3].color = UnityEngine.Color.cyan;
                    array[3].time = 0.5f;
                    array[4].color = UnityEngine.Color.blue;
                    array[4].time = 0.6f;
                    array[5].color = UnityEngine.Color.magenta;
                    array[5].time = 0.8f;
                    array[6].color = UnityEngine.Color.red;
                    array[6].time = 1f;
                    Gradient gradient = new Gradient();
                    gradient.colorKeys = array;
                    float num = Mathf.PingPong(Time.time / 2f, 1f);
                    UnityEngine.Color color = gradient.Evaluate(num);

                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        if (vrrig != GorillaTagger.Instance.offlineVRRig)
                        {
                            GameObject ESPBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            ESPBox.transform.position = vrrig.transform.position;
                            UnityEngine.Object.Destroy(ESPBox.GetComponent<BoxCollider>());
                            ESPBox.transform.localScale = new Vector3(0.5f, 1f, 0f);
                            ESPBox.transform.LookAt(GorillaTagger.Instance.headCollider.transform.position);
                            ESPBox.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                            color.a = 0.5f;
                            ESPBox.GetComponent<Renderer>().material.color = color;
                            UnityEngine.Object.Destroy(ESPBox, Time.deltaTime);
                        }
                    }
                }
                if (espSetting == 4)
                {
                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        if (vrrig != GorillaTagger.Instance.offlineVRRig)
                        {
                            GameObject ESPBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            ESPBox.transform.position = vrrig.transform.position;
                            UnityEngine.Object.Destroy(ESPBox.GetComponent<BoxCollider>());
                            ESPBox.transform.localScale = new Vector3(0.5f, 1f, 0f);
                            ESPBox.transform.LookAt(GorillaTagger.Instance.headCollider.transform.position);
                            ESPBox.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                            UnityEngine.Color color = ColorLib.MenuMat[Theme-1].color;
                            color.a = 0.5f;
                            ESPBox.GetComponent<Renderer>().material.color = color;
                            UnityEngine.Object.Destroy(ESPBox, Time.deltaTime);
                        }
                    }
                }
            }
            else if (d == true)
            {
                if (espSetting == 1)
                {
                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        if (vrrig != GorillaTagger.Instance.offlineVRRig)
                        {
                            GameObject ESPBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            ESPBox.transform.position = vrrig.transform.position;
                            UnityEngine.Object.Destroy(ESPBox.GetComponent<BoxCollider>());
                            ESPBox.transform.localScale = new Vector3(0.5f, 1f, 0.5f);
                            if (vrrig.mainSkin.material.name.Contains("fected"))
                            {
                                UnityEngine.Color color = UnityEngine.Color.red;
                                color.a = 0.5f;
                                ESPBox.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                                ESPBox.GetComponent<Renderer>().material.color = color;
                            }
                            else
                            {
                                UnityEngine.Color color = UnityEngine.Color.green;
                                color.a = 0.5f;
                                ESPBox.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                                ESPBox.GetComponent<Renderer>().material.color = color;
                            }
                            UnityEngine.Object.Destroy(ESPBox, Time.deltaTime);
                        }
                    }
                }
                if (espSetting == 2)
                {
                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        if (vrrig != GorillaTagger.Instance.offlineVRRig)
                        {
                            GameObject ESPBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            ESPBox.transform.position = vrrig.transform.position;
                            UnityEngine.Object.Destroy(ESPBox.GetComponent<BoxCollider>());
                            ESPBox.transform.localScale = new Vector3(0.5f, 1f, 0.5f);
                            ESPBox.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                            UnityEngine.Color color = vrrig.playerColor;
                            color.a = 0.5f;
                            ESPBox.GetComponent<Renderer>().material.color = color;
                            UnityEngine.Object.Destroy(ESPBox, Time.deltaTime);
                        }
                    }
                }
                if (espSetting == 3)
                {
                    GradientColorKey[] array = new GradientColorKey[7];
                    array[0].color = UnityEngine.Color.red;
                    array[0].time = 0f;
                    array[1].color = UnityEngine.Color.yellow;
                    array[1].time = 0.2f;
                    array[2].color = UnityEngine.Color.green;
                    array[2].time = 0.3f;
                    array[3].color = UnityEngine.Color.cyan;
                    array[3].time = 0.5f;
                    array[4].color = UnityEngine.Color.blue;
                    array[4].time = 0.6f;
                    array[5].color = UnityEngine.Color.magenta;
                    array[5].time = 0.8f;
                    array[6].color = UnityEngine.Color.red;
                    array[6].time = 1f;
                    Gradient gradient = new Gradient();
                    gradient.colorKeys = array;
                    float num = Mathf.PingPong(Time.time / 2f, 1f);
                    UnityEngine.Color color = gradient.Evaluate(num);

                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        if (vrrig != GorillaTagger.Instance.offlineVRRig)
                        {
                            GameObject ESPBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            ESPBox.transform.position = vrrig.transform.position;
                            UnityEngine.Object.Destroy(ESPBox.GetComponent<BoxCollider>());
                            ESPBox.transform.localScale = new Vector3(0.5f, 1f, 0.5f);
                            ESPBox.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                            color.a = 0.5f;
                            ESPBox.GetComponent<Renderer>().material.color = color;
                            UnityEngine.Object.Destroy(ESPBox, Time.deltaTime);
                        }
                    }
                }
                if (espSetting == 4)
                {
                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        if (vrrig != GorillaTagger.Instance.offlineVRRig)
                        {
                            GameObject ESPBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            ESPBox.transform.position = vrrig.transform.position;
                            UnityEngine.Object.Destroy(ESPBox.GetComponent<BoxCollider>());
                            ESPBox.transform.localScale = new Vector3(0.5f, 1f, 0.5f);
                            ESPBox.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                            UnityEngine.Color color = ColorLib.MenuMat[Theme-1].color;
                            color.a = 0.5f;
                            ESPBox.GetComponent<Renderer>().material.color = color;
                            UnityEngine.Object.Destroy(ESPBox, Time.deltaTime);
                        }
                    }
                }
            }
        }
        public static void Tracers()
        {
            if (espSetting == 1)
            {
                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig)
                    {
                        GameObject tracer1 = new GameObject("Line");
                        LineRenderer tracer2 = tracer1.AddComponent<LineRenderer>();
                        tracer2.SetPosition(0, GorillaTagger.Instance.rightHandTransform.position);
                        tracer2.SetPosition(1, vrrig.transform.position);
                        tracer2.startWidth = 0.0225f;
                        tracer2.endWidth = 0.0225f;

                        tracer2.material.shader = Shader.Find("GUI/Text Shader");

                        UnityEngine.Object.Destroy(tracer1, Time.deltaTime);

                        if (vrrig.mainSkin.material.name.Contains("fected"))
                        {
                            UnityEngine.Color color = UnityEngine.Color.red;
                            color.a = 0.5f;
                            tracer2.startColor = color;
                            tracer2.endColor = color;
                        }
                        else
                        {
                            UnityEngine.Color color = UnityEngine.Color.green;
                            color.a = 0.5f;
                            tracer2.startColor = color;
                            tracer2.endColor = color;
                        }
                    }
                }
            }
            else if (espSetting == 2)
            {
                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig)
                    {
                        GameObject line = new GameObject("Line");
                        LineRenderer Line = line.AddComponent<LineRenderer>();
                        Line.SetPosition(0, GorillaTagger.Instance.rightHandTransform.position);
                        Line.SetPosition(1, vrrig.transform.position);
                        Line.startWidth = 0.0225f;
                        Line.endWidth = 0.0225f;
                        UnityEngine.Color color = vrrig.playerColor;
                        color.a = 0.5f;
                        Line.startColor = color;
                        Line.endColor = color;
                        Line.material.shader = Shader.Find("GUI/Text Shader");

                        UnityEngine.Object.Destroy(line, Time.deltaTime);
                    }
                }
            }
            else if (espSetting == 3)
            {
                GradientColorKey[] array = new GradientColorKey[7];
                array[0].color = UnityEngine.Color.red;
                array[0].time = 0f;
                array[1].color = UnityEngine.Color.yellow;
                array[1].time = 0.2f;
                array[2].color = UnityEngine.Color.green;
                array[2].time = 0.3f;
                array[3].color = UnityEngine.Color.cyan;
                array[3].time = 0.5f;
                array[4].color = UnityEngine.Color.blue;
                array[4].time = 0.6f;
                array[5].color = UnityEngine.Color.magenta;
                array[5].time = 0.8f;
                array[6].color = UnityEngine.Color.red;
                array[6].time = 1f;
                Gradient gradient = new Gradient();
                gradient.colorKeys = array;
                float num = Mathf.PingPong(Time.time / 2f, 1f);
                UnityEngine.Color color = gradient.Evaluate(num);

                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig)
                    {
                        GameObject line = new GameObject("Line");
                        LineRenderer Line = line.AddComponent<LineRenderer>();
                        Line.SetPosition(0, GorillaTagger.Instance.rightHandTransform.position);
                        Line.SetPosition(1, vrrig.transform.position);
                        Line.startWidth = 0.0225f;
                        Line.endWidth = 0.0225f;
                        color.a = 0.5f;
                        Line.startColor = color;
                        Line.endColor = color;
                        Line.material.shader = Shader.Find("GUI/Text Shader");

                        UnityEngine.Object.Destroy(line, Time.deltaTime);
                    }
                }
            }
            else if (espSetting == 4)
            {
                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig)
                    {
                        GameObject line = new GameObject("Line");
                        LineRenderer Line = line.AddComponent<LineRenderer>();
                        Line.SetPosition(0, GorillaTagger.Instance.rightHandTransform.position);
                        Line.SetPosition(1, vrrig.transform.position);
                        Line.startWidth = 0.0225f;
                        Line.endWidth = 0.0225f;
                        UnityEngine.Color color = ColorLib.MenuMat[Theme-1].color;
                        color.a = 0.5f;
                        Line.startColor = color;
                        Line.endColor = color;
                        Line.material.shader = Shader.Find("GUI/Text Shader");

                        UnityEngine.Object.Destroy(line, Time.deltaTime);
                    }
                }
            }
        }
        public static void DistanceESP()
        {
            foreach (VRRig Player in GorillaParent.instance.vrrigs)
            {
                if (Player == null || Player == GorillaTagger.Instance.offlineVRRig) continue;

                GameObject distance = new GameObject($"{Player.name}'s Distance");
                TextMeshPro textMeshPro = distance.AddComponent<TextMeshPro>();

                textMeshPro.fontSize = 3.5f;
                textMeshPro.alignment = TextAlignmentOptions.Center;
                textMeshPro.color = RGB.color;

                textMeshPro.font = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdBodyText").GetComponent<TextMeshPro>().font;

                distance.transform.position = Player.headMesh.transform.position + new Vector3(0f, 0.65f, 0f);
                distance.transform.LookAt(Camera.main.transform);
                distance.transform.Rotate(0, 180, 0);

                float distanceToPlayer = Vector3.Distance(GorillaLocomotion.GTPlayer.Instance.headCollider.transform.position, Player.transform.position);
                textMeshPro.text = $"{Mathf.RoundToInt(distanceToPlayer)}m";

                GameObject.Destroy(distance, Time.deltaTime);
            }
        }
        public static void Nametags()
        {
            foreach (VRRig Player in GorillaParent.instance.vrrigs)
            {
                if (Player == GorillaTagger.Instance.offlineVRRig) continue;

                GameObject name = new GameObject($"{Player.name}'s Nametag");
                TextMeshPro textMeshPro = name.AddComponent<TextMeshPro>();

                textMeshPro.color = RGB.color;
                textMeshPro.material.shader = Shader.Find("GUI/Text Shader");
                textMeshPro.fontSize = 3.5f;
                textMeshPro.fontStyle = FontStyles.Normal;
                textMeshPro.alignment = TextAlignmentOptions.Center;
                textMeshPro.text = RigManager.GetPlayerFromVRRig(Player).NickName;
                textMeshPro.font = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdBodyText").GetComponent<TextMeshPro>().font;

                name.transform.position = Player.headMesh.transform.position + new Vector3(0f, 0.90f, 0f);
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
            textMeshPro.enableWordWrapping = false;
            textMeshPro.richText = true;

            Transform head = GTPlayer.Instance.headCollider.transform;
            info.transform.position = head.position + head.forward * 2f + head.up * 0.5f - head.right * 0.5f;
            info.transform.forward = head.forward;
            info.transform.localScale = Vector3.one * 0.1f;

            Object.Destroy(info, Time.deltaTime);
        }
        public static void SnakeESP()
        {
            if (espSetting == 1)
            {
                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig)
                    {
                        Color color;
                        if (vrrig.mainSkin.material.name.Contains("fected"))
                        {
                            color = UnityEngine.Color.red;
                            color.a = 0.5f;
                        }
                        else
                        {
                            color = UnityEngine.Color.green;
                            color.a = 0.5f;
                        }

                        UnityEngine.Color playerColor = vrrig.playerColor;
                        GameObject trailObject = new GameObject("PlayerTrail");
                        trailObject.transform.position = vrrig.transform.position;
                        trailObject.transform.SetParent(vrrig.transform);
                        TrailRenderer trailRenderer = trailObject.AddComponent<TrailRenderer>();
                        trailRenderer.material = new Material(Shader.Find("Unlit/Color"));
                        trailRenderer.material.color = color;
                        trailRenderer.time = 2f;
                        trailRenderer.startWidth = 0.2f;
                        trailRenderer.endWidth = 0f;
                        trailRenderer.startColor = playerColor;
                        trailRenderer.endColor = new UnityEngine.Color(playerColor.r, playerColor.g, playerColor.b, 0f);
                        trailRenderer.autodestruct = true;
                        GameObject.Destroy(trailObject, trailRenderer.time + 0.5f);
                    }
                }
            }
            else if (espSetting == 2)
            {
                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig)
                    {
                        UnityEngine.Color playerColor = vrrig.playerColor;
                        GameObject trailObject = new GameObject("PlayerTrail");
                        trailObject.transform.position = vrrig.transform.position;
                        trailObject.transform.SetParent(vrrig.transform);
                        TrailRenderer trailRenderer = trailObject.AddComponent<TrailRenderer>();
                        trailRenderer.material = new Material(Shader.Find("Unlit/Color"));
                        Color color = vrrig.playerColor;
                        color.a = 0.5f;
                        trailRenderer.material.color = color;
                        trailRenderer.time = 2f;
                        trailRenderer.startWidth = 0.2f;
                        trailRenderer.endWidth = 0f;
                        trailRenderer.startColor = playerColor;
                        trailRenderer.endColor = new UnityEngine.Color(playerColor.r, playerColor.g, playerColor.b, 0f);
                        trailRenderer.autodestruct = true;
                        GameObject.Destroy(trailObject, trailRenderer.time + 0.5f);
                    }
                }
            }
            else if (espSetting == 3)
            {
                GradientColorKey[] array = new GradientColorKey[7];
                array[0].color = UnityEngine.Color.red;
                array[0].time = 0f;
                array[1].color = UnityEngine.Color.yellow;
                array[1].time = 0.2f;
                array[2].color = UnityEngine.Color.green;
                array[2].time = 0.3f;
                array[3].color = UnityEngine.Color.cyan;
                array[3].time = 0.5f;
                array[4].color = UnityEngine.Color.blue;
                array[4].time = 0.6f;
                array[5].color = UnityEngine.Color.magenta;
                array[5].time = 0.8f;
                array[6].color = UnityEngine.Color.red;
                array[6].time = 1f;
                Gradient gradient = new Gradient();
                gradient.colorKeys = array;
                float num = Mathf.PingPong(Time.time / 2f, 1f);
                UnityEngine.Color color = gradient.Evaluate(num);

                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig)
                    {
                        UnityEngine.Color playerColor = vrrig.playerColor;
                        GameObject trailObject = new GameObject("PlayerTrail");
                        trailObject.transform.position = vrrig.transform.position;
                        trailObject.transform.SetParent(vrrig.transform);
                        TrailRenderer trailRenderer = trailObject.AddComponent<TrailRenderer>();
                        trailRenderer.material = new Material(Shader.Find("Unlit/Color"));
                        color.a = 0.5f;
                        trailRenderer.startColor = color;
                        trailRenderer.time = 2f;
                        trailRenderer.startWidth = 0.2f;
                        trailRenderer.endWidth = 0f;
                        trailRenderer.autodestruct = true;
                        GameObject.Destroy(trailObject, trailRenderer.time + 0.5f);
                    }
                }
            }
            else if (espSetting == 4)
            {
                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig)
                    {
                        UnityEngine.Color playerColor = vrrig.playerColor;
                        GameObject trailObject = new GameObject("PlayerTrail");
                        trailObject.transform.position = vrrig.transform.position;
                        trailObject.transform.SetParent(vrrig.transform);
                        TrailRenderer trailRenderer = trailObject.AddComponent<TrailRenderer>();
                        trailRenderer.material = new Material(Shader.Find("Unlit/Color"));
                        Color color = ColorLib.MenuMat[Theme-1].color;
                        color.a = 0.5f;
                        trailRenderer.material.color = color;
                        trailRenderer.time = 2f;
                        trailRenderer.startWidth = 0.2f;
                        trailRenderer.endWidth = 0f;
                        trailRenderer.startColor = playerColor;
                        trailRenderer.endColor = new UnityEngine.Color(playerColor.r, playerColor.g, playerColor.b, 0f);
                        trailRenderer.autodestruct = true;
                        GameObject.Destroy(trailObject, trailRenderer.time + 0.5f);
                    }
                }
            }
        }
        public static void EntityESP(bool bat)
        {
            GameObject ESP = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            ESP.transform.position = bat ? Fun.Bat.transform.position : Fun.Bug.transform.position;
            UnityEngine.Object.Destroy(ESP.GetComponent<SphereCollider>());
            ESP.transform.localScale = new Vector3(0.5f, 0.5f, 0f);
            ESP.transform.LookAt(GorillaTagger.Instance.headCollider.transform.position);
            ESP.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
            UnityEngine.Color color = ColorLib.MenuMat[Theme-1].color;
            color.a = 0.5f;
            ESP.GetComponent<Renderer>().material.color = color;
            UnityEngine.Object.Destroy(ESP, Time.deltaTime);
        }
        public static void Ignore(bool All)
        {
            if (All)
            {
                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    if (vrrig.bodyRenderer.gameModeBodyType == GorillaBodyType.Default)
                    {
                        vrrig.bodyRenderer.SetGameModeBodyType(GorillaBodyType.Invisible);
                        vrrig.bodyRenderer.bodySkeleton.material.shader = Shader.Find("GUI/Text Shader");
                        vrrig.bodyRenderer.bodySkeleton.material.color = espColor;
                        #region mute
                        GorillaPlayerScoreboardLine[] Board = UnityEngine.Object.FindObjectsOfType<GorillaPlayerScoreboardLine>();
                        foreach (GorillaPlayerScoreboardLine mute in Board)
                        {
                            if (mute.linePlayer != null)
                            {
                                mute.PressButton(true, GorillaPlayerLineButton.ButtonType.Mute);
                                mute.muteButton.isOn = true;
                                mute.muteButton.UpdateColor();
                            }
                        }
                        #endregion
                    }
                }
            }
            else
            {
                GunTemplate.StartBothGuns(() =>
                {
                    if (GunTemplate.LockedPlayer.bodyRenderer.gameModeBodyType == GorillaBodyType.Default)
                    {
                        GunTemplate.LockedPlayer.bodyRenderer.SetGameModeBodyType(GorillaBodyType.Invisible);
                        GunTemplate.LockedPlayer.bodyRenderer.bodySkeleton.material.shader = Shader.Find("GUI/Text Shader");
                        GunTemplate.LockedPlayer.bodyRenderer.bodySkeleton.material.color = espColor;
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
    }
}

