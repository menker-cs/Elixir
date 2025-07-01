using Hidden.Mods;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Hidden.Utilities.Variables;
using static Hidden.Utilities.ColorLib;
using static Hidden.Menu.Optimizations;
using static Hidden.Menu.ButtonHandler;
using BepInEx;
using UnityEngine.InputSystem;
using HarmonyLib;
using static Hidden.Initialization.PluginInfo;
using Hidden.Utilities;
using Photon.Pun;
using System.Net;
using TMPro;
using Hidden.Utilities.Notifs;
using System.Collections;
using UnityEngine.Networking;
using Oculus.Platform;
using Hidden.Mods.Categories;
using System.IO;
using System.Reflection;
using UnityEngine.ProBuilder.MeshOperations;

namespace Hidden.Menu
{
    [HarmonyPatch(typeof(GorillaLocomotion.GTPlayer), "LateUpdate")]
    public class Main : MonoBehaviour
    {
        [HarmonyPrefix]
        public static void Prefix()
        {
            fps = (Time.deltaTime > 0) ? Mathf.RoundToInt(1.0f / Time.deltaTime) : 0;
            try
            {
                GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdHeadingText").GetComponent<TextMeshPro>().text = $"<color={hexColor}>Hidden | V{Hidden.Initialization.PluginInfo.menuVersion}</color>\n--------------------------------------------";
                GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdHeadingText").GetComponent<TextMeshPro>().color = DFade.color;
                TextMeshPro textMeshPro = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdBodyText").GetComponent<TextMeshPro>();
                textMeshPro.GetComponent<TextMeshPro>().color = DFade.color;
                textMeshPro.text = $"" +
                    $"\nThank You For Using Hidden!\n\n" +
                    $"Status: <color={hexColor}>{status}</color>\n" +
                    $"Current User: <color={hexColor}>{PhotonNetwork.LocalPlayer.NickName.ToUpper()}</color> \n" +
                    $"Current Ping: <color={hexColor}>{PhotonNetwork.GetPing().ToString().ToUpper()}</color>\n" +
                    $"Current FPS: <color={hexColor}>{fps}</color> \nCurrent Room: <color={hexColor}>{(PhotonNetwork.InRoom ? PhotonNetwork.CurrentRoom.Name.ToUpper() : "Not Connected To A Room")} </color> \n\n" +
                    $" <color={hexColor}>I Hope You Enjoy The Menu</color>";

                textMeshPro.alignment = TextAlignmentOptions.Top;

                GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/CodeOfConductHeadingText").GetComponent<TextMeshPro>().text = $"<color={hexColor}>Menu Meanings</color>\n-----------------------------";
                GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/CodeOfConductHeadingText").GetComponent<TextMeshPro>().color = RGB.color;
                TextMeshPro textMeshPro2 = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/COCBodyText_TitleData").GetComponent<TextMeshPro>();
                textMeshPro2.GetComponent<TextMeshPro>().color = DFade.color;
                textMeshPro2.text = $"" +
                    $"\n[D?] - Maybe Detected \n" +
                    $"[D] - Detected\n" +
                    $"[U] - Use\n" +
                    $"[P] - Primary\n" +
                    $"[S] - Secondary\n" +
                    $"[G] - Grip\n" +
                    $"[T] - Trigger\n" +
                    $"[W?] - Maybe Working\n" +
                    $"[B] - Buggy\n\n" +
                    $"If A Mod Has No Symbol It Is Probably Because I Forgot To Put One";
                textMeshPro2.alignment = TextAlignmentOptions.Top;

                GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/GameModes Title Text").GetComponent<TextMeshPro>().text = "Hidden";
                GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/GameModes Title Text").GetComponent<TextMeshPro>().color = RGB.color;
            }
            catch
            {
            }
            try
            {
                if (playerInstance == null || taggerInstance == null)
                {
                    UnityEngine.Debug.LogError("Player instance or GorillaTagger is null. Skipping menu updates.");
                    return;
                }

                foreach (ButtonHandler.Button bt in ModButtons.buttons)
                {
                    try
                    {
                        if (bt.Enabled && bt.onEnable != null)
                        {
                            bt.onEnable.Invoke();
                        }
                    }
                    catch (Exception ex)
                    {
                        UnityEngine.Debug.LogError($"Error invoking button action: {bt.buttonText}. Exception: {ex}");
                    }
                }

                if (NotificationLib.Instance != null)
                {
                    try
                    {
                        NotificationLib.Instance.UpdateNotifications();
                    }
                    catch (Exception ex)
                    {
                        UnityEngine.Debug.LogError($"Error updating notifications. Exception: {ex}");
                    }
                }

                if (UnityInput.Current.GetKeyDown(PCMenuKey))
                {
                    PCMenuOpen = !PCMenuOpen;
                }

                HandleMenuInteraction();
                UpdateClr();
            }
            catch (NullReferenceException ex)
            {
                UnityEngine.Debug.LogError($"NullReferenceException: {ex.Message}\nStack Trace: {ex.StackTrace}");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"Unexpected error: {ex.Message}\nStack Trace: {ex.StackTrace}");
            }
        }
        public static float j = 0f;
        public static float k = 0.2f;
        public static void Trail(GameObject obj, Color clr, Color clr2)
        {
            GameObject trailObject = new GameObject("trail");
            trailObject.transform.position = obj.transform.position;
            trailObject.transform.SetParent(obj.transform);
            TrailRenderer trailRenderer = trailObject.AddComponent<TrailRenderer>();
            trailRenderer.material = new Material(Shader.Find("Unlit/Color"));
            trailRenderer.material.color = clr;
            trailRenderer.time = 0.5f;
            trailRenderer.startWidth = 0.025f;
            trailRenderer.endWidth = 0f;
            trailRenderer.startColor = clr;
            trailRenderer.endColor = clr2;
        }
        public void Awake()
        {
            ResourceLoader.LoadResources();
            taggerInstance = GorillaTagger.Instance;
            playerInstance = GorillaLocomotion.GTPlayer.Instance;
            pollerInstance = ControllerInputPoller.instance;
            thirdPersonCamera = GameObject.Find("Player Objects/Third Person Camera/Shoulder Camera");
            cm = GameObject.Find("Player Objects/Third Person Camera/Shoulder Camera/CM vcam1");

            ExitGames.Client.Photon.Hashtable table = Photon.Pun.PhotonNetwork.LocalPlayer.CustomProperties;
            table.Add("Hidden Menu", true);
            Photon.Pun.PhotonNetwork.LocalPlayer.SetCustomProperties(table);
        }

