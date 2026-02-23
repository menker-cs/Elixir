using BepInEx;
using Elixir.Management;
using Elixir.Notifications;
using Elixir.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Elixir.Management.Menu;
using static Elixir.Utilities.ButtonManager;
using static Elixir.Utilities.Variables;

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


        static string PrefsFolder = Path.Combine(Paths.GameRootPath, "Elixir");
        static string EnabledModsTxt = "EnabledMods.txt";
        static string SettingsTxt = "Settings.txt";
        static int SettingsVersion = 1;

        public static void SavePrefs()
        {
            if (!Directory.Exists(PrefsFolder))
                Directory.CreateDirectory(PrefsFolder);

            File.WriteAllText(Path.Combine(PrefsFolder, EnabledModsTxt), string.Empty);

            var enabledMods = Menu.categories
                .Where(c => c?.buttons != null)
                .SelectMany(c => c.buttons)
                .Where(b => b.isToggleable && b.toggled)
                .Select(b => b.title);

            File.WriteAllLines(Path.Combine(PrefsFolder, EnabledModsTxt), enabledMods);

            var settings = new Dictionary<string, string>
            {
                ["version"] = SettingsVersion.ToString(),
                ["espSetting"] = espSetting.ToString(),
                ["tracePos"] = tracePos.ToString(),
                ["gunSetting"] = gunSetting.ToString(),
                ["pageSetting"] = pageSetting.ToString(),
                ["speedboost"] = speedboostchanger.ToString(),
                ["flyspeed"] = flyspeedchanger.ToString(),
            };

            File.WriteAllLines(
                Path.Combine(PrefsFolder, SettingsTxt),
                settings.Select(kv => $"{kv.Key}={kv.Value}")
            );

            NotificationLib.SendNotification("<color=white>[</color>Save<color=white>]</color> Saved Preferences");
        }

        public static void LoadPrefs()
        {
            string enabledPath = Path.Combine(PrefsFolder, EnabledModsTxt);
            string settingPath = Path.Combine(PrefsFolder, SettingsTxt);

            if (!File.Exists(enabledPath) || !File.Exists(settingPath))
            {
                NotificationLib.SendNotification("<color=white>[</color>Load<color=white>]</color> No Save Found");
                return;
            }

            var settings = File.ReadAllLines(settingPath).Where(line => line.Contains('=')).Select(line => line.Split('=', 2)).ToDictionary(parts => parts[0].Trim(), parts => parts[1].Trim());

            espSetting = ParseInt(settings, "espSetting", espSetting);
            tracePos = ParseInt(settings, "tracePos", tracePos);
            gunSetting = ParseInt(settings, "gunSetting", gunSetting);
            pageSetting = ParseInt(settings, "pageSetting", pageSetting);
            speedboostchanger = ParseInt(settings, "speedboost", speedboostchanger);
            flyspeedchanger = ParseInt(settings, "flyspeed", flyspeedchanger);
            disconnect = ParseBool(settings, "disconnect", disconnect);
            vCounter = ParseBool(settings, "vCounter", vCounter);
            tips = ParseBool(settings, "tips", tips);
            alphabet = ParseBool(settings, "alphabet", alphabet);
            Menu.menuRHand = ParseBool(settings, "menuRHand", Menu.menuRHand);
            VisReportBool = ParseBool(settings, "visReport", VisReportBool);

            triggerMenu = pageSetting == 2;
            gripMenu = pageSetting == 3;

            var enabledMods = new HashSet<string>(File.ReadAllLines(enabledPath));
            DisableAllMods();
            foreach (var cat in Menu.categories.Where(c => c?.buttons != null))
            {
                foreach (var mod in cat.buttons.Where(m => m.isToggleable && enabledMods.Contains(m.title)))
                {
                    mod.toggled = true;
                }
            }

            UpdateButtonDisplay();

            NotificationLib.SendNotification("<color=white>[</color>Load<color=white>]</color> Loaded Preferences");
            Buttons();
        }

        private static int ParseInt(Dictionary<string, string> d, string key, int fallback)
        {
            if (d.TryGetValue(key, out string val) && int.TryParse(val, out int result))
                return result;
            return fallback;
        }

        private static bool ParseBool(Dictionary<string, string> d, string key, bool fallback)
        {
            if (d.TryGetValue(key, out string val))
                return val == "1";
            return fallback;
        }

        static void SetTooltip(string contains, string tooltip)
        {
            var btn = GetButton(contains);
            if (btn != null) btn.tooltip = tooltip;
        }

        static void UpdateButtonDisplay()
        {
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

        public static void AutoLoadPrefs()
        {
            string enabledPath = Path.Combine(PrefsFolder, EnabledModsTxt);

            if (!File.Exists(enabledPath))
            {
                Debug.Log("AutoLoadPrefs: Enabled mods file not found.");
                return;
            }

            var lines = File.ReadAllLines(enabledPath).Where(line => !string.IsNullOrWhiteSpace(line)).Select(line => line.Trim()).ToArray();
            bool load = lines.Any(line => string.Equals(line, "Auto Load Preferences", StringComparison.OrdinalIgnoreCase));

            if (load)
            {
                LoadPrefs();
                Debug.Log("Auto Loaded Preferences");
            }
        }


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