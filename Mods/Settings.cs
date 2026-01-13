using Elixir.Management;
using static Elixir.Management.Menu;
using Elixir.Utilities;
using static Elixir.Utilities.ButtonManager;
using System.Collections.Generic;
using System.IO;
using static Elixir.Utilities.Variables;
using Elixir.Notifications;
using BepInEx;
using UnityEngine.UI;

namespace Elixir.Mods.Categories
{
    public class Settings
    { 
        public static void ToggleDisconnect(bool setActive)
        {
            disconnect = setActive;

            var visual = Menu.menu.transform.Find("Canvas/Visual")?.gameObject;
            visual.transform.Find("Home (1)").gameObject.SetActive(Variables.disconnect);
        }
        public static void RoundedMenu(bool rounded)
        {
            var visual = Menu.menu.transform.Find("Canvas/Visual")?.gameObject;
            visual.GetComponent<Image>().pixelsPerUnitMultiplier = rounded ? 100f : 3.5f; ;
        }
        public static void ToggleVCounter(bool setActive)
        {
            vCounter = setActive;

            var visual = Menu.menu.transform.Find("Canvas/Visual")?.gameObject;
            visual.transform.Find("Title/Version").gameObject.SetActive(Variables.vCounter);
        }
        public static void ToggleTips(bool setActive)
        {
            tips = setActive;
        }
        public static void Alphabet(bool setActive)
        {
            alphabet = setActive;
        }
        public static void MenuHand(bool setActive)
        {
            Menu.menuRHand = setActive;
        }
        public static void FlySpeed()
        {
            flyspeedchanger++;
            if (flyspeedchanger > 4)
            {
                flyspeedchanger = 1;
            }
            switch (flyspeedchanger)
            {
                case 1:
                    flyspeedchangerspeed = 15f;
                    NotificationLib.SendNotification("<color=white>[</color>Speed:<color=white>] </color>Normal");
                    GetButton("Change Fly Speed").tooltip = "Current Setting: Normal";
                    break;
                case 2:
                    flyspeedchangerspeed = 7f;
                    NotificationLib.SendNotification("<color=white>[</color>Speed:<color=white>] </color>Slow");
                    GetButton("Change Fly Speed").tooltip = "Current Setting: Slow";
                    break;
                case 3:
                    flyspeedchangerspeed = 30f;
                    NotificationLib.SendNotification("<color=white>[</color>Speed:<color=white>] </color>Fast");
                    GetButton("Change Fly Speed").tooltip = "Current Setting: Fast";
                    break;
                case 4:
                    flyspeedchangerspeed = 60f;
                    NotificationLib.SendNotification("<color=white>[</color>Speed:<color=white>] </color>Very Fast");
                    GetButton("Change Fly Speed").tooltip = "Current Setting: Very Fast";
                    break;
            }
        }
        public static void SpeedSpeed()
        {
            speedboostchanger++;
            if (speedboostchanger > 4)
            {
                speedboostchanger = 1;
            }
            switch (speedboostchanger)
            {
                case 1:
                    speedboostchangerspeed = 8f;
                    NotificationLib.SendNotification("<color=white>[</color>Speed:<color=white>] </color>Normal");
                    GetButton("Change Speed Boost").tooltip = "Current Setting: Normal";
                    break;
                case 2:
                    speedboostchangerspeed = 7.3f;
                    NotificationLib.SendNotification("<color=white>[</color>Speed:<color=white>] </color>Slow");
                    GetButton("Change Speed Boost").tooltip = "Current Setting: Slow";
                    break;
                case 3:
                    speedboostchangerspeed = 15f;
                    NotificationLib.SendNotification("<color=white>[</color>Speed:<color=white>] </color>Fast");
                    GetButton("Change Speed Boost").tooltip = "Current Setting: Fast";
                    break;
                case 4:
                    speedboostchangerspeed = 50f;
                    NotificationLib.SendNotification("<color=white>[</color>Speed:<color=white>] </color>Very Fast");
                    GetButton("Change Speed Boost").tooltip = "Current Setting: Very Fast";
                    break;
            }
        }
        public static void ESPChange()
        {
            espSetting++;
            if (espSetting > 4)
            {
                espSetting = 1;
            }
            switch (espSetting)
            {
                case 1:
                    NotificationLib.SendNotification("<color=white>[</color>ESP Color:<color=white>] </color>Infection");
                    GetButton("Change ESP Color").tooltip = "Current Setting: Infection";
                    break;
                case 2:
                    NotificationLib.SendNotification("<color=white>[</color>ESP Color:<color=white>] </color>Casual");
                    GetButton("Change ESP Color").tooltip = "Current Setting: Casual";
                    break;
                case 3:
                    NotificationLib.SendNotification("<color=white>[</color>ESP Color:<color=white>] </color>RGB");
                    GetButton("Change ESP Color").tooltip = "Current Setting: RGB";
                    break;
                case 4:
                    NotificationLib.SendNotification("<color=white>[</color>ESP Color:<color=white>] </color>Menu Color");
                    GetButton("Change ESP Color").tooltip = "Current Setting: Menu Color";
                    break;
            }
        }
        public static void TracerPos()
        {
            tracePos++;
            if (tracePos > 4)
            {
                tracePos = 1;
            }
            switch (tracePos)
            {
                case 1:
                    NotificationLib.SendNotification("<color=white>[</color>Tracer Position:<color=white>] </color>Right Hand");
                    GetButton("Change Tracer Position").tooltip = "Current Setting: Right Hand";
                    break;
                case 2:
                    NotificationLib.SendNotification("<color=white>[</color>Tracer Position:<color=white>] </color>Left Hand");
                    GetButton("Change Tracer Position").tooltip = "Current Setting: Left Hand";
                    break;
                case 3:
                    NotificationLib.SendNotification("<color=white>[</color>Tracer Position:<color=white>] </color>Body");
                    GetButton("Change Tracer Position").tooltip = "Current Setting: Body";
                    break;
                case 4:
                    NotificationLib.SendNotification("<color=white>[</color>Tracer Position:<color=white>] </color>Head");
                    GetButton("Change Tracer Position").tooltip = "Current Setting: Head";
                    break;
            }
        }
        public static void GunChange()
        {
            gunSetting++;
            if (gunSetting > 2)
            {
                gunSetting = 1;
            }
            switch (gunSetting)
            {
                case 1:
                    NotificationLib.SendNotification("<color=white>[</color>Gun Setting:<color=white>] </color>Ball + Line");
                    GetButton("Change Gun Type").tooltip = "Current Setting: Ball + Line";
                    break;
                case 2:
                    NotificationLib.SendNotification("<color=white>[</color>Gun Setting:<color=white>] </color>Ball");
                    GetButton("Change Gun Type").tooltip = "Current Setting: Ball";
                    break;
            }
        }
        public static void ReportChange()
        {
            gunSetting++;
            if (gunSetting > 2)
            {
                gunSetting = 1;
            }
            switch (gunSetting)
            {
                case 1:
                    NotificationLib.SendNotification("<color=white>[</color>Anti Report:<color=white>] </color>Disconnect");
                    Room.reconnectReport = false;
                    GetButton("Change Anti Report Mode").tooltip = "Current Setting: Disconnect";
                    break;
                case 2:
                    NotificationLib.SendNotification("<color=white>[</color>Anti Report:<color=white>]</color> Reconnect");
                    Room.reconnectReport = true;
                    GetButton("Change Anti Report Mode").tooltip = "Current Setting: Reconnect";
                    break;
            }
        }
        public static void PagesChange()
        {
            pageSetting++;
            if (pageSetting > 3)
            {
                pageSetting = 1;
            }
            switch (pageSetting)
            {
                case 1:
                    NotificationLib.SendNotification("<color=white>[</color>Page Mode:<color=white>] </color>Page Buttons");
                    GetButton("Change Page Mode").tooltip = "Current Setting: Page Buttons";
                    triggerMenu = false;
                    gripMenu = false;
                    break;
                case 2:
                    NotificationLib.SendNotification("<color=white>[</color>Page Mode:<color=white>] </color>Triggers");
                    GetButton("Change Page Mode").tooltip = "Current Setting: Triggers";
                    triggerMenu = true;
                    gripMenu = false;
                    break;
                case 3:
                    NotificationLib.SendNotification("<color=white>[</color>Page Mode:<color=white>]</color> Grips");
                    GetButton("Change Page Mode").tooltip = "Current Setting: Grips";
                    gripMenu = true;
                    triggerMenu = false;
                    break;
            }

            UpdateButtonDisplay();
            Menu.Buttons();
        }
        public static void VisReport(bool e)
        {
            VisReportBool = e;
        }
        public static void Discord()
        {
            UnityEngine.Application.OpenURL("https://discord.gg/QFeUpmg8vd");
        }