        static string status = new WebClient().DownloadString("https://raw.githubusercontent.com/menker-cs/Hidden/refs/heads/main/status.txt");
        public static void HandleMenuInteraction()
        {
            try
            {
                if (PCMenuOpen && !InMenuCondition && !pollerInstance.leftControllerPrimaryButton && !pollerInstance.rightControllerPrimaryButton && !menuOpen)
                {
                    InPcCondition = true;
                    cm?.SetActive(false);

                    if (menuObj == null)
                    {
                        Draw();
                        AddButtonClicker(thirdPersonCamera?.transform);
                    }
                    else
                    {
                        AddButtonClicker(thirdPersonCamera?.transform);

                        if (thirdPersonCamera != null)
                        {
                            PositionMenuForKeyboard();

                            try
                            {
                                if (Mouse.current.leftButton.isPressed)
                                {
                                    Ray ray = thirdPersonCamera.GetComponent<Camera>().ScreenPointToRay(Mouse.current.position.ReadValue());
                                    if (Physics.Raycast(ray, out RaycastHit hit))
                                    {
                                        BtnCollider btnCollider = hit.collider?.GetComponent<BtnCollider>();
                                        if (btnCollider != null && clickerObj != null)
                                        {
                                            btnCollider.OnTriggerEnter(clickerObj.GetComponent<Collider>());
                                        }
                                    }
                                }
                                else if (clickerObj != null)
                                {
                                    Optimizations.DestroyObject(ref clickerObj);
                                }
                            }
                            catch (Exception ex)
                            {
                                UnityEngine.Debug.LogError($"Error handling mouse click. Exception: {ex}");
                            }
                        }
                    }
                }
                else if (menuObj != null && InPcCondition)
                {
                    InPcCondition = false;
                    CleanupMenu(0);
                    cm?.SetActive(true);
                }

                openMenu = rightHandedMenu ? pollerInstance.rightControllerPrimaryButton : pollerInstance.leftControllerSecondaryButton;

                if (openMenu && !InPcCondition)
                {
                    InMenuCondition = true;
                    if (menuObj == null)
                    {
                        Draw();
                        AddRigidbodyToMenu();
                        AddButtonClicker(rightHandedMenu ? playerInstance.leftControllerTransform : playerInstance.rightControllerTransform);
                    }
                    else
                    {
                        PositionMenuForHand();
                    }
                }
                else if (menuObj != null && InMenuCondition)
                {
                    InMenuCondition = false;
                    AddRigidbodyToMenu();

                    Vector3 currentVelocity = rightHandedMenu ? playerInstance.rightHandCenterVelocityTracker.GetAverageVelocity(true, 0f, false) : playerInstance.leftHandCenterVelocityTracker.GetAverageVelocity(true, 0f, false);
                    if (Vector3.Distance(currentVelocity, previousVelocity) > velocityThreshold)
                    {
                        currentMenuRigidbody.velocity = currentVelocity;
                        previousVelocity = currentVelocity;
                    }

                    CleanupMenu(1);
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"Error handling menu interaction. Exception: {ex}");
            }
        }

