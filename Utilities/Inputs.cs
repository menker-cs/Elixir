using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.XR;
using UnityEngine;
using Valve.VR;

namespace Elixir.Utilities
{
    public class Inputs
    {
        public static bool leftGrip()
        {
            return ControllerInputPoller.instance.leftGrab;
        }

        public static bool leftTrigger()
        {
            return ControllerInputPoller.instance.leftControllerIndexFloat > 0.2f;
        }

        public static bool leftPrimary()
        {
            return ControllerInputPoller.instance.leftControllerPrimaryButton;
        }

        public static bool leftSecondary()
        {
            return ControllerInputPoller.instance.leftControllerSecondaryButton;
        }

        public static bool rightGrip()
        {
            return ControllerInputPoller.instance.rightGrab;
        }

        public static bool rightTrigger()
        {
            return ControllerInputPoller.instance.rightControllerIndexFloat > 0.2f;
        }

        public static bool rightPrimary()
        {
            return ControllerInputPoller.instance.rightControllerPrimaryButton;
        }

        public static bool rightSecondary()
        {
            return ControllerInputPoller.instance.rightControllerSecondaryButton;
        }
    }
}
