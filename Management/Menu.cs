using BepInEx;
using Elixir.Components;
using Elixir.Mods;
using Elixir.Mods.Categories;
using Elixir.Utilities;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Elixir.Components.ButtonInteractor;
using static Elixir.Management.Buttons;
using static Elixir.Utilities.ColorLib;
using static GorillaLocomotion.GTPlayer;

namespace Elixir.Management
{
    public class Menu : MonoBehaviour
    {
        public static GameObject menu = null;
        private static GameObject nextPage;
        private static GameObject lastPage;
        private static List<Module> searchResults = new List<Module>();
        private static int currentPage = 0;
        private const int btnPerPage =6;
        private static int pageIndex = 0;
        public static List<Category> categories = new List<Category>();
        private static List<GameObject> buttons = new List<GameObject>();
        private static AudioClip defaultClickSound = null;
        public static bool menuRHand = false;


        public static AssetBundle LoadAssetBundle(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            AssetBundle bundle = AssetBundle.LoadFromStream(stream);
            stream.Close();
            return bundle;
        }

        private static void OnButtonClick()
        {
            GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.tagHapticStrength / 2f, GorillaTagger.Instance.tagHapticDuration / 2f);
            if (defaultClickSound != null)
                GorillaTagger.Instance.offlineVRRig.rightHandPlayer.PlayOneShot(defaultClickSound);
        }

        static TextMeshPro? motdHeading;
        static TextMeshPro? motdBody;
        static TextMeshPro? cocHeading;
        static TextMeshPro? cocBody;
        static TextMeshPro? gameModeText;

        static Renderer? computer;
        static Renderer? wallMonitor;

        static GameObject? ThirdCam;

        public static void Start()
        {
            CoroutineHandler.StartCoroutine1(GetObjects());

            ExitGames.Client.Photon.Hashtable table = Photon.Pun.PhotonNetwork.LocalPlayer.CustomProperties;
            table.Add("Elixir", true);
            Photon.Pun.PhotonNetwork.LocalPlayer.SetCustomProperties(table);

            var bundle = LoadAssetBundle("Elixir.Resources.ElixirBundle");
            var asset = bundle.LoadAsset<GameObject>("Elixir");
            menu = GameObject.Instantiate(asset);

            GameObject.DontDestroyOnLoad(menu);
            menu.name = "Elixir Prefab";
            menu.AddComponent<ButtonInteractor>();
            menu.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

            var canvas = menu.transform.Find("Canvas");
            var visual = menu.transform.Find("Canvas/Visual")?.gameObject;
            var modules = visual.transform.Find("Buttons")?.gameObject;
            var buttonTemplate = modules.transform.Find("Button");

            var tempButton = modules.transform.Find("Button")?.gameObject;
            tempButton.SetActive(false);

            var version = visual.transform.Find("Title/Version")?.GetComponent<TextMeshProUGUI>();
            version.text = $"V{Elixir.PluginInfo.Version} {(PluginInfo.MenuBeta ? "Beta" : "")}";

            var leaveButton = visual.transform.Find("Home (1)")?.GetComponent<UnityEngine.UI.Button>();
            var backButton = visual.transform.Find("Home")?.GetComponent<UnityEngine.UI.Button>();

            if (leaveButton != null)
            {
                leaveButton.onClick.AddListener(() =>
                {
                    PhotonNetwork.Disconnect();
                    OnButtonClick();
                });
            }

            if (backButton != null)
            {
                backButton.onClick.AddListener(() =>
                {
                    showingCategories = true;
                    Buttons();
                    OnButtonClick();
                });
            }

            nextPage = visual.transform.Find("NextPage")?.gameObject;
            lastPage = visual.transform.Find("LastPage")?.gameObject;

            CreateButtons();
            Buttons();
            menu.SetActive(true);
        }

