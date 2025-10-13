using BepInEx;
using ExitGames.Client.Photon;
using Elixir.Menu;
using Elixir.Utilities;
using Photon.Pun;
using System;
using UnityEngine;
using static Elixir.Menu.Main;
using static Elixir.Utilities.ColorLib;
using static Elixir.Utilities.GunTemplate;
using static Elixir.Utilities.Variables;
using Photon.Realtime;

namespace Elixir.Mods.Categories
{
    public class Fun
    {
        #region bug bat ball
        public static void CopySelfID()
        {
            string id = PhotonNetwork.LocalPlayer.UserId;
            GUIUtility.systemCopyBuffer = id;
        }
        public static void CopyIDGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                string id = LockedPlayer!.Creator.UserId;
                GUIUtility.systemCopyBuffer = id;
            }, true);
        }
        public static void GrabBug()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                Bug.transform.position = GorillaTagger.Instance.rightHandTransform.position;
            }
            if (ControllerInputPoller.instance.leftGrab)
            {
                Bug.transform.position = GorillaTagger.Instance.leftHandTransform.position;
            }
        }
        public static void BugGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                Bug.transform.position = GunTemplate.spherepointer!.transform.position;
            }, false);
        }
        public static void GrabBat()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                Bat.transform.position = GorillaTagger.Instance.rightHandTransform.position;
            }
            if (ControllerInputPoller.instance.leftGrab)
            {
                Bat.transform.position = GorillaTagger.Instance.leftHandTransform.position;
            }
        }
        public static void BatGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                Bat.transform.position = GunTemplate.spherepointer!.transform.position;
            }, false);
        }
        public static void SnipeBug()
        {
            GorillaTagger.Instance.rightHandTransform.transform.position = Bug.transform.position;
        }
        public static void SnipeBat()
        {
            GorillaTagger.Instance.rightHandTransform.transform.position = Bat.transform.position;
        }
        public static void GrabSBall()
        {
            if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                SBall.transform.position = GorillaTagger.Instance.rightHandTransform.position;
            }
            if (ControllerInputPoller.instance.leftGrab)
            {
                SBall.transform.position = GorillaTagger.Instance.leftHandTransform.position;
            }
        }
        public static void SBallGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                SBall.transform.position = GunTemplate.spherepointer!.transform.position;
            }, false);
        }
        public static void SnipeSBall()
        {
            GorillaTagger.Instance.rightHandTransform.transform.position = SBall.transform.position;
        }
        public static void SBallHalo()
        {
            SBall.transform.position = GorillaTagger.Instance.headCollider.transform.position + new Vector3(MathF.Cos((float)Time.frameCount / 30), 1f, MathF.Sin((float)Time.frameCount / 30));
            SBall.transform.rotation = Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360)));
        }
        public static void BugHalo()
        {
            Bug.transform.position = GorillaTagger.Instance.headCollider.transform.position + new Vector3(MathF.Cos((float)Time.frameCount / 30), 1f, MathF.Sin((float)Time.frameCount / 30));
            Bug.transform.rotation = Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360)));
        }
        public static void BatHalo()
        {
            Bat.transform.position = GorillaTagger.Instance.headCollider.transform.position + new Vector3(MathF.Cos((float)Time.frameCount / 30), 1f, MathF.Sin((float)Time.frameCount / 30));
            Bat.transform.rotation = Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360)));
        }

        public static GameObject Bat = GameObject.Find("Cave Bat Holdable");
        public static GameObject Bug = GameObject.Find("Floating Bug Holdable");
        public static GameObject SBall = GameObject.Find("GameBall");
        #endregion

        #region fun spammers cs
        public static void Spam1()
        {
            if (pollerInstance == null) return;

            if (pollerInstance.rightGrab)
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.position;
                orb.GetComponent<Renderer>().material.color = SkyBlue;
                GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/Terrain/pitgeo/pit ground").layer = orb.layer;

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;
                body.linearVelocity = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.forward * 10f;

                GameObject.Destroy(orb, 5f);
            }
            if (pollerInstance.leftGrab)
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.position;
                orb.GetComponent<Renderer>().material.color = SkyBlue;
                GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/Terrain/pitgeo/pit ground").layer = orb.layer;

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;
                body.linearVelocity = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.forward * 10f;

                GameObject.Destroy(orb, 5f);
            }
        }
        public static void Spam2()
        {
            if (pollerInstance == null) return;

            if (pollerInstance.rightGrab)
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.position;
                orb.GetComponent<Renderer>().material.color = SkyBlue;
                GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/Terrain/pitgeo/pit ground").layer = orb.layer;
                Trail(orb, SkyBlue, DarkDodgerBlue);

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;
                body.linearVelocity = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.forward * 10f;

                GameObject.Destroy(orb, 5f);
            }
            if (pollerInstance.leftGrab)
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.position;
                orb.GetComponent<Renderer>().material.color = SkyBlue;
                GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/Terrain/pitgeo/pit ground").layer = orb.layer;
                Trail(orb, SkyBlue, DarkDodgerBlue);

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;
                body.linearVelocity = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.forward * 10f;

                GameObject.Destroy(orb, 5f);
            }
        }
        public static void Spam3()
        {
            if (pollerInstance == null) return;

            if (pollerInstance.rightGrab)
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.position;
                orb.GetComponent<Renderer>().material.color = SkyBlue;
                GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/Terrain/pitgeo/pit ground").layer = orb.layer;
                Trail(orb, SkyBlue, DarkDodgerBlue);

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.useGravity = false;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;
                body.linearVelocity = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.forward * 10f;

                GameObject.Destroy(orb, 5f);
            }
            if (pollerInstance.leftGrab)
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.position;
                orb.GetComponent<Renderer>().material.color = SkyBlue;
                GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/Terrain/pitgeo/pit ground").layer = orb.layer;
                Trail(orb, SkyBlue, DarkDodgerBlue);

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.useGravity = false;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;
                body.linearVelocity = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.forward * 10f;

                GameObject.Destroy(orb, 5f);
            }
        }
        public static void Draw()
        {
            if (pollerInstance == null) return;

            if (pollerInstance.rightGrab)
            {
                draw = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                draw.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                UnityEngine.Object.Destroy(draw.GetComponent<SphereCollider>());
                draw.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                draw.GetComponent<Renderer>().material.color = ColorLib.MenuMat[Theme-1].color;
                GameObject.Destroy(draw, 5f);
            }
            if (pollerInstance.leftGrab)
            {
                draw = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                draw.transform.position = GorillaTagger.Instance.leftHandTransform.position;
                UnityEngine.Object.Destroy(draw.GetComponent<BoxCollider>());
                draw.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                draw.GetComponent<Renderer>().material.color = ColorLib.MenuMat[Theme-1].color;
                GameObject.Destroy(draw, 5f);
            }
        }
        public static void GravDraw()
        {
            if (pollerInstance == null) return;

            if (pollerInstance.rightGrab)
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.position;
                orb.GetComponent<Renderer>().material.color = SkyBlue;
                GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/Terrain/pitgeo/pit ground").layer = orb.layer;

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;

                GameObject.Destroy(orb, 5f);
            }
            if (pollerInstance.leftGrab)
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.position;
                orb.GetComponent<Renderer>().material.color = SkyBlue;
                GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/Terrain/pitgeo/pit ground").layer = orb.layer;

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;

                GameObject.Destroy(orb, 5f);
            }
        }
        public static void BigSpam()
        {
            if (pollerInstance == null) return;

            if (pollerInstance.rightGrab)
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.position;
                orb.GetComponent<Renderer>().material.color = SkyBlue;
                GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/Terrain/pitgeo/pit ground").layer = orb.layer;
                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;
                body.linearVelocity = body.linearVelocity;

                GameObject.Destroy(orb, 5f);
            }
            if (pollerInstance.leftGrab)
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.position;
                orb.GetComponent<Renderer>().material.color = SkyBlue;
                GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/Terrain/pitgeo/pit ground").layer = orb.layer;

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;
                body.linearVelocity = body.linearVelocity;

                GameObject.Destroy(orb, 5f);
            }
        }
        public static void OrbGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                orb.transform.position = spherepointer!.transform.position;
                orb.GetComponent<Renderer>().material.color = SkyBlue;
                GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/Terrain/pitgeo/pit ground").layer = orb.layer;

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;
                body.linearVelocity = body.linearVelocity;

                GameObject.Destroy(orb, 5f);
            }, false);
        }
        public static void OrbGun1()
        {
            GunTemplate.StartBothGuns(() =>
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                orb.transform.position = spherepointer!.transform.position;
                orb.GetComponent<Renderer>().material.color = SkyBlue;
                GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/Terrain/pitgeo/pit ground").layer = orb.layer;

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;
                body.linearVelocity = body.linearVelocity;

                GameObject.Destroy(orb, 5f);
            }, false);
        }
        public static void SpazOrb()
        {
            if (pollerInstance == null) return;

            if (pollerInstance.rightGrab)
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.position;
                orb.GetComponent<Renderer>().material.color = SkyBlue;
                GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/Terrain/pitgeo/pit ground").layer = orb.layer;

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;
                body.rotation = new Quaternion(UnityEngine.Random.Range(-360, 360), UnityEngine.Random.Range(-360, 360), UnityEngine.Random.Range(-360, 360), UnityEngine.Random.Range(-360, 360));
                body.linearVelocity = new Vector3(UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-3, 3)) * 25f;

                GameObject.Destroy(orb, 5f);
            }
            if (pollerInstance.leftGrab)
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.position;
                orb.GetComponent<Renderer>().material.color = SkyBlue;
                GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/Terrain/pitgeo/pit ground").layer = orb.layer;

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;
                body.rotation = new Quaternion(UnityEngine.Random.Range(-360, 360), UnityEngine.Random.Range(-360, 360), UnityEngine.Random.Range(-360, 360), UnityEngine.Random.Range(-360, 360));
                body.linearVelocity = new Vector3(UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-3, 3)) * 25f;

                GameObject.Destroy(orb, 5f);


            }
        }
        public static void OrbRain()
        {
            if (pollerInstance == null) return;

            if (pollerInstance.rightGrab)
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.position + new Vector3(UnityEngine.Random.Range(-20, 20), 10, UnityEngine.Random.Range(-20, 20));
                orb.GetComponent<Renderer>().material.color = SkyBlue;
                GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/Terrain/pitgeo/pit ground").layer = orb.layer;
                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;
                body.linearVelocity = body.linearVelocity;
                GameObject.Destroy(orb, 5f);
            }
            if (pollerInstance.leftGrab)
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.position + new Vector3(UnityEngine.Random.Range(-20, 20), 10, UnityEngine.Random.Range(-20, 20));
                orb.GetComponent<Renderer>().material.color = SkyBlue;
                GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/Terrain/pitgeo/pit ground").layer = orb.layer;

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;
                body.linearVelocity = body.linearVelocity;
                GameObject.Destroy(orb, 5f);
            }
        }
        public static void OrbRain1()
        {
            if (pollerInstance == null) return;

            if (pollerInstance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.position + new Vector3(UnityEngine.Random.Range(-20, 20), 10, UnityEngine.Random.Range(-20, 20));
                orb.GetComponent<Renderer>().material.color = SkyBlue;
                GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/Terrain/pitgeo/pit ground").layer = orb.layer;
                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;
                body.linearVelocity = body.linearVelocity;
                Trail(orb, SkyBlue, DarkDodgerBlue);
                GameObject.Destroy(orb, 5f);
            }
            if (pollerInstance.leftGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.position + new Vector3(UnityEngine.Random.Range(-20, 20), 10, UnityEngine.Random.Range(-20, 20));
                orb.GetComponent<Renderer>().material.color = SkyBlue;
                GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/Terrain/pitgeo/pit ground").layer = orb.layer;

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;
                body.linearVelocity = body.linearVelocity;
                Trail(orb, SkyBlue, DarkDodgerBlue);
                GameObject.Destroy(orb, 5f);
            }
        }

        static GameObject? draw;
        #endregion

        public static void Vibrator()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                GorillaTagger.Instance.StartVibration(false, 1f, 2f);
            }
            if (ControllerInputPoller.instance.leftGrab)
            {
                GorillaTagger.Instance.StartVibration(true, 1f, 2f);
            }
        }
        static float splashDelay;
        public static void Splash(Vector3 splashPosition, Quaternion splashRotation, float splashScale)
        {
            if (Time.time > splashDelay)
            {
                splashDelay = Time.time + 0.4f;

                GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlaySplashEffect", RpcTarget.All, new object[]
                {
                    splashPosition,
                    splashRotation,
                    splashScale,
                    splashScale,
                    true,
                    true,
                });
            }
            RPC2();
        }
        public static void SplashHands()
        {
            if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                Splash(GorillaTagger.Instance.rightHandTransform.position, GorillaTagger.Instance.rightHandTransform.rotation, 4f);
            }
            if (ControllerInputPoller.instance.leftGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                Splash(GorillaTagger.Instance.leftHandTransform.position, GorillaTagger.Instance.leftHandTransform.rotation, 4f);
            }
        }
        public static void SplashGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = GunTemplate.spherepointer!.transform.position + new Vector3(0f, -2f, 0f);

                Splash(GunTemplate.spherepointer.transform.position, GunTemplate.spherepointer.transform.rotation, 4f);
            }, false);
            { GorillaTagger.Instance.offlineVRRig.enabled = true; }
        }
        public static void SplashAura()
        {
            if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                Splash(Annoy(GorillaTagger.Instance.bodyCollider.transform, 1f), GorillaTagger.Instance.headCollider.transform.rotation, 4f);
            }
        }
        public static void GiveSplash()
        {
            GunTemplate.StartBothGuns(() =>
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = LockedPlayer!.transform.position + new Vector3(0f, -2f, 0f);
                Splash(LockedPlayer.rightHandTransform.position, LockedPlayer.rightHandTransform.rotation, 4f);
            }, true); 
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void SchitzoV1()
        {
            GunTemplate.StartBothGuns(() =>
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = GunTemplate.spherepointer!.transform.position + new Vector3(0f, -2f, 0f);

                if (Time.time > splashDelay)
                {
                    splashDelay = Time.time + 0.4f;

                    GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlaySplashEffect", RigManager.GetPlayerFromVRRig(GunTemplate.LockedPlayer!), new object[]
                    {
                        Annoy(LockedPlayer!.transform, 0.5f),
                        GunTemplate.spherepointer.transform.rotation,
                        4f,
                        4f,
                        true,
                        true,
                    });
                }
                RPC2();
            }, true);
            { GorillaTagger.Instance.offlineVRRig.enabled = true; }
        }
        public static void SchitzoV2()
        {
            GunTemplate.StartBothGuns(() =>
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = GunTemplate.spherepointer!.transform.position + new Vector3(0f, -2f, 0f);

                if (Time.time > splashDelay)
                {
                    splashDelay = Time.time + 0.4f;

                    GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlaySplashEffect", RigManager.GetPlayerFromVRRig(GunTemplate.LockedPlayer!), new object[]
                    {
                        GunTemplate.spherepointer.transform.position + new Vector3(0f, -2f, 0f),
                        GunTemplate.spherepointer.transform.rotation,
                        4f,
                        4f,
                        true,
                        true,
                    });
                }
                RPC2();
            }, true);
            { GorillaTagger.Instance.offlineVRRig.enabled = true; }
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
    }
}
