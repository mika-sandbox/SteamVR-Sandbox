using System.Collections.Generic;
using System.Linq;

using RootMotion.FinalIK;

using SteamVR_Sandbox.SteamVR;
using SteamVR_Sandbox.UI;

using UnityEngine;

using Valve.VR;

#pragma warning disable 649

namespace SteamVR_Sandbox.Avatar
{
    [AddComponentMenu("Scripts/Mochizuki.VR/Avatar/Avatar Calibrator")]
    public class AvatarCalibrator : MonoBehaviour
    {
        private const float PlayerHandDistanceByHeight = .78f;
        private float _distanceOfAvatarArms;
        private bool _enableCalibrateMode;

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        [SerializeField]
        private bool AlwaysShowsTracker = false;

        [SerializeField]
        private GameObject CameraRig;

        [SerializeField]
        private SteamVR_Behaviour_Pose ControllerLeft;

        [SerializeField]
        private SteamVR_Behaviour_Pose ControllerRight;

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        [SerializeField]
        private bool EnableAutoCalibration = true;

        // ReSharper disable InconsistentNaming
        [SerializeField]
        private SteamVRHMDTracker HeadTracker;

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        [SerializeField]
        private SteamVR_Action_Boolean InteractUI = SteamVR_Input.GetBooleanAction("InteractUI");

        [SerializeField]
        private VRIK IK;

        [SerializeField]
        [Tooltip("Player Real Height (m)")]
        private float PlayerHeight;

        [SerializeField]
        private NumericUpDown PlayerHeightByInput;

        [SerializeField]
        private List<SteamVRTracker> Trackers;

        [SerializeField]
        [Tooltip("VRChat View Position (Camera Position for Avatar)")]
        private Vector3 ViewPosition;

        [SerializeField]
        private GameObject World;

        private void Start()
        {
            if (World == null)
                return;

            if (!AlwaysShowsTracker)
                HideTrackers();

            // store initialize values
            _distanceOfAvatarArms = Vector3.Distance(IK.references.leftHand.position, IK.references.rightHand.position);

            if (!EnableAutoCalibration)
                return;

            var avatarHeadPosition = IK.references.head.position;
            var avatarViewPosition = ViewPosition;

            var headTrackerPosition = HeadTracker.Target.transform.localPosition;
            headTrackerPosition.x -= ViewPosition.x;
            headTrackerPosition.y -= avatarViewPosition.y - avatarHeadPosition.y;
            headTrackerPosition.z -= ViewPosition.z;
            HeadTracker.Target.transform.localPosition = headTrackerPosition;

            var playerHandDistance = PlayerHeight * PlayerHandDistanceByHeight;
            var worldScale = playerHandDistance / _distanceOfAvatarArms;

            World.transform.localScale = Vector3.one * worldScale;
        }

        private void Update()
        {
            if (!_enableCalibrateMode)
                return;

            if (!InteractUI.GetState(ControllerLeft.inputSource) || !InteractUI.GetState(ControllerRight.inputSource))
                return;

            _enableCalibrateMode = false;
            HideTrackers();

            IK.solver.FixTransforms();

            var scale = CalibrateAvatarScale();

            HeadTracker.Assign(IK.solver);
            HeadTracker.Calibrate(IK, ViewPosition * scale);

            // calibrate with extra trackers (without HMD, controllers)
            var trackers = GetEnabledTrackers();
            if (trackers.Count == 0)
                return;

            // HMD, two controllers, seven trackers
            // trackers assigned to pelvis, left hand, left elbow, right hand, right elbow, left knee, left foot, right knee and right foot
            foreach (var tracker in trackers)
            {
                tracker.Assign(IK.solver);
                tracker.Calibrate(IK);
            }

            IK.solver.spine.minHeadHeight = 0f;

            // see: https://twitter.com/ikko/status/966894056142864385
            if (IK.solver.spine.pelvisTarget && IK.solver.leftLeg.target && IK.solver.rightLeg.target)
            {
                IK.gameObject.AddComponent<VRIKRootController>();
                IK.solver.locomotion.weight = 1f;
            }
            else
            {
                IK.solver.locomotion.weight = 0f;
            }
        }

        public void OnAvatarCalibrate()
        {
            // Calibrate by Player Height and Arm Length
            _enableCalibrateMode = true;

            // reset all solvers to default
            ResetPlayer();
            ResetSolvers();
            ShowTrackers();
        }

        private float CalibrateAvatarScale()
        {
            PlayerHeight = PlayerHeightByInput.Value / 100f;
            var scale = PlayerHeight / ViewPosition.y * 0.95f;

            World.transform.localScale = Vector3.one * scale;

            return scale;
        }

        private void ResetPlayer()
        {
            IK.gameObject.transform.position = new Vector3(0, 0, 0);
            IK.gameObject.transform.rotation = Quaternion.identity;
            IK.solver.Reset();
        }

        private void ResetSolvers()
        {
            HeadTracker.UnAssign(IK.solver);

            foreach (var tracker in Trackers)
                tracker.UnAssign(IK.solver);
        }

        private List<SteamVRTracker> GetEnabledTrackers()
        {
            return Trackers.Where(w => w.IsActive).ToList();
        }

        private void ShowTrackers()
        {
            foreach (var tracker in GetEnabledTrackers())
                tracker.Show();
        }

        private void HideTrackers()
        {
            foreach (var tracker in GetEnabledTrackers())
                tracker.Hide();
        }
    }
}