        public static void Draw()
        {
            if (menuObj != null)
            {
                ClearMenuObjects();
                return;
            }

            CreateMenuObject();
            CreateBackground();
            CreateMenuCanvasAndTitle();
            AddDisconnectButton();
            AddReturnButton();
            AddTitleAndFPSCounter();
            AddPageButton(">");
            AddPageButton("<");

            ButtonPool.ResetPool();
            var PageToDraw = GetButtonInfoByPage(currentPage).Skip(currentCategoryPage * ButtonsPerPage).Take(ButtonsPerPage).ToArray();
            for (int i = 0; i < PageToDraw.Length; i++)
            {
                AddModButtons(i * 0.09f, PageToDraw[i]);
            }
        }
        static Vector3 pos = new Vector3(0.56f, 0.54f, 0.23f);
        private static void CreateMenuObject()
        {
            // Menu Object
            menuObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            menuObj.GetComponent<BoxCollider>();
            Destroy(menuObj.GetComponent<Renderer>());
            menuObj.name = "menu";
            menuObj.transform.localScale = new Vector3(0.1f, 0.3f, 0.3825f);
        }
        public static void Outline(GameObject obj, Color clr)
        {
            if (outl)
            {
                GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
                UnityEngine.Object.Destroy(gameObject.GetComponent<BoxCollider>());
                gameObject.transform.parent = obj.transform;
                gameObject.transform.rotation = Quaternion.identity;
                gameObject.transform.localPosition = obj.transform.localPosition;
                gameObject.transform.localScale = obj.transform.localScale + new Vector3(-0.01f, 0.0145f, 0.0145f);
                gameObject.GetComponent<Renderer>().material.color = clr;
            }
        }
        private static void CreateBackground()
        {
            // Background
            background = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Destroy(background.GetComponent<Rigidbody>());
            Destroy(background.GetComponent<BoxCollider>());
            Outline(background, outColor);

            background.transform.parent = menuObj.transform;
            background.transform.rotation = Quaternion.identity;
            background.transform.localScale = new Vector3(0.1f, 1f, 1.03f);
            background.name = "menucolor";
            background.transform.position = new Vector3(0.05f, 0f, 0f);

            if (Theme == 2 || Theme == 3)
            {
                background.GetComponent<MeshRenderer>().material = Theme == 2 ? DFade : DBreath;
            }
            else if (Theme == 4)
            {
                background.GetComponent<MeshRenderer>().material = RGB;
            }
            else
            {
                background.GetComponent<MeshRenderer>().material.color = MenuColor;
            }
        }
        #region settings
        public static int Theme = 1;
        public static Color MenuColorT = ColorLib.Hidden;
        public static Color MenuColor = ColorLib.Hidden;
        public static Color ButtonColorOff = DarkGrey;
        public static Color ButtonColorOn = new Color32(35,35,35,255);
        public static Color outColor = DarkerGrey;
        public static Color DisconnecyColor = ButtonColorOff;
        public static Color disOut = outColor;
        public static void ChangeTheme()
        {
            Theme++;
            if (Theme > 7)
            {
                Theme = 1;
                RefreshMenu();
            }
            foreach (ButtonHandler.Button btn in ModButtons.buttons)
            {
                if (Theme == 1)
                {
                    if (btn.buttonText == "Change Theme: Green")
                    {
                        btn.SetText("Change Theme: Dark");
                        MenuColorT = ColorLib.Hidden;
                        MenuColor = ColorLib.Hidden;
                        ButtonColorOff = DarkGrey;
                        ButtonColorOn = new Color32(35, 35, 35, 255);
                        outColor = DarkerGrey;
                        DisconnecyColor = ButtonColorOff;
                        disOut = outColor;
                        NotificationLib.SendNotification("<color=white>[</color><color=blue>Theme</color><color=white>] Dark</color>");
                        RefreshMenu();
                    }
                }
                if (Theme == 2)
                {
                    if (btn.buttonText == "Change Theme: Dark")
                    {
                        btn.SetText("Change Theme: Fading");
                        MenuColorT = ColorLib.Hidden;
                        MenuColor = ColorLib.Hidden;
                        ButtonColorOff = DarkGrey;
                        ButtonColorOn = new Color32(35, 35, 35, 255);
                        outColor = DarkerGrey;
                        DisconnecyColor = ButtonColorOff;
                        disOut = outColor;
                        NotificationLib.SendNotification("<color=white>[</color><color=blue>Theme</color><color=white>] Fading</color>");
                        RefreshMenu();
                    }
                }
                if (Theme == 3)
                {
                    if (btn.buttonText == "Change Theme: Fading")
                    {
                        btn.SetText("Change Theme: Breathing");
                        MenuColorT = ColorLib.Hidden;
                        MenuColor = ColorLib.Hidden;
                        ButtonColorOff = DarkGrey;
                        ButtonColorOn = new Color32(35, 35, 35, 255);
                        outColor = DarkerGrey;
                        DisconnecyColor = ButtonColorOff;
                        disOut = outColor;
                        NotificationLib.SendNotification("<color=white>[</color><color=blue>Theme</color><color=white>] Breathing</color>");
                        RefreshMenu();
                    }
                }
                if (Theme == 4)
                {
                    if (btn.buttonText == "Change Theme: Breathing")
                    {
                        btn.SetText("Change Theme: RGB");
                        MenuColorT = ColorLib.Hidden;
                        MenuColor = RGB.color;
                        ButtonColorOff = new Color32(30, 30, 30, 255);
                        ButtonColorOn = DarkerGrey;
                        outColor = Black;
                        DisconnecyColor = ButtonColorOff;
                        disOut = outColor;
                        NotificationLib.SendNotification("<color=white>[</color><color=blue>Theme</color><color=white>] RGB</color>");
                        RefreshMenu();
                    }
                }
                if (Theme == 5)
                {
                    if (btn.buttonText == "Change Theme: RGB")
                    {
                        btn.SetText("Change Theme: Blue");
                        MenuColorT = SkyBlueTransparent;
                        MenuColor = SkyBlue;
                        ButtonColorOff = RoyalBlue;
                        ButtonColorOn = DodgerBlue;
                        outColor = DarkDodgerBlue;
                        DisconnecyColor = MenuColor;
                        disOut = outColor;
                        NotificationLib.SendNotification("<color=white>[</color><color=blue>Theme</color><color=white>] Blue</color>");
                        RefreshMenu();
                    }
                }
                if (Theme == 6)
                {

                    if (btn.buttonText == "Change Theme: Blue")
                    {
                        btn.SetText("Change Theme: Red");
                        MenuColorT = FireBrickTransparent;
                        MenuColor = FireBrick;
                        ButtonColorOff = WineRed;
                        ButtonColorOn = IndianRed;
                        outColor = IndianRed;
                        DisconnecyColor = Crimson;
                        disOut = WineRed;
                        NotificationLib.SendNotification("<color=white>[</color><color=blue>Theme</color><color=white>] Red</color>");
                        RefreshMenu();
                    }
                }
                if (Theme == 7)
                {
                    if (btn.buttonText == "Change Theme: Red")
                    {
                        btn.SetText("Change Theme: Green");
                        MenuColorT = MediumAquamarineTransparent;
                        MenuColor = MediumAquamarine;
                        ButtonColorOff = MediumSeaGreen;
                        ButtonColorOn = SeaGreen;
                        DisconnecyColor = ButtonColorOff;
                        outColor = Lime;
                        disOut = outColor;
                        NotificationLib.SendNotification("<color=white>[</color><color=blue>Theme</color><color=white>] Green</color>");
                        RefreshMenu();
                    }
                }
            }
        }
       
