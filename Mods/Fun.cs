using BepInEx;
using ExitGames.Client.Photon;
using Elixir.Utilities;
using Photon.Pun;
using System;
using UnityEngine;
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
            if (ControllerInputPoller.instance == null) return;

            if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.position;
                orb.GetComponent<Renderer>().material.color = SkyBlue;

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;
                body.velocity = GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.forward * 10f;

                GameObject.Destroy(orb, 5f);
            }
            if (ControllerInputPoller.instance.leftGrab)
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.LeftHand.controllerTransform.position;
                orb.GetComponent<Renderer>().material.color = SkyBlue;

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;
                body.velocity = GorillaLocomotion.GTPlayer.Instance.LeftHand.controllerTransform.forward * 10f;

                GameObject.Destroy(orb, 5f);
            }
        }

        public static void Spam2()
        {
            if (ControllerInputPoller.instance == null) return;

            if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.position;
                orb.GetComponent<Renderer>().material.color = SkyBlue;
                Trail(orb, SkyBlue, DarkDodgerBlue);

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;
                body.velocity = GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.forward * 10f;

                GameObject.Destroy(orb, 5f);
            }
            if (ControllerInputPoller.instance.leftGrab)
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.LeftHand.controllerTransform.position;
                orb.GetComponent<Renderer>().material.color = SkyBlue;
                Trail(orb, SkyBlue, DarkDodgerBlue);

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;
                body.velocity = GorillaLocomotion.GTPlayer.Instance.LeftHand.controllerTransform.forward * 10f;

                GameObject.Destroy(orb, 5f);
            }
        }

        public static void Spam3()
        {
            if (ControllerInputPoller.instance == null) return;

            if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.position;
                orb.GetComponent<Renderer>().material.color = SkyBlue;
                Trail(orb, SkyBlue, DarkDodgerBlue);

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.useGravity = false;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;
                body.velocity = GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.forward * 10f;

                GameObject.Destroy(orb, 5f);
            }
            if (ControllerInputPoller.instance.leftGrab)
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.LeftHand.controllerTransform.position;
                orb.GetComponent<Renderer>().material.color = SkyBlue;
                Trail(orb, SkyBlue, DarkDodgerBlue);

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.useGravity = false;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;
                body.velocity = GorillaLocomotion.GTPlayer.Instance.LeftHand.controllerTransform.forward * 10f;

                GameObject.Destroy(orb, 5f);
            }
        }

        public static void Draw()
        {
            if (ControllerInputPoller.instance == null) return;

            if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                draw = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                draw.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                UnityEngine.Object.Destroy(draw.GetComponent<SphereCollider>());
                draw.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                draw.GetComponent<Renderer>().material.color = ColorLib.MenuMat[0].color;
                GameObject.Destroy(draw, 5f);
            }
            if (ControllerInputPoller.instance.leftGrab)
            {
                draw = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                draw.transform.position = GorillaTagger.Instance.leftHandTransform.position;
                UnityEngine.Object.Destroy(draw.GetComponent<SphereCollider>());
                draw.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                draw.GetComponent<Renderer>().material.color = ColorLib.MenuMat[0].color;
                GameObject.Destroy(draw, 5f);
            }
        }

        public static void GravDraw()
        {
            if (ControllerInputPoller.instance == null) return;

            if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.position;
                orb.GetComponent<Renderer>().material.color = SkyBlue;

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;

                GameObject.Destroy(orb, 5f);
            }
            if (ControllerInputPoller.instance.leftGrab)
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.LeftHand.controllerTransform.position;
                orb.GetComponent<Renderer>().material.color = SkyBlue;

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;

                GameObject.Destroy(orb, 5f);
            }
        }

        public static void BigSpam()
        {
            if (ControllerInputPoller.instance == null) return;

            if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.position;
                orb.GetComponent<Renderer>().material.color = SkyBlue;

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;

                GameObject.Destroy(orb, 5f);
            }
            if (ControllerInputPoller.instance.leftGrab)
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.LeftHand.controllerTransform.position;
                orb.GetComponent<Renderer>().material.color = SkyBlue;

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;

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

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;

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

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;

                GameObject.Destroy(orb, 5f);
            }, false);
        }

        public static void SpazOrb()
        {
            if (ControllerInputPoller.instance == null) return;

            if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.position;
                orb.GetComponent<Renderer>().material.color = SkyBlue;

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;
                body.rotation = UnityEngine.Random.rotation;
                body.velocity = new Vector3(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f)) * 25f;

                GameObject.Destroy(orb, 5f);
            }
            if (ControllerInputPoller.instance.leftGrab)
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.LeftHand.controllerTransform.position;
                orb.GetComponent<Renderer>().material.color = SkyBlue;

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;
                body.rotation = UnityEngine.Random.rotation;
                body.velocity = new Vector3(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f)) * 25f;

                GameObject.Destroy(orb, 5f);
            }
        }

        public static void OrbRain()
        {
            if (ControllerInputPoller.instance == null) return;

            if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.position + new Vector3(UnityEngine.Random.Range(-20f, 20f), 10f, UnityEngine.Random.Range(-20f, 20f));
                orb.GetComponent<Renderer>().material.color = SkyBlue;

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;

                GameObject.Destroy(orb, 5f);
            }
            if (ControllerInputPoller.instance.leftGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.LeftHand.controllerTransform.position + new Vector3(UnityEngine.Random.Range(-20f, 20f), 10f, UnityEngine.Random.Range(-20f, 20f));
                orb.GetComponent<Renderer>().material.color = SkyBlue;

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;

                GameObject.Destroy(orb, 5f);
            }
        }

        public static void OrbRain1()
        {
            if (ControllerInputPoller.instance == null) return;

            if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.position + new Vector3(UnityEngine.Random.Range(-20f, 20f), 10f, UnityEngine.Random.Range(-20f, 20f));
                orb.GetComponent<Renderer>().material.color = SkyBlue;

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;

                Trail(orb, SkyBlue, DarkDodgerBlue);
                GameObject.Destroy(orb, 5f);
            }
            if (ControllerInputPoller.instance.leftGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
                orb.transform.position = GorillaLocomotion.GTPlayer.Instance.LeftHand.controllerTransform.position + new Vector3(UnityEngine.Random.Range(-20f, 20f), 10f, UnityEngine.Random.Range(-20f, 20f));
                orb.GetComponent<Renderer>().material.color = SkyBlue;

                Rigidbody body = orb.AddComponent<Rigidbody>();
                body.mass = 0.5f;
                body.useGravity = true;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;

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
            RPCFlush();
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
            if (GorillaTagger.Instance == null) return;

            GunTemplate.StartBothGuns(() =>
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = GunTemplate.spherepointer!.transform.position + new Vector3(0f, -2f, 0f);

                Splash(GunTemplate.spherepointer.transform.position, GunTemplate.spherepointer.transform.rotation, 4f);
            }, false);
        }
        public static void SplashAura()
        {
            if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                Splash(RandomPos(GorillaTagger.Instance.bodyCollider.transform, 1f), GorillaTagger.Instance.headCollider.transform.rotation, 4f);
            }
        }
        public static void GiveSplash()
        {
            if (GorillaTagger.Instance == null) return;

            GunTemplate.StartBothGuns(() =>
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = LockedPlayer!.transform.position + new Vector3(0f, -2f, 0f);
                Splash(LockedPlayer.rightHandTransform.position, LockedPlayer.rightHandTransform.rotation, 4f);
            }, true); 
        }

        public static void SchitzoV1()
        {
            if (GorillaTagger.Instance == null) return;

            GunTemplate.StartBothGuns(() =>
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = GunTemplate.spherepointer!.transform.position + new Vector3(0f, -2f, 0f);

                if (Time.time > splashDelay)
                {
                    splashDelay = Time.time + 0.4f;

                    GorillaTagger.Instance.myVRRig.SendRPC("RPC_ReportSlingshotHit", RigManager.GetPlayerFromVRRig(GunTemplate.LockedPlayer!), new object[]
                    {
                        RandomPos(LockedPlayer!.transform, 0.5f),
                        GunTemplate.spherepointer.transform.rotation,
                        4f,
                        4f,
                        true,
                        true,
                    });
                }
                RPCFlush();
                RPC2();
            }, true);
        }
        public static void SchitzoV2()
        {
            if (GorillaTagger.Instance == null) return;

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
                RPCFlush();
                RPC2();
            }, true);
        }
    }
}
