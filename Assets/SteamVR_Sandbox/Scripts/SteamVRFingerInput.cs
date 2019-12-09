using UnityEngine;

namespace SteamVR_Sandbox.Scripts
{
    // ReSharper disable once InconsistentNaming
    public class SteamVRFingerInput : MonoBehaviour
    {
        public enum FingerCategory
        {
            RightIndex,

            RightLittle,

            RightMiddle,

            RightRing,

            RightThumb,

            LeftIndex,

            LeftLittle,

            LeftMiddle,

            LeftRing,

            LeftThumb
        }

		[SerializeField]
		private Animator Avatar;

		[SerializeField]
        private FingerCategory Finger;

		private HumanPose _humanPose;

		// Start is called before the first frame update
		private void Start() { }

        // Update is called once per frame
        private void Update()
        { 
            if (this.Avatar == null) {
				return;
			}

			var handler = new HumanPoseHandler(this.Avatar.avatar, this.Avatar.transform);
			handler.GetHumanPose(ref this._humanPose);

			handler.SetHumanPose(ref this._humanPose);
		}
    }
}