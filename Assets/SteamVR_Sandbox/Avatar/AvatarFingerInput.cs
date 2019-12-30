﻿using SteamVR_Sandbox.Animations;
using SteamVR_Sandbox.Enums;
using SteamVR_Sandbox.Humanoid;
using SteamVR_Sandbox.Models;

using UnityEngine;

namespace SteamVR_Sandbox.Avatar
{
    [AddComponentMenu("Scripts/Mochizuki.VR/Avatar/Avatar Finger Input")]
    public class AvatarFingerInput : AnimatorIKReceiver
    {
        private bool _hasAnimationController;
        private int _leftLayerIndex;
        private int _rightLayerIndex;

        [SerializeField]
        private Animator Animator;

        [SerializeField]
        private Hand LeftHand;

        [SerializeField]
        private Hand RightHand;

        private void Start()
        {
            // compatibility with IndexControllerFingerTracking Scene
            _hasAnimationController = Animator.runtimeAnimatorController != null;
            if (!_hasAnimationController)
                return;

            _leftLayerIndex = Animator.GetLayerIndex("LEFT_HAND");
            _rightLayerIndex = Animator.GetLayerIndex("RIGHT_HAND");

            LeftHand.StoreAxisDirection(Animator.GetBoneTransform(HumanBodyBones.LeftIndexProximal));
            RightHand.StoreAxisDirection(Animator.GetBoneTransform(HumanBodyBones.RightIndexProximal));
            StoreStretchTransforms(Side.Left);
            StoreStretchTransforms(Side.Right);
        }

        private static HumanBodyBones GetHumanBodyBoneFromString(string category)
        {
            return ReflectionHelper.GetEnumValue<HumanBodyBones>(category);
        }

        private static MuscleName GetMuscleNameFromString(string category)
        {
            return ReflectionHelper.GetEnumValue<MuscleName>(category);
        }

        #region In AnimatorIK Event

        private void StoreStretchTransforms(Side side)
        {
            var hand = side == Side.Left ? LeftHand : RightHand;

            hand.SetReverseCalcForRadian(side == Side.Left);

            // Index
            hand.StoreStretchTransform(Animator.GetBoneTransform(GetHumanBodyBoneFromString($"{side}IndexProximal")), FingerCategory.Index, FingerJoint.Stretch1);
            hand.StoreStretchTransform(Animator.GetBoneTransform(GetHumanBodyBoneFromString($"{side}IndexIntermediate")), FingerCategory.Index, FingerJoint.Stretch2);
            hand.StoreStretchTransform(Animator.GetBoneTransform(GetHumanBodyBoneFromString($"{side}IndexDistal")), FingerCategory.Index, FingerJoint.Stretch3);

            // Little
            hand.StoreStretchTransform(Animator.GetBoneTransform(GetHumanBodyBoneFromString($"{side}LittleProximal")), FingerCategory.Little, FingerJoint.Stretch1);
            hand.StoreStretchTransform(Animator.GetBoneTransform(GetHumanBodyBoneFromString($"{side}LittleIntermediate")), FingerCategory.Little, FingerJoint.Stretch2);
            hand.StoreStretchTransform(Animator.GetBoneTransform(GetHumanBodyBoneFromString($"{side}LittleDistal")), FingerCategory.Little, FingerJoint.Stretch3);

            // Middle
            hand.StoreStretchTransform(Animator.GetBoneTransform(GetHumanBodyBoneFromString($"{side}MiddleProximal")), FingerCategory.Middle, FingerJoint.Stretch1);
            hand.StoreStretchTransform(Animator.GetBoneTransform(GetHumanBodyBoneFromString($"{side}MiddleIntermediate")), FingerCategory.Middle, FingerJoint.Stretch2);
            hand.StoreStretchTransform(Animator.GetBoneTransform(GetHumanBodyBoneFromString($"{side}MiddleDistal")), FingerCategory.Middle, FingerJoint.Stretch3);

            // Ring
            hand.StoreStretchTransform(Animator.GetBoneTransform(GetHumanBodyBoneFromString($"{side}RingProximal")), FingerCategory.Ring, FingerJoint.Stretch1);
            hand.StoreStretchTransform(Animator.GetBoneTransform(GetHumanBodyBoneFromString($"{side}RingIntermediate")), FingerCategory.Ring, FingerJoint.Stretch2);
            hand.StoreStretchTransform(Animator.GetBoneTransform(GetHumanBodyBoneFromString($"{side}RingDistal")), FingerCategory.Ring, FingerJoint.Stretch3);

            // Thumb
            hand.StoreStretchTransform(Animator.GetBoneTransform(GetHumanBodyBoneFromString($"{side}ThumbProximal")), FingerCategory.Thumb, FingerJoint.Stretch1);
            hand.StoreStretchTransform(Animator.GetBoneTransform(GetHumanBodyBoneFromString($"{side}ThumbIntermediate")), FingerCategory.Thumb, FingerJoint.Stretch2);
            hand.StoreStretchTransform(Animator.GetBoneTransform(GetHumanBodyBoneFromString($"{side}ThumbDistal")), FingerCategory.Thumb, FingerJoint.Stretch3);
        }

