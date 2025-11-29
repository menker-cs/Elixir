using BepInEx;
using System.Collections;
using static Elixir.Utilities.ColorLib;
using static Elixir.Utilities.Variables;
using UnityEngine;

namespace Elixir.Mods.Categories
{
    public class SherbertClass
    {
        static GameObject? sherbert = null;
        static bool hold = false;
        static Vector3 lastPosition;
        private static Coroutine chaseCoroutine;
        public static void SpawnSherbert(bool collide, bool isLaunching = false, bool isChasing = false)
        {
            if (sherbert == null || isLaunching)
            {
                sherbert = GameObject.CreatePrimitive(PrimitiveType.Cube);
                sherbert.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
                sherbert.transform.position = RandomPos(GorillaLocomotion.GTPlayer.Instance.transform, 10f);
                sherbert.GetComponent<Renderer>().material = isChasing ? AngrySherbMat : SherbMat;

                if (!collide)
                {
                    Object.Destroy(sherbert.GetComponent<Collider>());
                }

                int sherb = LayerMask.NameToLayer("sherbert");
                if (sherb == -1) sherb = 8;
                sherbert.layer = sherb;

                int rHand = GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.gameObject.layer;
                int lHand = GorillaLocomotion.GTPlayer.Instance.LeftHand.controllerTransform.gameObject.layer;
                Physics.IgnoreLayerCollision(sherb, rHand);
                Physics.IgnoreLayerCollision(sherb, lHand);

                Rigidbody rb = sherbert.AddComponent<Rigidbody>();
                rb.mass = 0.5f;
                rb.useGravity = true;
                rb.isKinematic = true;
            }
        }
        public static void Sherbert()
        {
            if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                SpawnSherbert(true);

                sherbert.transform.position = GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.position;
                sherbert.transform.rotation = GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.rotation;
                hold = true;
            }
            else if (hold && sherbert != null)
            {
                Rigidbody rb = sherbert.GetComponent<Rigidbody>();
                rb.isKinematic = false;

                Vector3 handVelocity = (GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.position - lastPosition) / Time.deltaTime;
                rb.linearVelocity = handVelocity;

                hold = false;
            }

            lastPosition = GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.position;
        }
        public static void LaunchSherbert()
        {
            if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                if (sherbert != null)
                {
                    Object.Destroy(sherbert);
                    sherbert = null;
                }

                SpawnSherbert(true, true);
                sherbert.transform.position = GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.position;
                sherbert.transform.rotation = GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.rotation;

                Rigidbody rb = sherbert.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                rb.linearVelocity = GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.forward * 10f;
            }
            if (ControllerInputPoller.instance.leftGrab)
            {
                if (sherbert != null)
                {
                    Object.Destroy(sherbert);
                    sherbert = null;
                }

                SpawnSherbert(true, true);
                sherbert.transform.position = GorillaLocomotion.GTPlayer.Instance.LeftHand.controllerTransform.position;
                sherbert.transform.rotation = GorillaLocomotion.GTPlayer.Instance.LeftHand.controllerTransform.rotation;

                Rigidbody rb = sherbert.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                rb.linearVelocity = GorillaLocomotion.GTPlayer.Instance.LeftHand.controllerTransform.forward * 10f;
            }
        }
        public static void SherbertFollow()
        {
            SpawnSherbert(false);
            if (chaseCoroutine == null)
            {
                chaseCoroutine = GorillaLocomotion.GTPlayer.Instance.StartCoroutine(Follow(1f));
            }
        }
        public static void StopSherbertFollow()
        {
            if (chaseCoroutine != null)
            {
                GorillaLocomotion.GTPlayer.Instance.StopCoroutine(chaseCoroutine);
                chaseCoroutine = null;
                sherbert.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            }
        }
        public static void SherbertKiller()
        {
            SpawnSherbert(false, false, true);
            if (chaseCoroutine == null)
            {
                chaseCoroutine = GorillaLocomotion.GTPlayer.Instance.StartCoroutine(Follow(0.35f, true));
            }
        }
        private static IEnumerator Follow(float followDistance, bool isKiller = false)
        {
            Transform sherber = sherbert.transform;
            sherber.LookAt(GorillaLocomotion.GTPlayer.Instance.headCollider.transform);

            Rigidbody rb = sherbert.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = false;

            while (sherbert != null && GorillaLocomotion.GTPlayer.Instance != null)
            {
                Vector3 targetPosition = GorillaLocomotion.GTPlayer.Instance.bodyCollider.transform.position;
                float distance = Vector3.Distance(sherber.position, targetPosition);

                if (isKiller && distance <= followDistance)
                {
                    Application.Quit();
                }

                if (distance > followDistance)
                {
                    Vector3 direction = (targetPosition - sherber.position).normalized;
                    rb.linearVelocity = direction * 5f;
                }
                else
                {
                    rb.linearVelocity = Vector3.zero;
                }

                yield return null;
            }

            chaseCoroutine = null;
        }
        public static void KillSherbert()
        {
            StopSherbertFollow();
            if (sherbert != null)
            {
                Object.Destroy(sherbert);
                sherbert = null;
            }
        }
    }
}
