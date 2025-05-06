using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using UnityEngine.InputSystem;
using UnityEngine;
using Object = UnityEngine.Object;
using static Hidden.Utilities.ColorLib;
using static Hidden.Utilities.Variables;
using static Hidden.Menu.Main;
using static Hidden.Menu.ButtonHandler;
using static Hidden.Menu.Optimizations;
using Hidden.Utilities;
using Hidden.Menu;
using static Hidden.Mods.Categories.Move;
using static Hidden.Utilities.Patches.OtherPatches;
using static Hidden.Utilities.GunTemplate;
using System.Linq;
using Oculus.Platform;
using Photon.Pun;

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
        public static void ClearNotifications()
        {
            NotificationLib.ClearAllNotifications();
        }

        public static void ToggleNotifications(bool setActive)
        {
            toggleNotifications = setActive;
        }

        public static void ToggleDisconnectButton(bool setActive)
        {
            toggledisconnectButton = setActive;
        }
        public static void ESPChange()
        {
            espSetting++;
            if (espSetting > 4)
            {
                espSetting = 1;
            }
            if (espSetting == 1)
            {
                espColor = 1;
                NotificationLib.SendNotification("<color=white>[</color><color=blue>ESP Color:</color><color=white>] Infection</color>");
            }
            if (espSetting == 2)
            {
                espColor = 2;
                NotificationLib.SendNotification("<color=white>[</color><color=blue>ESP Color:</color><color=white>] Casual</color>");
            }
            if (espSetting == 3)
            {
                espColor = 3;
                NotificationLib.SendNotification("<color=white>[</color><color=blue>ESP Color:</color><color=white>] RGB</color>");
            }
            if (espSetting == 4)
            {
                espColor = 4;
                NotificationLib.SendNotification("<color=white>[</color><color=blue>ESP Color:</color><color=white>] Menu Color</color>");
            }
        }
        public static void VisReport()
        {
            e = !e;
            VisReportBool = e;
        }
        public static void Discord()
        {
            UnityEngine.Application.OpenURL("https://discord.gg/QFeUpmg8vd");
        }

        public static int espColor = 1;
        public static int espSetting;

        public static bool VisReportBool = true;
        public static bool e = false;
    }
}
