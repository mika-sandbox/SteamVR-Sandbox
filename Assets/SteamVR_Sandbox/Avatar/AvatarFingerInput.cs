using SteamVR_Sandbox.Models;

using UnityEngine;

namespace SteamVR_Sandbox.Avatar
{
    [AddComponentMenu("Scripts/Mochizuki.VR/Avatar/Avatar Finger Input")]
    public class AvatarFingerInput : AnimatorIKReceiver
    {
        private bool _hasAnimationController;

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
        }

        private static HumanBodyBones GetHumanBodyBoneFromString(string category)
        {
            return ReflectionHelper.GetEnumValue<HumanBodyBones>(category);
        }

        private static MuscleName GetMuscleNameFromString(string category)
        {
            return ReflectionHelper.GetEnumValue<MuscleName>(category);
        }

        #region OnAnimatorIK

        public override void OnAnimatorIKReceived()
        {
            if (!_hasAnimationController)
                return;

            var state = Animator.GetCurrentAnimatorStateInfo(2); // FINGER_EMOTES
            if (!state.IsName("FINGER_EMOTES_IDLE"))
                return;

            SetFingerCurlsInAnimatorIK(ControllerSide.Left);
            SetFingerCurlsInAnimatorIK(ControllerSide.Right);
        }

        private void SetFingerCurlsInAnimatorIK(ControllerSide side)
        {
            var hand = side == ControllerSide.Left ? LeftHand : RightHand;
            var skeleton = hand.Skeleton;

            // Index
            SetFingerCurlInAnimatorIK($"{side}Index1Stretched", $"{side}IndexProximal", skeleton.indexCurl, hand.Index.Stretch1Weight, hand.Index.Stretch1Axis);
            SetFingerCurlInAnimatorIK($"{side}Index2Stretched", $"{side}IndexIntermediate", skeleton.indexCurl, hand.Index.Stretch2Weight, hand.Index.Stretch2Axis);
            SetFingerCurlInAnimatorIK($"{side}Index3Stretched", $"{side}IndexDistal", skeleton.indexCurl, hand.Index.Stretch3Weight, hand.Index.Stretch3Axis);

            // Little
            SetFingerCurlInAnimatorIK($"{side}Little1Stretched", $"{side}LittleProximal", skeleton.pinkyCurl, hand.Little.Stretch1Weight, hand.Little.Stretch1Axis);
            SetFingerCurlInAnimatorIK($"{side}Little2Stretched", $"{side}LittleIntermediate", skeleton.pinkyCurl, hand.Little.Stretch2Weight, hand.Little.Stretch2Axis);
            SetFingerCurlInAnimatorIK($"{side}Little3Stretched", $"{side}LittleDistal", skeleton.pinkyCurl, hand.Little.Stretch3Weight, hand.Little.Stretch3Axis);

            // Middle
            SetFingerCurlInAnimatorIK($"{side}Middle1Stretched", $"{side}MiddleProximal", skeleton.middleCurl, hand.Middle.Stretch1Weight, hand.Middle.Stretch1Axis);
            SetFingerCurlInAnimatorIK($"{side}Middle2Stretched", $"{side}MiddleIntermediate", skeleton.middleCurl, hand.Middle.Stretch2Weight, hand.Middle.Stretch2Axis);
            SetFingerCurlInAnimatorIK($"{side}Middle3Stretched", $"{side}MiddleDistal", skeleton.middleCurl, hand.Middle.Stretch3Weight, hand.Middle.Stretch3Axis);

            // Ring
            SetFingerCurlInAnimatorIK($"{side}Ring1Stretched", $"{side}RingProximal", skeleton.ringCurl, hand.Ring.Stretch1Weight, hand.Ring.Stretch1Axis);
            SetFingerCurlInAnimatorIK($"{side}Ring2Stretched", $"{side}RingIntermediate", skeleton.ringCurl, hand.Ring.Stretch2Weight, hand.Ring.Stretch2Axis);
            SetFingerCurlInAnimatorIK($"{side}Ring3Stretched", $"{side}RingDistal", skeleton.ringCurl, hand.Ring.Stretch3Weight, hand.Ring.Stretch3Axis);

            // Thumb
            SetFingerCurlInAnimatorIK($"{side}Thumb1Stretched", $"{side}ThumbProximal", skeleton.thumbCurl, hand.Thumb.Stretch1Weight, hand.Thumb.Stretch1Axis);
            SetFingerCurlInAnimatorIK($"{side}Thumb2Stretched", $"{side}ThumbIntermediate", skeleton.thumbCurl, hand.Thumb.Stretch2Weight, hand.Thumb.Stretch2Axis);
            SetFingerCurlInAnimatorIK($"{side}Thumb3Stretched", $"{side}ThumbDistal", skeleton.thumbCurl, hand.Thumb.Stretch3Weight, hand.Thumb.Stretch3Axis);
        }

        private void SetFingerCurlInAnimatorIK(string muscleCategory, string boneCategory, float curl, float weight, Vector3 axis)
        {
            var muscle = GetMuscleNameFromString(muscleCategory);
            var bone = GetHumanBodyBoneFromString(boneCategory);

            var x = HumanTrait.MuscleFromBone((int) bone, 0);
            var y = HumanTrait.MuscleFromBone((int) bone, 1);
            var z = HumanTrait.MuscleFromBone((int) bone, 2);

            // Animator.SetBoneLocalRotation(bone, Quaternion.AngleAxis(CalcFingerCurl(muscle, curl, weight), axis));
            Animator.SetBoneLocalRotation(bone, HumanPoseSimulator.CalcFingerCurlByQuaternion(muscle, curl * weight));

            // if (x == (int) muscle) Animator.SetBoneLocalRotation(bone, Quaternion.Euler(0, 0, CalcFingerCurl(muscle, curl, weight)));
            // if (y == (int) muscle) Animator.SetBoneLocalRotation(bone, Quaternion.Euler(0, CalcFingerCurl(muscle, curl, weight), 0));
            // if (z == (int) muscle) Animator.SetBoneLocalRotation(bone, Quaternion.Euler(CalcFingerCurl(muscle, curl, weight), 0, 0));
        }

        private static float CalcFingerCurl(MuscleName muscle, float curl, float weight)
        {
            return HumanPoseSimulator.CalcFingerCurlByFloat(muscle, curl * weight);
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

                SetFingerCurlsInUpdate(ref humanPose, ControllerSide.Left);
                SetFingerCurlsInUpdate(ref humanPose, ControllerSide.Right);

                handler.SetHumanPose(ref humanPose);
            }
        }

        private void SetFingerCurlsInUpdate(ref HumanPose humanPose, ControllerSide side)
        {
            var hand = side == ControllerSide.Left ? LeftHand : RightHand;
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