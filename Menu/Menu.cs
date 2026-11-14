using BepInEx;
using Elixir.Components;
using Elixir.Notifications;
using Elixir.Utilities;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.XR;
using static Elixir.Components.ButtonInteractor;
using static Elixir.Management.Buttons;
using static Elixir.Plugin;
using static Elixir.Utilities.ButtonManager;
using static MarkOneMitts;
namespace Elixir.Management
{
    public class Menu : MonoBehaviour
    {
        public static GameObject? menu = null;
        private static GameObject? nextPage = null;
        private static GameObject? lastPage = null;
        private static int currentPage = 0;
        private const int btnPerPage = 7;
        public static int pageIndex = 0;
        public static List<Category> categories = new List<Category>();
        private static List<GameObject> buttons = new List<GameObject>();
        public static bool menuRHand = false;
        public static bool triggerMenu = false;
        public static bool gripMenu = false;
        public static float cooldownTime = 0f;
        private const float cooldownDelay = 0.25f;

        private static Transform? visualTransform = null;
        private static Transform? homeTransform = null;
        private static Transform? bigHomeTransform = null;
        private static bool lastPageState = false;

        private static int GetMaxCategoryPage()
        {
            int count = categories.Count;
            return (count > 0) ? (count - 1) / btnPerPage : 0;
        }

        public static AssetBundle LoadAssetBundle(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            AssetBundle bundle = AssetBundle.LoadFromStream(stream);
            stream.Close();
            return bundle;
        }

        public static int CurrentClickSound = 114;

        public static void ChangeClickSound()
        {
            switch(CurrentClickSound)
            {
                case 114:
                    CurrentClickSound = 67;
                    break;
                case 67:
                    CurrentClickSound = 66;
                    break;
                case 66:
                    CurrentClickSound = 84;
                    break;
                case 84:
                    CurrentClickSound = 114;
                    break;
            }
            switch (CurrentClickSound)
            {
                case 114:
                    NotificationLib.SendNotification("<color=white>[</color>Button Sound:<color=white>] </color>Default");
                    GetButton("Change Button Sound").tooltip = "Current Setting: Default";
                    break;
                case 67:
                    NotificationLib.SendNotification("<color=white>[</color>Button Sound:<color=white>] </color>Button");
                    GetButton("Change Button Sound").tooltip = "Current Setting: Button";
                    break;
                case 66:
                    NotificationLib.SendNotification("<color=white>[</color>Button Sound:<color=white>] </color>Keyboard");
                    GetButton("Change Button Sound").tooltip = "Current Setting: Keyboard";
                    break;
                case 84:
                    NotificationLib.SendNotification("<color=white>[</color>Button Sound:<color=white>] </color>Pop");
                    GetButton("Change Button Sound").tooltip = "Current Setting: Pop";
                    break;
            }
        }

        private static void OnButtonClick()
        {
            GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.tagHapticStrength / 2f, GorillaTagger.Instance.tagHapticDuration / 2f);
            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(CurrentClickSound, false, 1);
        }

        private static void Home()
        {
            showingCategories = true;
            pageIndex = 0;
            currentPage = 0;

            if (lastPage != null)
            {
                var l = lastPage.GetComponent<UnityEngine.UI.Button>();
                l.onClick.RemoveAllListeners();
                lastPage.SetActive(false);
            }

            if (nextPage != null)
            {
                var n = nextPage.GetComponent<UnityEngine.UI.Button>();
                n.onClick.RemoveAllListeners();
                nextPage.SetActive(false);
            }

            Buttons();
        }

        public static void Start()
        {
            GameObject OnScreenGUI = new GameObject("Elixir GUI");
            OnScreenGUI.AddComponent<GUIHandler>();

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
            var modules = visual!.transform.Find("Buttons")?.gameObject;
            var buttonTemplate = modules!.transform.Find("Button");

            visualTransform = visual.transform;
            homeTransform = menu.transform.Find("Canvas/Visual/Dock/Home");
            bigHomeTransform = menu.transform.Find("Canvas/Visual/Dock/BigHome");

            var tempButton = modules.transform.Find("Button")?.gameObject;
            tempButton!.SetActive(false);

            var version = visual.transform.Find("Title/Version")?.GetComponent<TextMeshProUGUI>();
            version!.text = $"V{Elixir.PluginInfo.Version} {(PluginInfo.MenuBeta ? "Beta" : "")}";

            var leaveButton = visual.transform.Find("Home (1)")?.GetComponent<UnityEngine.UI.Button>();
            var backButton = menu.transform.Find("Canvas/Visual/Dock/Home").gameObject?.GetComponent<UnityEngine.UI.Button>();
            var backButton2 = menu.transform.Find("Canvas/Visual/Dock/BigHome").gameObject?.GetComponent<UnityEngine.UI.Button>();

            var SearchButtons = visual.transform.Find("Search").gameObject?.GetComponent<UnityEngine.UI.Button>();

            if (SearchButtons != null)
            {
                SearchButtons.onClick.AddListener(() =>
                {
                    HandleSearchLogic();
                    OnButtonClick();
                });
            }

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
                backButton.onClick.RemoveAllListeners();
                backButton.onClick.AddListener(() =>
                {
                    Home();
                    OnButtonClick();
                });
            }

