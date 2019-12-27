using System;

using UnityEngine;

namespace SteamVR_Sandbox.Humanoid
{
    [Serializable]
    public class Finger
    {
        [SerializeField]
        public Vector3 Stretch1Axis;

        [SerializeField]
        [Range(0f, 1f)]
        public float Stretch1Weight;

        [SerializeField]
        public Vector3 Stretch2Axis;

        [SerializeField]
        [Range(0f, 1f)]
        public float Stretch2Weight;

        [SerializeField]
        public Vector3 Stretch3Axis;

        [SerializeField]
        [Range(0f, 1f)]
        public float Stretch3Weight;

        public Quaternion Stretch1Rotation { get; set; }

        public Quaternion Stretch2Rotation { get; set; }

        public Quaternion Stretch3Rotation { get; set; }
    }
}