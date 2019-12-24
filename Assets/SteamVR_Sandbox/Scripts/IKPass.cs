using SteamVR_Sandbox.Models;

using UnityEngine;

namespace SteamVR_Sandbox.Scripts
{
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