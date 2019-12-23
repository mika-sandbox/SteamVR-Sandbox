using SteamVR_Sandbox.Models;

using UnityEngine;

namespace SteamVR_Sandbox.Scripts
{
    public class AvatarFingerInput : MonoBehaviour
    {
        [SerializeField]
        private Animator Animator;

        [SerializeField]
        private Hand LeftHand;

        [SerializeField]
        private Hand RightHand;

        private void Update()
        {
            using (var handler = new HumanPoseHandler(Animator.avatar, Animator.transform))
            {
                var humanPose = new HumanPose();
                handler.GetHumanPose(ref humanPose);

                SetMuscleCurls(ref humanPose, LeftHand, "Left");
                SetMuscleCurls(ref humanPose, RightHand, "Right");

                handler.SetHumanPose(ref humanPose);
            }
        }

        private void SetMuscleCurls(ref HumanPose humanPose, Hand hand, string side)
        {
            var skeleton = hand.Skeleton;

            // Index
            humanPose.muscles[GetMuscleIndexFromString("Index1Stretched", side)] = CalcMuscleCurl(skeleton.indexCurl, hand.Index.Stretch1Weight);
            humanPose.muscles[GetMuscleIndexFromString("Index2Stretched", side)] = CalcMuscleCurl(skeleton.indexCurl, hand.Index.Stretch2Weight);
            humanPose.muscles[GetMuscleIndexFromString("Index3Stretched", side)] = CalcMuscleCurl(skeleton.indexCurl, hand.Index.Stretch3Weight);

            // Little
            humanPose.muscles[GetMuscleIndexFromString("Little1Stretched", side)] = CalcMuscleCurl(skeleton.pinkyCurl, hand.Little.Stretch1Weight);
            humanPose.muscles[GetMuscleIndexFromString("Little2Stretched", side)] = CalcMuscleCurl(skeleton.pinkyCurl, hand.Little.Stretch2Weight);
            humanPose.muscles[GetMuscleIndexFromString("Little3Stretched", side)] = CalcMuscleCurl(skeleton.pinkyCurl, hand.Little.Stretch3Weight);

            // Middle
            humanPose.muscles[GetMuscleIndexFromString("Middle1Stretched", side)] = CalcMuscleCurl(skeleton.middleCurl, hand.Middle.Stretch1Weight);
            humanPose.muscles[GetMuscleIndexFromString("Middle2Stretched", side)] = CalcMuscleCurl(skeleton.middleCurl, hand.Middle.Stretch2Weight);
            humanPose.muscles[GetMuscleIndexFromString("Middle3Stretched", side)] = CalcMuscleCurl(skeleton.middleCurl, hand.Middle.Stretch3Weight);

            // Ring
            humanPose.muscles[GetMuscleIndexFromString("Ring1Stretched", side)] = CalcMuscleCurl(skeleton.ringCurl, hand.Ring.Stretch1Weight);
            humanPose.muscles[GetMuscleIndexFromString("Ring2Stretched", side)] = CalcMuscleCurl(skeleton.ringCurl, hand.Ring.Stretch2Weight);
            humanPose.muscles[GetMuscleIndexFromString("Ring3Stretched", side)] = CalcMuscleCurl(skeleton.ringCurl, hand.Ring.Stretch3Weight);

            // Thumb
            humanPose.muscles[GetMuscleIndexFromString("Thumb1Stretched", side)] = CalcMuscleCurl(skeleton.thumbCurl, hand.Thumb.Stretch1Weight);
            humanPose.muscles[GetMuscleIndexFromString("Thumb2Stretched", side)] = CalcMuscleCurl(skeleton.thumbCurl, hand.Thumb.Stretch2Weight);
            humanPose.muscles[GetMuscleIndexFromString("Thumb3Stretched", side)] = CalcMuscleCurl(skeleton.thumbCurl, hand.Thumb.Stretch3Weight);
        }

        private static int GetMuscleIndexFromString(string category, string side)
        {
            return (int) ReflectionHelper.GetEnumValue<MuscleName>($"{side}{category}");
        }

        private static float CalcMuscleCurl(double fingerCurl, double weight)
        {
            return Mathf.Lerp(-.75f, 1f, (float) (fingerCurl * weight)) * -1;
        }
    }
}