        public static int ActuallSound = 114;
        public static int LOJUHFDG = 6;
        public static void ChangeSound()
        {
            LOJUHFDG++;
            if (LOJUHFDG > 6)
            {
                LOJUHFDG = 1;
                ActuallSound = 66;
            }
            if (LOJUHFDG == 1)
            {
                ActuallSound = 66;
            }
            if (LOJUHFDG == 2)
            {
                ActuallSound = 8;
            }
            if (LOJUHFDG == 3)
            {
                ActuallSound = 203;
            }
            if (LOJUHFDG == 4)
            {
                ActuallSound = 50;
            }
            if (LOJUHFDG == 5)
            {
                ActuallSound = 67;
            }
            if (LOJUHFDG == 6)
            {
                ActuallSound = 114;
            }
        }
        public static int Laytou = 1;
        public static void ChangeLayout()
        {
            Laytou++;
            if (Laytou > 3)
            {
                Laytou = 1;
                RefreshMenu();
            }
            foreach (ButtonHandler.Button btn in ModButtons.buttons)
            {
                if (Laytou == 1)
                {
                    if (btn.buttonText == "Change Layout: Top")
                    {
                        btn.SetText("Change Layout: Sides");
                        NotificationLib.SendNotification("<color=white>[</color><color=blue>Layout</color><color=white>] Sides</color>");
                    }
                }
                if (Laytou == 2)
                {
                    if (btn.buttonText == "Change Layout: Sides")
                    {
                        btn.SetText("Change Layout: Bottom");
                        NotificationLib.SendNotification("<color=white>[</color><color=blue>Layout</color><color=white>] Bottom</color>");
                    }
                }
                if (Laytou == 3)
                {
                    if (btn.buttonText == "Change Layout: Bottom")
                    {
                        btn.SetText("Change Layout: Top");
                        NotificationLib.SendNotification("<color=white>[</color><color=blue>Layout</color><color=white>] Top</color>");
                    }
                }
            }
            RefreshMenu();
        }
        #endregion
        public static void AddDisconnectButton()
        {
            if (toggledisconnectButton)
            {
                // Disconnect Button
                disconnectButton = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Destroy(disconnectButton.GetComponent<Rigidbody>());
                disconnectButton.GetComponent<BoxCollider>().isTrigger = true;
                //RoundObj(disconnectButton);
                Outline(disconnectButton, disOut);
                disconnectButton.transform.parent = menuObj.transform;
                disconnectButton.transform.rotation = Quaternion.identity;
                disconnectButton.transform.localScale = new Vector3(0.09f, 0.9f, 0.08f);
                disconnectButton.transform.localPosition = new Vector3(0.56f, 0f, 0.59f);
                disconnectButton.AddComponent<BtnCollider>().clickedButton = new ButtonHandler.Button("DisconnectButton", Category.Home, false, false, null, null);
                disconnectButton.GetComponent<Renderer>().material.color = DisconnecyColor;

                // Disconnect Button Text
                Text discontext = new GameObject { transform = { parent = canvasObj.transform } }.AddComponent<Text>();
                discontext.text = "Disconnect";
                discontext.font = font;
                discontext.fontStyle = FontStyle.Bold;
                if (Theme == 5)
                {
                    discontext.color = Black;
                }
                else
                {
                    discontext.color = White;
                }
                discontext.alignment = TextAnchor.MiddleCenter;
                discontext.resizeTextForBestFit = true;
                discontext.resizeTextMinSize = 0;
                RectTransform rectt = discontext.GetComponent<RectTransform>();
                rectt.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                rectt.sizeDelta = new Vector2(0.13f, 0.023f);
                rectt.localPosition = new Vector3(0.064f, 0f, 0.227f);
                rectt.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            }
        }
        private static GameObject StumpText = new GameObject("Stump");
        public static void Stumpy()
        {
            if (StumpText == null)
            {
                StumpText = new GameObject("Stump");
            }

            TextMeshPro tmp = StumpText.GetComponent<TextMeshPro>();
            if (tmp == null)
            {
                tmp = StumpText.AddComponent<TextMeshPro>();
                tmp.fontSize = 2f;
                tmp.fontStyle = FontStyles.Bold;
                tmp.characterSpacing = 1f;
                tmp.alignment = TextAlignmentOptions.Center;
                tmp.color = ColorLib.Hidden;
                tmp.font = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdBodyText").GetComponent<TextMeshPro>().font;
            }

            tmp.text =
                $"<color={hexColor}>Hidden Menu</color>\n" +
                $"<size=2>Status: <color={hexColor}>{status}</color>\n" +
                $"VERSION: <color={hexColor}>{menuVersion}</color></size>\n" +
                $"<size=1.5>Made By <color={hexColor}>Menker</color>";

            StumpText.transform.position = new Vector3(-66.8087f, 12.1808f, -82.5265f);
            StumpText.transform.LookAt(Camera.main.transform);
            StumpText.transform.Rotate(0f, 180f, 0f);
        }

