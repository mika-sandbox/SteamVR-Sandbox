using SteamVR_Sandbox.Models;

using UnityEngine;

using Valve.VR;

namespace SteamVR_Sandbox.Scripts
{
    // ReSharper disable once InconsistentNaming
    public class SteamVRFingerInput : MonoBehaviour
    {
        public enum FingerCategory
        {
            RightIndex,

            RightLittle,

            RightMiddle,

            RightRing,

            RightThumb,

            LeftIndex,

            LeftLittle,

            LeftMiddle,

            LeftRing,

            LeftThumb
        }

        [SerializeField]
        private Animator Animator;

        [SerializeField]
        private FingerCategory Finger;

        [SerializeField]
        private SteamVR_Action_Skeleton SkeletonAction;

        [SerializeField]
        [Range(0f, 1f)]
        private float Stretch1Weight;

        [SerializeField]
        [Range(0f, 1f)]
        private float Stretch2Weight;

        [SerializeField]
        [Range(0f, 1f)]
        private float Stretch3Weight;

        // Update is called once per frame
        private void LateUpdate()
        {
            using (var handler = new HumanPoseHandler(Animator.avatar, Animator.transform))
            {
                var humanPose = new HumanPose();
                handler.GetHumanPose(ref humanPose);

                var fingerCurl = 0.0f;
                var weights = new[] { Stretch1Weight, Stretch2Weight, Stretch3Weight };
                var indexes = new MuscleName[] { };

                switch (Finger)
                {
                    case FingerCategory.RightIndex:
                        fingerCurl = SkeletonAction.indexCurl;
                        indexes = new[] { MuscleName.RightIndex1Stretched, MuscleName.RightIndex2Stretched, MuscleName.RightIndex3Stretched };
                        break;

                    case FingerCategory.RightLittle:
                        fingerCurl = SkeletonAction.pinkyCurl;
                        indexes = new[] { MuscleName.RightLittle1Stretched, MuscleName.RightLittle2Stretched, MuscleName.RightLittle3Stretched };
                        break;

                    case FingerCategory.RightMiddle:
                        fingerCurl = SkeletonAction.middleCurl;
                        indexes = new[] { MuscleName.RightMiddle1Stretched, MuscleName.RightMiddle2Stretched, MuscleName.RightMiddle3Stretched };
                        break;

                    case FingerCategory.RightRing:
                        fingerCurl = SkeletonAction.ringCurl;
                        indexes = new[] { MuscleName.RightRing1Stretched, MuscleName.RightRing2Stretched, MuscleName.RightRing3Stretched };
                        break;

                    case FingerCategory.RightThumb:
                        fingerCurl = SkeletonAction.thumbCurl;
                        indexes = new[] { MuscleName.RightThumb1Stretched, MuscleName.RightThumb2Stretched, MuscleName.RightThumb3Stretched };
                        break;

                    case FingerCategory.LeftIndex:
                        fingerCurl = SkeletonAction.indexCurl;
                        indexes = new[] { MuscleName.LeftIndex1Stretched, MuscleName.LeftIndex2Stretched, MuscleName.LeftIndex3Stretched };
                        break;

                    case FingerCategory.LeftLittle:
                        fingerCurl = SkeletonAction.pinkyCurl;
                        indexes = new[] { MuscleName.LeftLittle1Stretched, MuscleName.LeftLittle2Stretched, MuscleName.LeftLittle3Stretched };
                        break;

                    case FingerCategory.LeftMiddle:
                        fingerCurl = SkeletonAction.middleCurl;
                        indexes = new[] { MuscleName.LeftMiddle1Stretched, MuscleName.LeftMiddle2Stretched, MuscleName.LeftMiddle3Stretched };
                        break;

                    case FingerCategory.LeftRing:
                        fingerCurl = SkeletonAction.ringCurl;
                        indexes = new[] { MuscleName.LeftRing1Stretched, MuscleName.LeftRing2Stretched, MuscleName.LeftRing3Stretched };
                        break;

                    case FingerCategory.LeftThumb:
                        fingerCurl = SkeletonAction.thumbCurl;
                        indexes = new[] { MuscleName.LeftThumb1Stretched, MuscleName.LeftThumb2Stretched, MuscleName.LeftThumb3Stretched };
                        break;
                }

                for (var i = 0; i < 3; i++)
                    humanPose.muscles[(int) indexes[i]] = Mathf.Lerp(-.75f, 1f, fingerCurl * weights[i]) * -1;

                handler.SetHumanPose(ref humanPose);
            }
        }
    }
}