using BepInEx;
using Elixir.Utilities;
using GorillaLocomotion;
using GorillaLocomotion.Swimming;
using Oculus.Interaction;
using UnityEngine;
using Valve.VR;
using static Elixir.Mods.Categories.Settings;
using static Elixir.Utilities.GunTemplate;
using static Elixir.Utilities.Inputs;
using static Elixir.Utilities.Variables;
using static Oculus.Interaction.Context;

namespace Elixir.Mods.Categories
{
    public class Move
    {
        //cha made this
        public static void WASDFly()
        {
            float currentSpeed = 5;
            Transform bodyTransform = Camera.main.transform;
            GorillaTagger.Instance.rigidbody.useGravity = false;
            GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
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
        public static void DroneFly()
        {
            GTPlayer instance = GTPlayer.Instance;

            GorillaTagger.Instance.rigidbody.useGravity = false;
            GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;

            Vector2 leftJoystickAxis = SteamVR_Actions.gorillaTag_LeftJoystick2DAxis.axis;
            float rightJoystickY = SteamVR_Actions.gorillaTag_RightJoystick2DAxis.axis.y;

            Vector3 right = instance.bodyCollider.transform.right;
            right.y = 0f;
            Vector3 forward = instance.bodyCollider.transform.forward;
            forward.y = 0f;

            Vector3 movement = new Vector3(leftJoystickAxis.x, rightJoystickY, leftJoystickAxis.y);
            Vector3 finalMovement = movement.x * right + movement.z * forward + rightJoystickY * Vector3.up;
            finalMovement *= instance.scale * flyspeedchangerspeed * 3f;

            Rigidbody rigidbody = instance.bodyCollider.attachedRigidbody;
            rigidbody.linearVelocity = Vector3.Lerp(rigidbody.linearVelocity, finalMovement, 0.1287f);
        }
        public static void Fly()
        {
            if (rightPrimary() | UnityInput.Current.GetKey(KeyCode.P))
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward * Time.deltaTime * flyspeedchangerspeed;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            }
        }
        public static void TriggerFly()
        {
            if (rightTrigger() | UnityInput.Current.GetKey(KeyCode.T))
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward * Time.deltaTime * flyspeedchangerspeed;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
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
                    GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                }
                else
                {
                    collider.enabled = true;
                }
            }
        }
        public static void Carmonkey()
        {
            Vector3 forward = GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward;
            forward.y = 0;
            forward.Normalize();

            if (rightTrigger() | UnityInput.Current.GetKey(KeyCode.T))
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += forward * Time.deltaTime * 25;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            }
            if (ControllerInputPoller.instance.leftControllerIndexFloat > 0.2f | UnityInput.Current.GetKey(KeyCode.Y))
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position -= forward * Time.deltaTime * 25;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            }
        }
        public static void MoonWalk()
        {
            GorillaTagger.Instance.rigidbody.AddForce(Vector3.up * 6.66f, ForceMode.Acceleration);
        }

        public static void NoGravity()
        {
            GorillaTagger.Instance.rigidbody.AddForce(-Physics.gravity, ForceMode.Acceleration);
        }

        public static void JupiterWalk()
        {
            GorillaTagger.Instance.rigidbody.AddForce(Vector3.down * 7.77f, ForceMode.Acceleration);
        }
        public static void Noclip()
        {
            DoNoclip(rightTrigger() || UnityInput.Current.GetKey(KeyCode.T));
        }
        public static void Speedboost()
        {
            GorillaLocomotion.GTPlayer.Instance.maxJumpSpeed = speedboostchangerspeed;
            GorillaLocomotion.GTPlayer.Instance.jumpMultiplier = speedboostchangerspeed - .5f;
        }
        public static void GSpeedboost()
        { 
            if (rightGrip())
            {
                GorillaLocomotion.GTPlayer.Instance.maxJumpSpeed = speedboostchangerspeed;
                GorillaLocomotion.GTPlayer.Instance.jumpMultiplier = speedboostchangerspeed - .5f;
            }
        }
        public static void Platforms(ref GameObject? platform, bool grabbing, Transform hand, bool invis, bool loong)
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
                    platform.GetComponent<Renderer>().material = ColorLib.MenuMat[0];
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
            Platforms(ref RP, ControllerInputPoller.instance.rightGrab, GTPlayer.Instance.RightHand.controllerTransform, invis, loong);
            Platforms(ref LP, ControllerInputPoller.instance.leftGrab, GTPlayer.Instance.LeftHand.controllerTransform, invis, loong);
        }

        public static void Checkpoint()
        {
            if (rightGrip())
            {
                if (!Ir)
                {
                    Pointy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    Pointy.GetComponent<Renderer>().material = ColorLib.MenuMat[0];
                    Pointy.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    Pointy.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                    Pointy.transform.rotation = GorillaTagger.Instance.rightHandTransform.rotation;
                    Ir = true;
                }
            }
            if (rightPrimary())
            {
                if (Pointy == null) return; 

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
                GorillaLocomotion.GTPlayer.Instance.transform.position = GunTemplate.spherepointer!.transform.position;
                GorillaTagger.Instance.transform.position = GunTemplate.spherepointer.transform.position;

                DoNoclip(true);
            }, false);
            { DoNoclip(false); }
        }

        public static void TagFreeze(bool b)
        {
            GorillaLocomotion.GTPlayer.Instance.disableMovement = b;
        }
        public static void CarMonkey()
        {
            Vector3 forward = GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward;
            forward.y = 0;
            forward.Normalize();

            if (rightTrigger() | UnityInput.Current.GetKey(KeyCode.T))
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += forward * Time.deltaTime * 25;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            }
            if (ControllerInputPoller.instance.leftControllerIndexFloat > 0.2f | UnityInput.Current.GetKey(KeyCode.Y))
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position -= forward * Time.deltaTime * 25;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            }
        }
        public static void UpAndDown()
        {
            if (rightTrigger() | UnityInput.Current.GetKey(KeyCode.T))
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += GorillaLocomotion.GTPlayer.Instance.bodyCollider.transform.up * Time.deltaTime * 15f;
            }
            if (ControllerInputPoller.instance.leftControllerIndexFloat > 0.2f | UnityInput.Current.GetKey(KeyCode.Y))
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position -= GorillaLocomotion.GTPlayer.Instance.bodyCollider.transform.up * Time.deltaTime * 15f;
            }
        }
        public static void TPPlayerGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position = LockedPlayer!.transform.position;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            }, true);
        }
        public static void HoverGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position = LockedPlayer!.transform.position + new Vector3(0f, 2.5f, 0f);
            }, true);
        }
        public static void IronMonkey()
        {
            if(rightGrip())
            {
                GorillaTagger.Instance.rigidbody.linearVelocity += GorillaTagger.Instance.rightHandTransform.gameObject.transform.forward * 8.5f * Time.deltaTime;
                GorillaTagger.Instance.StartVibration(false, 1f, 1f);
            }
            if (leftGrip())
            {
                GorillaTagger.Instance.rigidbody.linearVelocity += GorillaTagger.Instance.leftHandTransform.gameObject.transform.forward * 8.5f * Time.deltaTime;
                GorillaTagger.Instance.StartVibration(true, 1f, 1f);
            }
        }
        public static void SlingshotFly()
        {
            if (rightTrigger())
            {
                GorillaTagger.Instance.rigidbody.linearVelocity += GorillaTagger.Instance.headCollider.gameObject.transform.forward * 40 * Time.deltaTime;
            }
        }
        public static void HandFly()
        {
            if (rightPrimary())
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.transform.forward * Time.deltaTime * flyspeedchangerspeed;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            }
        }
        public static void VelocityFly()
        {
            if (rightPrimary() | UnityInput.Current.GetKey(KeyCode.P))
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward * Time.deltaTime * flyspeedchangerspeed;
            }
        }
        public static void Hertz(int hz)
        {
            Application.targetFrameRate = hz;
        }
        public static void Velocity(float d)
        {
            GorillaTagger.Instance.rigidbody.linearDamping = d;
        }
        public static void Swimminger(bool swim)
        {
            if (!swim)
            {
                if (swimmer)
                {
                    GameObject.Destroy(swimmer);
                    swimmer = null;
                    return;
                }
            }
            if (swimmer == null)
            {
                GameObject WhenBritishPeopleSayBottleOfWaterItSoundsFunny = GameObject.Find("Environment Objects/LocalObjects_Prefab/ForestToBeach/ForestToBeach_Prefab_V4/ForestToBeach_Geo/CaveWaterVolume/");

                if (WhenBritishPeopleSayBottleOfWaterItSoundsFunny != null)
                {
                    WhenBritishPeopleSayBottleOfWaterItSoundsFunny.SetActive(true);

                    swimmer = GameObject.Instantiate(WhenBritishPeopleSayBottleOfWaterItSoundsFunny);
                    swimmer.transform.localScale = new Vector3(.5f, 1f, .5f);
                }
            }
            swimmer!.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + new Vector3(0f, 2.5f, 0f);
        }
        public static void SuperMonke()
        {
            Vector3 right = GTPlayer.Instance.bodyCollider.transform.right;
            right.y = 0f;
            Vector3 forward = GTPlayer.Instance.bodyCollider.transform.forward;
            forward.y = 0f;

            if (rightPrimary() | UnityInput.Current.GetKey(KeyCode.P))
            {
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                GorillaLocomotion.GTPlayer.Instance.transform.position += GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward * Time.deltaTime * flyspeedchangerspeed;
            }
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
                        GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().linearVelocity += GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward * 12f;
                    }
                    else
                    {
                        Vector3 flinger = Random.onUnitSphere;
                        flinger.y = Mathf.Clamp(flinger.y, 0.5f, 1.5f);
                        GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().linearVelocity += flinger.normalized * 12f;
                    }
                }
            }
        }
        public static void Slippyyyyyyy(int slippery)
        {
            GTPlayer.Instance.currentOverride.slidePercentageOverride = slippery;
        }

        static float dashDelay;

        static Vector3 oldMousePos;

        static GameObject? RP;
        static GameObject? LP;

        static GameObject? swimmer;

        static GameObject? Pointy;
        static bool Ir = false;
    }
}
