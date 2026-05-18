using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Elixir.Components
{
    public class GUIHandler : MonoBehaviour
    {
        public string watermarkText = "Elixir | V9";
        public string subText = "by Menker & sodaa";

        public int paddingX = 12;
        public int paddingY = 10;
        public int watermarkWidth = 180;
        public int watermarkHeight = 48;

        [Range(0f, 1f)] public float glowPulseSpeed = 1.2f;
        [Range(0f, 1f)] public float glowMinAlpha = 0.4f;

        private readonly Color bgColor = new Color(0.04f, 0.02f, 0.08f, 0.72f);
        private readonly Color lineColor = new Color(0.65f, 0.15f, 1.00f, 1.00f);
        private readonly Color glowColor = new Color(0.55f, 0.05f, 0.90f, 0.35f);
        private readonly Color textColor = new Color(0.95f, 0.90f, 1.00f, 1.00f);
        private readonly Color subColor = new Color(0.70f, 0.60f, 0.85f, 0.85f);

        private readonly Color consoleBg = new Color(0.03f, 0.01f, 0.07f, 0.93f);
        private readonly Color consoleBorder = new Color(0.65f, 0.15f, 1.00f, 0.80f);
        private readonly Color consoleGlow = new Color(0.55f, 0.05f, 0.90f, 0.20f);
        private readonly Color logNormal = new Color(0.85f, 0.80f, 0.95f, 1.00f);
        private readonly Color logSuccess = new Color(0.40f, 1.00f, 0.60f, 1.00f);
        private readonly Color logError = new Color(1.00f, 0.35f, 0.35f, 1.00f);
        private readonly Color logSystem = new Color(0.75f, 0.45f, 1.00f, 1.00f);
        private readonly Color inputColor = new Color(0.95f, 0.90f, 1.00f, 1.00f);
        private readonly Color promptColor = new Color(0.65f, 0.15f, 1.00f, 1.00f);
        private readonly Color devmodeBg = new Color(0.20f, 0.00f, 0.40f, 0.35f);

        private Texture2D backgroundTex;
        private Texture2D glowLineTex;
        private Texture2D glowCoreTex;
        private Texture2D consoleBgTex;
        private Texture2D consoleBorderTex;
        private Texture2D consoleGlowTex;
        private Texture2D devmodeBgTex;
        private Texture2D inputBgTex;
        private Texture2D clearTex;

        private GUIStyle watermarkStyle;
        private GUIStyle subTextStyle;
        private GUIStyle consoleLogStyle;
        private GUIStyle consoleLogSuccessStyle;
        private GUIStyle consoleLogErrorStyle;
        private GUIStyle consoleLogSystemStyle;
        private GUIStyle consoleInputStyle;
        private GUIStyle consolePromptStyle;
        private GUIStyle consoleTitleStyle;
        private GUIStyle devmodeBadgeStyle;
        private GUIStyle runButtonStyle;

        private float glowAlpha = 1f;
        private float glowDir = -1f;

        private bool consoleOpen = false;
        private string inputText = "";
        private bool focusInput = false;

        public bool devModeActive = false;

        private struct LogEntry { public string text; public int type; }
        private List<LogEntry> logs = new List<LogEntry>();
        private Vector2 scrollPos = Vector2.zero;

        private const float CON_W = 480f;
        private const float CON_H = 260f;
        private const float CON_X = 20f;
        private const float CON_Y = 80f;
        private const string INPUT_CTRL = "ConsoleInput";

        void Awake()
        {
            backgroundTex = MakeTex(1, 1, bgColor);
            glowCoreTex = MakeTex(1, 1, lineColor);
            glowLineTex = MakeTex(1, 1, glowColor);
            consoleBgTex = MakeTex(1, 1, consoleBg);
            consoleBorderTex = MakeTex(1, 1, consoleBorder);
            consoleGlowTex = MakeTex(1, 1, consoleGlow);
            devmodeBgTex = MakeTex(1, 1, devmodeBg);
            inputBgTex = MakeTex(1, 1, new Color(0.08f, 0.03f, 0.14f, 0.95f));
            clearTex = MakeTex(1, 1, Color.clear);

            Log("System initialised. Press ` to toggle console.", 3);
        }

        void OnDestroy()
        {
            Destroy(backgroundTex);
            Destroy(glowCoreTex);
            Destroy(glowLineTex);
            Destroy(consoleBgTex);
            Destroy(consoleBorderTex);
            Destroy(consoleGlowTex);
            Destroy(devmodeBgTex);
            Destroy(inputBgTex);
            Destroy(clearTex);
        }

        void Update()
        {
            if (Keyboard.current == null) return;

            if (Keyboard.current.backquoteKey.wasPressedThisFrame)
            {
                consoleOpen = !consoleOpen;
                if (consoleOpen)
                {
                    inputText = "";
                    focusInput = true;
                    var uiModule = FindObjectOfType<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
                    if (uiModule != null) uiModule.enabled = false;
                }
                else
                {
                    ReenableInputModule();
                }
            }

            if (consoleOpen && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                consoleOpen = false;
                ReenableInputModule();
            }
        }

        void ReenableInputModule()
        {
            var uiModule = FindObjectOfType<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
            if (uiModule != null) uiModule.enabled = true;
        }

        void OnGUI()
        {
            InitStyles();

            glowAlpha += glowDir * glowPulseSpeed * Time.deltaTime;
            if (glowAlpha <= glowMinAlpha) { glowAlpha = glowMinAlpha; glowDir = 1f; }
            if (glowAlpha >= 1f) { glowAlpha = 1f; glowDir = -1f; }

            DrawWatermark();

            if (consoleOpen)
                DrawConsole();
        }

        void DrawWatermark()
        {
            Rect bgRect = new Rect(paddingX, paddingY, watermarkWidth, watermarkHeight);
            GUI.DrawTexture(bgRect, backgroundTex);

            if (devModeActive)
            {
                Rect badgeRect = new Rect(bgRect.xMax - 58, bgRect.y + 6, 52, 16);
                GUI.DrawTexture(badgeRect, devmodeBgTex);
                GUI.Label(badgeRect, "  DEVMODE", devmodeBadgeStyle);
            }

            GUI.Label(new Rect(bgRect.x + 10, bgRect.y + 5, bgRect.width - 70, 22), watermarkText, watermarkStyle);
            GUI.Label(new Rect(bgRect.x + 11, bgRect.y + 22, bgRect.width - 12, 14), subText, subTextStyle);

            Color prev = GUI.color;
            GUI.color = new Color(glowColor.r, glowColor.g, glowColor.b, glowColor.a * glowAlpha);
            GUI.DrawTexture(new Rect(bgRect.x - 1, bgRect.yMax, bgRect.width + 2, 5f), glowLineTex);
            GUI.color = new Color(lineColor.r, lineColor.g, lineColor.b, 0.90f + 0.10f * glowAlpha);
            GUI.DrawTexture(new Rect(bgRect.x, bgRect.yMax, bgRect.width, 2f), glowCoreTex);
            GUI.color = new Color(glowColor.r, glowColor.g, glowColor.b, 0.18f * glowAlpha);
            GUI.DrawTexture(new Rect(bgRect.x - 2, bgRect.yMax + 2, bgRect.width + 4, 5f), glowLineTex);
            GUI.color = prev;
        }

        void DrawConsole()
        {
            Rect con = new Rect(CON_X, CON_Y, CON_W, CON_H);

            Color prev = GUI.color;
            GUI.color = new Color(consoleGlow.r, consoleGlow.g, consoleGlow.b, 0.5f * glowAlpha);
            GUI.DrawTexture(new Rect(con.x - 4, con.y - 4, con.width + 8, con.height + 8), consoleGlowTex);
            GUI.color = prev;

            GUI.DrawTexture(con, consoleBgTex);

            DrawBorder(new Rect(con.x, con.y, con.width, 1));
            DrawBorder(new Rect(con.x, con.yMax - 1, con.width, 1));          
            DrawBorder(new Rect(con.x, con.y, 1, con.height));
            DrawBorder(new Rect(con.xMax - 1, con.y, 1, con.height));

            GUI.Label(new Rect(con.x + 10, con.y + 6, con.width - 20, 18), "▸ DEVELOPER CONSOLE", consoleTitleStyle);
            DrawBorder(new Rect(con.x, con.y + 26, con.width, 1));

            float logH = con.height - 26 - 32;
            Rect logOuter = new Rect(con.x + 1, con.y + 27, con.width - 2, logH);
            Rect logInner = new Rect(0, 0, con.width - 18, Mathf.Max(logH, logs.Count * 18f));

            scrollPos = GUI.BeginScrollView(logOuter, scrollPos, logInner, false, false);
            float y = 4f;
            foreach (var entry in logs)
            {
                GUIStyle s = entry.type switch
                {
                    1 => consoleLogSuccessStyle,
                    2 => consoleLogErrorStyle,
                    3 => consoleLogSystemStyle,
                    _ => consoleLogStyle
                };
                string prefix = entry.type switch { 1 => "✓ ", 2 => "✗ ", 3 => "» ", _ => "  " };
                GUI.Label(new Rect(6, y, logInner.width - 8, 18), prefix + entry.text, s);
                y += 18f;
            }
            GUI.EndScrollView();

            float inputY = con.yMax - 30;
            DrawBorder(new Rect(con.x, inputY, con.width, 1));
            GUI.DrawTexture(new Rect(con.x + 1, inputY + 1, con.width - 2, 28), inputBgTex);

            GUI.Label(new Rect(con.x + 8, inputY + 6, 16, 18), ">", consolePromptStyle);

            const float btnW = 52f;
            const float btnGap = 4f;
            float fieldW = con.width - 34 - btnW - btnGap - 4f;

            GUI.SetNextControlName(INPUT_CTRL);
            inputText = GUI.TextField(new Rect(con.x + 24, inputY + 6, fieldW, 18), inputText, consoleInputStyle);

            if (focusInput) { GUI.FocusControl(INPUT_CTRL); focusInput = false; }

            Rect btnRect = new Rect(con.x + 24 + fieldW + btnGap, inputY + 4, btnW, 22);
            bool pressedEnter = Event.current.type == EventType.KeyDown &&
                               (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter) &&
                                GUI.GetNameOfFocusedControl() == INPUT_CTRL;

            if (GUI.Button(btnRect, "RUN", runButtonStyle) || pressedEnter)
            {
                SubmitCommand(inputText.Trim());
                inputText = "";
                focusInput = true;
                scrollPos.y = float.MaxValue;
                if (pressedEnter) Event.current.Use();
            }
        }

        void DrawBorder(Rect r)
        {
            Color prev = GUI.color;
            GUI.color = consoleBorder;
            GUI.DrawTexture(r, consoleBorderTex);
            GUI.color = prev;
        }

        void SubmitCommand(string raw)
        {
            if (string.IsNullOrEmpty(raw)) return;

            Log(raw, 0);

            string cmd = raw.ToLower();

            switch (cmd)
            {
                case "help":
                    Log("Commands: help  |  clear  |  quit", 3);
                    break;

                case "clear":
                    logs.Clear();
                    break;

                case "quit":
                    Log("Console closed.", 3);
                    consoleOpen = false;
                    break;

                case "devmode":
                    devModeActive = !devModeActive;

                    if (devModeActive)
                    {
                        Log("Dev mode ENABLED.", 1);

                        ExitGames.Client.Photon.Hashtable table = Photon.Pun.PhotonNetwork.LocalPlayer.CustomProperties;
                        table.Remove("Elixir");
                    }
                    else
                    {
                        Log("Dev mode DISABLED.", 2);
                    }
                    break;

                default:
                    Log($"Unknown command: '{raw}'  —  type 'help' for a list.", 2);
                    break;
            }
        }

        public void Log(string message, int type = 0)
        {
            logs.Add(new LogEntry { text = message, type = type });
            scrollPos.y = float.MaxValue;
        }

        void InitStyles()
        {
            if (watermarkStyle != null) return;

            watermarkStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 13,
                fontStyle = FontStyle.Bold,
                normal = { textColor = textColor }
            };

            subTextStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 9,
                normal = { textColor = subColor }
            };

            devmodeBadgeStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 8,
                fontStyle = FontStyle.Bold,
                normal = { textColor = new Color(0.85f, 0.50f, 1.00f, 1f) }
            };

            consoleTitleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 11,
                fontStyle = FontStyle.Bold,
                normal = { textColor = new Color(0.75f, 0.45f, 1.00f, 1f) }
            };

            consoleLogStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 11,
                normal = { textColor = logNormal }
            };

            consoleLogSuccessStyle = new GUIStyle(consoleLogStyle) { normal = { textColor = logSuccess } };
            consoleLogErrorStyle = new GUIStyle(consoleLogStyle) { normal = { textColor = logError } };
            consoleLogSystemStyle = new GUIStyle(consoleLogStyle) { normal = { textColor = logSystem } };

            consoleInputStyle = new GUIStyle(GUI.skin.textField)
            {
                fontSize = 12,
                normal = { textColor = inputColor, background = clearTex },
                focused = { textColor = inputColor, background = clearTex },
                hover = { textColor = inputColor, background = clearTex },
                active = { textColor = inputColor, background = clearTex }
            };

            consolePromptStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                normal = { textColor = promptColor }
            };

            runButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 10,
                fontStyle = FontStyle.Bold,
                normal = { textColor = new Color(0.90f, 0.70f, 1.00f, 1f), background = MakeTex(1, 1, new Color(0.25f, 0.05f, 0.50f, 0.90f)) },
                hover = { textColor = new Color(1.00f, 0.90f, 1.00f, 1f), background = MakeTex(1, 1, new Color(0.40f, 0.10f, 0.75f, 0.95f)) },
                active = { textColor = new Color(1.00f, 1.00f, 1.00f, 1f), background = MakeTex(1, 1, new Color(0.55f, 0.15f, 0.90f, 1.00f)) },
                border = new RectOffset(2, 2, 2, 2)
            };
        }

        Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++) pixels[i] = col;
            Texture2D tex = new Texture2D(width, height);
            tex.SetPixels(pixels);
            tex.Apply();
            return tex;
        }
    }
}
