using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BepInEx;
using UnityEngine.Animations.Rigging;
using Elixir.Management;
using UnityEngine.InputSystem.HID;
using Elixir.Mods.Categories;

namespace Elixir.Components
{
    public class ButtonInteractor : MonoBehaviour
    {
        private float cooldown = 1;
        public static GameObject clickerObj = null;

        public void Update()
        {
            if (Time.frameCount >= cooldown + 25)
            {
                foreach (Transform child in Menu.menu.transform)
                {
                    Button[] buttons = child.GetComponentsInChildren<Button>();
                    foreach (Button button in buttons)
                    {
                        if (button != null)
                        {
                            if (Vector3.Distance(clickerObj.transform.position, button.transform.position) < 0.025f)
                            {
                                button.onClick.Invoke();
                                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(114, false, 1);
                                cooldown = Time.frameCount;
                            }
                        }
                    }
                }
            }
        }
        public static void AddButtonClicker(Transform parentTransform)
        {
            if (clickerObj == null)
            {
                clickerObj = new GameObject("buttonclicker");
                BoxCollider clickerCollider = clickerObj.AddComponent<BoxCollider>();
                if (clickerCollider != null)
                {
                    clickerCollider.isTrigger = true;
                }
                MeshFilter meshFilter = clickerObj.AddComponent<MeshFilter>();
                if (meshFilter != null)
                {
                    meshFilter.mesh = Resources.GetBuiltinResource<Mesh>("Sphere.fbx");
                }
                Renderer clickerRenderer = clickerObj.AddComponent<MeshRenderer>();
                if (clickerRenderer != null)
                {
                    clickerRenderer.material.color = Color.white;
                    clickerRenderer.material.shader = Shader.Find("GUI/Text Shader");
                }
                if (parentTransform != null)
                {
                    clickerObj.transform.parent = parentTransform;
                    clickerObj.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
                    clickerObj.transform.localPosition = new Vector3(0f, -0.1f, 0f);
                }
            }
        }
    }
}