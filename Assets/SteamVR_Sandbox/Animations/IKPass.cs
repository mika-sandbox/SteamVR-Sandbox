using SteamVR_Sandbox.Models;

using UnityEngine;

namespace SteamVR_Sandbox.Animations
{
    [AddComponentMenu("Scripts/Mochizuki.VR/Animation/IK Pass")]
    public class IKPass : MonoBehaviour
    {
        [SerializeField]
        private AnimatorIKReceiver Receiver;

        private void OnAnimatorIK()
        {
            Receiver.OnAnimatorIKReceived();
        }
    }
}