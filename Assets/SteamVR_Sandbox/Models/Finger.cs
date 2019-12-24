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

        [SerializeField]
        public Vector3 Stretch1Axis;

        [SerializeField]
        public Vector3 Stretch2Axis;

        [SerializeField]
        public Vector3 Stretch3Axis;
    }
}