        private static IEnumerator GetObjects()
        {
            var obj1 = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdHeadingText");
                    if (obj1 != null) motdHeading = obj1.GetComponent<TextMeshPro>();
            var obj2 = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdBodyText");
                    if (obj2 != null) motdBody = obj2.GetComponent<TextMeshPro>();
            var obj3 = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/CodeOfConductHeadingText");
                    if (obj3 != null) cocHeading = obj3.GetComponent<TextMeshPro>();
            var obj4 = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/COCBodyText_TitleData");
                    if (obj4 != null) cocBody = obj4.GetComponent<TextMeshPro>();
            var obj5 = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/GameModes Title Text");
                    if (obj5 != null) gameModeText = obj5.GetComponent<TextMeshPro>();
            var obj6 = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/TreeRoomInteractables/GorillaComputerObject/ComputerUI/monitor/monitorScreen");
                    if (obj6 != null) computer = obj6.GetComponent<Renderer>();
            var obj7 = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/TreeRoomBoundaryStones/BoundaryStoneSet_Forest/wallmonitorforestbg");
                    if (obj7 != null) wallMonitor = obj7.GetComponent<Renderer>();

            ThirdCam = GameObject.Find("Player Objects/Third Person Camera/");

            yield return null;
        }


        private static bool showingCategories = true;

