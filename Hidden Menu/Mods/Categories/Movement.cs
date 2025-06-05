using System;
using System.Collections.Generic;
using System.Text;
using static Hidden.Utilities.GunTemplate;
using static Hidden.Menu.Main;
using static Hidden.Utilities.Variables;
using static Hidden.Utilities.ColorLib;
using static Hidden.Utilities.RigManager;
using static Hidden.Menu.ButtonHandler;
using static Hidden.Mods.ModButtons;
using static Hidden.Mods.Categories.Settings;
using UnityEngine;
using Valve.VR;
using System.Reflection;
using BepInEx;
using Photon.Voice;
using Hidden.Utilities;
using static Hidden.Utilities.ControllerInputs;
using UnityEngine.UIElements;
using Hidden.Menu;

namespace Hidden.Mods.Categories
{
    public class Move
    {
        public static void WASDFly()
        {
            float currentSpeed = 5;
            Transform bodyTransform = Camera.main.transform;
            GorillaTagger.Instance.rigidbody.useGravity = false;
            GorillaTagger.Instance.rigidbody.velocity = Vector3.zero;
            if (UnityInput.Current.GetKey(KeyCode.LeftShift))
            {
                currentSpeed *= 2.5f;
            }
            if (UnityInput.Current.GetKey(KeyCode.W) || UnityInput.Current.GetKey(KeyCode.UpArrow))
            {
                bodyTransform.position += bodyTransform.forward * currentSpeed * Time.deltaTime;
            }
            if (UnityInput.Current.GetKey(KeyCode.A) || UnityInput.Current.GetKey(KeyCode.LeftArrow))
            {
                bodyTransform.position += -bodyTransform.right * currentSpeed * Time.deltaTime;
            }
            if (UnityInput.Current.GetKey(KeyCode.S) || UnityInput.Current.GetKey(KeyCode.DownArrow))
            {
                bodyTransform.position += -bodyTransform.forward * currentSpeed * Time.deltaTime;
            }
            if (UnityInput.Current.GetKey(KeyCode.D) || UnityInput.Current.GetKey(KeyCode.RightArrow))
            {
                bodyTransform.position += bodyTransform.right * currentSpeed * Time.deltaTime;
            }
            if (UnityInput.Current.GetKey(KeyCode.Space))
            {
                bodyTransform.position += bodyTransform.up * currentSpeed * Time.deltaTime;
            }
            if (UnityInput.Current.GetKey(KeyCode.LeftControl))
            {
                bodyTransform.position += -bodyTransform.up * currentSpeed * Time.deltaTime;
            }
            if (UnityInput.Current.GetMouseButton(1))
            {
                Vector3 pos = UnityInput.Current.mousePosition - oldMousePos;
                float x = bodyTransform.localEulerAngles.x - pos.y * 0.3f;
                float y = bodyTransform.localEulerAngles.y + pos.x * 0.3f;
                bodyTransform.localEulerAngles = new Vector3(x, y, 0f);
            }
            oldMousePos = UnityInput.Current.mousePosition;
        }
        public static void Fly()
        {
            if (rightPrimary() | UnityInput.Current.GetKey(KeyCode.P))
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward * Time.deltaTime * flyspeedchangerspeed;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
        public static void TriggerFly()
        {
            if (rightTrigger() | UnityInput.Current.GetKey(KeyCode.T))
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward * Time.deltaTime * flyspeedchangerspeed;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
        public static void NoclipFly()
        {
            foreach (MeshCollider collider in Resources.FindObjectsOfTypeAll<MeshCollider>())
            {
                if (rightPrimary() | UnityInput.Current.GetKey(KeyCode.P))
                {
                    collider.enabled = false;
                    GorillaLocomotion.GTPlayer.Instance.transform.position += GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward * Time.deltaTime * flyspeedchangerspeed;
                    GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
                else
                {
                    collider.enabled = true;
                }
            }
        }
        public static void carmonkey()
        {
            Vector3 forward = GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward;
            forward.y = 0;
            forward.Normalize();

            if (rightTrigger() | UnityInput.Current.GetKey(KeyCode.T))
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += forward * Time.deltaTime * 25;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            if (ControllerInputPoller.instance.leftControllerIndexFloat > 0.2f | UnityInput.Current.GetKey(KeyCode.Y))
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position -= forward * Time.deltaTime * 25;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
        public static void Gravity(float g)
        {
            GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.up * (Time.deltaTime * (g / Time.deltaTime)), ForceMode.Acceleration);
        }
        public static void JupiterWalk()
        {
            GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.down * (Time.deltaTime * (8f / Time.deltaTime)), ForceMode.Acceleration);
        }
        public static void Noclip()
        {
            foreach (MeshCollider collider in Resources.FindObjectsOfTypeAll<MeshCollider>())
            {
                if (rightTrigger() | UnityInput.Current.GetKey(KeyCode.T))
                {
                    collider.enabled = false;
                }
                else
                {
                    collider.enabled = true;
                }
            }
        }
        public static void Speedboost()
        {
            GorillaLocomotion.GTPlayer.Instance.maxJumpSpeed = speedboostchangerspeed;
            GorillaLocomotion.GTPlayer.Instance.jumpMultiplier = speedboostchangerspeed + .5f;
        }
        public static void Platforms()
        {
            if (rightGrip())
            {
                if (!RPA)
                {
                    RP = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    RP.GetComponent<Renderer>().material.shader = Shader.Find("UI/Default");
                    RP.GetComponent<Renderer>().material.color = MenuColorT;
                    Outline(RP, outColor);
                    RP.transform.rotation = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.rotation;
                    RP.transform.localScale = new Vector3(0.01f, 0.3f, 0.4f);
                    RP.transform.position = GorillaTagger.Instance.rightHandTransform.position - Vector3.up * 0.045f;
                    RPA = true;
                }
            }
            else
            {
                GameObject.Destroy(RP);
                RPA = false;
            }

            if (ControllerInputPoller.instance.leftGrab)
            {
                if (!LPA)
                {
                    LP = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    LP.GetComponent<Renderer>().material.shader = Shader.Find("UI/Default");
                    LP.GetComponent<Renderer>().material.color = MenuColorT;
                    Outline(LP, outColor);
                    LP.transform.rotation = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.rotation;
                    LP.transform.localScale = new Vector3(0.01f, 0.3f, 0.4f);
                    LP.transform.position = GorillaTagger.Instance.leftHandTransform.position - Vector3.up * 0.045f; ;
                    LPA = true;
                }
            }
            else
            {
                GameObject.Destroy(LP);
                LPA = false;
            }
        }
        public static void StickyPlatforms()
        {
            if (rightGrip())
            {
                if (!RPA2)
                {
                    RP1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    RP1.GetComponent<Renderer>().material.shader = Shader.Find("UI/Default");
                    RP1.GetComponent<Renderer>().material.color = MenuColorT;
                    RP1.transform.rotation = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.rotation;
                    RP1.transform.localScale = new Vector3(0.01f, 0.3f, 0.4f);
                    RP1.transform.position = GorillaTagger.Instance.rightHandTransform.position - Vector3.up * 0.045f;

                    RP2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    RP2.GetComponent<Renderer>().material.shader = Shader.Find("UI/Default");
                    RP2.GetComponent<Renderer>().material.color = MenuColorT;
                    RP2.transform.rotation = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.rotation;
                    RP2.transform.localScale = new Vector3(0.01f, 0.3f, 0.4f);
                    RP2.transform.position = GorillaTagger.Instance.rightHandTransform.position + Vector3.up * 0.045f;
                    RPA2 = true;
                    Outline(RP1, outColor);
                    Outline(RP2, outColor);
                }
            }
            else
            {
                GameObject.Destroy(RP1);
                GameObject.Destroy(RP2);
                RPA2 = false;
            }

            if (ControllerInputPoller.instance.leftGrab)
            {
                if (!LPA2)
                {
                    LP1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    LP1.GetComponent<Renderer>().material.shader = Shader.Find("UI/Default");
                    LP1.GetComponent<Renderer>().material.color = MenuColorT;
                    LP1.transform.rotation = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.rotation;
                    LP1.transform.localScale = new Vector3(0.01f, 0.3f, 0.4f);
                    LP1.transform.position = GorillaTagger.Instance.leftHandTransform.position - Vector3.up * 0.045f;

                    LP2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    LP2.GetComponent<Renderer>().material.shader = Shader.Find("UI/Default");
                    LP2.GetComponent<Renderer>().material.color = MenuColorT;
                    LP2.transform.rotation = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.rotation;
                    LP2.transform.localScale = new Vector3(0.01f, 0.3f, 0.4f);
                    LP2.transform.position = GorillaTagger.Instance.leftHandTransform.position + Vector3.up * 0.045f;
                    LPA2 = true;
                    Outline(LP1, outColor);
                    Outline(LP2, outColor);
                }
            }
            else
            {
                GameObject.Destroy(LP1);
                GameObject.Destroy(LP2);
                LPA2 = false;
            }
        }
        public static void InvisPlatforms()
        {
            if (rightGrip())
            {
                if (!RPA)
                {
                    RP = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    UnityEngine.Object.Destroy(RP.GetComponent<Renderer>());
                    RP.transform.rotation = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.rotation;
                    RP.transform.localScale = new Vector3(0.01f, 0.3f, 0.4f);
                    RP.transform.position = GorillaTagger.Instance.rightHandTransform.position - Vector3.up * 0.045f;
                    RPA = true;
                }
            }
            else
            {
                GameObject.Destroy(RP);
                RPA = false;
            }

            if (ControllerInputPoller.instance.leftGrab)
            {
                if (!LPA)
                {
                    LP = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    UnityEngine.Object.Destroy(RP.GetComponent<Renderer>());
                    LP.transform.rotation = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.rotation;
                    LP.transform.localScale = new Vector3(0.01f, 0.3f, 0.4f);
                    LP.transform.position = GorillaTagger.Instance.leftHandTransform.position - Vector3.up * 0.045f; ;
                    LPA = true;
                }
            }
            else
            {
                GameObject.Destroy(LP);
                LPA = false;
            }
        }
        public static void Checkpoint()
        {
            if (rightGrip())
            {
                if (!Ir)
                {
                    Pointy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    Pointy.GetComponent<Renderer>().material.color = MenuColor;
                    Pointy.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    Pointy.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                    Pointy.transform.rotation = GorillaTagger.Instance.rightHandTransform.rotation;
                    Ir = true;
                }
            }
            if (rightPrimary())
            {
                GorillaTagger.Instance.transform.position = Pointy.transform.position;
                GorillaLocomotion.GTPlayer.Instance.transform.position = Pointy.transform.position;
            }
            if (rightTrigger())
            {
                Ir = false;
                GameObject.Destroy(Pointy);
            }
        }
        public static void TPGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position = GunTemplate.spherepointer.transform.position;
                GorillaTagger.Instance.transform.position = GunTemplate.spherepointer.transform.position;
                GameObject.Destroy(spherepointer, Time.deltaTime);
            }, false);
        }