        #region Prefs
        public static void SavePrefs()
        {
            string menuFolder = Path.Combine(Paths.GameRootPath, "Elixir");
            if (!Directory.Exists(menuFolder))
            {
                Directory.CreateDirectory(menuFolder);
            }

            List<string> enabledMods = new List<string>();
            List<string> settings = new List<string>();

            foreach (var category in Menu.categories)
            {
                if (category == null || category.buttons == null) continue;

                foreach (var mod in category.buttons)
                {
                    if (mod.isToggleable && mod.toggled)
                    {
                        enabledMods.Add(mod.title);
                    }
                }
            }

            settings.Add(espSetting.ToString());
            settings.Add(tracePos.ToString());
            settings.Add(gunSetting.ToString());
            settings.Add(pageSetting.ToString()); // persist page mode
            settings.Add(speedboostchanger.ToString());
            settings.Add(flyspeedchanger.ToString());
            settings.Add(disconnect ? "1" : "0");
            settings.Add(vCounter ? "1" : "0");
            settings.Add(tips ? "1" : "0");
            settings.Add(alphabet ? "1" : "0");
            settings.Add(Menu.menuRHand ? "1" : "0");
            settings.Add(VisReportBool ? "1" : "0");

            File.WriteAllLines(Path.Combine(menuFolder, "EnabledMods.txt"), enabledMods);
            File.WriteAllLines(Path.Combine(menuFolder, "Settings.txt"), settings);

            NotificationLib.SendNotification("<color=white>[</color>Save<color=white>]</color> Saved Preferences");
        }
        public static void LoadPrefs()
        {
            string menuFolder = Path.Combine(Paths.GameRootPath, "Elixir");
            string enabledPath = Path.Combine(menuFolder, "EnabledMods.txt");
            string settingPath = Path.Combine(menuFolder, "Settings.txt");

            if (!File.Exists(enabledPath) || !File.Exists(settingPath))
            {
                NotificationLib.SendNotification("<color=white>[</color>Load<color=white>]</color> No Save Found");
                return;
            }

            string[] enabledMods = File.ReadAllLines(enabledPath);
            string[] settings = File.ReadAllLines(settingPath);

            foreach (var category in Menu.categories)
            {
                if (category == null || category.buttons == null) continue;

                foreach (var mod in category.buttons)
                {
                    foreach (string enabledMod in enabledMods)
                    {
                        if (mod.title == enabledMod && mod.isToggleable)
                        {
                            mod.toggled = true;
                        }
                    }
                }
            }

            if (settings.Length >= 12)
            {
                espSetting = int.Parse(settings[0]);
                tracePos = int.Parse(settings[1]);
                gunSetting = int.Parse(settings[2]);
                pageSetting = int.Parse(settings[3]);
                speedboostchanger = int.Parse(settings[4]);
                flyspeedchanger = int.Parse(settings[5]);
                disconnect = settings[6] == "1";
                vCounter = settings[7] == "1";
                tips = settings[8] == "1";
                alphabet = settings[9] == "1";
                Menu.menuRHand = settings[10] == "1";
                VisReportBool = settings[11] == "1";

                // Apply pageSetting to menu navigation flags
                switch (pageSetting)
                {
                    case 1:
                        triggerMenu = false;
                        gripMenu = false;
                        break;
                    case 2:
                        triggerMenu = true;
                        gripMenu = false;
                        break;
                    case 3:
                        triggerMenu = false;
                        gripMenu = true;
                        break;
                }
            }

            UpdateButtonDisplay();
            Menu.Buttons();

            NotificationLib.SendNotification("<color=white>[</color>Load<color=white>]</color> Loaded Preferences");
        }
        static void UpdateButtonDisplay()
        {
            void SetTooltip(string contains, string tooltip)
            {
                var btn = GetButton(contains);
                if (btn != null) btn.tooltip = tooltip;
            }

            foreach (var cat in Menu.categories)
            {
                if (cat == null || cat.buttons == null) continue;

                foreach (var mod in cat.buttons)
                {
                    if (mod == null) continue;

                    string title = mod.title ?? "";

                    if (title.Contains("Right Handed Menu"))
                        mod.toggled = Menu.menuRHand;
                    else if (title.Contains("Toggle Tooltips"))
                        mod.toggled = tips;
                    else if (title.Contains("Toggle Disconnect"))
                        mod.toggled = disconnect;
                    else if (title.Contains("Toggle Version Counter"))
                        mod.toggled = vCounter;
                    else if (title.Contains("Sort Buttons Alphabetically"))
                        mod.toggled = alphabet;
                    else if (title.Contains("Visualize Antireport"))
                        mod.toggled = VisReportBool;
                }
            }

            switch (espSetting)
            {
                case 1: SetTooltip("Change ESP Color", "Current Setting: Infection"); break;
                case 2: SetTooltip("Change ESP Color", "Current Setting: Casual"); break;
                case 3: SetTooltip("Change ESP Color", "Current Setting: RGB"); break;
                case 4: SetTooltip("Change ESP Color", "Current Setting: Menu Color"); break;
            }

            switch (tracePos)
            {
                case 1: SetTooltip("Change Tracer Position", "Current Setting: Right Hand"); break;
                case 2: SetTooltip("Change Tracer Position", "Current Setting: Left Hand"); break;
                case 3: SetTooltip("Change Tracer Position", "Current Setting: Body"); break;
                case 4: SetTooltip("Change Tracer Position", "Current Setting: Head"); break;
            }

            switch (gunSetting)
            {
                case 1: SetTooltip("Change Gun Type", "Current Setting: Ball + Line"); break;
                case 2: SetTooltip("Change Gun Type", "Current Setting: Ball"); break;
            }

            switch (flyspeedchanger)
            {
                case 1: SetTooltip("Change Fly Speed", "Current Setting: Normal"); break;
                case 2: SetTooltip("Change Fly Speed", "Current Setting: Slow"); break;
                case 3: SetTooltip("Change Fly Speed", "Current Setting: Fast"); break;
                case 4: SetTooltip("Change Fly Speed", "Current Setting: Very Fast"); break;
            }

            switch (speedboostchanger)
            {
                case 1: SetTooltip("Change Speed Boost", "Current Setting: Normal"); break;
                case 2: SetTooltip("Change Speed Boost", "Current Setting: Slow"); break;
                case 3: SetTooltip("Change Speed Boost", "Current Setting: Fast"); break;
                case 4: SetTooltip("Change Speed Boost", "Current Setting: Very Fast"); break;
            }

            switch (pageSetting)
            {
                case 1: SetTooltip("Change Page Mode", "Current Setting: Page Buttons"); break;
                case 2: SetTooltip("Change Page Mode", "Current Setting: Triggers"); break;
                case 3: SetTooltip("Change Page Mode", "Current Setting: Grips"); break;
            }

            SetTooltip("Change Anti Report Mode", Room.reconnectReport ? "Current Setting: Reconnect" : "Current Setting: Disconnect");
        }
#endregion

        public static int espSetting = 1;

        public static int pageSetting = 1;

        public static int tracePos = 1;

        public static int gunSetting = 1;

        static int speedboostchanger = 1;
        public static float speedboostchangerspeed = 8;

        static int flyspeedchanger = 1;
        public static float flyspeedchangerspeed = 15;

        public static bool VisReportBool = true;
    }
}
