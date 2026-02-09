using BepInEx;
using Elixir.Components;
using Elixir.Utilities;
using Oculus.Interaction.Body.Input;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Elixir.Components.ButtonInteractor;
using static Elixir.Management.Buttons;
using static Elixir.Utilities.ButtonManager;

namespace Elixir.Management
{
    public class Menu : MonoBehaviour
    {
        public static GameObject? menu = null;
        private static GameObject? nextPage = null;
        private static GameObject? lastPage = null;
        private static int currentPage = 0;
        private const int btnPerPage = 6;
        public static int pageIndex = 0;
        public static List<Category> categories = new List<Category>();
        private static List<GameObject> buttons = new List<GameObject>();
        public static bool menuRHand = false;
        public static bool triggerMenu = false;
        public static bool gripMenu = false;
        public static float cooldownTime = 0f;
        private const float cooldownDelay = 0.25f;

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

        private static void OnButtonClick()
        {
            GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.tagHapticStrength / 2f, GorillaTagger.Instance.tagHapticDuration / 2f);
            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(114, false, 1);
        }

        public static TextMeshPro? motdHeading;
        public static TextMeshPro? motdBody;
        public static TextMeshPro? cocHeading;
        public static TextMeshPro? cocBody;
        public static TextMeshPro? gameModeText;

        public static Renderer? computer;
        public static Renderer? wallMonitor;

        public static GameObject? ThirdCam;

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

            var tempButton = modules.transform.Find("Button")?.gameObject;
            tempButton!.SetActive(false);

            var version = visual.transform.Find("Title/Version")?.GetComponent<TextMeshProUGUI>();
            version!.text = $"V{Elixir.PluginInfo.Version} {(PluginInfo.MenuBeta ? "Beta" : "")}";

            var leaveButton = visual.transform.Find("Home (1)")?.GetComponent<UnityEngine.UI.Button>();
            var backButton = visual.transform.Find("Home")?.GetComponent<UnityEngine.UI.Button>();
            var backButton2 = visual.transform.Find("BigHome")?.GetComponent<UnityEngine.UI.Button>();

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

            nextPage = visual.transform.Find("NextPage").gameObject;
            lastPage = visual.transform.Find("LastPage").gameObject;

            CreateButtons();
            Buttons();
            menu.SetActive(true);
            CoroutineHandler.StartCoroutine1(GetObjects());
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
            var obj6 = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/TreeRoomInteractables/GorillaComputerObject/ComputerUI/monitor/monitorScreen");
            if (obj6 != null) computer = obj6.GetComponent<Renderer>();
            var obj7 = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/TreeRoomBoundaryStones/BoundaryStoneSet_Forest/wallmonitorforestbg");
            if (obj7 != null) wallMonitor = obj7.GetComponent<Renderer>();

            ThirdCam = GameObject.Find("Player Objects/Third Person Camera/Shoulder Camera");

