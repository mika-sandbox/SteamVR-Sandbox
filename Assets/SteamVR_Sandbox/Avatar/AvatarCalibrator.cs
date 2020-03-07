using System.Collections.Generic;
using System.Linq;

using RootMotion.FinalIK;

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

        [SerializeField]
        private SteamVR_Behaviour_Pose ControllerLeft;

        [SerializeField]
        private SteamVR_Behaviour_Pose ControllerRight;

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        [SerializeField]
        private bool EnableAutoCalibration = true;

        // ReSharper disable InconsistentNaming
        [SerializeField]
        private GameObject HeadTracker;

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
        private List<SteamVR_Behaviour_Pose> Trackers;

        [SerializeField]
        [Tooltip("VRChat View Position (Camera Position for Avatar)")]
        private Vector3 ViewPosition;

        [SerializeField]
        private GameObject World;

        private void Start()
        {
            if (World == null)
                return;

            // store initialize values
            _distanceOfAvatarArms = Vector3.Distance(IK.references.leftHand.position, IK.references.rightHand.position);

            if (!EnableAutoCalibration)
                return;

            var avatarHeadPosition = IK.references.head.position;
            var avatarViewPosition = ViewPosition;

            var headTrackerPosition = HeadTracker.transform.localPosition;
            headTrackerPosition.x -= ViewPosition.x;
            headTrackerPosition.y -= avatarViewPosition.y - avatarHeadPosition.y;
            headTrackerPosition.z -= ViewPosition.z;
            HeadTracker.transform.localPosition = headTrackerPosition;

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

            // run calibrate

            // calibrate with extra trackers (without HMD, controllers)
            var enabledTrackers = Trackers.Where(w => w.isActive).ToList();
            if (enabledTrackers.Count == 0)
                return;

            // HMD, two controllers, seven trackers
            // trackers assigned to pelvis, left hand, left elbow, right hand, right elbow, left knee, left foot, right knee and right foot
            foreach (var tracker in enabledTrackers)
                switch (tracker.inputSource)
                {
                    case SteamVR_Input_Sources.Waist:
                        IK.solver.spine.pelvisTarget = tracker.transform;
                        IK.solver.spine.pelvisPositionWeight = 1f;
                        IK.solver.spine.pelvisRotationWeight = 1f;
                        break;

                    case SteamVR_Input_Sources.LeftFoot:
                        IK.solver.leftLeg.target = tracker.transform;
                        IK.solver.leftLeg.positionWeight = 1f;
                        IK.solver.leftLeg.rotationWeight = 1f;
                        break;

                    case SteamVR_Input_Sources.RightFoot:
                        IK.solver.rightLeg.target = tracker.transform;
                        IK.solver.rightLeg.positionWeight = 1f;
                        IK.solver.rightLeg.rotationWeight = 1f;
                        break;

                    case SteamVR_Input_Sources.LeftElbow:
                        IK.solver.leftArm.bendGoal = tracker.transform;
                        IK.solver.leftArm.bendGoalWeight = 1f;
                        break;

                    case SteamVR_Input_Sources.RightElbow:
                        IK.solver.rightArm.bendGoal = tracker.transform;
                        IK.solver.rightArm.bendGoalWeight = 1f;
                        break;

                    case SteamVR_Input_Sources.LeftKnee:
                        IK.solver.leftLeg.bendGoal = tracker.transform;
                        IK.solver.leftLeg.bendGoalWeight = 1f;
                        break;

                    case SteamVR_Input_Sources.RightKnee:
                        IK.solver.rightLeg.bendGoal = tracker.transform;
                        IK.solver.rightLeg.bendGoalWeight = 1f;
                        break;
                }
        }

        public void OnAvatarCalibrate()
        {
            // Calibrate by Player Height and Arm Length
            _enableCalibrateMode = true;
            PlayerHeight = PlayerHeightByInput.Value;
        }
    }
}