            if (backButton2 != null)
            {
                backButton2.onClick.RemoveAllListeners();
                backButton2.onClick.AddListener(() =>
                {
                    Home();
                    OnButtonClick();
                });
            }

            nextPage = menu.transform.Find("Canvas/Visual/Dock/NextPage").gameObject;
            lastPage = menu.transform.Find("Canvas/Visual/Dock/LastPage").gameObject;

            CreateButtons();
            Buttons();
            menu.SetActive(true);
        }
        public static bool IsSearching = false;
        public static bool AddedSearchLogic = false;
        public static string SearchQuery = string.Empty;
        public static void HandleSearchLogic()
        {
            IsSearching = !IsSearching;

            var Keyboard = menu.transform.Find("keyboard")?.gameObject;
            var KeyboardBG = menu.transform.Find("backKeyboard")?.gameObject;
            var Dock = menu.transform.Find("Canvas/Visual/Dock")?.gameObject;
            if (!XRSettings.isDeviceActive)
            {
                if (IsSearching)
                {
                    ThirdCam.SetActive(false);
                }
                else
                {
                    ThirdCam.SetActive(true);
                }
            }

            if (Keyboard == null || KeyboardBG == null || Dock == null) return;

            Keyboard.SetActive(IsSearching);
            KeyboardBG.SetActive(false);
            Dock.SetActive(!IsSearching);

            if (IsSearching)
            {
                showingCategories = false;
                currentPage = 0;
                Buttons();
            }
            else
            {
                SearchQuery = string.Empty;
                currentPage = 0;
                Buttons();
            }

            if (AddedSearchLogic) return;

            var Canvas = Keyboard.transform.Find("Canvas");
            if (Canvas == null) return;

            void AddKey(Transform parent, string key, Action action)
            {
                var button = parent?.Find(key)?.GetComponent<UnityEngine.UI.Button>();
                if (button != null) button.onClick.AddListener(() => action());
            }

            void AddCharacter(string key)
            {
                AddKey(Canvas.Find("Keys1"), key, () => SearchQuery += key);
                AddKey(Canvas.Find("Keys 2"), key, () => SearchQuery += key);
                AddKey(Canvas.Find("Keys 3"), key, () => SearchQuery += key);
            }

            string[] rowOne = { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P" };
            string[] rowTwo = { "A", "S", "D", "F", "G", "H", "J", "K", "L" };
            string[] rowThree = { "Z", "X", "C", "V", "B", "N", "M" };

            foreach (string key in rowOne)
            {
                string captured = key;
                AddKey(Canvas.Find("Keys1"), captured, () =>
                {
                    SearchQuery += captured;
                    currentPage = 0;
                    Buttons();
                    OnButtonClick();
                });
            }

            foreach (string key in rowTwo)
            {
                string captured = key;
                AddKey(Canvas.Find("Keys 2"), captured, () =>
                {
                    SearchQuery += captured;
                    currentPage = 0;
                    Buttons();
                    OnButtonClick();
                });
            }

            foreach (string key in rowThree)
            {
                string captured = key;
                AddKey(Canvas.Find("Keys 3"), captured, () =>
                {
                    SearchQuery += captured;
                    currentPage = 0;
                    Buttons();
                    OnButtonClick();
                });
            }

            AddKey(Canvas, "Delete", () =>
            {
                if (!string.IsNullOrEmpty(SearchQuery))
                {
                    SearchQuery = SearchQuery.Substring(0, SearchQuery.Length - 1);
                    currentPage = 0;
                    Buttons();
                    OnButtonClick();
                }
            });

            AddKey(Canvas, "Space", () =>
            {
                SearchQuery += " ";
                currentPage = 0;
                Buttons();
                OnButtonClick();
            });

            AddedSearchLogic = true;
        }

        public static IEnumerator GetObjects()
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

            ThirdCam = GameObject.Find("Player Objects/Third Person Camera/Shoulder Camera");

            yield return null;
        }
        private static Module[] GetSearchResults()
        {
            List<Module> allMods = new List<Module>();
            foreach (var category in categories)
            {
                if (category == null || category.buttons == null) continue;
                allMods.AddRange(category.buttons);
            }

            if (!string.IsNullOrEmpty(SearchQuery))
                allMods = allMods.Where(m => m.title.IndexOf(SearchQuery, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            if (Variables.alphabet)
                allMods = allMods.OrderBy(m => m.title, StringComparer.OrdinalIgnoreCase).ToList();

            return allMods.ToArray();
        }

        private static bool showingCategories = true;

        public static void Buttons()
        {
            RefreshCategory();

            var modules = menu!.transform.Find("Canvas/Visual/Buttons").gameObject;
            var templateButton = modules.transform.Find("Button").gameObject;

            foreach (GameObject btn in buttons)
                GameObject.Destroy(btn);

            buttons.Clear();

            if (showingCategories)
                menu.transform.Find("Canvas/Visual/Dock/Home")?.gameObject.SetActive(false);

            if (showingCategories)
            {
                int maxCategoryPage = GetMaxCategoryPage();
                int catCount = categories.Count;

                currentPage = Mathf.Clamp(currentPage, 0, maxCategoryPage);

                for (int i = currentPage * btnPerPage; i < Mathf.Min(currentPage * btnPerPage + btnPerPage, catCount); i++)
                {
                    int catIndex = i;
                    var module = GameObject.Instantiate(templateButton, modules.transform);
                    buttons.Add(module);
                    module.SetActive(true);

                    var buttonNameText = module.transform.Find("ButtonName").GetComponent<TextMeshProUGUI>();
                    buttonNameText.text = categories[i].name;
                    buttonNameText.ForceMeshUpdate();

                    module.transform.Find("enabled")?.gameObject.SetActive(false);
                    module.transform.Find("disabled")?.gameObject.SetActive(false);
                    module.transform.Find("nontoggle")?.gameObject.SetActive(false);

                    var btn = module.transform.Find("enabled (1)")?.GetComponent<UnityEngine.UI.Button>();
                    btn!.onClick.AddListener(() =>
                    {
                        pageIndex = catIndex;
                        currentPage = 0;
                        showingCategories = false;
                        Buttons();
                        OnButtonClick();
                        HandleInformationTablet(categories[catIndex].buttons[catIndex]);
                    });
                }

                if (visualTransform != null)
                {
                    var bigHome = menu.transform.Find("Canvas/Visual/Dock/BigHome");
                    bigHome?.gameObject.SetActive(false);
                }

                if (lastPage != null)
                {
                    var lastBtn = lastPage.GetComponent<UnityEngine.UI.Button>();
                    lastBtn.onClick.RemoveAllListeners();

                    if (currentPage > 0)
                        lastBtn.onClick.AddListener(() =>
                        {
                            currentPage--;
                            Buttons();
                            OnButtonClick();
                        });

                    lastPage.SetActive(currentPage > 0);
                }

                if (nextPage != null)
                {
                    var nextBtn = nextPage.GetComponent<UnityEngine.UI.Button>();
                    nextBtn.onClick.RemoveAllListeners();

                    if (currentPage < maxCategoryPage)
                        nextBtn.onClick.AddListener(() =>
                        {
                            currentPage++;
                            Buttons();
                            OnButtonClick();
                        });

                    nextPage.SetActive(currentPage < maxCategoryPage);
                }

                return;
            }

            Module[] sorted;

            if (IsSearching)
                sorted = GetSearchResults();
            else
            {
                sorted = categories[pageIndex].buttons;

                if (Variables.alphabet)
                    sorted = sorted.OrderBy(m => m.title, StringComparer.OrdinalIgnoreCase).ToArray();
            }

            int maxPage = sorted.Length > 0 ? (sorted.Length - 1) / btnPerPage : 0;
            currentPage = Mathf.Clamp(currentPage, 0, maxPage);

            int startIndex = currentPage * btnPerPage;
            int endIndex = Mathf.Min(startIndex + btnPerPage, sorted.Length);

            for (int i = startIndex; i < endIndex; i++)
            {
                var mod = sorted[i];

                var module = GameObject.Instantiate(templateButton, modules.transform);
                buttons.Add(module);
                module.SetActive(true);

                var buttonNameText = module.transform.Find("ButtonName").GetComponent<TextMeshProUGUI>();
                buttonNameText.text = mod.title;
                buttonNameText.ForceMeshUpdate();

                var enabledObj = module.transform.Find("enabled")?.gameObject;
                var disabledObj = module.transform.Find("disabled")?.gameObject;
                var nontoggleObj = module.transform.Find("nontoggle")?.gameObject;

                if (mod.isToggleable)
                {
                    enabledObj?.SetActive(mod.toggled);
                    disabledObj?.SetActive(!mod.toggled);
                    nontoggleObj?.SetActive(false);
                }
                else
                {
                    enabledObj?.SetActive(false);
                    disabledObj?.SetActive(false);
                    nontoggleObj?.SetActive(true);
                }

                var btn = module.transform.Find("enabled (1)")?.GetComponent<UnityEngine.UI.Button>();

                btn!.onClick.AddListener(() =>
                {
                    if (mod.isToggleable)
                    {
                        mod.toggled = !mod.toggled;

                        enabledObj?.SetActive(mod.toggled);
                        disabledObj?.SetActive(!mod.toggled);

                        if (mod.toggled)
                            mod.action?.Invoke();
                        else
                            mod.disableAction?.Invoke();
                    }
                    else
                        mod.action?.Invoke();

                    Buttons();
                    OnButtonClick();
                    HandleInformationTablet(mod);
                });
            }

            bool hasPreviousPage = currentPage > 0;
            bool hasNextPage = currentPage < maxPage;

            homeTransform?.gameObject.SetActive(hasPreviousPage || hasNextPage);
            bigHomeTransform?.gameObject.SetActive(!hasPreviousPage && !hasNextPage);

            if (lastPage != null)
            {
                var lastBtn = lastPage.GetComponent<UnityEngine.UI.Button>();
                lastBtn.onClick.RemoveAllListeners();

                if (hasPreviousPage)
                    lastBtn.onClick.AddListener(() =>
                    {
                        currentPage--;
                        Buttons();
                        OnButtonClick();
                    });

                lastPage.SetActive(hasPreviousPage);
            }

            if (nextPage != null)
            {
                var nextBtn = nextPage.GetComponent<UnityEngine.UI.Button>();
                nextBtn.onClick.RemoveAllListeners();

                if (hasNextPage)
                    nextBtn.onClick.AddListener(() =>
                    {
                        currentPage++;
                        Buttons();
                        OnButtonClick();
                    });

                nextPage.SetActive(hasNextPage);
            }
        }
        public static bool ConstantInformationUse = false;
        public static void ConstantInformation()
        {
            foreach (var button in buttons)
            {
                if (Vector3.Distance(ButtonInteractor.clickerObj.transform.position, button.transform.position) < 0.025f)
                {
                    string title = button.transform.Find("ButtonName").GetComponent<TextMeshProUGUI>().text;

                    foreach (var category in categories)
                    {
                        var mod = category.buttons.FirstOrDefault(m => m.title == title);

                        if (mod != null)
                        {
                            HandleInformationTablet(mod);
                            return;
                        }
                    }
                }
            }
        }
        public static void HandleInformationTablet(Module button)
        {
            Transform informationMain = menu.transform.Find("InformationCanvas/Front/Side");

            TextMeshProUGUI modName = informationMain.Find("ModName")?.GetComponent<TextMeshProUGUI>();

            TextMeshProUGUI modDesc = informationMain.Find("ModDesc")?.GetComponent<TextMeshProUGUI>();

            modName.text = button.title;
            modDesc.text = button.tooltip;

            modName.ForceMeshUpdate();
            modDesc.ForceMeshUpdate();
        }

        private static bool prevState = false;
        public static void Update()
        {
            if (menu == null || GorillaTagger.Instance == null) return;

            var input = UnityInput.Current;

            bool pc = input.GetKey(KeyCode.Q);
            bool leftSeocon = ControllerInputPoller.instance.leftControllerSecondaryButton;
            bool rightSeocon = ControllerInputPoller.instance.rightControllerSecondaryButton;
            bool shouldShow = false;


            if (!IsSearching)
            {
                if (leftSeocon && !menuRHand)
                {
                    shouldShow = !shouldShow;
                    var leftHand = GorillaTagger.Instance.leftHandTransform;

                    if (leftHand != null)
                    {
                        menu.transform.position = leftHand.position + leftHand.right * 0.045f;
                        menu.transform.rotation = leftHand.rotation * Quaternion.Euler(0f, -90f, -90f);
                    }
                }
                else if (rightSeocon && menuRHand)
                {
                    shouldShow = !shouldShow;
                    var rightHand = GorillaTagger.Instance.rightHandTransform;

                    if (rightHand != null)
                    {
                        menu.transform.position = rightHand.position - rightHand.right * 0.045f;
                        menu.transform.rotation = rightHand.rotation * Quaternion.Euler(0f, 90f, 90f);
                    }
                }
                else if (pc)
                {
                    shouldShow = !shouldShow;
                    Transform body = GorillaLocomotion.GTPlayer.Instance.bodyCollider.transform;

                    if (body != null)
                    {
                        menu.transform.position = body.position + body.forward * 0.4f + body.up * 0.3f;
                        menu.transform.rotation = Quaternion.LookRotation(body.forward, body.up);
                        menu.transform.localScale = Vector3.one * 0.01f;
                    }
                }
            }
            else
            {
                shouldShow = true;
                Transform body = GorillaLocomotion.GTPlayer.Instance.bodyCollider.transform;
                if (body != null)
                {
                    var Keyboard = menu.transform.Find("keyboard")?.gameObject;
                    if (Keyboard != null)
                    {
                        if (Vector3.Distance(body.position, Keyboard.transform.position) > 1.5f)
                        {
                            menu.transform.position = body.position + body.forward * 0.4f + body.up * 0.3f;
                            menu.transform.rotation = Quaternion.LookRotation(body.forward, body.up);
                            menu.transform.localScale = Vector3.one * 0.01f;
                        }
                    }
                }
            }

            if (shouldShow)
            {
                if (!menu.activeSelf)
                {
                    menu.SetActive(true);
                    ButtonInteractor.AddButtonClicker(menuRHand ? GorillaLocomotion.GTPlayer.Instance.LeftHand.controllerTransform : GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform);
                }
            }
            else if (menu.activeSelf)
            {
                menu.SetActive(false);
                GameObject.Destroy(clickerObj);
            }

            if (!IsSearching)
            {
                if (pc != prevState)
                    ThirdCam?.SetActive(!pc);
            }

            prevState = pc;

            if (lastPage != null && nextPage != null)
            {
                bool currentPageState = !lastPage.activeSelf && !nextPage.activeSelf;

                if (currentPageState != lastPageState)
                {
                    lastPageState = currentPageState;
                    homeTransform?.gameObject.SetActive(!currentPageState);
                    bigHomeTransform?.gameObject.SetActive(currentPageState);
                }
            }

            Module[] currentModules = IsSearching ? GetSearchResults() : categories[pageIndex].buttons;
            int maxPage = currentModules.Length > 0 ? (currentModules.Length - 1) / btnPerPage : 0;

            if ((triggerMenu || gripMenu) && menu != null)
            {
                lastPage?.SetActive(false);
                nextPage?.SetActive(false);

                bool cooldown = Time.time >= cooldownTime;

                if (cooldown && currentPage < maxPage)
                {
                    bool inputR = triggerMenu ? ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f : ControllerInputPoller.instance.rightGrab;
                    bool arrowR = input.GetKeyDown(KeyCode.RightArrow);

                    if (inputR || arrowR)
                    {
                        currentPage++;
                        Buttons();
                        OnButtonClick();
                        cooldownTime = Time.time + cooldownDelay;
                    }
                }

                if (cooldown && currentPage > 0)
                {
                    bool inputL = triggerMenu ? ControllerInputPoller.instance.leftControllerIndexFloat > 0.5f : ControllerInputPoller.instance.leftGrab;
                    bool arrowL = input.GetKeyDown(KeyCode.LeftArrow);

                    if (inputL || arrowL)
                    {
                        currentPage--;
                        Buttons();
                        OnButtonClick();
                        cooldownTime = Time.time + cooldownDelay;
                    }
                }
            }
            else
            {
                lastPage?.SetActive(currentPage > 0);
                nextPage?.SetActive(currentPage < maxPage);
            }

            if (categories != null)
            {
                foreach (var category in categories)
                {
                    if (category == null || category.buttons == null) continue;

                    foreach (var mod in category.buttons)
                    {
                        if (mod.isToggleable && mod.toggled)
                            mod.action?.Invoke();
                    }
                }
            }

            if (IsSearching)
            {
                var SearchQueryBar = menu.transform.Find("keyboard/Canvas/Typing/Text")?.GetComponent<TextMeshProUGUI>();

                if (SearchQueryBar != null)
                    SearchQueryBar.text = SearchQuery;

            }

            if (ConstantInformationUse) { ConstantInformation(); }
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
        public static float delay;
        public static bool contBool = false;
        public static bool prevLPrimaryState = false;
    }
}