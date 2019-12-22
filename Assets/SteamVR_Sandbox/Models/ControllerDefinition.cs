using System;

using UnityEngine;

namespace SteamVR_Sandbox.Models
{
    public class ControllerDefinition
    {
        private static readonly ControllerDefinition IndexController;
        private static readonly ControllerDefinition OculusTouch;

        private Vector3 leftHandPosition;
        private Quaternion leftHandRotation;

        private Vector3 rightHandPosition;
        private Quaternion rightHandRotation;

        static ControllerDefinition()
        {
            // Index HMD
            IndexController = new ControllerDefinition
            {
                leftHandPosition = new Vector3(0, .025f, -.1f),
                leftHandRotation = Quaternion.Euler(135f, -75, -35),
                rightHandPosition = new Vector3(0, -.025f, -.1f),
                rightHandRotation = Quaternion.Euler(135f, 75, 35)
            };

            // Oculus Rift S, Oculus Quest
            OculusTouch = new ControllerDefinition
            {
                leftHandPosition = new Vector3(0, .025f, .11f),
                leftHandRotation = Quaternion.Euler(5, 90, 105),
                rightHandPosition = new Vector3(0, -.025f, .11f),
                rightHandRotation = Quaternion.Euler(5, -90, -105)
            };
        }

        public static Vector3 GetControllerPositionOffset(string controller, ControllerSide side)
        {
            switch (controller)
            {
                case "indexhmd":
                    return GetControllerPositionOffset(IndexController, side);

                case "rift":
                    return GetControllerPositionOffset(OculusTouch, side);

                default:
                    throw new ArgumentOutOfRangeException(nameof(controller));
            }
        }

        public static Quaternion GetControllerRotationOffset(string controller, ControllerSide side)
        {
            switch (controller)
            {
                case "indexhmd":
                    return GetControllerRotationOffset(IndexController, side);

                case "rift":
                    return GetControllerRotationOffset(OculusTouch, side);

                default:
                    throw new ArgumentOutOfRangeException(nameof(controller));
            }
        }

        private static Vector3 GetControllerPositionOffset(ControllerDefinition definition, ControllerSide side)
        {
            switch (side)
            {
                case ControllerSide.Left:
                    return definition.leftHandPosition;

                case ControllerSide.Right:
                    return definition.rightHandPosition;

                default:
                    throw new ArgumentOutOfRangeException(nameof(side));
            }
        }

        private static Quaternion GetControllerRotationOffset(ControllerDefinition definition, ControllerSide side)
        {
            switch (side)
            {
                case ControllerSide.Left:
                    return definition.leftHandRotation;

                case ControllerSide.Right:
                    return definition.rightHandRotation;

                default:
                    throw new ArgumentOutOfRangeException(nameof(side));
            }
        }
    }
}