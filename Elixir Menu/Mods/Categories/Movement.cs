using static Elixir.Utilities.GunTemplate;
using static Elixir.Menu.Main;
using static Elixir.Mods.Categories.Settings;
using UnityEngine;
using BepInEx;
using Elixir.Utilities;
using static Elixir.Utilities.Inputs;
using static Elixir.Utilities.Variables;
using Elixir.Menu;
using Oculus.Interaction;

namespace Elixir.Mods.Categories
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
        public static void Platforms(ref GameObject platform, bool grabbing, Transform hand, bool invis, bool loong)
        {
            if (grabbing)
            {
                if (!platform)
                {
                    platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    platform.transform.localScale = loong ? new Vector3(0.80f, 0.015f, 0.28f) : new Vector3(0.28f, 0.015f, 0.28f);
                    platform.transform.position = hand.position + new Vector3(0f, -0.02f, 0f);
                    platform.transform.rotation = hand.rotation * Quaternion.Euler(0f, 0f, -90f);
                    platform.GetComponent<Renderer>().material.shader = Shader.Find("UI/Default");
                    platform.GetComponent<Renderer>().material = Variables.background.GetComponent<Renderer>().material;
                    if (invis) { platform.GetComponent<Renderer>().enabled = false; }
                }
            }
            else
            {
                if (platform)
                {
                    Object.Destroy(platform);
                    platform = null;
                }
            }
        }

        public static void Platforms(bool invis, bool loong)
        {
            Platforms(ref RP, ControllerInputPoller.instance.rightGrab, Variables.playerInstance.rightControllerTransform, invis, loong);
            Platforms(ref LP, ControllerInputPoller.instance.leftGrab, Variables.playerInstance.leftControllerTransform, invis, loong);
        }

        public static void Checkpoint()
        {
            if (rightGrip())
            {
                if (!Ir)
                {
                    Pointy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    Pointy.GetComponent<Renderer>().material = ColorLib.MenuMat[Theme-1];
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
                Gravity(9.81f);
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
                GorillaLocomotion.GTPlayer.Instance.transform.position += GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward * Time.deltaTime * flyspeedchangerspeed;
            }
            else { GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().useGravity = true; }
        }
        public static void DashJump(bool Dash)
        {
            if (rightPrimary() | UnityInput.Current.GetKey(KeyCode.P))
            {
                if (Time.time > dashDelay)
                {
                    dashDelay = Time.time + 2.5f;
                    if (Dash)
                    {
                        GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward * 12f;
                    }
                    else
                    {
                        Vector3 flinger = Random.onUnitSphere;
                        flinger.y = Mathf.Clamp(flinger.y, 0.5f, 1.5f);
                        GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += flinger.normalized * 12f;
                    }
                }
            }
        }

        static float dashDelay;

        static Vector3 oldMousePos;

        static GameObject RP;
        static GameObject LP;

        static GameObject Pointy;
        static bool Ir = false;
    }
}