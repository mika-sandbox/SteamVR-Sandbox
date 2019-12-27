using System.Collections.Generic;

using SteamVR_Sandbox.Animations;
using SteamVR_Sandbox.Enums;

using UnityEngine;

using Valve.VR;

namespace SteamVR_Sandbox.Avatar
{
    [AddComponentMenu("Scripts/Mochizuki.VR/Avatar/Avatar Animator")]
    public class AvatarAnimator : MonoBehaviour
    {
        [SerializeField]
        private Animator Animator;

        [SerializeField]
        private List<AvatarAnimation> AvatarAnimations;

        [SerializeField]
        private Side Side;

        [SerializeField]
        private SteamVR_Action_Skeleton Skeleton;

        // Update is called once per frame
        private void Update()
        {
            if (Animator == null || Skeleton == null)
                return;

            foreach (var avatarAnimation in AvatarAnimations) avatarAnimation.OnUpdate(Animator, Side, Skeleton);
        }
    }
}