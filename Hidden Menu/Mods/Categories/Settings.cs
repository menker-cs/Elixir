using static Hidden.Utilities.Variables;
using static Hidden.Menu.Main;
using Hidden.Menu;
using Hidden.Utilities.Notifs;

namespace Hidden.Mods.Categories
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
                if (flyspeedchanger == 1)
                {
                    if (btn.buttonText == "Change Fly Speed: Very Fast")
                    {
                        btn.SetText("Change Fly Speed: Normal");
                        speedboostchangerspeed = 15f;
                        NotificationLib.SendNotification("<color=white>[</color><color=blue>Speed:</color><color=white>] Normal</color>");
                    }
                }
                if (flyspeedchanger == 2)
                {
                    if (btn.buttonText == "Change Fly Speed: Normal")
                    {
                        btn.SetText("Change Fly Speed: Slow");
                        speedboostchangerspeed = 7f;
                        NotificationLib.SendNotification("<color=white>[</color><color=blue>Speed:</color><color=white>] Slow</color>");
                    }
                }
                if (flyspeedchanger == 3)
                {
                    if (btn.buttonText == "Change Fly Speed: Slow")
                    {
                        btn.SetText("Change Fly Speed: Fast");
                        speedboostchangerspeed = 30f;
                        NotificationLib.SendNotification("<color=white>[</color><color=blue>Speed:</color><color=white>] Fast</color>");
                    }
                }
                if (flyspeedchanger == 4)
                {
                    if (btn.buttonText == "Change Fly Speed: Fast")
                    {
                        btn.SetText("Change Fly Speed: Very Fast");
                        speedboostchangerspeed = 60f;
                        NotificationLib.SendNotification("<color=white>[</color><color=blue>Speed:</color><color=white>] Very Fast</color>");
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
                if (speedboostchanger == 1)
                {
                    if (btn.buttonText == "Change Speed Boost: Very Fast")
                    {
                        btn.SetText("Change Speed Boost: Normal");
                        speedboostchangerspeed = 8f;
                        NotificationLib.SendNotification("<color=white>[</color><color=blue>Speed:</color><color=white>] Normal</color>");
                    }
                }
                if (speedboostchanger == 2)
                {
                    if (btn.buttonText == "Change Speed Boost: Normal")
                    {
                        btn.SetText("Change Speed Boost: Slow");
                        speedboostchangerspeed = 7.3f;
                        NotificationLib.SendNotification("<color=white>[</color><color=blue>Speed:</color><color=white>] Slow</color>");
                    }
                }
                if (speedboostchanger == 3)
                {
                    if (btn.buttonText == "Change Speed Boost: Slow")
                    {
                        btn.SetText("Change Speed Boost: Fast");
                        speedboostchangerspeed = 15f;
                        NotificationLib.SendNotification("<color=white>[</color><color=blue>Speed:</color><color=white>] Fast</color>");
                    }
                }
                if (speedboostchanger == 4)
                {
                    if (btn.buttonText == "Change Speed Boost: Fast")
                    {
                        btn.SetText("Change Speed Boost: Very Fast");
                        speedboostchangerspeed = 50f;
                        NotificationLib.SendNotification("<color=white>[</color><color=blue>Speed:</color><color=white>] Very Fast</color>");
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
                if (espSetting == 1)
                {
                    if (btn.buttonText == "Change ESP Color: Menu Color")
                    {
                        btn.SetText("Change ESP Color: Infection");
                        NotificationLib.SendNotification("<color=white>[</color><color=blue>ESP Color:</color><color=white>] Infection</color>");
                    }
                }
                if (espSetting == 2)
                {
                    if (btn.buttonText == "Change ESP Color: Infection")
                    {
                        btn.SetText("Change ESP Color: Casual");
                        NotificationLib.SendNotification("<color=white>[</color><color=blue>ESP Color:</color><color=white>] Casual</color>");
                    }
                }
                if (espSetting == 3)
                {
                    if (btn.buttonText == "Change ESP Color: Casual")
                    {
                        btn.SetText("Change ESP Color: RGB");
                        NotificationLib.SendNotification("<color=white>[</color><color=blue>ESP Color:</color><color=white>] RGB</color>");
                    }
                }
                if (espSetting == 4)
                {
                    if (btn.buttonText == "Change ESP Color: RGB")
                    {
                        btn.SetText("Change ESP Color: Menu Color");
                        NotificationLib.SendNotification("<color=white>[</color><color=blue>ESP Color:</color><color=white>] Menu Color</color>");
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
                if (gunSetting == 1)
                {
                    if (btn.buttonText == "Change Gun Type: Ball")
                    {
                        btn.SetText("Change Gun Type: Ball + Line");
                        NotificationLib.SendNotification("<color=white>[</color><color=blue>ESP Color:</color><color=white>] Ball + Line</color>");
                    }
                }
                if (gunSetting == 2)
                {
                    if (btn.buttonText == "Change Gun Type: Ball + Line")
                    {
                        btn.SetText("Change Gun Type: Ball");
                        NotificationLib.SendNotification("<color=white>[</color><color=blue>ESP Color:</color><color=white>] Ball</color>");
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
        public static void Link(string url)
        {
            UnityEngine.Application.OpenURL(url);
        }

        public static int espSetting = 1;

        public static int gunSetting = 1;

        static int speedboostchanger = 1;
        public static float speedboostchangerspeed = 15;

        static int flyspeedchanger = 1;
        public static float flyspeedchangerspeed = 15;

        public static bool VisReportBool = true;
    }
}