        public override void OnAnimatorIKReceived()
        {
            if (!_hasAnimationController)
                return;

            if (Animator.GetLayerWeight(_leftLayerIndex) < 1)
                SetFingerCurlsInAnimatorIK(Side.Left);
            if (Animator.GetLayerWeight(_rightLayerIndex) < 1)
                SetFingerCurlsInAnimatorIK(Side.Right);
        }

        private void SetFingerCurlsInAnimatorIK(Side side)
        {
            var hand = side == Side.Left ? LeftHand : RightHand;

            // Index
            Animator.SetBoneLocalRotation(GetHumanBodyBoneFromString($"{side}IndexProximal"), hand.CalcFingerCurlByQuaternion(FingerCategory.Index, FingerJoint.Stretch1));
            Animator.SetBoneLocalRotation(GetHumanBodyBoneFromString($"{side}IndexIntermediate"), hand.CalcFingerCurlByQuaternion(FingerCategory.Index, FingerJoint.Stretch2));
            Animator.SetBoneLocalRotation(GetHumanBodyBoneFromString($"{side}IndexDistal"), hand.CalcFingerCurlByQuaternion(FingerCategory.Index, FingerJoint.Stretch3));

            // Little
            Animator.SetBoneLocalRotation(GetHumanBodyBoneFromString($"{side}LittleProximal"), hand.CalcFingerCurlByQuaternion(FingerCategory.Little, FingerJoint.Stretch1));
            Animator.SetBoneLocalRotation(GetHumanBodyBoneFromString($"{side}LittleIntermediate"), hand.CalcFingerCurlByQuaternion(FingerCategory.Little, FingerJoint.Stretch2));
            Animator.SetBoneLocalRotation(GetHumanBodyBoneFromString($"{side}LittleDistal"), hand.CalcFingerCurlByQuaternion(FingerCategory.Little, FingerJoint.Stretch3));

            // Middle
            Animator.SetBoneLocalRotation(GetHumanBodyBoneFromString($"{side}MiddleProximal"), hand.CalcFingerCurlByQuaternion(FingerCategory.Middle, FingerJoint.Stretch1));
            Animator.SetBoneLocalRotation(GetHumanBodyBoneFromString($"{side}MiddleIntermediate"), hand.CalcFingerCurlByQuaternion(FingerCategory.Middle, FingerJoint.Stretch2));
            Animator.SetBoneLocalRotation(GetHumanBodyBoneFromString($"{side}MiddleDistal"), hand.CalcFingerCurlByQuaternion(FingerCategory.Middle, FingerJoint.Stretch3));

            // Ring
            Animator.SetBoneLocalRotation(GetHumanBodyBoneFromString($"{side}RingProximal"), hand.CalcFingerCurlByQuaternion(FingerCategory.Ring, FingerJoint.Stretch1));
            Animator.SetBoneLocalRotation(GetHumanBodyBoneFromString($"{side}RingIntermediate"), hand.CalcFingerCurlByQuaternion(FingerCategory.Ring, FingerJoint.Stretch2));
            Animator.SetBoneLocalRotation(GetHumanBodyBoneFromString($"{side}RingDistal"), hand.CalcFingerCurlByQuaternion(FingerCategory.Ring, FingerJoint.Stretch3));

            // Thumb
            Animator.SetBoneLocalRotation(GetHumanBodyBoneFromString($"{side}ThumbProximal"), hand.CalcFingerCurlByQuaternion(FingerCategory.Thumb, FingerJoint.Stretch1));
            Animator.SetBoneLocalRotation(GetHumanBodyBoneFromString($"{side}ThumbIntermediate"), hand.CalcFingerCurlByQuaternion(FingerCategory.Thumb, FingerJoint.Stretch2));
            Animator.SetBoneLocalRotation(GetHumanBodyBoneFromString($"{side}ThumbDistal"), hand.CalcFingerCurlByQuaternion(FingerCategory.Thumb, FingerJoint.Stretch3));
        }

