using UnityEngine;

namespace SteamVR_Sandbox.Models
{
    public class ControllerDefinition
    {
        public static ControllerDefinition OculusTouch { get; }

        public Vector3 LeftHandPosition { get; private set; }
        public Quaternion LeftHandRotation { get; private set; }

        public Vector3 RightHandPosition { get; private set; }
        public Quaternion RightHandRotation { get; private set; }

        static ControllerDefinition()
        {
            OculusTouch = new ControllerDefinition
            {
                LeftHandPosition = new Vector3(0, .025f, .11f),
                LeftHandRotation = Quaternion.Euler(5, 90, 105),
                RightHandPosition = new Vector3(0, -.025f, .11f),
                RightHandRotation = Quaternion.Euler(5, -90, -105)
            };
        }
    }
}