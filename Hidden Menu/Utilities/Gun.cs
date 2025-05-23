using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Hidden.Utilities;
using static Hidden.Utilities.ColorLib;
using static Hidden.Mods.Categories.Settings;
using BepInEx;
using static Hidden.Menu.Main;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using System.Reflection;

namespace Hidden.Utilities
{
    public class ClientInput
    {
        public static bool GetInputValue(float grabValue)
        {
            return grabValue >= 0.75f;
        }
    }
    public class GunTemplate : MonoBehaviour
    {
        public static int LineCurve = 150;
        private const float PointerScale = 0.15f;
        private const float LineWidth = 0.025f;
        private const float LineSmoothFactor = 6f;
        private const float DestroyDelay = 0.02f;
        private const float PulseSpeed = 2f;
        private const float PulseAmplitude = 0.03f;

        public static GameObject spherepointer;
        public static VRRig LockedPlayer;
        public static Vector3 lr;
        public static Color32 TriggeredPointerColor = new Color32(53, 0, 0, 255);
        public static Color32 TriggeredLineColor = new Color32(53, 0, 0, 255);

        public static RaycastHit raycastHit;
        public static void StartVrGun(Action action, bool LockOn)
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                Physics.Raycast(GorillaTagger.Instance.rightHandTransform.position, -GorillaTagger.Instance.rightHandTransform.up, out raycastHit, float.MaxValue);
                if (spherepointer == null && gunSetting !=3)
                {
                    spherepointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    spherepointer.AddComponent<Renderer>();
                    spherepointer.transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
                    spherepointer.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                    GameObject.Destroy(spherepointer.GetComponent<BoxCollider>());
                    GameObject.Destroy(spherepointer.GetComponent<Rigidbody>());
                    GameObject.Destroy(spherepointer.GetComponent<Collider>());
                    lr = GorillaTagger.Instance.offlineVRRig.rightHandTransform.position;
                }
                if (gunSetting == 3)
                {
                    Destroy(spherepointer.GetComponent<Renderer>());
                }
                if (LockedPlayer == null)
                {
                    spherepointer.transform.position = raycastHit.point;
                    spherepointer.GetComponent<Renderer>().material.color = DarkGrey;
                }
                else
                {
                    spherepointer.transform.position = LockedPlayer.transform.position;
                }
                lr = Vector3.Lerp(lr, (GorillaTagger.Instance.rightHandTransform.position + spherepointer.transform.position) / 2f, Time.deltaTime * 6f);
                GameObject gameObject = new GameObject("Line");
                LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
                lineRenderer.startWidth = 0.022f;
                lineRenderer.endWidth = 0.022f;
                lineRenderer.startColor = Color.black;
                lineRenderer.endColor = DarkGrey;
                lineRenderer.useWorldSpace = true;
                lineRenderer.material = new Material(Shader.Find("GUI/Text Shader"));
                if (gunSetting != 2)
                {
                    lineRenderer.SetPositions(new Vector3[] { GorillaTagger.Instance.rightHandTransform.position, spherepointer.transform.position });
                }
                GameObject.Destroy(lineRenderer, Time.deltaTime);
                if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f)
                {
                    trigger = true;
                    lineRenderer.startColor = TriggeredLineColor;
                    lineRenderer.endColor = TriggeredLineColor;
                    spherepointer.GetComponent<Renderer>().material.color = TriggeredPointerColor;
                    if (LockOn)
                    {
                        if (LockedPlayer == null)
                        {
                            LockedPlayer = raycastHit.collider.GetComponentInParent<VRRig>();
                        }
                        if (LockedPlayer != null)
                        {
                            spherepointer.transform.position = LockedPlayer.transform.position;
                            action();
                        }
                        return;
                    }
                    action();
                    return;
                }
                else if (LockedPlayer != null)
                {
                    LockedPlayer = null;
                    return;
                }
            }
            else if (spherepointer != null)
            {
                GameObject.Destroy(spherepointer);
                spherepointer = null;
                LockedPlayer = null;
            }
        }

        public static void StartPcGun(Action action, bool LockOn)
        {
            Ray ray = GameObject.Find("Shoulder Camera").activeSelf ? GameObject.Find("Shoulder Camera").GetComponent<Camera>().ScreenPointToRay(UnityInput.Current.mousePosition) : GorillaTagger.Instance.mainCamera.GetComponent<Camera>().ScreenPointToRay(UnityInput.Current.mousePosition);
            if (Mouse.current.rightButton.isPressed)
            {
                RaycastHit raycastHit;
                if (Physics.Raycast(ray.origin, ray.direction, out raycastHit, float.PositiveInfinity, -32777) && spherepointer == null)
                {
                    if (spherepointer == null && gunSetting != 3)
                    {
                        spherepointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        spherepointer.AddComponent<Renderer>();
                        spherepointer.transform.localScale = gunSetting == 3 ? new Vector3(0.0001f, 0.0001f, 0.0001f) : new Vector3(0.12f, 0.12f, 0.12f);
                        spherepointer.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                        GameObject.Destroy(spherepointer.GetComponent<BoxCollider>());
                        GameObject.Destroy(spherepointer.GetComponent<Rigidbody>());
                        GameObject.Destroy(spherepointer.GetComponent<Collider>());
                        lr = GorillaTagger.Instance.offlineVRRig.rightHandTransform.position;
                    }
                    if (gunSetting == 3)
                    {
                        Destroy(spherepointer.GetComponent<Renderer>());
                    }
                }
                if (LockedPlayer == null)
                {
                    spherepointer.transform.position = raycastHit.point;
                    spherepointer.GetComponent<Renderer>().material.color = DarkGrey;
                }
                else
                {
                    spherepointer.transform.position = LockedPlayer.transform.position;
                }
                lr = Vector3.Lerp(lr, (GorillaTagger.Instance.rightHandTransform.position + spherepointer.transform.position) / 2f, Time.deltaTime * 6f);
                GameObject gameObject = new GameObject("Line");
                LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
                lineRenderer.startWidth = 0.022f;
                lineRenderer.endWidth = 0.022f;
                lineRenderer.startColor = Color.black;
                lineRenderer.endColor = DarkGrey;
                lineRenderer.useWorldSpace = true;
                lineRenderer.material = new Material(Shader.Find("GUI/Text Shader"));
                if (gunSetting != 2)
                {
                    lineRenderer.SetPositions(new Vector3[] { GorillaTagger.Instance.headCollider.transform.position, spherepointer.transform.position });
                }
                GameObject.Destroy(lineRenderer, Time.deltaTime);
                if (Mouse.current.leftButton.isPressed)
                {
                    trigger = true;
                    lineRenderer.startColor = TriggeredLineColor;
                    lineRenderer.endColor = TriggeredLineColor;
                    spherepointer.GetComponent<Renderer>().material.color = TriggeredPointerColor;
                    if (LockOn)
                    {
                        if (LockedPlayer == null)
                        {
                            LockedPlayer = raycastHit.collider.GetComponentInParent<VRRig>();
                        }
                        if (LockedPlayer != null)
                        {
                            spherepointer.transform.position = LockedPlayer.transform.position;
                            action();
                        }
                        return;
                    }
                    action();
                    return;
                }
                else if (LockedPlayer != null)
                {
                    LockedPlayer = null;
                    return;
                }
            }
            else if (spherepointer != null)
            {
                GameObject.Destroy(spherepointer);
                spherepointer = null;
                LockedPlayer = null;
            }
        }

        public static void StartBothGuns(Action action, bool locko)
        {
            if (XRSettings.isDeviceActive)
            {
                StartVrGun(action, locko);
            }
            if (!XRSettings.isDeviceActive)
            {
                StartPcGun(action, locko);
            }
        }
        public static bool trigger = false;
        
    }

}
