using BepInEx;
using Elixir.Utilities;
using static Elixir.Utilities.Variables;
using UnityEngine;

namespace Elixir.Mods.Categories
{
    public class SherbertClass
    {
        static GameObject? sherbert = null;
        static bool hold = false;
        static Vector3 lastPosition;
        public static void Sherbert()
        {
            if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetKey(KeyCode.G))
            {
                if (sherbert == null)
                {
                    sherbert = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    sherbert.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
                    sherbert.GetComponent<Renderer>().material = ColorLib.Url2Mat("https://raw.githubusercontent.com/Cha554/Stone-Networking/main/Stone/Sherbert.jpg");

                    int sherb = LayerMask.NameToLayer("sherbert");
                    if (sherb == -1) sherb = 8;
                    sherbert.layer = sherb;

                    int rHand = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.gameObject.layer;
                    int lHand = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.gameObject.layer;
                    Physics.IgnoreLayerCollision(sherb, rHand);
                    Physics.IgnoreLayerCollision(sherb, lHand);

                    Rigidbody rb = sherbert.AddComponent<Rigidbody>();
                    rb.mass = 0.5f;
                    rb.useGravity = true;
                    rb.isKinematic = true;
                }

                sherbert.transform.position = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.position;
                sherbert.transform.rotation = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.rotation;
                hold = true;
            }
            else if (hold && sherbert != null)
            {
                Rigidbody rb = sherbert.GetComponent<Rigidbody>();
                rb.isKinematic = false;

                Vector3 handVelocity = (GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.position - lastPosition) / Time.deltaTime;
                rb.linearVelocity = handVelocity;

                hold = false;
            }

            lastPosition = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.position;
        }
        public static void KillSherbert()
        {
            if (sherbert != null)
            {
                Object.Destroy(sherbert);
                sherbert = null;
            }
        }
    }
}
