using System;

using SteamVR_Sandbox.Enums;

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

        public void OnUpdate(Animator animator, Side side, SteamVR_Action_Skeleton skeleton)
        {
            var layer = animator.GetLayerIndex($"{side.ToString().ToUpper()}_HAND");
            if (IndexThreshold.Compare(skeleton.indexCurl) && LittleThreshold.Compare(skeleton.pinkyCurl) && MiddleThreshold.Compare(skeleton.middleCurl) && RingThreshold.Compare(skeleton.ringCurl) && ThumbThreshold.Compare(skeleton.thumbCurl))
            {
                animator.SetLayerWeight(layer, 1); // FINGER_EMOTES
                animator.SetBool(AnimationState.GetEnumMemberValue(), true);
            }
            else
            {
                animator.SetLayerWeight(layer, 0); // FINGER_EMOTES
                animator.SetBool(AnimationState.GetEnumMemberValue(), false);
            }
        }
    }
}