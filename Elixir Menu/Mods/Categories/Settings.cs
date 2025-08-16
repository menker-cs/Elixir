using static Elixir.Utilities.Variables;
using static Elixir.Menu.Main;
using Elixir.Menu;
using Elixir.Utilities.Notifs;

namespace Elixir.Mods.Categories
{
    public class Settings
    { 
        public static void SwitchHands(bool setActive)
        {
            rightHandedMenu = setActive;
        }

        public static void Bark(bool setActive)
        {
            bark = setActive;
        }
        public static void OLine(bool setActive)
        {
            outl = setActive;
        }
        public static void Grav(bool setActive)
        {
            grav = setActive;
        }
        public static void ClearNotifications()
        {
            NotificationLib.ClearAllNotifications();
        }

        public static void ToggleNotifications(bool setActive)
        {
            toggleNotifications = setActive;
        }
        public static void ToggleTip(bool setActive)
        {
            tip = setActive;
        }
        public static void ToggleVCounter(bool setActive)
        {
            vCounter = setActive;
        }
        public static void ToggleDisconnectButton(bool setActive)
        {
            toggledisconnectButton = setActive;
        }
        public static void FlySpeed()
        {
            flyspeedchanger++;
            if (flyspeedchanger > 4)
            {
                flyspeedchanger = 1;
            }
            foreach (ButtonHandler.Button btn in ModButtons.buttons)
            {
                if (btn.buttonText.Contains("Change Fly Speed:"))
                {
                    switch (flyspeedchanger)
                    {
                        case 1:
                            btn.SetText("Change Fly Speed: Normal");
                            speedboostchangerspeed = 15f;
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>Speed:</color><color=white>] Normal</color>");
                            break;
                        case 2:
                            btn.SetText("Change Fly Speed: Slow");
                            speedboostchangerspeed = 7f;
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>Speed:</color><color=white>] Slow</color>");
                            break;
                        case 3:
                            btn.SetText("Change Fly Speed: Fast");
                            speedboostchangerspeed = 30f;
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>Speed:</color><color=white>] Fast</color>");
                            break;
                        case 4:
                            btn.SetText("Change Fly Speed: Very Fast");
                            speedboostchangerspeed = 60f;
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>Speed:</color><color=white>] Very Fast</color>");
                            break;
                    }
                }
            }
        }
        public static void SpeedSpeed()
        {
            speedboostchanger++;
            if (speedboostchanger > 4)
            {
                speedboostchanger = 1;
            }
            foreach (ButtonHandler.Button btn in ModButtons.buttons)
            {
                if (btn.buttonText.Contains("Change Speed Boost:"))
                {
                    switch (speedboostchanger)
                    {
                        case 1:
                            btn.SetText("Change Speed Boost: Normal");
                            speedboostchangerspeed = 8f;
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>Speed:</color><color=white>] Normal</color>");
                            break;
                        case 2:
                            btn.SetText("Change Speed Boost: Slow");
                            speedboostchangerspeed = 7.3f;
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>Speed:</color><color=white>] Slow</color>");
                            break;
                        case 3:
                            btn.SetText("Change Speed Boost: Fast");
                            speedboostchangerspeed = 15f;
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>Speed:</color><color=white>] Fast</color>");
                            break;
                        case 4:
                            btn.SetText("Change Speed Boost: Very Fast");
                            speedboostchangerspeed = 50f;
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>Speed:</color><color=white>] Very Fast</color>");
                            break;
                    }
                }
            }
        }
        public static void ESPChange()
        {
            espSetting++;
            if (espSetting > 4)
            {
                espSetting = 1;
            }
            foreach (ButtonHandler.Button btn in ModButtons.buttons)
            {
                if (btn.buttonText.Contains("Change ESP Color:"))
                {
                    switch (espSetting)
                    {
                        case 1:
                            btn.SetText("Change ESP Color: Infection");
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>ESP Color:</color><color=white>] Infection</color>");
                            break;
                        case 2:
                            btn.SetText("Change ESP Color: Casual");
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>ESP Color:</color><color=white>] Casual</color>");
                            break;
                        case 3:
                            btn.SetText("Change ESP Color: RGB");
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>ESP Color:</color><color=white>] RGB</color>");
                            break;
                        case 4:
                            btn.SetText("Change ESP Color: Menu Color");
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>ESP Color:</color><color=white>] Menu Color</color>");
                            break;
                    }
                }
            }
        }
        public static void TracerPos()
        {
            tracePos++;
            if (tracePos > 4)
            {
                tracePos = 1;
            }
            foreach (ButtonHandler.Button btn in ModButtons.buttons)
            {
                if (btn.buttonText.Contains("Change Tracer Position:"))
                {
                    switch (tracePos)
                    {
                        case 1:
                            btn.SetText("Change Tracer Position: Right");
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>Tracer Position:</color><color=white>] Right Hand</color>");
                            break;
                        case 2:
                            btn.SetText("Change Tracer Position: Left");
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>Tracer Position:</color><color=white>] Left Hand</color>");
                            break;
                        case 3:
                            btn.SetText("Change Tracer Position: Body");
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>Tracer Position:</color><color=white>] Body</color>");
                            break;
                        case 4:
                            btn.SetText("Change Tracer Position: Head");
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>Tracer Position:</color><color=white>] Head</color>");
                            break;
                    }
                }
            }
        }
        public static void GunChange()
        {
            gunSetting++;
            if (gunSetting > 2)
            {
                gunSetting = 1;
            }
            foreach (ButtonHandler.Button btn in ModButtons.buttons)
            {
                if (btn.buttonText.Contains("Change Gun Type:"))
                {
                    switch (gunSetting)
                    {
                        case 1:
                            btn.SetText("Change Gun Type: Ball + Line");
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>ESP Color:</color><color=white>] Ball + Line</color>");
                            break;
                        case 2:
                            btn.SetText("Change Gun Type: Ball");
                            NotificationLib.SendNotification("<color=white>[</color><color=blue>ESP Color:</color><color=white>] Ball</color>");
                            break;
                    }
                }
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

        public static int espSetting = 1;

        public static int tracePos = 1;

        public static int gunSetting = 1;

        static int speedboostchanger = 1;
        public static float speedboostchangerspeed = 15;

        static int flyspeedchanger = 1;
        public static float flyspeedchangerspeed = 15;

        public static bool VisReportBool = true;
    }
}