        public static void Buttons()
        {
            RefreshCategory();

            var modules = menu.transform.Find("Canvas/Visual/Buttons").gameObject;
            var templateButton = modules.transform.Find("Button").gameObject;
            foreach (GameObject btn in buttons) GameObject.Destroy(btn);
            buttons.Clear();

            var visual = menu.transform.Find("Canvas/Visual");
            if (visual != null && visual.Find("Home") != null)
            {
                visual.Find("Home").gameObject.SetActive(!showingCategories);
            }

            #region Categories Logic
            if (showingCategories)
            {
                int catCount = categories.Count;
                int maxCategoryPage = (catCount > 0) ? (catCount - 1) / btnPerPage : 0;

                if (currentPage < 0) currentPage = 0;
                if (currentPage > maxCategoryPage) currentPage = maxCategoryPage;

                for (int i = currentPage * btnPerPage; i < Mathf.Min(currentPage * btnPerPage + btnPerPage, catCount); i++)
                {
                    int catIndex = i;
                    var module = GameObject.Instantiate(templateButton, modules.transform);
                    buttons.Add(module);
                    module.SetActive(true);

                    module.transform.Find("ButtonName").GetComponent<TextMeshProUGUI>().text = categories[i].name;
                    module.transform.Find("ButtonDescription").GetComponent<TextMeshProUGUI>().text = $"Opens the {categories[i].name} category";

                    module.transform.Find("ButtonName").GetComponent<TextMeshProUGUI>().ForceMeshUpdate();
                    module.transform.Find("ButtonDescription").GetComponent<TextMeshProUGUI>().ForceMeshUpdate();

                    module.transform.Find("ButtonDescription").gameObject.SetActive(Variables.tips);

                    module.transform.Find("enabled")?.gameObject.SetActive(false);
                    module.transform.Find("disabled")?.gameObject.SetActive(false);
                    module.transform.Find("nontoggle")?.gameObject.SetActive(false);

                    var btn = module.transform.Find("enabled (1)")?.GetComponent<UnityEngine.UI.Button>();
                    btn.onClick.AddListener(() =>
                    {
                        pageIndex = catIndex;
                        currentPage = 0; 
                        showingCategories = false;
                        Buttons();
                        OnButtonClick();
                    });
                }

                if (lastPage != null) lastPage.SetActive(currentPage > 0);
                if (nextPage != null) nextPage.SetActive(currentPage < maxCategoryPage);

                if (currentPage > 0 && lastPage != null)
                {
                    var lastBtn = lastPage.GetComponent<UnityEngine.UI.Button>();
                    lastBtn.onClick.RemoveAllListeners();
                    lastBtn.onClick.AddListener(() =>
                    {
                        currentPage--;
                        Buttons();
                        OnButtonClick();
                    });
                }
                if (currentPage < maxCategoryPage && nextPage != null)
                {
                    var nextBtn = nextPage.GetComponent<UnityEngine.UI.Button>();
                    nextBtn.onClick.RemoveAllListeners();
                    nextBtn.onClick.AddListener(() =>
                    {
                        currentPage++;
                        Buttons();
                        OnButtonClick();
                    });
                }
                return;
            }
            #endregion
            #region Mods Logic
            int mods = categories[pageIndex].buttons.Length;
            int maxPage = (mods > 0) ? (mods - 1) / btnPerPage : 0;

            if (currentPage < 0) currentPage = 0;
            if (currentPage > maxPage) currentPage = maxPage;

            Module[] sorted;
            if (Variables.alphabet)
            {
                sorted = categories[pageIndex].buttons
                    .OrderBy(m => m.title, StringComparer.OrdinalIgnoreCase)
                    .ToArray();
            }
            else
            {
                sorted = categories[pageIndex].buttons;
            }

            for (int i = currentPage * btnPerPage; i < Mathf.Min(currentPage * btnPerPage + btnPerPage, sorted.Length); i++)
            {
                int index = i;
                var mod = sorted[index];

                var module = GameObject.Instantiate(templateButton, modules.transform);
                buttons.Add(module);
                module.SetActive(true);

                module.transform.Find("ButtonName").GetComponent<TextMeshProUGUI>().text = mod.title;
                module.transform.Find("ButtonDescription").GetComponent<TextMeshProUGUI>().text = mod.tooltip;

                module.transform.Find("ButtonName").GetComponent<TextMeshProUGUI>().ForceMeshUpdate();
                module.transform.Find("ButtonDescription").GetComponent<TextMeshProUGUI>().ForceMeshUpdate();

                module.transform.Find("ButtonDescription").gameObject.SetActive(Variables.tips);

                var enabledObj = module.transform.Find("enabled")?.gameObject;
                var disabledObj = module.transform.Find("disabled")?.gameObject;
                var nontoggleObj = module.transform.Find("nontoggle")?.gameObject;

                if (mod.isToggleable)
                {
                    if (mod.toggled)
                    {
                        enabledObj.SetActive(true);
                        disabledObj.SetActive(false);
                        nontoggleObj.SetActive(false);
                    }
                    else
                    {
                        enabledObj.SetActive(false);
                        disabledObj.SetActive(true);
                        nontoggleObj.SetActive(false);
                    }
                }
                else
                {
                    enabledObj.SetActive(false);
                    disabledObj.SetActive(false);
                    nontoggleObj.SetActive(true);
                }

                var btn = module.transform.Find("enabled (1)")?.GetComponent<UnityEngine.UI.Button>();
                btn.onClick.AddListener(() =>
                {
                    if (mod.isToggleable)
                    {
                        mod.toggled = !mod.toggled;

                        if (enabledObj != null && disabledObj != null)
                        {
                            enabledObj.SetActive(mod.toggled);
                            disabledObj.SetActive(!mod.toggled);
                        }

                        if (mod.toggled)
                        {
                            mod.action?.Invoke();
                        }
                        else
                        {
                            mod.disableAction?.Invoke();
                        }
                    }
                    else
                    {
                        mod.action?.Invoke();
                    }

                    Buttons();
                    OnButtonClick();
                });
            }
            #endregion
            #region Page Navigation Logic
            if (lastPage != null) lastPage.SetActive(currentPage > 0);
            if (nextPage != null) nextPage.SetActive(currentPage < maxPage);

            if (currentPage > 0 && lastPage != null)
            {
                var lastBtn = lastPage.GetComponent<UnityEngine.UI.Button>();
                lastBtn.onClick.RemoveAllListeners();
                lastBtn.onClick.AddListener(() =>
                {
                    currentPage--;
                    Buttons();
                    OnButtonClick();
                });
            }
            if (currentPage < maxPage && nextPage != null)
            {
                var nextBtn = nextPage.GetComponent<UnityEngine.UI.Button>();
                nextBtn.onClick.RemoveAllListeners();
                nextBtn.onClick.AddListener(() =>
                {
                    currentPage++;
                    Buttons();
                    OnButtonClick();
                });
            }
            #endregion
        }
        static int fps;
        private static bool prevState = false;
        public static void Update()
        {
            if (menu == null || GorillaTagger.Instance == null) return;

            var input = UnityInput.Current;
            bool pc = input.GetKey(KeyCode.Q),
                 leftSeocon = ControllerInputPoller.instance.leftControllerSecondaryButton,
                 rightSeocon = ControllerInputPoller.instance.rightControllerSecondaryButton,
                 shouldShow = false;

            if (menuRHand ? rightSeocon : leftSeocon)
            {
                shouldShow = !shouldShow;
                var leftHand = GorillaTagger.Instance.leftHandTransform;
                if (leftHand != null)
                {
                    menu.transform.position = leftHand.position + leftHand.right * 0.045f;
                    menu.transform.rotation = leftHand.rotation * Quaternion.Euler(0f, -90f, -90f);
                }

            }
            else if (pc)
            {
                shouldShow = !shouldShow;
                Camera cam = Camera.current;
                if (cam == null || !cam.isActiveAndEnabled) cam = Camera.main;
                if (cam != null)
                {
                    menu.transform.position = cam.transform.position + cam.transform.forward * 0.4f;
                    menu.transform.rotation = Quaternion.LookRotation(cam.transform.forward, cam.transform.up);
                    menu.transform.localScale = Vector3.one * 0.01f;
                }
            }

            if (shouldShow)
            {
                if (!menu.activeSelf)
                {
                    menu.SetActive(true);
                    ButtonInteractor.AddButtonClicker(!menuRHand ? GorillaLocomotion.GTPlayer.Instance.LeftHand.controllerTransform : GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform);
                }
            }
            else
            {
                if (menu.activeSelf)
                {
                    menu.SetActive(false);
                    GameObject.Destroy(clickerObj);
                }
            }
            if (UnityInput.Current.GetKey(KeyCode.Q) != prevState)
            {
                ThirdCam?.SetActive(!UnityInput.Current.GetKey(KeyCode.Q));
            }
            prevState = UnityInput.Current.GetKey(KeyCode.Q);

            if (categories != null)
            {
                foreach (var category in categories)
                {
                    if (category == null || category.buttons == null) continue;

                    foreach (var mod in category.buttons)
                    {
                        if (mod.isToggleable && mod.toggled)
                        {
                            mod.action?.Invoke();
                        }
                    }
                }
            }
            #region MOTD
            fps = (Time.deltaTime > 0) ? Mathf.RoundToInt(1 / Time.deltaTime) : 0;
            try
            {
                GameObject goop = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/SpectralGooPile (combined by EdMeshCombiner)");
                if (goop != null && computer != null && wallMonitor != null)
                {
                    computer.material = goop.GetComponent<Renderer>().material;
                    wallMonitor.material = goop.GetComponent<Renderer>().material;
                    ChangeBoardMaterial("Environment Objects/LocalObjects_Prefab/TreeRoom", "UnityTempFile", 5, goop.GetComponent<Renderer>().material, ref originalMat1!);
                    ChangeBoardMaterial("Environment Objects/LocalObjects_Prefab/Forest", "UnityTempFile", 10, goop.GetComponent<Renderer>().material, ref originalMat2!);
                }

                #region MOTD
                if (motdHeading == null || motdBody == null) return;
                motdHeading.SetText(GradientText.MakeAnimatedGradient(ColorLib.ClrToHex(Magenta), ColorLib.ClrToHex(Purple), $"Elixir | V{Elixir.PluginInfo.Version}", Time.time) + $"<color={hexColor1}>\n--------------------------------------------</color>"); motdHeading.color = Pink;
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
                cocHeading.SetText(GradientText.MakeAnimatedGradient(ColorLib.ClrToHex(Magenta), ColorLib.ClrToHex(Purple), "Menu Meanings", Time.time) + $"<color={hexColor1}>\n-----------------------------</color>");
                cocHeading.color = Pink;

                cocBody.color = Pink;
                cocBody.SetText($"\n[D?] - Maybe Detected \n[NW] - Not Working\n[U] - Use\n[P] - Primary\n[S] - Secondary\n[G] - Grip\n[T] - Trigger\n[W?] - Maybe Working\n[B] - Buggy\n\nIf A Mod Has No Symbol It Is Probably Because I Forgot To Put One");
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

            #endregion
            UpdateClr();


        }

        // Thx GLXY for this
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
        public static float delay;
        public static bool contBool = false;
        public static bool prevLPrimaryState = false;
    }
}
