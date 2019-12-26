using SteamVR_Sandbox.Enums;
using SteamVR_Sandbox.Models;

using UnityEngine;

using Valve.VR;

// ReSharper disable once InconsistentNaming
namespace SteamVR_Sandbox.SteamVR
{
    [AddComponentMenu("Scripts/Mochizuki.VR/SteamVR/SteamVR Controller Calibrator")]
    public class SteamVRControllerCalibrator : MonoBehaviour
    {
        private bool _isFixed;

        [SerializeField]
        private Side Side;

        [SerializeField]
        private GameObject Tracker;

        public SteamVRControllerCalibrator()
        {
            _isFixed = false;
        }

        // Start is called before the first frame update
        private void Update()
        {
            if (_isFixed)
                return;

            var controller = Valve.VR.SteamVR.instance.GetStringProperty(ETrackedDeviceProperty.Prop_ControllerType_String);
            Tracker.transform.localPosition = ControllerDefinition.GetControllerPositionOffset(controller, Side);
            Tracker.transform.localRotation = ControllerDefinition.GetControllerRotationOffset(controller, Side);

            _isFixed = true;
        }
    }
}