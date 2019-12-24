using System;

using UnityEngine;

using Valve.VR;

namespace SteamVR_Sandbox.Models
{
    [Serializable]
    public class AvatarAnimation
    {
        [SerializeField]
        private State AnimationState;

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

        public void OnUpdate(Animator animator, SteamVR_Action_Skeleton skeleton)
        {
            if (IndexThreshold.Compare(skeleton.indexCurl) && LittleThreshold.Compare(skeleton.pinkyCurl) && MiddleThreshold.Compare(skeleton.middleCurl) && RingThreshold.Compare(skeleton.ringCurl) && ThumbThreshold.Compare(skeleton.thumbCurl))
            {
                animator.SetLayerWeight(2, 1); // FINGER_EMOTES
                animator.SetBool(AnimationState.GetEnumMemberValue(), true);
            }
            else
            {
                animator.SetLayerWeight(2, 0); // FINGER_EMOTES
                animator.SetBool(AnimationState.GetEnumMemberValue(), false);
            }
        }
    }
}