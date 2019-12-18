using RootMotion.FinalIK;

using UnityEngine;

#pragma warning disable 649

namespace SteamVR_Sandbox.Scripts
{
    public class AvatarCalibrator : MonoBehaviour
    {
        private const float PlayerHandDistanceByHeight = .78f;

        // Start is called before the first frame update
        private void Start()
        {
            if (World == null)
                return;

            var avatarHeadPosition = IK.references.head.position;
            var avatarViewPosition = ViewPosition;

            var headTrackerPosition = HeadTracker.transform.localPosition;
            headTrackerPosition.x -= ViewPosition.x;
            headTrackerPosition.y -= avatarViewPosition.y - avatarHeadPosition.y;
            headTrackerPosition.z -= ViewPosition.z;
            HeadTracker.transform.localPosition = headTrackerPosition;

            var avatarHandDistance = Vector3.Distance(IK.references.leftHand.position, IK.references.rightHand.position);
            var playerHandDistance = PlayerHeight * PlayerHandDistanceByHeight;
            var handTrackerScale = playerHandDistance / avatarHandDistance;

            World.transform.localScale = new Vector3(handTrackerScale, handTrackerScale, handTrackerScale);

            // World.transform.localScale *= handTrackerScale;
        }

        // LateUpdate is called once per frame
        private void LateUpdate() { }

        // ReSharper disable InconsistentNaming

        [SerializeField]
        private GameObject HeadTracker;

        [SerializeField]
        private GameObject LeftHandTracker;

        [SerializeField]
        private GameObject RightHandTracker;

        [SerializeField]
        private GameObject World;

        [SerializeField]
        private VRIK IK;

        [SerializeField]
        [Tooltip("Player Real Height (m)")]
        private float PlayerHeight;

        [SerializeField]
        [Tooltip("VRChat View Position (Camera Position for Avatar)")]
        public Vector3 ViewPosition;

        // ReSharper restore InconsistentNaming
    }
}