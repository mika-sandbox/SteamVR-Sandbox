using System.Collections.Generic;

using SteamVR_Sandbox.Animations;
using SteamVR_Sandbox.Enums;
using SteamVR_Sandbox.Models;

using UnityEngine;

using Valve.VR;

namespace SteamVR_Sandbox.Avatar
{
    [AddComponentMenu("Scripts/Mochizuki.VR/Avatar/Avatar Animator")]
    public class AvatarAnimator : MonoBehaviour
    {
        private readonly Dictionary<AnimatorState, bool[]> _states;

        [SerializeField]
        private Animator Animator;

        [SerializeField]
        private List<AvatarAnimation> AvatarAnimations;

        [SerializeField]
        private SteamVR_Action_Skeleton LeftSkeleton;

        [SerializeField]
        private SteamVR_Action_Skeleton RightSkeleton;

        public AvatarAnimator()
        {
            _states = new Dictionary<AnimatorState, bool[]>();
        }

        private void Start()
        {
            foreach (var anim in AvatarAnimations)
            {
                if (_states.ContainsKey(anim.TransitionTo))
                    continue;
                _states.Add(anim.TransitionTo, new[] { false, false });
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (Animator == null || LeftSkeleton == null || RightSkeleton == null)
                return;

            foreach (var anim in AvatarAnimations)
            {
                var left = anim.ShouldChangeAnimationState(LeftSkeleton);
                var right = anim.ShouldChangeAnimationState(RightSkeleton);

                Animator.SetBool(anim.TransitionTo.GetEnumMemberValue(), left || right);
            }
        }
    }
}