using RootMotion.FinalIK;

using UnityEngine;

using Valve.VR;

namespace SteamVR_Sandbox.SteamVR
{
    [AddComponentMenu("Scripts/Mochizuki.VR/SteamVR/SteamVR HMD Tracker")]
    public class SteamVRHMDTracker : SteamVRTrackerBase
    {
        public override bool IsActive => true; // Always true
        protected override SteamVR_Input_Sources InputSource => SteamVR_Input_Sources.Head;
        protected override GameObject TrackerModel => null;
        protected override GameObject TargetObject => transform.GetChild(0).gameObject;

        public GameObject Target => TargetObject;

        public void Calibrate(VRIK avatar, Vector3 viewPosition)
        {
            if (InputSource != SteamVR_Input_Sources.Head)
            {
                Calibrate(avatar);
                return;
            }

            var target = TargetObject.transform;
            var diff = viewPosition - (avatar.references.head.position - avatar.references.root.position);

            target.localPosition = -diff;
            target.rotation = transform.rotation;
        }
    }
}