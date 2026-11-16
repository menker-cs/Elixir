using Elixir.Management;
using Elixir.Mods.Categories;
using Elixir.Notifications;
using Elixir.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Elixir.Components
{
    public class GUIHandler : MonoBehaviour
    {
        private string inputText = "";
        public static bool devModeActive = false;
        public static bool ArrayList = true;
        public static bool OpenGUI = false;
        public static GUIStyle LabelStyle;
        private static Texture2D arrayListBackground;

        public static void InitStyles()
        {
            if (arrayListBackground == null)
            {
                arrayListBackground = new Texture2D(1, 1);
                arrayListBackground.SetPixel(0, 0, new Color(0.12f, 0.02f, 0.18f, 0.85f));
                arrayListBackground.Apply();
            }

            if (LabelStyle == null)
            {
                LabelStyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 14,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleRight,
                    padding = new RectOffset(8, 9, 3, 3),
                    normal =
                    {
                        textColor = new Color(0.6f, 0.2f, 0.8f, 1f),
                        background = arrayListBackground
                    }
                };
            }
        }

        void Update()
        {
            if (Keyboard.current == null) return;

            if (Keyboard.current.f8Key.wasPressedThisFrame)
            {
                OpenGUI = !OpenGUI;
            }

            if (Keyboard.current.f1Key.wasPressedThisFrame)
            {
                SubmitCommand("devmode");
            }
        }

        void OnGUI()
        {
            InitStyles();

            if (OpenGUI)
            {
                if (ArrayList)
                    DrawArrayList();
            }
            else
            {
                GUI.Label(new Rect(10f, 10f, 200f, 25f), "F8 To Toggle GUI", LabelStyle);
            }
        }

        void DrawArrayList()
        {
            float y = 10f;

            foreach (var category in Menu.categories)
            {
                foreach (var button in category.buttons)
                {
                    if (!button.toggled)
                        continue;

                    float width = LabelStyle.CalcSize(new GUIContent(button.title)).x + 12f;
                    Rect rect = new Rect(Screen.width - width - 10f, y, width, 27f);

                    GUI.Label(rect, button.title, LabelStyle);

                    y += 32f;
                }
            }
        }

        void SubmitCommand(string raw)
        {
            if (string.IsNullOrEmpty(raw)) return;

            string cmd = raw.ToLower();

            switch (cmd)
            {
                case "devmode":
                    devModeActive = !devModeActive;

                    if (devModeActive)
                    {
                        Menu.Start();
                        Settings.AutoLoadPrefs();
                        CoroutineHandler.StartCoroutine1(Menu.GetObjects());
                        Menu.ChangeBoardMaterial("Environment Objects/LocalObjects_Prefab/TreeRoom", "UnityTempFile", 4, ColorLib.Color2Mat(new Color(0.94f, 0.43f, 0.94f, 1f)), ref Menu.originalMat1!);
                        NotificationLib.SendNotification("Dev Mode Toggled, Killswitch Bypassed.");
                    }
                    break;
            }
        }
    }
}