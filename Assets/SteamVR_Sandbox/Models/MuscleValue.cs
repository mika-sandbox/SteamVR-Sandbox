using System;

using UnityEngine;

namespace SteamVR_Sandbox.Models
{
    [Serializable]
    public class MuscleValue<T>
    {
        [SerializeField]
        public T Max { get; set; }

        [SerializeField]
        public T Min { get; set; }
    }
}