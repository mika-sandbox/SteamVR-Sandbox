using System;

using SteamVR_Sandbox.Enums;
using SteamVR_Sandbox.Models;

using UnityEngine;

using Valve.VR;

namespace SteamVR_Sandbox.Animations
{
    [Serializable]
    public class AvatarAnimation
    {
        [SerializeField]
        private AnimatorState AnimatorState;

        [SerializeField]
        private Threshold IndexThreshold;

        [SerializeField]
        private Threshold LittleThreshold;

        [SerializeField]
        private Threshold MiddleThreshold;

        [SerializeField]
        private Threshold RingThreshold;

        [SerializeField]
        private Threshold ThumbThreshold;

        public AnimatorState TransitionTo => AnimatorState;

        public bool ShouldChangeAnimationState(SteamVR_Action_Skeleton skeleton)
        {
            return IndexThreshold.Compare(skeleton.indexCurl) && LittleThreshold.Compare(skeleton.pinkyCurl) && MiddleThreshold.Compare(skeleton.middleCurl) && RingThreshold.Compare(skeleton.ringCurl) && ThumbThreshold.Compare(skeleton.thumbCurl);
        }
    }
}