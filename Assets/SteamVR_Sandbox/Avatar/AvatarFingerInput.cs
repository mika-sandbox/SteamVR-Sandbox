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

            if (Animator.GetCurrentAnimatorStateInfo(_leftLayerIndex).IsName("IDLE"))
                SetFingerCurlsInAnimatorIK(Side.Left);
            if (Animator.GetCurrentAnimatorStateInfo(_rightLayerIndex).IsName("IDLE"))
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
            var skeleton = hand.Skeleton;

            // Index
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Index1Stretched")] = CalcMuscleCurl(skeleton.indexCurl, hand.Index.Stretch1Weight);
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Index2Stretched")] = CalcMuscleCurl(skeleton.indexCurl, hand.Index.Stretch2Weight);
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Index3Stretched")] = CalcMuscleCurl(skeleton.indexCurl, hand.Index.Stretch3Weight);

            // Little
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Little1Stretched")] = CalcMuscleCurl(skeleton.pinkyCurl, hand.Little.Stretch1Weight);
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Little2Stretched")] = CalcMuscleCurl(skeleton.pinkyCurl, hand.Little.Stretch2Weight);
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Little3Stretched")] = CalcMuscleCurl(skeleton.pinkyCurl, hand.Little.Stretch3Weight);

            // Middle
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Middle1Stretched")] = CalcMuscleCurl(skeleton.middleCurl, hand.Middle.Stretch1Weight);
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Middle2Stretched")] = CalcMuscleCurl(skeleton.middleCurl, hand.Middle.Stretch2Weight);
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Middle3Stretched")] = CalcMuscleCurl(skeleton.middleCurl, hand.Middle.Stretch3Weight);

            // Ring
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Ring1Stretched")] = CalcMuscleCurl(skeleton.ringCurl, hand.Ring.Stretch1Weight);
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Ring2Stretched")] = CalcMuscleCurl(skeleton.ringCurl, hand.Ring.Stretch2Weight);
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Ring3Stretched")] = CalcMuscleCurl(skeleton.ringCurl, hand.Ring.Stretch3Weight);

            // Thumb
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Thumb1Stretched")] = CalcMuscleCurl(skeleton.thumbCurl, hand.Thumb.Stretch1Weight);
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Thumb2Stretched")] = CalcMuscleCurl(skeleton.thumbCurl, hand.Thumb.Stretch2Weight);
            humanPose.muscles[(int) GetMuscleNameFromString($"{side}Thumb3Stretched")] = CalcMuscleCurl(skeleton.thumbCurl, hand.Thumb.Stretch3Weight);
        }

        private static float CalcMuscleCurl(double fingerCurl, double weight)
        {
            return Mathf.Lerp(-1f, 1f, (float) (fingerCurl * weight)) * -1;
        }

        #endregion
    }
}