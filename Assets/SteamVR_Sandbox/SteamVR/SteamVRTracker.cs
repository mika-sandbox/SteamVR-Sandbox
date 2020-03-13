using UnityEngine;

using Valve.VR;

namespace SteamVR_Sandbox.SteamVR
{
    // ReSharper disable once InconsistentNaming
    [AddComponentMenu("Scripts/Mochizuki.VR/SteamVR/SteamVR Tracker")]
    [RequireComponent(typeof(SteamVR_Behaviour_Pose))]
    public class SteamVRTracker : SteamVRTrackerBase
    {
        public override bool IsActive => Pose.isActive && Pose.isValid;
        protected override SteamVR_Behaviour_Pose Pose => GetComponent<SteamVR_Behaviour_Pose>();
        protected override SteamVR_Input_Sources InputSource => Pose.inputSource;
        protected override GameObject TrackerModel => transform.GetChild(0).gameObject;
        protected override GameObject TargetObject => transform.GetChild(1).gameObject;
    }
}