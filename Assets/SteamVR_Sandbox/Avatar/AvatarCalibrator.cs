using System;
using System.Collections.Generic;
using System.Linq;

using RootMotion.FinalIK;

using SteamVR_Sandbox.SteamVR;
using SteamVR_Sandbox.UI;

using UnityEngine;

using Valve.VR;
using Valve.VR.InteractionSystem;

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
            HideTrackers();

            // run calibrate

            IK.solver.spine.headTarget = HeadTracker.transform;
            IK.solver.spine.positionWeight = 1f;
            IK.solver.spine.rotationWeight = 1f;

            // calibrate with extra trackers (without HMD, controllers)
            var enabledTrackers = Trackers.Where(w => w.IsActive).ToList();
            if (enabledTrackers.Count == 0)
                return;

            // HMD, two controllers, seven trackers
            // trackers assigned to pelvis, left hand, left elbow, right hand, right elbow, left knee, left foot, right knee and right foot
            foreach (var tracker in enabledTrackers)
            {
                switch (tracker.Pose.inputSource)
                {
                    case SteamVR_Input_Sources.LeftHand:
                        IK.solver.leftArm.target = tracker.Target.transform;
                        IK.solver.leftArm.positionWeight = 1f;
                        IK.solver.leftArm.rotationWeight = 1f;
                        break;

                    case SteamVR_Input_Sources.RightHand:
                        IK.solver.rightArm.target = tracker.Target.transform;
                        IK.solver.rightArm.positionWeight = 1f;
                        IK.solver.rightArm.rotationWeight = 1f;
                        break;

                    case SteamVR_Input_Sources.Waist:
                        IK.solver.spine.pelvisTarget = tracker.Target.transform;
                        IK.solver.spine.pelvisPositionWeight = 1f;
                        IK.solver.spine.pelvisRotationWeight = 1f;
                        IK.solver.plantFeet = false;
                        break;

                    case SteamVR_Input_Sources.LeftFoot:
                        IK.solver.leftLeg.target = tracker.Target.transform;
                        IK.solver.leftLeg.positionWeight = 1f;
                        IK.solver.leftLeg.rotationWeight = 1f;
                        break;

                    case SteamVR_Input_Sources.RightFoot:
                        IK.solver.rightLeg.target = tracker.Target.transform;
                        IK.solver.rightLeg.positionWeight = 1f;
                        IK.solver.rightLeg.rotationWeight = 1f;
                        break;

                    case SteamVR_Input_Sources.LeftElbow:
                        IK.solver.leftArm.bendGoal = tracker.Target.transform;
                        IK.solver.leftArm.bendGoalWeight = 1f;
                        break;

                    case SteamVR_Input_Sources.RightElbow:
                        IK.solver.rightArm.bendGoal = tracker.Target.transform;
                        IK.solver.rightArm.bendGoalWeight = 1f;
                        break;

                    case SteamVR_Input_Sources.LeftKnee:
                        IK.solver.leftLeg.bendGoal = tracker.Target.transform;
                        IK.solver.leftLeg.bendGoalWeight = 1f;
                        break;

                    case SteamVR_Input_Sources.RightKnee:
                        IK.solver.rightLeg.bendGoal = tracker.Target.transform;
                        IK.solver.rightLeg.bendGoalWeight = 1f;
                        break;

                    case SteamVR_Input_Sources.LeftShoulder:
                        break;

                    case SteamVR_Input_Sources.RightShoulder:
                        break;

                    case SteamVR_Input_Sources.Chest:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                tracker.Calibrate(IK);
            }

            // see: https://twitter.com/ikko/status/966894056142864385
            if (IK.solver.spine.pelvisTarget && IK.solver.leftLeg.target && IK.solver.rightLeg.target)
                IK.gameObject.AddComponent<VRIKRootController>();
        }

        public void OnAvatarCalibrate()
        {
            // Calibrate by Player Height and Arm Length
            _enableCalibrateMode = true;
            PlayerHeight = PlayerHeightByInput.Value;

            // reset all solvers to default
            ResetSolvers();
            ShowTrackers();
        }

        private void ResetSolvers()
        {
            // spine
            IK.solver.spine.headTarget = null;
            IK.solver.spine.positionWeight = 0f;
            IK.solver.spine.rotationWeight = 0f;
            IK.solver.spine.pelvisTarget = null;
            IK.solver.spine.pelvisPositionWeight = 0f;
            IK.solver.spine.pelvisRotationWeight = 0f;
            IK.solver.spine.chestGoal = null;
            IK.solver.spine.chestGoalWeight = 0f;

            // left arm
            IK.solver.leftArm.target = null;
            IK.solver.leftArm.positionWeight = 0f;
            IK.solver.leftArm.rotationWeight = 0f;
            IK.solver.leftArm.bendGoal = null;
            IK.solver.leftArm.bendGoalWeight = 0f;

            // right arm
            IK.solver.rightArm.target = null;
            IK.solver.rightArm.positionWeight = 0f;
            IK.solver.rightArm.rotationWeight = 0f;
            IK.solver.rightArm.bendGoal = null;
            IK.solver.rightArm.bendGoalWeight = 0f;

            // left leg
            IK.solver.leftLeg.target = null;
            IK.solver.leftLeg.positionWeight = 0f;
            IK.solver.leftLeg.rotationWeight = 0f;
            IK.solver.leftLeg.bendGoal = null;
            IK.solver.leftLeg.bendGoalWeight = 0f;

            // right leg
            IK.solver.rightLeg.target = null;
            IK.solver.rightLeg.positionWeight = 0f;
            IK.solver.rightLeg.rotationWeight = 0f;
            IK.solver.rightLeg.bendGoal = null;
            IK.solver.rightLeg.bendGoalWeight = 0f;
        }

        private void ShowTrackers()
        {
            Trackers.Where(w => w.IsActive).ForEach(w => w.transform.GetChild(0).gameObject.SetActive(true));
        }

        private void HideTrackers()
        {
            Trackers.Where(w => w.IsActive).ForEach(w => w.transform.GetChild(0).gameObject.SetActive(false));
        }
    }
}