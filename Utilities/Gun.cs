using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Elixir.Utilities;
using static Elixir.Utilities.ColorLib;
using static Elixir.Mods.Categories.Settings;
using BepInEx;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using static Elixir.Utilities.Variables;

namespace Elixir.Utilities
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
        public static GameObject? spherepointer;
        public static VRRig? LockedPlayer;
        public static Vector3 lr;
        public static Color32 TriggeredPointerColor = new Color(0.15f, 0.00f, 0.28f, 1f);
        public static Color32 TriggeredLineColor = new Color(0.15f, 0.00f, 0.28f, 1f);

        public static RaycastHit raycastHit;
        
        private static Vector3 CalculateBezierPoint(Vector3 start, Vector3 mid, Vector3 end, float t) =>
            (1f - t) * (1f - t) * start + 2f * (1f - t) * t * mid + t * t * end;

        public static void CurveLineRenderer(LineRenderer lr, Vector3 starte, Vector3 mid, Vector3 ende, int positioncount = 150)
        {
            lr.positionCount = positioncount;
            lr.positionCount = positioncount;
            for (int i = 0; i < positioncount; i++)
            {
                float t = (float)i / (positioncount - 1);
                Vector3 vector = CalculateBezierPoint(starte, mid, ende, t);
                lr.SetPosition(i, vector);
            }
        }

        private static void CreatePointer()
        {
            if (spherepointer != null || gunSetting == 3)
                return;

            spherepointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            spherepointer.transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
            spherepointer.AddComponent<Renderer>();
            spherepointer.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");

            GameObject.Destroy(spherepointer.GetComponent<BoxCollider>());
            GameObject.Destroy(spherepointer.GetComponent<Rigidbody>());
            GameObject.Destroy(spherepointer.GetComponent<Collider>());

            lr = GorillaTagger.Instance.offlineVRRig.rightHandTransform.position;
        }

        private static void RemovePointer()
        {
            if (spherepointer == null)
                return;

            GameObject.Destroy(spherepointer);
            spherepointer = null;
            LockedPlayer = null;
            trigger = false;
        }

        private static void DrawLine(Vector3 start)
        {
            lr = Vector3.Lerp(lr, (start + spherepointer.transform.position) / 2f, Time.deltaTime * 6f);

            var obj = new GameObject("Line");
            var line = obj.AddComponent<LineRenderer>();

            line.startWidth = 0.022f;
            line.endWidth = 0.022f;
            line.startColor = Color.black;
            line.endColor = Indigo;
            if(trigger)
            {
                line.endColor = TriggeredLineColor;
            }
            line.useWorldSpace = true;
            line.material = new Material(Shader.Find("GUI/Text Shader"));

            CurveLineRenderer(line, start, lr, spherepointer.transform.position);
            GameObject.Destroy(line, Time.deltaTime);
        }

        public static void StartVrGun(Action action, bool lockOn, Action disableAction = null)
        {
            if (!ControllerInputPoller.instance.rightGrab)
            {
                RemovePointer();
                return;
            }

            Physics.Raycast(GorillaTagger.Instance.rightHandTransform.position, -GorillaTagger.Instance.rightHandTransform.up, out raycastHit, float.MaxValue);

            CreatePointer();

            if (gunSetting == 3 && spherepointer != null)
                GameObject.Destroy(spherepointer.GetComponent<Renderer>());

            spherepointer.transform.position = LockedPlayer ? LockedPlayer.transform.position : raycastHit.point;

            if (LockedPlayer == null)
                spherepointer.GetComponent<Renderer>().material.color = Indigo;

            DrawLine(GorillaTagger.Instance.rightHandTransform.position);

            if (!ControllerInputPoller.instance.rightControllerIndexFloat.Equals(0f) && ControllerInputPoller.instance.rightControllerIndexFloat > .5f)
            {
                trigger = true;
                spherepointer.GetComponent<Renderer>().material.color = TriggeredPointerColor;

                if (lockOn)
                {
                    LockedPlayer ??= raycastHit.collider.GetComponentInParent<VRRig>();

                    if (LockedPlayer != null)
                    {
                        spherepointer.transform.position = LockedPlayer.transform.position;
                        action();
                    }

                    return;
                }
                else
                {
                    action();
                }
                
                return;
            }

            trigger = false;
            LockedPlayer = null;
            disableAction?.Invoke();
        }

        public static void StartPcGun(Action action, bool lockOn, Action disableAction = null)
        {
            Camera cam = GameObject.Find("Shoulder Camera").activeSelf
                ? GameObject.Find("Shoulder Camera").GetComponent<Camera>()
                : GorillaTagger.Instance.mainCamera.GetComponent<Camera>();

            Ray ray = cam.ScreenPointToRay(UnityInput.Current.mousePosition);

            if (!Mouse.current.rightButton.isPressed)
            {
                RemovePointer();
                return;
            }

            if (!Physics.Raycast(ray, out raycastHit, float.PositiveInfinity, -32777))
                return;

            CreatePointer();

            if (gunSetting == 3 && spherepointer != null)
                GameObject.Destroy(spherepointer.GetComponent<Renderer>());

            spherepointer.transform.position = LockedPlayer ? LockedPlayer.transform.position : raycastHit.point;

            if (LockedPlayer == null)
                spherepointer.GetComponent<Renderer>().material.color = Indigo;

            DrawLine(GorillaTagger.Instance.headCollider.transform.position);

            if (Mouse.current.leftButton.isPressed)
            {
                trigger = true;
                spherepointer.GetComponent<Renderer>().material.color = TriggeredPointerColor;

                if (lockOn)
                {
                    LockedPlayer ??= raycastHit.collider.GetComponentInParent<VRRig>();

                    if (LockedPlayer != null)
                    {
                        spherepointer.transform.position = LockedPlayer.transform.position;
                        action();
                    }

                    return;
                }
                else
                {
                    action();
                }
                return;
            }

            trigger = false;
            LockedPlayer = null;
            disableAction?.Invoke();
        }

        public static void StartBothGuns(Action action, bool locko, Action disableAction = null)
        {
            if (XRSettings.isDeviceActive)
            {
                StartVrGun(action, locko, disableAction);
            }
            if (!XRSettings.isDeviceActive)
            {
                StartPcGun(action, locko, disableAction);
            }
        }
        public static bool trigger = false;

    }

}