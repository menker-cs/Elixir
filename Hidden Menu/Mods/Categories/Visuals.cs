using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static Hidden.Utilities.Variables;
using static Hidden.Utilities.ColorLib;
using static Hidden.Mods.Categories.Settings;
using static Hidden.Menu.Main;
using Photon.Pun;
using Object = UnityEngine.Object;
using System.Linq;
using Photon.Realtime;
using UnityEngine.InputSystem.Controls;
using System.Xml.Linq;
using HarmonyLib;
using Hidden.Utilities;
using Cinemachine;
using Hidden.Utilities;
using UnityEngine.Splines.Interpolators;
using static UnityEngine.Rendering.DebugUI;
using GorillaLocomotion;

namespace Hidden.Mods.Categories
{
    public class Visuals
    {
        public static void HiddenESP()
        {
            foreach (Player player in PhotonNetwork.PlayerListOthers)
            {
                VRRig rig = RigManager.GetVRRigFromPlayer(player);
                string Properties = player.CustomProperties.ToString();
                if (Properties.Contains("Hidden Menu"))
                {
                    if (rig == GorillaTagger.Instance.offlineVRRig) continue;

                    GameObject gameObject = new GameObject("Line");
                    LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
                    lineRenderer.SetPosition(0, GorillaTagger.Instance.rightHandTransform.position);
                    lineRenderer.SetPosition(1, rig.transform.position);
                    lineRenderer.startWidth = 0.0225f;
                    lineRenderer.endWidth = 0.0225f;

                    lineRenderer.material.shader = Shader.Find("GUI/Text Shader");
                    lineRenderer.material.color = Yellow;

                    UnityEngine.Object.Destroy(gameObject, Time.deltaTime);
                }
            }
        }
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
            if (espColor == 1)
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
            else if (espColor == 2)
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
            else if (espColor == 3)
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
            else if (espColor == 4)
            {
                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig)
                    {
                        vrrig.mainSkin.material.shader = Shader.Find("GUI/Text Shader");
                        vrrig.mainSkin.material.color = MenuColor;
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
            if (espColor == 1)
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
            else if (espColor == 2)
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
            else if (espColor == 3)
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
            else if (espColor == 4)
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
                        UnityEngine.Color color = MenuColor;
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
                if (espColor == 1)
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
                                UnityEngine.Color color = UnityEngine.Color.red;
                                color.a = 0.5f;
                                ESPBox.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                                ESPBox.GetComponent<Renderer>().material.color = color;
                            }
                            UnityEngine.Object.Destroy(ESPBox, Time.deltaTime);
                        }
                    }
                }
                if (espColor == 2)
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
                if (espColor == 3)
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
                if (espColor == 4)
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
                            UnityEngine.Color color = MenuColor;
                            color.a = 0.5f;
                            ESPBox.GetComponent<Renderer>().material.color = color;
                            UnityEngine.Object.Destroy(ESPBox, Time.deltaTime);
                        }
                    }
                }
            }
            else if (d == true)
            {
                if (espColor == 1)
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
                if (espColor == 2)
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
                if (espColor == 3)
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
                if (espColor == 4)
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
                            UnityEngine.Color color = MenuColor;
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
            if (espColor == 1)
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
            else if (espColor == 2)
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
            else if (espColor == 3)
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
            else if (espColor == 4)
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
                        UnityEngine.Color color = MenuColor;
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
                if (Player == GorillaTagger.Instance.offlineVRRig) continue;
                distance = new GameObject($"{Player.name}'s Distance");
                TextMesh textMesh = distance.AddComponent<TextMesh>();
                textMesh.fontSize = 20;
                textMesh.fontStyle = FontStyle.Normal;
                textMesh.characterSize = 0.1f;
                textMesh.anchor = TextAnchor.MiddleCenter;
                textMesh.alignment = TextAlignment.Center;
                textMesh.color = Green;
                textMesh.text = Player.name;
                float textWidth = textMesh.GetComponent<Renderer>().bounds.size.x;
                distance.transform.position = Player.headMesh.transform.position + new Vector3(0f, .65f, 0f);
                distance.transform.LookAt(Camera.main.transform.position);
                distance.transform.Rotate(0, 180, 0);
                distance.GetComponent<TextMesh>().text = $"{Convert.ToInt32(Vector3.Distance(GorillaLocomotion.GTPlayer.Instance.headCollider.transform.position, Player.transform.position))}m";
                GameObject.Destroy(distance, Time.deltaTime);
            }
        }
        public static void Nametags()
        {
            foreach (VRRig Player in GorillaParent.instance.vrrigs)
            {
                if (Player == GorillaTagger.Instance.offlineVRRig) continue;
                name = new GameObject($"{Player.name}'s Nametag");
                TextMesh textMesh = name.AddComponent<TextMesh>();
                textMesh.fontSize = 20;
                textMesh.fontStyle = FontStyle.Normal;
                textMesh.characterSize = 0.1f;
                textMesh.anchor = TextAnchor.MiddleCenter;
                textMesh.alignment = TextAlignment.Center;
                textMesh.color = White;
                textMesh.text = Player.name;
                float textWidth = textMesh.GetComponent<Renderer>().bounds.size.x;
                name.transform.position = Player.headMesh.transform.position + new Vector3(0f, .90f, 0f);
                name.transform.LookAt(Camera.main.transform.position);
                name.transform.Rotate(0, 180, 0);
                name.GetComponent<TextMesh>().text = RigManager.GetPlayerFromVRRig(Player).NickName;
                GameObject.Destroy(name, Time.deltaTime);
            }
        }
        public static void AdvNametags()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig == GorillaTagger.Instance.offlineVRRig) continue;
                name = new GameObject($"{vrrig.name}'s Nametag");
                TextMesh textMesh = name.AddComponent<TextMesh>();
                textMesh.fontSize = 20;
                textMesh.fontStyle = FontStyle.Normal;
                textMesh.characterSize = 0.1f;
                textMesh.anchor = TextAnchor.MiddleCenter;
                textMesh.alignment = TextAlignment.Left;
                textMesh.color = White;
                textMesh.text = vrrig.name;
                float textWidth = textMesh.GetComponent<Renderer>().bounds.size.x;
                name.transform.position = vrrig.headMesh.transform.position + new Vector3(0f, .90f, 0f);
                name.transform.LookAt(Camera.main.transform.position);
                name.transform.Rotate(0, 180, 0);
                name.GetComponent<TextMesh>().text = $"<color=#6ffcf3>{vrrig.OwningNetPlayer.NickName}</color>\nFPS: <color=#6ffcf3>{Traverse.Create(vrrig).Field("fps".ToString()).GetValue<int>()}</color>\nID: <color=#6ffcf3>{vrrig.Creator.UserId}</color>\nActor Number: <color=#6ffcf3>{vrrig.Creator.ActorNumber}</color>";
                GameObject.Destroy(name, Time.deltaTime);
            }
        }
        public static void InfoDisplay()
        {
            fps = (Time.deltaTime > 0) ? Mathf.RoundToInt(1.0f / Time.deltaTime) : 0;
            Vector3 position = GTPlayer.Instance.headCollider.transform.position + GTPlayer.Instance.headCollider.transform.forward * 5f + GTPlayer.Instance.headCollider.transform.up * 0.5f - GTPlayer.Instance.headCollider.transform.right * 0.5f;
            GameObject Textobj;
            Textobj = new GameObject("Text");
            TextMesh textMesh = Textobj.AddComponent<TextMesh>();
            textMesh.text = $"Name: <color=#6ffcf3>{PhotonNetwork.LocalPlayer.NickName}</color>\nFPS: <color=#6ffcf3>{fps}</color>\nID: <color=#6ffcf3>{PhotonNetwork.LocalPlayer.UserId}</color>\nActor Number: <color=#6ffcf3>{PhotonNetwork.LocalPlayer.ActorNumber}</color>";
            textMesh.color = White;
            textMesh.fontSize = 17;
            textMesh.alignment = TextAlignment.Center;
            textMesh.anchor = TextAnchor.MiddleCenter;
            Textobj.transform.position = position;
            Textobj.transform.forward = GTPlayer.Instance.headCollider.transform.forward;
            Textobj.transform.localScale *= 0.1f;
            Object.Destroy(Textobj, Time.deltaTime);
        }

        public static void SnakeESP()
        {
            if (espColor == 1)
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
            else if (espColor == 2)
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
            else if (espColor == 3)
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
            else if (espColor == 4)
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
                        Color color = MenuColor;
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
        }//
        public static GameObject name;
        public static GameObject distance;
        public static float l;
        public static bool fps1;
    }
}

