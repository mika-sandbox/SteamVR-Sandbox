using UnityEngine;

namespace SteamVR_Sandbox.Models
{
    public class MuscleValue<T>
    {
        public T Max { get; set; }

        public T Min { get; set; }

        public Vector3 Axis { get; set; } = Vector3.zero;
    }
}