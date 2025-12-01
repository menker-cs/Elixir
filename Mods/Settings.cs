using BepInEx;
using Elixir.Management;
using Elixir.Utilities;
using static Elixir.Management.Buttons;
using System.Collections.Generic;
using System.IO;
using static Elixir.Utilities.Variables;
using Elixir.Notifications;

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
                    speedboostchangerspeed = 15f;
                    NotificationLib.SendNotification("<color=white>[</color><color=blue>Speed:</color><color=white>] Normal</color>");
                    GetButton("Fly Speed").tooltip = "Current Setting: Normal";
                    break;
                case 2:
                    speedboostchangerspeed = 7f;
                    NotificationLib.SendNotification("<color=white>[</color><color=blue>Speed:</color><color=white>] Slow</color>");
                    GetButton("Fly Speed").tooltip = "Current Setting: Slow";
                    break;
                case 3:
                    speedboostchangerspeed = 30f;
                    NotificationLib.SendNotification("<color=white>[</color><color=blue>Speed:</color><color=white>] Fast</color>");
                    GetButton("Fly Speed").tooltip = "Current Setting: Fast";
                    break;
                case 4:
                    speedboostchangerspeed = 60f;
                    NotificationLib.SendNotification("<color=white>[</color><color=blue>Speed:</color><color=white>] Very Fast</color>");
                    GetButton("Fly Speed").tooltip = "Current Setting: Very Fast";
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
                    NotificationLib.SendNotification("<color=white>[</color><color=blue>Speed:</color><color=white>] Normal</color>");
                    GetButton("Change Speed Boost").tooltip = "Current Setting: Normal";
                    break;
                case 2:
                    speedboostchangerspeed = 7.3f;
                    NotificationLib.SendNotification("<color=white>[</color><color=blue>Speed:</color><color=white>] Slow</color>");
                    GetButton("Change Speed Boost").tooltip = "Current Setting: Slow";
                    break;
                case 3:
                    speedboostchangerspeed = 15f;
                    NotificationLib.SendNotification("<color=white>[</color><color=blue>Speed:</color><color=white>] Fast</color>");
                    GetButton("Change Speed Boost").tooltip = "Current Setting: Fast";
                    break;
                case 4:
                    speedboostchangerspeed = 50f;
                    NotificationLib.SendNotification("<color=white>[</color><color=blue>Speed:</color><color=white>] Very Fast</color>");
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
                    NotificationLib.SendNotification("<color=white>[</color><color=blue>ESP Color:</color><color=white>] Infection</color>");
                    GetButton("Change ESP Color").tooltip = "Current Setting: Infection";
                    break;
                case 2:
                    NotificationLib.SendNotification("<color=white>[</color><color=blue>ESP Color:</color><color=white>] Casual</color>");
                    GetButton("Change ESP Color").tooltip = "Current Setting: Casual";
                    break;
                case 3:
                    NotificationLib.SendNotification("<color=white>[</color><color=blue>ESP Color:</color><color=white>] RGB</color>");
                    GetButton("Change ESP Color").tooltip = "Current Setting: RGB";
                    break;
                case 4:
                    NotificationLib.SendNotification("<color=white>[</color><color=blue>ESP Color:</color><color=white>] Menu Color</color>");
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
                    NotificationLib.SendNotification("<color=white>[</color><color=blue>Tracer Position:</color><color=white>] Right Hand</color>");
                    GetButton("Change Tracer").tooltip = "Current Setting: Right Hand";
                    break;
                case 2:
                    NotificationLib.SendNotification("<color=white>[</color><color=blue>Tracer Position:</color><color=white>] Left Hand</color>");
                    GetButton("Change Tracer").tooltip = "Current Setting: Left Hand";
                    break;
                case 3:
                    NotificationLib.SendNotification("<color=white>[</color><color=blue>Tracer Position:</color><color=white>] Body</color>");
                    GetButton("Change Tracer").tooltip = "Current Setting: Body";
                    break;
                case 4:
                    NotificationLib.SendNotification("<color=white>[</color><color=blue>Tracer Position:</color><color=white>] Head</color>");
                    GetButton("Change Tracer").tooltip = "Current Setting: Head";
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
                    NotificationLib.SendNotification("<color=white>[</color><color=blue>Gun Setting:</color><color=white>] Ball + Line</color>");
                    GetButton("Change Gun Type").tooltip = "Current Setting: Ball + Line";
                    break;
                case 2:
                    NotificationLib.SendNotification("<color=white>[</color><color=blue>Gun Setting:</color><color=white>] Ball</color>");
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
                    NotificationLib.SendNotification("<color=white>[</color><color=blue>Anti Report:</color><color=white>] Disconnect</color>");
                    Room.reconnectReport = false;
                    GetButton("Change Anti Report Mode").tooltip = "Current Setting: Disconnect";
                    break;
                case 2:
                    NotificationLib.SendNotification("<color=white>[</color><color=blue>Anti Report:</color><color=white>] Reconnect</color>");
                    Room.reconnectReport = true;
                    GetButton("Change Anti Report Mode").tooltip = "Current Setting: Reconnect";
                    break;
            }
        }
        public static void VisReport(bool e)
        {
            VisReportBool = e;
        }
        public static void Discord()
        {
            UnityEngine.Application.OpenURL("https://discord.gg/QFeUpmg8vd");
        }

        public static List<string> enabledBtns = new List<string>();

        public static int espSetting = 1;

        public static int tracePos = 1;

        public static int gunSetting = 1;

        static int speedboostchanger = 1;
        public static float speedboostchangerspeed = 8;

        static int flyspeedchanger = 1;
        public static float flyspeedchangerspeed = 15;

        public static bool VisReportBool = true;
    }
}