        #endregion

        #region In Update Event

        private void Update()
        {
            if (_hasAnimationController)
                return;

            using (var handler = new HumanPoseHandler(Animator.avatar, Animator.transform))
            {
                var humanPose = new HumanPose();
                handler.GetHumanPose(ref humanPose);

                SetFingerCurlsInUpdate(ref humanPose, Side.Left);
                SetFingerCurlsInUpdate(ref humanPose, Side.Right);

                handler.SetHumanPose(ref humanPose);
            }
        }

        private void SetFingerCurlsInUpdate(ref HumanPose humanPose, Side side)
        {
            var hand = side == Side.Left ? LeftHand : RightHand;

            // Index
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Index1Stretched")] = hand.CalcFingerCurlBySingle(FingerCategory.Index, FingerJoint.Stretch1);
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Index2Stretched")] = hand.CalcFingerCurlBySingle(FingerCategory.Index, FingerJoint.Stretch2);
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Index3Stretched")] = hand.CalcFingerCurlBySingle(FingerCategory.Index, FingerJoint.Stretch3);

            // Little
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Little1Stretched")] = hand.CalcFingerCurlBySingle(FingerCategory.Little, FingerJoint.Stretch1);
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Little2Stretched")] = hand.CalcFingerCurlBySingle(FingerCategory.Little, FingerJoint.Stretch2);
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Little3Stretched")] = hand.CalcFingerCurlBySingle(FingerCategory.Little, FingerJoint.Stretch3);

            // Middle
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Middle1Stretched")] = hand.CalcFingerCurlBySingle(FingerCategory.Middle, FingerJoint.Stretch1);
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Middle2Stretched")] = hand.CalcFingerCurlBySingle(FingerCategory.Middle, FingerJoint.Stretch2);
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Middle3Stretched")] = hand.CalcFingerCurlBySingle(FingerCategory.Middle, FingerJoint.Stretch3);

            // Ring
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Ring1Stretched")] = hand.CalcFingerCurlBySingle(FingerCategory.Ring, FingerJoint.Stretch1);
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Ring2Stretched")] = hand.CalcFingerCurlBySingle(FingerCategory.Ring, FingerJoint.Stretch2);
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Ring3Stretched")] = hand.CalcFingerCurlBySingle(FingerCategory.Ring, FingerJoint.Stretch3);

            // Thumb
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Thumb1Stretched")] = hand.CalcFingerCurlBySingle(FingerCategory.Thumb, FingerJoint.Stretch1);
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Thumb2Stretched")] = hand.CalcFingerCurlBySingle(FingerCategory.Thumb, FingerJoint.Stretch2);
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Thumb3Stretched")] = hand.CalcFingerCurlBySingle(FingerCategory.Thumb, FingerJoint.Stretch3);
        }

        private static float CalcMuscleCurl(double fingerCurl, double weight)
        {
            return Mathf.Lerp(-1f, 1f, (float) (fingerCurl * weight)) * -1;
        }

        #endregion
    }
}