        public static void STUMPY()
        {
            UnityEngine.Object.Destroy(StumpText);
        }
        private static void CreateMenuCanvasAndTitle()
        {
            // Menu Canvas
            canvasObj = new GameObject();
            canvasObj.transform.parent = menuObj.transform;
            canvasObj.name = "canvas";
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            CanvasScaler canvasScale = canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvasScale.dynamicPixelsPerUnit = 1000;

            // Menu Title
            GameObject titleObj = new GameObject();
            titleObj.transform.parent = canvasObj.transform;
            titleObj.transform.localScale = new Vector3(0.875f, 0.875f, 1f);
            title = titleObj.AddComponent<Text>();
            title.font = font;
            title.fontStyle = FontStyle.Bold;
            if (Theme == 5)
            {
                title.color = Black;
            }
            else
            {
                title.color = White;
            }

            title.fontSize = 5;
            title.alignment = TextAnchor.MiddleCenter;
            title.resizeTextForBestFit = true;
            title.resizeTextMinSize = 0;
            RectTransform titleTransform = title.GetComponent<RectTransform>();
            titleTransform.localPosition = Vector3.zero;
            titleTransform.position = new Vector3(0.07f, 0f, .1645f);
            titleTransform.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            titleTransform.sizeDelta = new Vector2(0.295f, 0.06f);
        }
        public static void AddTitleAndFPSCounter()
        {
            fps = (Time.deltaTime > 0) ? Mathf.RoundToInt(1.0f / Time.deltaTime) : 0;
            title.fontStyle = FontStyle.Bold;
            title.font = font;

            title.text =
            $"{menuName}{(vCounter ? " ┇ V" + menuVersion[0] : "")}";
        }
        public static void AddModButtons(float offset, ButtonHandler.Button button)
        {
            // Mod Buttons
            ModButton = ButtonPool.GetButton();
            Rigidbody btnRigidbody = ModButton.GetComponent<Rigidbody>();
            if (btnRigidbody != null) { Destroy(btnRigidbody); }
            BoxCollider btnCollider = ModButton.GetComponent<BoxCollider>();
            if (btnCollider != null) { btnCollider.isTrigger = true; }
            ModButton.transform.SetParent(menuObj.transform, false);
            ModButton.transform.rotation = Quaternion.identity;
            ModButton.transform.localScale = new Vector3(0.09f, 0.9f, 0.08f);
            ModButton.transform.localPosition = new Vector3(0.56f, 0f, Laytou == 3 ? 0.225f - offset : 0.32f - offset);
            BtnCollider btnColScript = ModButton.GetComponent<BtnCollider>() ?? ModButton.AddComponent<BtnCollider>();
            btnColScript.clickedButton = button;
            // Mod Buttons Text
            GameObject titleObj = TextPool.GetTextObject();
            titleObj.transform.SetParent(canvasObj.transform, false);
            titleObj.transform.localScale = new Vector3(0.95f, 0.95f, 1f);
            Text title = titleObj.GetComponent<Text>();
            title.text = button.buttonText;
            title.font = font;
            title.fontStyle = FontStyle.Bold;
            if (Theme == 5) { title.color = Black; }
            else { title.color = White; }

            RectTransform titleTransform = title.GetComponent<RectTransform>();
            titleTransform.localPosition = new Vector3(.064f, 0,Laytou == 3 ? .089f - offset / 2.6f : .126f - offset / 2.6f);
            titleTransform.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            titleTransform.sizeDelta = new Vector2(0.21f, 0.02225f);

            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Destroy(gameObject.GetComponent<Collider>());
            Outline(gameObject, outColor);
            gameObject.transform.parent = menuObj.transform;
            gameObject.transform.position = ModButton.transform.position;
            gameObject.transform.rotation = ModButton.transform.rotation;
            gameObject.GetComponent<Renderer>().material.color = ModButton.GetComponent<Renderer>().material.color;
            gameObject.transform.localScale = new Vector3(ModButton.transform.localScale.x - 0.006f, ModButton.transform.localScale.y + 0.005f, ModButton.transform.localScale.z + 0.005f);

            Renderer btnRenderer = ModButton.GetComponent<Renderer>();
            if (btnRenderer != null)
            {
                if (button.Enabled)
                {
                    btnRenderer.material.color = ButtonColorOn;
                }
                else
                {
                    btnRenderer.material.color = ButtonColorOff;
                }
            }
        }
        public static void AddPageButton(string button)
        {
            if (Laytou == 1)
            {
                PageButtons = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Destroy(PageButtons.GetComponent<Rigidbody>());
                PageButtons.GetComponent<BoxCollider>().isTrigger = true;
                //RoundObj(PageButtons);
                Outline(PageButtons, outColor);
                PageButtons.transform.parent = menuObj.transform;
                PageButtons.transform.rotation = Quaternion.identity;
                PageButtons.transform.localScale = new Vector3(0.09f, 0.15f, 0.9f);
                PageButtons.transform.localPosition = new Vector3(0.56f, button.Contains("<") ? 0.65f : -0.65f, -0);
                PageButtons.GetComponent<Renderer>().material.color = ButtonColorOff;
                PageButtons.AddComponent<BtnCollider>().clickedButton = new ButtonHandler.Button(button, Category.Home, false, false, null, null);

                GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Destroy(gameObject.GetComponent<Collider>());
                if (Theme == 0)
                {
                    Outline(gameObject, DarkerGrey);
                }
                else
                {
                    Outline(gameObject, outColor);
                }
                gameObject.transform.parent = menuObj.transform;
                gameObject.transform.position = PageButtons.transform.position;
                gameObject.transform.rotation = PageButtons.transform.rotation;
                gameObject.GetComponent<Renderer>().material.color = ButtonColorOff;
                gameObject.transform.localScale = new Vector3(PageButtons.transform.localScale.x - 0.006f, PageButtons.transform.localScale.y + 0.005f, PageButtons.transform.localScale.z + 0.005f);

                // Page Buttons Text
                GameObject titleObj = new GameObject();
                titleObj.transform.parent = canvasObj.transform;
                titleObj.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                Text title = titleObj.AddComponent<Text>();
                title.font = font;
                if (Theme == 5)
                {
                    title.color = Black;
                }
                else
                {
                    title.color = White;
                }
                title.fontSize = 5;
                title.fontStyle = FontStyle.Bold;
                title.alignment = TextAnchor.MiddleCenter;
                title.resizeTextForBestFit = true;
                title.resizeTextMinSize = 0;
                RectTransform titleTransform = title.GetComponent<RectTransform>();
                titleTransform.localPosition = Vector3.zero;
                titleTransform.sizeDelta = new Vector2(0.5f, 0.06f);
                title.text = button.Contains("<") ? "⋘" : "⋙";
                titleTransform.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
                titleTransform.position = new Vector3(0.064f, button.Contains("<") ? 0.1955f : -0.1955f, 0f);
            }
            if (Laytou == 2)
            {
                // Page Buttons
                PageButtons = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Destroy(PageButtons.GetComponent<Rigidbody>());
                PageButtons.GetComponent<BoxCollider>().isTrigger = true; ;
                PageButtons.transform.parent = menuObj.transform;
                PageButtons.transform.rotation = Quaternion.identity;
                PageButtons.transform.localScale = new Vector3(0.09f, 0.25f, 0.079f);
                PageButtons.transform.localPosition = new Vector3(0.56f, button.Contains("<") ? 0.2925f : -0.2925f, -0.435f);
                PageButtons.GetComponent<Renderer>().material.color = ButtonColorOff;
                PageButtons.AddComponent<BtnCollider>().clickedButton = new ButtonHandler.Button(button, Category.Home, false, false, null, null);

                GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Destroy(gameObject.GetComponent<Collider>());
                if (Theme == 0)
                {
                    Outline(gameObject, DarkerGrey);
                }
                else
                {
                    Outline(gameObject, outColor);
                }
                gameObject.transform.parent = menuObj.transform;
                gameObject.transform.position = PageButtons.transform.position;
                gameObject.transform.rotation = PageButtons.transform.rotation;
                gameObject.GetComponent<Renderer>().material.color = ButtonColorOff;
                gameObject.transform.localScale = new Vector3(PageButtons.transform.localScale.x - 0.006f, PageButtons.transform.localScale.y + 0.005f, PageButtons.transform.localScale.z + 0.005f);

                // Page Buttons Text
                GameObject titleObj = new GameObject();
                titleObj.transform.parent = canvasObj.transform;
                titleObj.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                Text title = titleObj.AddComponent<Text>();
                title.font = font;
                if (Theme == 5)
                {
                    title.color = Black;
                }
                else
                {
                    title.color = White;
                }
                title.fontSize = 5;
                title.fontStyle = FontStyle.Bold;
                title.alignment = TextAnchor.MiddleCenter;
                title.resizeTextForBestFit = true;
                title.resizeTextMinSize = 0;
                RectTransform titleTransform = title.GetComponent<RectTransform>();
                titleTransform.localPosition = Vector3.zero;
                titleTransform.sizeDelta = new Vector2(0.5f, 0.05f);
                title.text = button.Contains("<") ? "⋘" : "⋙";
                titleTransform.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
                titleTransform.position = new Vector3(0.064f, button.Contains("<") ? 0.087f : -.087f, -0.163f);
            }
            if (Laytou == 3)
            {
                // Page Buttons
                PageButtons = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Destroy(PageButtons.GetComponent<Rigidbody>());
                PageButtons.GetComponent<BoxCollider>().isTrigger = true; ;
                PageButtons.transform.parent = menuObj.transform;
                PageButtons.transform.rotation = Quaternion.identity;
                PageButtons.transform.localScale = new Vector3(0.09f, 0.25f, 0.079f);
                PageButtons.transform.localPosition = new Vector3(0.56f, button.Contains("<") ? 0.2925f : -0.2925f, 0.3223f);
                PageButtons.GetComponent<Renderer>().material.color = ButtonColorOff;
                PageButtons.AddComponent<BtnCollider>().clickedButton = new ButtonHandler.Button(button, Category.Home, false, false, null, null);

                GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Destroy(gameObject.GetComponent<Collider>());
                if (Theme == 0)
                {
                    Outline(gameObject, DarkerGrey);
                }
                else
                {
                    Outline(gameObject, outColor);
                }
                gameObject.transform.parent = menuObj.transform;
                gameObject.transform.position = PageButtons.transform.position;
                gameObject.transform.rotation = PageButtons.transform.rotation;
                gameObject.GetComponent<Renderer>().material.color = ButtonColorOff;
                gameObject.transform.localScale = new Vector3(PageButtons.transform.localScale.x - 0.006f, PageButtons.transform.localScale.y + 0.005f, PageButtons.transform.localScale.z + 0.005f);

                // Page Buttons Text
                // Page Buttons Text
                GameObject titleObj = new GameObject();
                titleObj.transform.parent = canvasObj.transform;
                titleObj.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                Text title = titleObj.AddComponent<Text>();
                title.font = font;
                if (Theme == 5)
                {
                    title.color = Black;
                }
                else
                {
                    title.color = White;
                }
                title.fontSize = 5;
                title.fontStyle = FontStyle.Bold;
                title.alignment = TextAnchor.MiddleCenter;
                title.resizeTextForBestFit = true;
                title.resizeTextMinSize = 0;
                RectTransform titleTransform = title.GetComponent<RectTransform>();
                titleTransform.localPosition = Vector3.zero;
                titleTransform.sizeDelta = new Vector2(0.5f, 0.05f);
                title.text = button.Contains("<") ? "⋘" : "⋙";
                titleTransform.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
                titleTransform.position = new Vector3(0.064f, button.Contains("<") ? 0.087f : -.087f, 0.124f);
            }
        }
        public static void AddReturnButton()
        {
            if (currentPage != Category.Home)
            {
                if (Laytou == 2)
                {
                    // Return Button
                    GameObject BackToStartButton = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    Destroy(BackToStartButton.GetComponent<Rigidbody>());
                    BackToStartButton.GetComponent<BoxCollider>().isTrigger = true;
                    BackToStartButton.transform.parent = menuObj.transform;
                    BackToStartButton.transform.rotation = Quaternion.identity;
                    BackToStartButton.transform.localScale = new Vector3(0.09f, 0.30625f, 0.08f);
                    BackToStartButton.transform.localPosition = new Vector3(0.56f, 0f, -0.435f);
                    BackToStartButton.AddComponent<BtnCollider>().clickedButton = new ButtonHandler.Button("ReturnButton", Category.Home, false, false, null, null);
                    BackToStartButton.GetComponent<Renderer>().material.color = ButtonColorOff;

                    GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    Destroy(gameObject.GetComponent<Collider>());
                    if (Theme == 0)
                    {
                        Outline(gameObject, DarkerGrey);
                    }
                    else
                    {
                        Outline(gameObject, outColor);
                    }
                    gameObject.transform.parent = menuObj.transform;
                    gameObject.transform.position = BackToStartButton.transform.position;
                    gameObject.transform.rotation = BackToStartButton.transform.rotation;
                    gameObject.GetComponent<Renderer>().material.color = ButtonColorOff;
                    gameObject.transform.localScale = new Vector3(BackToStartButton.transform.localScale.x - 0.006f, BackToStartButton.transform.localScale.y + 0.005f, BackToStartButton.transform.localScale.z + 0.005f);

                    // Return Button Text
                    GameObject titleObj = new GameObject();
                    titleObj.transform.parent = canvasObj.transform;
                    titleObj.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                    titleObj.transform.localPosition = new Vector3(0.85f, 0.85f, 0.85f);
                    Text title = titleObj.AddComponent<Text>();
                    title.font = font;
                    title.fontStyle = FontStyle.Bold;
                    title.text = "Return";
                    if (Theme == 5)
                    {
                        title.color = Black;
                    }
                    else
                    {
                        title.color = White;
                    }
                    title.fontSize = 3;
                    title.alignment = TextAnchor.MiddleCenter;
                    title.resizeTextForBestFit = true;
                    title.resizeTextMinSize = 0;
                    RectTransform titleTransform = title.GetComponent<RectTransform>();
                    titleTransform.localPosition = Vector3.zero;
                    titleTransform.sizeDelta = new Vector2(0.25f, 0.025f);
                    titleTransform.localPosition = new Vector3(.064f, 0f, -0.165f);
                    titleTransform.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
                }
                else if (Laytou == 3)
                {
                    // Return Button
                    GameObject BackToStartButton = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    Destroy(BackToStartButton.GetComponent<Rigidbody>());
                    BackToStartButton.GetComponent<BoxCollider>().isTrigger = true;
                    BackToStartButton.transform.parent = menuObj.transform;
                    BackToStartButton.transform.rotation = Quaternion.identity;
                    BackToStartButton.transform.localScale = new Vector3(0.09f, 0.30625f, 0.08f);
                    BackToStartButton.transform.localPosition = new Vector3(0.56f, 0f, 0.3223f);
                    BackToStartButton.AddComponent<BtnCollider>().clickedButton = new ButtonHandler.Button("ReturnButton", Category.Home, false, false, null, null);
                    BackToStartButton.GetComponent<Renderer>().material.color = ButtonColorOff;

                    GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    Destroy(gameObject.GetComponent<Collider>());
                    if (Theme == 0)
                    {
                        Outline(gameObject, DarkerGrey);
                    }
                    else
                    {
                        Outline(gameObject, outColor);
                    }
                    gameObject.transform.parent = menuObj.transform;
                    gameObject.transform.position = BackToStartButton.transform.position;
                    gameObject.transform.rotation = BackToStartButton.transform.rotation;
                    gameObject.GetComponent<Renderer>().material.color = ButtonColorOff;
                    gameObject.transform.localScale = new Vector3(BackToStartButton.transform.localScale.x - 0.006f, BackToStartButton.transform.localScale.y + 0.005f, BackToStartButton.transform.localScale.z + 0.005f);

                    // Return Button Text
                    GameObject titleObj = new GameObject();
                    titleObj.transform.parent = canvasObj.transform;
                    titleObj.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                    titleObj.transform.localPosition = new Vector3(0.85f, 0.85f, 0.85f);
                    Text title = titleObj.AddComponent<Text>();
                    title.font = font;
                    title.fontStyle = FontStyle.Bold;
                    title.text = "Return";
                    if (Theme == 5)
                    {
                        title.color = Black;
                    }
                    else
                    {
                        title.color = White;
                    }
                    title.fontSize = 3;
                    title.alignment = TextAnchor.MiddleCenter;
                    title.resizeTextForBestFit = true;
                    title.resizeTextMinSize = 0;
                    RectTransform titleTransform = title.GetComponent<RectTransform>();
                    titleTransform.localPosition = Vector3.zero;
                    titleTransform.sizeDelta = new Vector2(0.25f, 0.025f);
                    titleTransform.localPosition = new Vector3(.064f, 0f, 0.1235f);
                    titleTransform.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
                }
                else
                {
                    // Return Button
                    GameObject BackToStartButton = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    Destroy(BackToStartButton.GetComponent<Rigidbody>());
                    BackToStartButton.GetComponent<BoxCollider>().isTrigger = true;
                    BackToStartButton.transform.parent = menuObj.transform;
                    BackToStartButton.transform.rotation = Quaternion.identity;
                    BackToStartButton.transform.localScale = new Vector3(0.09f, 0.82f, 0.08f);
                    BackToStartButton.transform.localPosition = new Vector3(0.56f, 0f, -0.435f);
                    BackToStartButton.AddComponent<BtnCollider>().clickedButton = new ButtonHandler.Button("ReturnButton", Category.Home, false, false, null, null);
                    BackToStartButton.GetComponent<Renderer>().material.color = ButtonColorOff;

                    GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    Destroy(gameObject.GetComponent<Collider>());
                    if (Theme == 0)
                    {
                        Outline(gameObject, DarkerGrey);
                    }
                    else
                    {
                        Outline(gameObject, outColor);
                    }
                    gameObject.transform.parent = menuObj.transform;
                    gameObject.transform.position = BackToStartButton.transform.position;
                    gameObject.transform.rotation = BackToStartButton.transform.rotation;
                    gameObject.GetComponent<Renderer>().material.color = ButtonColorOff;
                    gameObject.transform.localScale = new Vector3(BackToStartButton.transform.localScale.x - 0.006f, BackToStartButton.transform.localScale.y + 0.005f, BackToStartButton.transform.localScale.z + 0.005f);

                    // Return Button Text
                    GameObject titleObj = new GameObject();
                    titleObj.transform.parent = canvasObj.transform;
                    titleObj.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                    titleObj.transform.localPosition = new Vector3(0.85f, 0.85f, 0.85f);
                    Text title = titleObj.AddComponent<Text>();
                    title.font = font;
                    title.fontStyle = FontStyle.Bold;
                    title.text = "Return";
                    if (Theme == 5)
                    {
                        title.color = Black;
                    }
                    else
                    {
                        title.color = White;
                    }
                    title.fontSize = 3;
                    title.alignment = TextAnchor.MiddleCenter;
                    title.resizeTextForBestFit = true;
                    title.resizeTextMinSize = 0;
                    RectTransform titleTransform = title.GetComponent<RectTransform>();
                    titleTransform.localPosition = Vector3.zero;
                    titleTransform.sizeDelta = new Vector2(0.25f, 0.025f);
                    titleTransform.localPosition = new Vector3(.064f, 0f, -0.165f);
                    titleTransform.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
                }
            }
        }

