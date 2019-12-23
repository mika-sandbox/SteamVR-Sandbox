using System;

using UnityEngine;

namespace SteamVR_Sandbox.Models
{
    [Serializable]
    public class Finger
    {
        [SerializeField]
        [Range(0f, 1f)]
        public float Stretch1Weight;

        [SerializeField]
        [Range(0f, 1f)]
        public float Stretch2Weight;

        [SerializeField]
        [Range(0f, 1f)]
        public float Stretch3Weight;
    }
}