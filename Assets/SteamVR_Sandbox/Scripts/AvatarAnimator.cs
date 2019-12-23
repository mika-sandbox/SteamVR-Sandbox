using System.Collections.Generic;

using SteamVR_Sandbox.Models;

using UnityEngine;

using Valve.VR;

namespace SteamVR_Sandbox.Scripts
{
    public class AvatarAnimator : MonoBehaviour
    {
        [SerializeField]
        private List<AnimationOverride> AnimationOverrides;

        [SerializeField]
        private Animator Animator;

        [SerializeField]
        private SteamVR_Action_Skeleton Skeleton;

        // Update is called once per frame
        private void Update()
        {
            if (Animator == null || Skeleton == null)
                return;

            foreach (var animationOverride in AnimationOverrides) animationOverride.OnUpdate(Animator, Skeleton);
        }
    }
}