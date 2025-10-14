using BepInEx;
using Elixir.Mods;
using Elixir.Mods.Categories;
using Elixir.Utilities;
using Elixir.Utilities.Notifs;
using HarmonyLib;
using Oculus.Platform;
using Photon.Pun;
using System;
using System.Collections;
//using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;
using static Elixir.Initialization.PluginInfo;
using static Elixir.Menu.ButtonHandler;
using static Elixir.Menu.Optimizations;
using static Elixir.Utilities.ColorLib;
using static Elixir.Utilities.Variables;

namespace Elixir.Menu
{
    [HarmonyPatch(typeof(GorillaLocomotion.GTPlayer), "LateUpdate")]
    public class Main : MonoBehaviour
    {
        [HarmonyPrefix]
        public static void Prefix()
        {
            fps = (Time.deltaTime > 0) ? Mathf.RoundToInt(1 / Time.deltaTime) : 0;
            try
            {
                GameObject goop = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/SpectralGooPile (combined by EdMeshCombiner)");
                if (goop != null && computer != null && wallMonitor !=null)
                {
                    computer.material = goop.GetComponent<Renderer>().material;
                    wallMonitor.material = goop.GetComponent<Renderer>().material;
                    ChangeBoardMaterial("Environment Objects/LocalObjects_Prefab/TreeRoom", "UnityTempFile", 5, goop.GetComponent<Renderer>().material, ref originalMat1!);
                    ChangeBoardMaterial("Environment Objects/LocalObjects_Prefab/Forest", "UnityTempFile", 13, goop.GetComponent<Renderer>().material, ref originalMat2!);
                }
                //hi
                #region MOTD
                if (motdHeading == null || motdBody == null) return;
                motdHeading.SetText($"Elixir | V{Elixir.Initialization.PluginInfo.menuVersion}<color={hexColor1}>\n--------------------------------------------</color>");
                motdHeading.color = Pink;
                motdBody.color = Pink;
                motdBody.SetText($"" +
                    $"\nThank You For Using Elixir!\n\n" +
                    $"Status: <color={hexColor1}>Undetected</color>\n" +
                    $"Current User: <color={hexColor1}>{PhotonNetwork.LocalPlayer.NickName.ToUpper()}</color> \n" +
                    $"Current Ping: <color={hexColor1}>{PhotonNetwork.GetPing().ToString().ToUpper()}</color>\n" +
                    $"Current FPS: <color={hexColor1}>{fps}</color> \n" +
                    $"Current Room: <color={hexColor1}>{(PhotonNetwork.InRoom ? PhotonNetwork.CurrentRoom.Name.ToUpper() : "Not Connected To A Room")} </color> \n\n" +
                    $"<color={hexColor1}>I Hope You Enjoy The Menu</color> \n" +
                    $"Made by <color={hexColor1}>Menker</color>");

                motdBody.alignment = TextAlignmentOptions.Top;
                #endregion

                #region COC
                if (cocHeading == null || cocBody == null) return;
                cocHeading.SetText($"Menu Meanings<color={hexColor1}>\n-----------------------------</color>");
                cocHeading.color = Pink;

                cocBody.color = Pink;
                cocBody.SetText($"\n[D?] - Maybe Detected \n[D] - Detected\n[U] - Use\n[P] - Primary\n[S] - Secondary\n[G] - Grip\n[T] - Trigger\n[W?] - Maybe Working\n[B] - Buggy\n\nIf A Mod Has No Symbol It Is Probably Because I Forgot To Put One");
                cocBody.alignment = TextAlignmentOptions.Top;
                #endregion

                if (gameModeText == null) return;
                gameModeText.SetText("Elixir");
                gameModeText.color = RGB.color;
            }
            catch (NullReferenceException ex)
            {
                UnityEngine.Debug.LogError($"NullReferenceException: {ex.Message}\nStack Trace: {ex.StackTrace}");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"Unexpected error: {ex.Message}\nStack Trace: {ex.StackTrace}");
            }
            try
            {
                if (playerInstance == null || GorillaTagger.Instance == null)
                {
                    UnityEngine.Debug.LogError("Player instance or GorillaTagger is null. Skipping menu updates.");
                    return;
                }

                foreach (ButtonHandler.Button bt in ModButtons.buttons)
                {
                    try
                    {
                        if (bt.Enabled && bt.OnEnable != null)
                        {
                            bt.OnEnable.Invoke();
                        }
                    }
                    catch (Exception ex)
                    {
                        UnityEngine.Debug.LogError($"Error invoking button action: {bt.ButtonText}. Exception: {ex}");
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
        public static Material? originalMat1;
        public static Material? originalMat2;
        public static void ChangeBoardMaterial(string parentPath, string boardID, int targetIndex, Material newMaterial, ref Material originalMat)
        {
            GameObject parent = GameObject.Find(parentPath);
            if (parent == null)
                return;
            int currentIndex = 0;
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                GameObject childObj = parent.transform.GetChild(i).gameObject;
                if (childObj.name.Contains(boardID))
                {
                    currentIndex++;
                    if (currentIndex == targetIndex)
                    {
                        Renderer renderer = childObj.GetComponent<Renderer>();
                        if (originalMat == null)
                            originalMat = renderer.material;
                        renderer.material = newMaterial;
                        break;
                    }
                }
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
            trailRenderer.material = new Material(Shader.Find("Unlit/Color")) { color = clr};
            trailRenderer.time = 0.5f;
            trailRenderer.startWidth = 0.025f;
            trailRenderer.endWidth = 0f;
            trailRenderer.startColor = clr;
            trailRenderer.endColor = clr2;
        }
        static TextMeshPro? motdHeading;
        static TextMeshPro? motdBody;
        static TextMeshPro? cocHeading;
        static TextMeshPro? cocBody;
        static TextMeshPro? gameModeText;

        static Renderer? computer;
        static Renderer? wallMonitor;
        public void Awake()
        {
            ResourceLoader.LoadResources();
            taggerInstance = GorillaTagger.Instance;
            playerInstance = GorillaLocomotion.GTPlayer.Instance;
            pollerInstance = ControllerInputPoller.instance;
            thirdPersonCamera = GameObject.Find("Player Objects/Third Person Camera/Shoulder Camera");
            cm = GameObject.Find("Player Objects/Third Person Camera/Shoulder Camera/CM vcam1");

            motdHeading = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdHeadingText").GetComponent<TextMeshPro>();
            motdBody = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdBodyText").GetComponent<TextMeshPro>();
            cocHeading = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/CodeOfConductHeadingText").GetComponent<TextMeshPro>();
            cocBody = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/COCBodyText_TitleData").GetComponent<TextMeshPro>();
            gameModeText = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/GameModes Title Text").GetComponent<TextMeshPro>();
            computer = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/TreeRoomInteractables/GorillaComputerObject/ComputerUI/monitor/monitorScreen").GetComponent<Renderer>();
            wallMonitor = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/TreeRoomBoundaryStones/BoundaryStoneSet_Forest/wallmonitorforestbg").GetComponent<Renderer>();

            ExitGames.Client.Photon.Hashtable table = Photon.Pun.PhotonNetwork.LocalPlayer.CustomProperties;
            table.Add("Elixir", true);
            Photon.Pun.PhotonNetwork.LocalPlayer.SetCustomProperties(table);
            //SendWeb($"**{PhotonNetwork.LocalPlayer.NickName}** has loaded into the game with **Elixir**!");
        }

        public static void HandleMenuInteraction()
        {
            try
            {
                if (playerInstance == null || pollerInstance == null || thirdPersonCamera == null) { return; }

                if (PCMenuOpen && !InMenuCondition && !pollerInstance.leftControllerPrimaryButton && !pollerInstance.rightControllerPrimaryButton && !menuOpen)
                {
                    InPcCondition = true;
                    cm?.SetActive(false);

                    if (menuObj == null)
                    {
                        Draw();
                        AddButtonClicker(thirdPersonCamera.transform);
                    }
                    else
                    {
                        AddButtonClicker(thirdPersonCamera.transform);

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
                                        BtnCollider? btnCollider = hit.collider.GetComponent<BtnCollider>();
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
                    CleanupMenu(0);
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

            switch (alphabet)
            {
                case true:
                    {
                        if (currentPage != Category.Home)
                        {
                            var PageToDraw = GetButtonInfoByPage(currentPage)
                            .OrderBy(b => b.ButtonText)
                            .Skip(currentCategoryPage * ButtonsPerPage)
                            .Take(ButtonsPerPage)
                            .ToArray();
                            for (int i = 0; i < PageToDraw.Length; i++)
                            {
                                AddModButtons(i * 0.09f, PageToDraw[i]);
                            }
                        }
                        else
                        {
                            var PageToDraw = GetButtonInfoByPage(currentPage)
                            .Skip(currentCategoryPage * ButtonsPerPage)
                            .Take(ButtonsPerPage)
                            .ToArray();
                            for (int i = 0; i < PageToDraw.Length; i++)
                            {
                                AddModButtons(i * 0.09f, PageToDraw[i]);
                            }
                        }
                        break;
                    }
                case false:
                    {
                        var PageToDraw = GetButtonInfoByPage(currentPage)
                            .Skip(currentCategoryPage * ButtonsPerPage)
                            .Take(ButtonsPerPage)
                            .ToArray();
                        for (int i = 0; i < PageToDraw.Length; i++)
                        {
                            AddModButtons(i * 0.09f, PageToDraw[i]);
                        }
                        break;
                    }
            }
        }
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
            if (menuObj == null) return;

            background = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Destroy(background.GetComponent<Rigidbody>());
            Destroy(background.GetComponent<BoxCollider>());
            Outline(background, OutlineClr[Theme-1]);

            background.transform.parent = menuObj.transform;
            background.transform.rotation = Quaternion.identity;
            background.transform.localScale = new Vector3(0.1f, 1f, 1.03f);
            background.name = "poo poo";
            background.transform.position = new Vector3(0.05f, 0f, 0f);
            background.GetComponent<MeshRenderer>().material = MenuMat[Theme-1];
        }
        #region settings
        public static int Theme = 1;
        public static Color textclr = White;
        public static void ChangeTheme()
        {
            Theme++;
            if (Theme > 11)
            {
                Theme = 1;
            }

            foreach (ButtonHandler.Button btn in ModButtons.buttons)
            {
                if (btn.ButtonText.Contains("Change Theme:"))
                {
                    switch (Theme)
                    {
                        case 1:
                            btn.SetText("Change Theme: Purple");
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>Theme</color><color=white>] Purple</color>");
                            break;
                        case 2:
                            btn.SetText("Change Theme: Fading");
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>Theme</color><color=white>] Fading</color>");
                            break;
                        case 3:
                            btn.SetText("Change Theme: Breathing");
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>Theme</color><color=white>] Breathing</color>");
                            break;
                        case 4:
                            btn.SetText("Change Theme: Dark");
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>Theme</color><color=white>] Dark</color>");
                            break;
                        case 5:
                            btn.SetText("Change Theme: Blue");
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>Theme</color><color=white>] Blue</color>");
                            break;
                        case 6:
                            btn.SetText("Change Theme: Red");
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>Theme</color><color=white>] Red</color>");
                            break;
                        case 7:
                            btn.SetText("Change Theme: Green");
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>Theme</color><color=white>] Green</color>");
                            break;
                        case 8:
                            btn.SetText("Change Theme: Gray");
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>Theme</color><color=white>] Gray</color>");
                            break;
                        case 9:
                            btn.SetText("Change Theme: Forest");
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>Theme</color><color=white>] Forest</color>");
                            break;
                        case 10:
                            btn.SetText("Change Theme: Peach");
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>Theme</color><color=white>] Peach</color>");
                            break;
                        case 11:
                            btn.SetText("Change Theme: Desert");
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>Theme</color><color=white>] Desert</color>");
                            break;
                    }
                }
            }
            RefreshMenu();
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
            switch (LOJUHFDG)
            {
                case 1:
                    ActuallSound = 66;
                    break;
                case 2:
                    ActuallSound = 8;
                    break;
                case 3:
                    ActuallSound = 203;
                    break;
                case 4:
                    ActuallSound = 50;
                    break;
                case 5:
                    ActuallSound = 67;
                    break;
                case 6:
                    ActuallSound = 114;
                    break;
            }
            RefreshMenu();
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
                if (btn.ButtonText.Contains("Change Layout:"))
                {
                    switch (Laytou)
                    {
                        case 1:
                            btn.SetText("Change Layout: Sides");
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>Layout</color><color=white>] Sides</color>");
                            break;
                        case 2:
                            btn.SetText("Change Layout: Bottom");
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>Layout</color><color=white>] Bottom</color>");
                            break;
                        case 3:
                            btn.SetText("Change Layout: Top");
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>Layout</color><color=white>] Top</color>");
                            break;
                    }
                }
            }
            RefreshMenu();
        }
        #endregion
        public static void AddDisconnectButton()
        {
            if (toggledisconnectButton && menuObj != null && canvasObj != null)
            {
                // Disconnect Button
                disconnectButton = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Destroy(disconnectButton.GetComponent<Rigidbody>());
                disconnectButton.GetComponent<BoxCollider>().isTrigger = true;
                Outline(disconnectButton, OutlineClr[Theme-1]);
                disconnectButton.transform.parent = menuObj.transform;
                disconnectButton.transform.rotation = Quaternion.identity;
                disconnectButton.transform.localScale = new Vector3(0.09f, 0.9f, 0.08f);
                disconnectButton.transform.localPosition = new Vector3(0.56f, 0f, 0.59f);
                disconnectButton.AddComponent<BtnCollider>().clickedButton = new ButtonHandler.Button("DisconnectButton", Category.Home, false, false, null, null);
                disconnectButton.GetComponent<Renderer>().material.color = BtnClrOff[Theme-1];

                // Disconnect Button Text
                Text discontext = new GameObject { transform = { parent = canvasObj.transform } }.AddComponent<Text>();
                discontext.text = "Disconnect";
                discontext.font = font;
                discontext.fontStyle = FontStyle.Bold;
                discontext.color = textclr;
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
                tmp.color = ColorLib.Elixir;
                tmp.font = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdBodyText").GetComponent<TextMeshPro>().font;
            }

            tmp.text =
                $"<color={hexColor}>Elixir Menu</color>\n" +
                $"<size=2>Status: <color={hexColor}>Undetected</color>\n" +
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
            if (menuObj == null) return;
            
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
            title.color = textclr;
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
            if (title == null) return;

            title.fontStyle = FontStyle.Bold;
            title.font = font;

            title.text =
            $"{menuName}{(vCounter ? " ┇ V" + menuVersion[0] : "")}";
        }
        public static void AddModButtons(float offset, ButtonHandler.Button button)
        {
            if (menuObj == null || canvasObj == null) return;

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
            title.text = button.ButtonText;
            title.font = font;
            title.fontStyle = FontStyle.Bold;
            title.color = textclr;

            RectTransform titleTransform = title.GetComponent<RectTransform>();
            titleTransform.localPosition = new Vector3(.064f, 0,Laytou == 3 ? .089f - offset / 2.6f : .126f - offset / 2.6f);
            titleTransform.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            titleTransform.sizeDelta = new Vector2(0.21f, 0.02225f);

            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Destroy(gameObject.GetComponent<Collider>());
            Outline(gameObject, OutlineClr[Theme-1]);
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
                    btnRenderer.material.color = BtnClrOn[Theme-1];
                }
                else
                {
                    btnRenderer.material.color = BtnClrOff[Theme-1];
                }
            }
        }
        public static void AddPageButton(string button)
        {
            if (menuObj == null || canvasObj == null) return;

            // Define layout-dependent transforms
            Vector3 buttonScale;
            Vector3 buttonPosition;
            Vector3 textPosition;
            Vector2 textSize;

            switch (Laytou)
            {
                case 1:
                    buttonScale = new Vector3(0.09f, 0.15f, 0.9f);
                    buttonPosition = new Vector3(0.56f, button.Contains("<") ? 0.65f : -0.65f, 0f);
                    textPosition = new Vector3(0.064f, button.Contains("<") ? 0.1955f : -0.1955f, 0f);
                    textSize = new Vector2(0.5f, 0.06f);
                    break;
                case 2:
                    buttonScale = new Vector3(0.09f, 0.25f, 0.079f);
                    buttonPosition = new Vector3(0.56f, button.Contains("<") ? 0.2925f : -0.2925f, -0.435f);
                    textPosition = new Vector3(0.064f, button.Contains("<") ? 0.087f : -0.087f, -0.163f);
                    textSize = new Vector2(0.5f, 0.05f);
                    break;
                case 3:
                    buttonScale = new Vector3(0.09f, 0.25f, 0.079f);
                    buttonPosition = new Vector3(0.56f, button.Contains("<") ? 0.2925f : -0.2925f, 0.3223f);
                    textPosition = new Vector3(0.064f, button.Contains("<") ? 0.087f : -0.087f, 0.124f);
                    textSize = new Vector2(0.5f, 0.05f);
                    break;
                default:
                    buttonScale = new Vector3(0.09f, 0.15f, 0.9f);
                    buttonPosition = new Vector3(0.56f, button.Contains("<") ? 0.65f : -0.65f, 0f);
                    textPosition = new Vector3(0.064f, button.Contains("<") ? 0.1955f : -0.1955f, 0f);
                    textSize = new Vector2(0.5f, 0.06f);
                    break;
            }

            PageButtons = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Destroy(PageButtons.GetComponent<Rigidbody>());
            PageButtons.GetComponent<BoxCollider>().isTrigger = true;
            //RoundObj(PageButtons);
            Outline(PageButtons, OutlineClr[Theme-1]);
            PageButtons.transform.parent = menuObj.transform;
            PageButtons.transform.rotation = Quaternion.identity;
            PageButtons.transform.localScale = buttonScale;
            PageButtons.transform.localPosition = buttonPosition;
            PageButtons.GetComponent<Renderer>().material.color = BtnClrOff[Theme-1];
            PageButtons.AddComponent<BtnCollider>().clickedButton = new ButtonHandler.Button(button, Category.Home, false, false, null, null);

            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Destroy(gameObject.GetComponent<Collider>());
            Outline(gameObject, OutlineClr[Theme-1]);
            gameObject.transform.parent = menuObj.transform;
            gameObject.transform.position = PageButtons.transform.position;
            gameObject.transform.rotation = PageButtons.transform.rotation;
            gameObject.GetComponent<Renderer>().material.color = BtnClrOff[Theme-1];
            gameObject.transform.localScale = new Vector3(buttonScale.x - 0.006f, buttonScale.y + 0.005f, buttonScale.z + 0.005f);

            // Page Buttons Text
            GameObject titleObj = new GameObject();
            titleObj.transform.parent = canvasObj.transform;
            titleObj.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            Text title = titleObj.AddComponent<Text>();
            title.font = font;
            title.color = textclr;
            title.fontSize = 5;
            title.fontStyle = FontStyle.Bold;
            title.alignment = TextAnchor.MiddleCenter;
            title.resizeTextForBestFit = true;
            title.resizeTextMinSize = 0;
            RectTransform titleTransform = title.GetComponent<RectTransform>();
            titleTransform.localPosition = Vector3.zero;
            titleTransform.sizeDelta = textSize;
            title.text = button.Contains("<") ? "⋘" : "⋙";
            titleTransform.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            titleTransform.position = textPosition;
        }
        public static void AddReturnButton()
        {
            if (currentPage != Category.Home && menuObj != null && canvasObj != null)
            {
                Vector3 buttonScale;
                Vector3 buttonPosition;
                Vector3 textPosition;

                switch (Laytou)
                {
                    case 2:
                        buttonScale = new Vector3(0.09f, 0.30625f, 0.08f);
                        buttonPosition = new Vector3(0.56f, 0f, -0.435f);
                        textPosition = new Vector3(.064f, 0f, -0.165f);
                        break;
                    case 3:
                        buttonScale = new Vector3(0.09f, 0.30625f, 0.08f);
                        buttonPosition = new Vector3(0.56f, 0f, 0.3223f);
                        textPosition = new Vector3(.064f, 0f, 0.1235f);
                        break;
                    default:
                        buttonScale = new Vector3(0.09f, 0.82f, 0.08f);
                        buttonPosition = new Vector3(0.56f, 0f, -0.435f);
                        textPosition = new Vector3(.064f, 0f, -0.165f);
                        break;
                }

                // Return Button
                GameObject BackToStartButton = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Destroy(BackToStartButton.GetComponent<Rigidbody>());
                BackToStartButton.GetComponent<BoxCollider>().isTrigger = true;
                BackToStartButton.transform.parent = menuObj.transform;
                BackToStartButton.transform.rotation = Quaternion.identity;
                BackToStartButton.transform.localScale = buttonScale;
                BackToStartButton.transform.localPosition = buttonPosition;
                BackToStartButton.AddComponent<BtnCollider>().clickedButton = new ButtonHandler.Button("ReturnButton", Category.Home, false, false, null, null);
                BackToStartButton.GetComponent<Renderer>().material.color = BtnClrOff[Theme-1];

                GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Destroy(gameObject.GetComponent<Collider>());
                Outline(gameObject, OutlineClr[Theme-1]);
                gameObject.transform.parent = menuObj.transform;
                gameObject.transform.position = BackToStartButton.transform.position;
                gameObject.transform.rotation = BackToStartButton.transform.rotation;
                gameObject.GetComponent<Renderer>().material.color = BtnClrOff[Theme-1];

                gameObject.transform.localScale = new Vector3(
                    BackToStartButton.transform.localScale.x - 0.006f,
                    BackToStartButton.transform.localScale.y + 0.005f,
                    BackToStartButton.transform.localScale.z + 0.005f
                );

                // Return Button Text
                GameObject titleObj = new GameObject();
                titleObj.transform.parent = canvasObj.transform;
                titleObj.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                titleObj.transform.localPosition = new Vector3(0.85f, 0.85f, 0.85f);
                Text title = titleObj.AddComponent<Text>();
                title.font = font;
                title.fontStyle = FontStyle.Bold;
                title.text = "Return";
                title.color = textclr;
                title.fontSize = 3;
                title.alignment = TextAnchor.MiddleCenter;
                title.resizeTextForBestFit = true;
                title.resizeTextMinSize = 0;
                RectTransform titleTransform = title.GetComponent<RectTransform>();
                titleTransform.localPosition = Vector3.zero;
                titleTransform.sizeDelta = new Vector2(0.25f, 0.025f);
                titleTransform.localPosition = textPosition;
                titleTransform.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
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
                    clickerRenderer.material.color = textclr;
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
            if (menuObj == null) return;
            if (playerInstance != null)
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
        }
        private static void PositionMenuForKeyboard()
        {
            if (thirdPersonCamera != null && menuObj != null)
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
        }
    }
}
