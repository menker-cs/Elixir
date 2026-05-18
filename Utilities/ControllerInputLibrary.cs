using UnityEngine;
using Valve.VR;

namespace Elixir.Utilities
{
    internal class ControllerInputLibrary
    {
        public static float triggerZone = 0.5f;
        public static bool RightTrigger() { return ControllerInputPoller.instance.rightControllerIndexFloat >= triggerZone; }
        public static bool LeftTrigger() { return ControllerInputPoller.instance.leftControllerIndexFloat >= triggerZone; }
        public static bool RightPrimary() { return ControllerInputPoller.instance.rightControllerPrimaryButton; }
        public static bool LeftPrimary() { return ControllerInputPoller.instance.leftControllerPrimaryButton; }
        public static bool RightSecondary() { return ControllerInputPoller.instance.rightControllerSecondaryButton; }
        public static bool LeftSecondary() { return ControllerInputPoller.instance.leftControllerSecondaryButton; }
        public static bool RightGrip() { return ControllerInputPoller.instance.rightGrab; }
        public static bool LeftGrip() { return ControllerInputPoller.instance.leftGrab; }
        public static bool RightStick() { return SteamVR_Actions.gorillaTag_RightJoystickClick.GetState(SteamVR_Input_Sources.RightHand); }
        public static bool LeftStick() { return SteamVR_Actions.gorillaTag_LeftJoystickClick.GetState(SteamVR_Input_Sources.LeftHand); }

        public static Vector2 RightStickAxis()
        {
            return ControllerInputPoller.instance.rightControllerPrimary2DAxis;
        }
        public static Vector2 LeftStickAxis()
        {
            return SteamVR_Actions.gorillaTag_LeftJoystick2DAxis.GetAxis((SteamVR_Input_Sources)1);
        }
    }
}
