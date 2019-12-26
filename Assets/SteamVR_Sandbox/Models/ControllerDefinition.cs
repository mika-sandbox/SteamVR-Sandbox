using System;

using SteamVR_Sandbox.Enums;

using UnityEngine;

namespace SteamVR_Sandbox.Models
{
    public class ControllerDefinition
    {
        private static readonly ControllerDefinition IndexController;
        private static readonly ControllerDefinition OculusTouch;

        private Vector3 _leftHandPosition;
        private Quaternion _leftHandRotation;

        private Vector3 _rightHandPosition;
        private Quaternion _rightHandRotation;

        static ControllerDefinition()
        {
            // Index HMD
            IndexController = new ControllerDefinition
            {
                _leftHandPosition = new Vector3(0, .025f, -.1f),
                _leftHandRotation = Quaternion.Euler(135f, -75, -35),
                _rightHandPosition = new Vector3(0, -.025f, -.1f),
                _rightHandRotation = Quaternion.Euler(135f, 75, 35)
            };

            // Oculus Rift S, Oculus Quest
            OculusTouch = new ControllerDefinition
            {
                _leftHandPosition = new Vector3(0, .025f, .11f),
                _leftHandRotation = Quaternion.Euler(5, 90, 105),
                _rightHandPosition = new Vector3(0, -.025f, .11f),
                _rightHandRotation = Quaternion.Euler(5, -90, -105)
            };
        }

        public static Vector3 GetControllerPositionOffset(string controller, Side side)
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

        public static Quaternion GetControllerRotationOffset(string controller, Side side)
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

        private static Vector3 GetControllerPositionOffset(ControllerDefinition definition, Side side)
        {
            switch (side)
            {
                case Side.Left:
                    return definition._leftHandPosition;

                case Side.Right:
                    return definition._rightHandPosition;

                default:
                    throw new ArgumentOutOfRangeException(nameof(side));
            }
        }

        private static Quaternion GetControllerRotationOffset(ControllerDefinition definition, Side side)
        {
            switch (side)
            {
                case Side.Left:
                    return definition._leftHandRotation;

                case Side.Right:
                    return definition._rightHandRotation;

                default:
                    throw new ArgumentOutOfRangeException(nameof(side));
            }
        }
    }
}