            yield return null;
        }


        private static bool showingCategories = true;

        public static void Buttons()
        {
            RefreshCategory();

            var modules = menu!.transform.Find("Canvas/Visual/Buttons").gameObject;
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
                int maxCategoryPage = GetMaxCategoryPage();
                int catCount = categories.Count;

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
                    btn!.onClick.AddListener(() =>
                    {
                        pageIndex = catIndex;
                        currentPage = 0;
                        showingCategories = false;
                        Buttons();
                        OnButtonClick();
                    });
                }

                if (visual != null)
                {
                    var bigHome = visual.Find("BigHome");
                    if (bigHome != null) bigHome.gameObject.SetActive(false);
                }

                if (lastPage != null)
                {
                    var lastBtn = lastPage.GetComponent<UnityEngine.UI.Button>();
                    lastBtn.onClick.RemoveAllListeners();
                    if (currentPage > 0)
                    {
                        lastBtn.onClick.AddListener(() =>
                        {
                            currentPage--;
                            Buttons();
                            OnButtonClick();
                        });
                    }
                    lastPage.SetActive(currentPage > 0);
                }

                if (nextPage != null)
                {
                    var nextBtn = nextPage.GetComponent<UnityEngine.UI.Button>();
                    nextBtn.onClick.RemoveAllListeners();
                    if (currentPage < maxCategoryPage)
                    {
                        nextBtn.onClick.AddListener(() =>
                        {
                            currentPage++;
                            Buttons();
                            OnButtonClick();
                        });
                    }
                    nextPage.SetActive(currentPage < maxCategoryPage);
                }

                return;
            }
            #endregion
            #region Mods Logic
            int mods = categories[pageIndex].buttons.Length;
            int maxPage = (mods > 0) ? (mods - 1) / btnPerPage : 0;

            if (currentPage < 0) currentPage = 0;
            if (currentPage > maxPage) currentPage = maxPage;

            Components.Module[] sorted;
            if (Variables.alphabet)
            {
                sorted = categories[pageIndex].buttons.OrderBy(m => m.title, StringComparer.OrdinalIgnoreCase).ToArray();
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
                btn!.onClick.AddListener(() =>
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
            bool last = lastPage?.active ?? false;
            bool next = nextPage?.active ?? false;
            if (!last && !next)
            {
                visual.Find("Home").gameObject.SetActive(false);
                visual.Find("BigHome").gameObject.SetActive(true);
            }
            else
            {
                visual.Find("Home").gameObject.SetActive(true);
                visual.Find("BigHome").gameObject.SetActive(false);
            }

            if (lastPage != null)
            {
                var lastBtn = lastPage.GetComponent<UnityEngine.UI.Button>();
                lastBtn.onClick.RemoveAllListeners();
                if (currentPage > 0)
                {
                    lastBtn.onClick.AddListener(() =>
                    {
                        currentPage--;
                        Buttons();
                        OnButtonClick();
                    });
                }
                lastPage.SetActive(currentPage > 0);
            }

            if (nextPage != null)
            {
                var nextBtn = nextPage.GetComponent<UnityEngine.UI.Button>();
                nextBtn.onClick.RemoveAllListeners();
                if (currentPage < maxPage)
                {
                    nextBtn.onClick.AddListener(() =>
                    {
                        currentPage++;
                        Buttons();
                        OnButtonClick();
                    });
                }
                nextPage.SetActive(currentPage < maxPage);
            }
            #endregion
        }

        private static bool prevState = false;
        public static void Update()
        {
            if (menu == null || GorillaTagger.Instance == null) return;

            var input = UnityInput.Current;
            bool pc = input.GetKey(KeyCode.Q),
                 leftSeocon = ControllerInputPoller.instance.leftControllerSecondaryButton,
                 rightSeocon = ControllerInputPoller.instance.rightControllerSecondaryButton,
                 shouldShow = false;

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
                    menu.transform.position = body.position + (body.forward * 0.4f) + (body.up * 0.3f);
                    menu.transform.rotation = Quaternion.LookRotation(body.forward, body.up);
                    menu.transform.localScale = Vector3.one * 0.01f;
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

            if (!lastPage!.activeSelf && !nextPage!.activeSelf)
            {
                menu.transform.Find("Canvas/Visual/Home").gameObject.SetActive(false);
                menu.transform.Find("Canvas/Visual/BigHome").gameObject.SetActive(true);
            }
            else
            {
                menu.transform.Find("Canvas/Visual/Home").gameObject.SetActive(true);
                menu.transform.Find("Canvas/Visual/BigHome").gameObject.SetActive(false);
            }

            if ((triggerMenu || gripMenu) && menu != null)
            {
                lastPage.SetActive(false);
                nextPage.SetActive(false);

                int mods = categories[pageIndex].buttons.Length;
                int maxPage = (mods > 0) ? (mods - 1) / btnPerPage : 0;

                bool coooldown = Time.time >= cooldownTime;

                if (coooldown && currentPage < maxPage)
                {
                    bool inputR = triggerMenu ? ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f : ControllerInputPoller.instance.rightGrab;
                    bool arrowR = UnityInput.Current.GetKeyDown(KeyCode.RightArrow);

                    if (inputR || arrowR)
                    {
                        currentPage++;
                        Buttons();
                        OnButtonClick();
                        cooldownTime = Time.time + cooldownDelay;
                    }
                }

                if (coooldown && currentPage > 0)
                {
                    bool inputL = triggerMenu ? ControllerInputPoller.instance.leftControllerIndexFloat > 0.5f : ControllerInputPoller.instance.leftGrab;
                    bool arrowL = UnityInput.Current.GetKeyDown(KeyCode.LeftArrow);

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
                int mods = categories[pageIndex].buttons.Length;
                int maxPage = (mods > 0) ? (mods - 1) / btnPerPage : 0;

                if (lastPage != null) lastPage.SetActive(currentPage > 0);
                if (nextPage != null) nextPage.SetActive(currentPage < maxPage);
            }

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