        public static void NoTagFreeze()
        {
            GorillaLocomotion.GTPlayer.Instance.disableMovement = false;
        }
        public static void TagFreeze()
        {
            foreach (ButtonHandler.Button button in ModButtons.buttons)
            {
                if (button.buttonText == "No Tag Freeze")
                {
                    button.Enabled = false;
                    GorillaLocomotion.GTPlayer.Instance.disableMovement = true;
                }
            }
        }
        public static void CarMonkey()
        {
            Vector3 forward = GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward;
            forward.y = 0;
            forward.Normalize();

            if (rightTrigger() | UnityInput.Current.GetKey(KeyCode.T))
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += forward * Time.deltaTime * 25;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            if (ControllerInputPoller.instance.leftControllerIndexFloat > 0.2f | UnityInput.Current.GetKey(KeyCode.Y))
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position -= forward * Time.deltaTime * 25;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
        public static void UpAndDown()
        {
            if (rightTrigger() | UnityInput.Current.GetKey(KeyCode.T))
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += GorillaLocomotion.GTPlayer.Instance.headCollider.transform.up * Time.deltaTime * 15f;
            }
            if (ControllerInputPoller.instance.leftControllerIndexFloat > 0.2f | UnityInput.Current.GetKey(KeyCode.Y))
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position -= GorillaLocomotion.GTPlayer.Instance.headCollider.transform.up * Time.deltaTime * 15f;
            }
        }
        public static void TPPlayerGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position = LockedPlayer.transform.position;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }, true);
        }
        public static void HoverGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position = LockedPlayer.transform.position + new Vector3(0f, 2.5f, 0f);
            }, true);
        }
        public static void IronMonkey()
        {
            if(rightGrip())
            {
                GorillaTagger.Instance.rigidbody.velocity += GorillaTagger.Instance.rightHandTransform.gameObject.transform.forward * 8.5f * Time.deltaTime;
                GorillaTagger.Instance.StartVibration(false, 1f, 1f);
            }
            if (leftGrip())
            {
                GorillaTagger.Instance.rigidbody.velocity += GorillaTagger.Instance.leftHandTransform.gameObject.transform.forward * 8.5f * Time.deltaTime;
                GorillaTagger.Instance.StartVibration(true, 1f, 1f);
            }
        }
        public static void SlingshotFly()
        {
            if (rightTrigger())
            {
                GorillaTagger.Instance.rigidbody.velocity += GorillaTagger.Instance.headCollider.gameObject.transform.forward * 40 * Time.deltaTime;
            }
        }
        public static void HandFly()
        {
            if (rightPrimary())
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.transform.forward * Time.deltaTime * flyspeedchangerspeed;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
        public static void Hertz(int hz)
        {
            Application.targetFrameRate = hz;
        }
        public static void PunchMod()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    float dis1 = Vector3.Distance(vrrig.rightHandTransform.position, GorillaTagger.Instance.offlineVRRig.bodyTransform.position);
                    float dis2 = Vector3.Distance(vrrig.leftHandTransform.position, GorillaTagger.Instance.offlineVRRig.bodyTransform.position);

                    if (dis1 < 0.2f)
                    {
                        GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += vrrig.rightHandTransform.forward * 10f;
                    }
                    if (dis2 < 0.2f)
                    {
                        GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += vrrig.leftHandTransform.forward * 10f;
                    }
                }
            }
        }
        public static void Velocity(float d)
        {
            GorillaTagger.Instance.rigidbody.drag = d;
        }
        public static void SuperMonke()
        {
            if (rightPrimary() | UnityInput.Current.GetKey(KeyCode.P))
            {
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().useGravity = false;
                GorillaLocomotion.GTPlayer.Instance.transform.position += GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward * Time.deltaTime * flyspeedchangerspeed;
            }
            else { GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().useGravity = true; }
        }

        static Vector3 oldMousePos;

        static bool RPA = false;
        static bool LPA = false;
        static GameObject RP;
        static GameObject LP;

        static bool RPA2 = false;
        static bool LPA2 = false;
        static GameObject RP1;
        static GameObject LP1;
        static GameObject RP2;
        static GameObject LP2;

        static GameObject Pointy;
        static bool Ir = false;
    }
}

public static void Leap()
		{
			bool rightControllerSecondaryButton = ControllerInputPoller.instance.rightControllerSecondaryButton;
			if (rightControllerSecondaryButton)
			{
				GTPlayer instance = GTPlayer.Instance;
				Transform transform = instance.bodyCollider.transform;
				Vector3 velocity = transform.forward * 10f;
				instance.bodyCollider.attachedRigidbody.velocity = velocity;
			}
		}