        public static void AddButtonClicker(Transform parentTransform)
        {
            // Button Clicker
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
                    clickerRenderer.material.color = White;
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
        public static bool bark = false;
        private static void PositionMenuForHand()
        {
            if (bark)
            {
                menuObj.transform.position = GorillaTagger.Instance.headCollider.transform.position + GorillaTagger.Instance.headCollider.transform.forward * 0.5f + GorillaTagger.Instance.headCollider.transform.up * -0.1f;
                menuObj.transform.LookAt(GorillaTagger.Instance.headCollider.transform);
                Vector3 rotModify = menuObj.transform.rotation.eulerAngles;
                rotModify += new Vector3(-90f, 0f, -90f);
                menuObj.transform.rotation = Quaternion.Euler(rotModify);
            }
            else if (rightHandedMenu)
            {
                menuObj.transform.position = playerInstance.rightControllerTransform.position;
                Vector3 rotation = playerInstance.rightControllerTransform.rotation.eulerAngles;
                rotation += new Vector3(0f, 0f, 180f);
                menuObj.transform.rotation = Quaternion.Euler(rotation);
            }
            else
            {
                menuObj.transform.position = playerInstance.leftControllerTransform.position;
                menuObj.transform.rotation = playerInstance.leftControllerTransform.rotation;
            }
        }
        private static void PositionMenuForKeyboard()
        {
            if (thirdPersonCamera != null)
            {
                thirdPersonCamera.transform.position = new Vector3(-65.3f, 12.5f, -82.5692f);
                thirdPersonCamera.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                menuObj.transform.SetParent(thirdPersonCamera.transform, true);

                Vector3 headPosition = thirdPersonCamera.transform.position;
                Quaternion headRotation = thirdPersonCamera.transform.rotation;
                float offsetDistance = 0.65f;
                Vector3 offsetPosition = headPosition + headRotation * Vector3.forward * offsetDistance;
                menuObj.transform.position = offsetPosition;

                Vector3 directionToHead = headPosition - menuObj.transform.position;
                menuObj.transform.rotation = Quaternion.LookRotation(directionToHead, Vector3.up);
                menuObj.transform.Rotate(Vector3.up, -90.0f);
                menuObj.transform.Rotate(Vector3.right, -90.0f);
            }
        }

        public static void AddRigidbodyToMenu()
        {
            if (currentMenuRigidbody == null && menuObj != null)
            {
                currentMenuRigidbody = menuObj.GetComponent<Rigidbody>();
                if (currentMenuRigidbody == null)
                {
                    currentMenuRigidbody = menuObj.AddComponent<Rigidbody>();
                }
                currentMenuRigidbody.useGravity = grav;
            }
        }
    }
}
