using SteamVR_Sandbox.Models;

using UnityEngine;

using Valve.VR;

// ReSharper disable once InconsistentNaming
namespace SteamVR_Sandbox.Scripts
{
    public class SteamVRControllerCalibrator : MonoBehaviour
    {
        private bool _isFixed;

        [SerializeField]
        private GameObject Model;

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

            var model = Model.GetComponent<SteamVR_RenderModel>();
            if (model == null)
                return;

            var controller = model.renderModelName;
            if (string.IsNullOrWhiteSpace(controller))
                return;

            var position = Vector3.zero;
            var rotation = Quaternion.identity;

            switch (controller)
            {
                case "oculus_rifts_controller_right":
                    position = ControllerDefinition.OculusTouch.RightHandPosition;
                    rotation = ControllerDefinition.OculusTouch.RightHandRotation;

                    break;

                case "oculus_rifts_controller_left":
                    position = ControllerDefinition.OculusTouch.LeftHandPosition;
                    rotation = ControllerDefinition.OculusTouch.LeftHandRotation;
                    break;
            }

            Tracker.transform.localPosition = position;
            Tracker.transform.localRotation = rotation;

            _isFixed = true;
        }
    }
}