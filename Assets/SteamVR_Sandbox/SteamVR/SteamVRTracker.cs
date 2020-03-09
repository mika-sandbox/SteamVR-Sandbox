using System;
using System.Linq;

using RootMotion.FinalIK;

using UnityEngine;

using Valve.VR;

namespace SteamVR_Sandbox.SteamVR
{
    // ReSharper disable once InconsistentNaming
    [AddComponentMenu("Scripts/Mochizuki.VR/SteamVR/SteamVR Tracker")]
    [RequireComponent(typeof(SteamVR_Behaviour_Pose))]
    public class SteamVRTracker : MonoBehaviour
    {
        public SteamVR_Behaviour_Pose Pose => GetComponent<SteamVR_Behaviour_Pose>();
        public bool IsActive => Pose.isActive && Pose.isValid;
        public GameObject Target => transform.GetChild(1).gameObject; // 1st GameObject is SteamVR Model Renderer.

        // Adjust Target GameObject to the current state is recognized as the state of Quaternion.zero.
        public void Calibrate(VRIK avatar)
        {
            Target.transform.localRotation = Quaternion.identity;
            Target.transform.localPosition = Vector3.zero;

            switch (Pose.inputSource)
            {
                case SteamVR_Input_Sources.Waist:
                {
                    Target.transform.rotation = avatar.references.pelvis.rotation;
                    break;
                }

                case SteamVR_Input_Sources.LeftElbow:
                    break;

                case SteamVR_Input_Sources.RightElbow:
                    break;

                case SteamVR_Input_Sources.LeftFoot:
                {
                    // XXX: Should I copy the source code of Final IK's VRIK Calibrator such as VMC????
                    // TODO: I should consider the case where the trackers are placed at an angle.
                    // In the foot position, Final IK expects different angles of the Target GameObject depending on the rotation of the last bone.
                    // For example, in the official sample model, the last bone was vertical, but the Shapell was rotated 45 degrees, so the offset value had to be considered.
                    var lastBone = avatar.references.leftToes ?? avatar.references.leftFoot;

                    // adjust foot tracker position to last bone's position
                    Target.transform.position = new Vector3(Target.transform.position.x, lastBone.position.y, Target.transform.position.z);

                    // match the rotation axis of the last bone and the target GameObject.
                    Target.transform.rotation = lastBone.rotation;

                    // match the target forward towards tracker forward
                    var footForward = DetectNearestAxis(avatar.references.root.forward, lastBone);
                    var toRotation = Quaternion.LookRotation(transform.rotation * footForward);
                    if (Vector3.Dot(Target.transform.rotation * footForward, transform.rotation * footForward) < 0.0f)
                        footForward = -footForward; // target forward and tracker forward are in opposite directions
                    var fromRotation = Quaternion.LookRotation(Target.transform.rotation * footForward);
                    Target.transform.rotation = toRotation * Quaternion.Inverse(fromRotation) * Target.transform.rotation;

                    break;
                }

                case SteamVR_Input_Sources.RightFoot:
                {
                    var lastBone = avatar.references.rightToes ?? avatar.references.rightFoot;

                    Target.transform.position = new Vector3(Target.transform.position.x, lastBone.position.y, Target.transform.position.z);
                    Target.transform.rotation = lastBone.rotation;

                    var footForward = DetectNearestAxis(avatar.references.root.forward, lastBone);
                    var toRotation = Quaternion.LookRotation(transform.rotation * footForward);
                    if (Vector3.Dot(Target.transform.rotation * footForward, transform.rotation * footForward) < 0.0f)
                        footForward = -footForward;
                    var fromRotation = Quaternion.LookRotation(Target.transform.rotation * footForward);
                    Target.transform.rotation = toRotation * Quaternion.Inverse(fromRotation) * Target.transform.rotation;

                    break;
                }

                case SteamVR_Input_Sources.LeftKnee:
                    break;

                case SteamVR_Input_Sources.RightKnee:
                    break;
            }
        }

        private Vector3 DetectNearestAxis(Vector3 vector, Transform transform)
        {
            var vector1 = Vector3.Dot(vector, transform.right);
            var vector2 = Vector3.Dot(vector, transform.up);
            var vector3 = Vector3.Dot(vector, transform.forward);
            var vectors = new[] { vector1, vector2, vector3 };
            var nearest = vectors.Select((w, i) => new { Value = w, Index = i }).OrderByDescending(w => Mathf.Abs(w.Value)).First();

            var x = nearest.Index == 0 ? Math.Sign(nearest.Value) * 1 : 0;
            var y = nearest.Index == 1 ? Math.Sign(nearest.Value) * 1 : 0;
            var z = nearest.Index == 2 ? Math.Sign(nearest.Value) * 1 : 0;

            return new Vector3(x, y, z);
        }
    }
}