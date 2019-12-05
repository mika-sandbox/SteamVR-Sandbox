using RootMotion.FinalIK;

using UnityEngine;

using Valve.VR.InteractionSystem;

namespace SteamVR_Sandbox.Scripts
{
    // ReSharper disable once InconsistentNaming
    [RequireComponent(typeof(Player))]
    public class SteamVRFallback : MonoBehaviour
    {
        private bool previous;

        // Start is called before the first frame update
        private void Start()
        {
            if (Debug.isDebugBuild) previous = Player.instance.rigSteamVR.activeSelf;
        }

        // Update is called once per frame
        private void Update()
        {
            if (!Debug.isDebugBuild)
                return;

            if (previous != Player.instance.rigSteamVR.activeSelf)
            {
                previous = Player.instance.rigSteamVR.activeSelf;

                if (Player.instance.rigSteamVR.activeSelf)
                {
                    IK.solver.spine.headTarget = HeadVRTracker;
                    IK.solver.leftArm.target = LeftHandVRTracker;
                    IK.solver.rightArm.target = RightHandVRTracker;
                }
                else
                {
                    IK.solver.spine.headTarget = HeadMockTracker;
                    IK.solver.leftArm.target = LeftHandMockTracker;
                    IK.solver.rightArm.target = RightHandMockTracker;
                }
            }
        }

        // ReSharper disable InconsistentNaming

        // VRIK Fallback

        [SerializeField]
        private VRIK IK;

        [SerializeField]
        private Transform HeadVRTracker;

        [SerializeField]
        private Transform HeadMockTracker;

        [SerializeField]
        private Transform LeftHandVRTracker;

        [SerializeField]
        private Transform LeftHandMockTracker;

        [SerializeField]
        private Transform RightHandVRTracker;

        [SerializeField]
        private Transform RightHandMockTracker;

        // ReSharper restore InconsistentNaming
    }
}