using SteamVR_Sandbox.Models;

using UnityEngine;

namespace SteamVR_Sandbox.Humanoid
{
    public struct Stretch
    {
        public Range<float> RangeOfMotion { get; set; }

        public Vector3 Direction { get; set; }
    }
}