using Elixir.Mods;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Elixir.Utilities.Variables;
using static Elixir.Menu.Optimizations;
using static Elixir.Mods.Categories.Room;
using System.IO;
using static Elixir.Menu.Main;
using Valve.VR;
using Elixir.Utilities.Notifs;


namespace Elixir.Menu
{
    public class ButtonHandler
    {
        public static void Toggle(Button button)
        {
            switch (button.ButtonText)
            {
                case "<":
                    NavigatePage(false);
                    break;
                case ">":
                    NavigatePage(true);
                    break;
                case "DisconnectButton":
                    PhotonNetwork.Disconnect();
                    break;
                case "ReturnButton":
                    ReturnToMainPage();
                    break;
                default:
                    ToggleButton(button);
                    break;
            }
        }

        public static void NavigatePage(bool forward)
        {
            int totalPages = GetTotalPages(currentPage);
            int lastPage = totalPages - 1;

            currentCategoryPage += forward ? 1 : -1;

            if (currentCategoryPage < 0)
            {
                currentCategoryPage = lastPage;
            }
            else if (currentCategoryPage > lastPage)
            {
                currentCategoryPage = 0;
            }

            RefreshMenu();
        }

        private static void ReturnToMainPage()
        {
            currentPage = Category.Home;
            currentCategoryPage = 0;

            RefreshMenu();
        }

        private static int GetTotalPages(Category page)
        {
            return (GetButtonInfoByPage(page).Count + ButtonsPerPage - 1) / ButtonsPerPage;
        }

        public static void ChangePage(Category page)
        {
            currentCategoryPage = 0;
            currentPage = page;

            RefreshMenu();
        }

        public static void ToggleButton(Button button)
        {
            try
            {
                if (!button.IsToggle)
                {
                    button.OnEnable?.Invoke();

                    if (button.Page == currentPage)
                    {
                        NotificationLib.SendNotification($"<color=green>Enabled</color> : {button.ButtonText}");
                    }
                    else
                    {
                        NotificationLib.SendNotification($"<color=green>Entered Category</color> : {button.ButtonText}");
                    }
                }
                else
                {
                    button.Enabled = !button.Enabled;

                    if (button.Enabled)
                    {
                        button.OnEnable?.Invoke();
                        NotificationLib.SendNotification($"<color=green>Enabled</color> : {button.ButtonText}");
                    }
                    else
                    {
                        button.OnDisable?.Invoke();
                        NotificationLib.SendNotification($"<color=red>Disabled</color> : {button.ButtonText}\n");
                    }
                }

                RefreshMenu();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error while toggling button '{button.ButtonText}': {ex.Message}\nStack Trace: {ex.StackTrace}");
            }
        }


        public class Button
        {
            public string ButtonText { get; set; }
            public bool IsToggle { get; set; }
            public bool NeedsMaster { get; set; }
            public bool Enabled { get; set; }
            public Action? OnEnable { get; set; }
            public Action? OnDisable { get; set; }
            public Category Page { get; set; }

            public Button(string label, Category page, bool isToggle, bool isActive, Action? onClick, Action? onDisable = null, bool doesNeedMaster = false)
            {
                ButtonText = label;
                this.IsToggle = isToggle;
                Enabled = isActive;
                OnEnable = onClick;
                Page = page;
                this.OnDisable = onDisable;
                NeedsMaster = doesNeedMaster;
            }
            public void SetText(string newText)
            {
                ButtonText = newText;
            }
        }

        public class BtnCollider : MonoBehaviour
        {
            public Button? clickedButton;
            public static int clickCooldown = 1;

            public void OnTriggerEnter(Collider collider)
            {
                if (Time.frameCount >= clickCooldown + 25 && collider.gameObject.name == "buttonclicker")
                {
                    transform.localScale = new Vector3(transform.localScale.x / 3, transform.localScale.y, transform.localScale.z);
                    clickCooldown = Time.frameCount;

                    GorillaTagger.Instance.StartVibration(rightHandedMenu, GorillaTagger.Instance.tagHapticStrength / 2, GorillaTagger.Instance.tagHapticDuration / 2);
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(ActuallSound, rightHandedMenu, 1);
                    GetComponent<BoxCollider>().enabled = true;

                    if (clickedButton == null) return;
                    Toggle(clickedButton);
                }
            }
        }

        public static List<Button> GetButtonInfoByPage(Category page)
        {
            if (page == Category.Enabled)
            {
                return Enumerable.ToList<ButtonHandler.Button>(Enumerable.Where<ButtonHandler.Button>(ModButtons.buttons, (ButtonHandler.Button button) => button.Enabled));
            }
            else
            {
                return ModButtons.buttons.Where(button => button.Page == page).ToList();
            }
        }

        public static void DisableAllMods()
        {
            foreach (ButtonHandler.Button button in ModButtons.buttons)
            {
                if (button.Enabled)
                {
                    button.Enabled = false;
                    button.OnDisable?.Invoke();
                }
            }

            RefreshMenu();
        }
    }
}
