using System;

using UnityEngine;

using Valve.VR;

namespace SteamVR_Sandbox.Models
{
    [Serializable]
    public class Hand
    {
        [SerializeField]
        public SteamVR_Action_Skeleton Skeleton;

        #region Fingers

        [SerializeField]
        public Finger Index;

        [SerializeField]
        public Finger Little;

        [SerializeField]
        public Finger Middle;

        [SerializeField]
        public Finger Ring;

        [SerializeField]
        public Finger Thumb;

        #endregion
    }
}