using System.Collections.Generic;

using SteamVR_Sandbox.Models;

using UnityEngine;

using Valve.VR;

namespace SteamVR_Sandbox.Scripts
{
    public class AvatarAnimator : MonoBehaviour
    {
        [SerializeField]
        private Animator Animator;

        [SerializeField]
        private List<AvatarAnimation> AvatarAnimations;

        [SerializeField]
        private SteamVR_Action_Skeleton Skeleton;

        // Update is called once per frame
        private void Update()
        {
            if (Animator == null || Skeleton == null)
                return;

            foreach (var avatarAnimation in AvatarAnimations) avatarAnimation.OnUpdate(Animator, Skeleton);
        }
    }
}