using BepInEx;
using System.Collections;
using System.Net.Http;
using UnityEngine;

namespace Elixir.Mods.Categories
{
    public class SherbertClass
    {
        static GameObject? sherbert = null;
        static bool hold = false;
        static Vector3 lastPosition;
        private static Coroutine chaseCoroutine;
        public static void SpawnSherbert(bool collide)
        {
            if (sherbert == null)
            {
                sherbert = GameObject.CreatePrimitive(PrimitiveType.Cube);
                sherbert.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
                sherbert.transform.position = RandomPos(GorillaLocomotion.GTPlayer.Instance.transform, 10f);
                sherbert.GetComponent<Renderer>().material = Url2Mat("https://raw.githubusercontent.com/Cha554/Stone-Networking/main/Stone/Sherbert.jpg");

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
        public static void SherbertFollow()
        {
            SpawnSherbert(false);
            if (chaseCoroutine == null)
            {
                chaseCoroutine = GorillaLocomotion.GTPlayer.Instance.StartCoroutine(Chase());
            }
        }
        public static void StopSherbertFollow()
        {
            if (chaseCoroutine != null)
            {
                GorillaLocomotion.GTPlayer.Instance.StopCoroutine(chaseCoroutine);
                chaseCoroutine = null;
            }
        }

        private static IEnumerator Chase()
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

                if (distance > 1f)
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
        public static Material Url2Mat(string url)
        {
            byte[] imageData;
            using (var httpClient = new HttpClient())
            {
                var response = httpClient.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                imageData = response.Content.ReadAsByteArrayAsync().Result;
            }

            var material = new Material(Shader.Find("GorillaTag/UberShader"))
            {
                shaderKeywords = new[] { "_USE_TEXTURE" }
            };

            var texture = new Texture2D(2, 2);
            ImageConversion.LoadImage(texture, imageData);
            texture.Apply();

            material.mainTexture = texture;

            return material;
        }
        public static Vector3 RandomPos(Transform transform, float range)
        {
            return transform.position + new Vector3(UnityEngine.Random.Range(-range, range), UnityEngine.Random.Range(-range, range), UnityEngine.Random.Range(-range, range));
        }
    }
}
