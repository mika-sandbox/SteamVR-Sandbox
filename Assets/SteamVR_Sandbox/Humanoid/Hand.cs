using System;
using System.Collections.Generic;

using SteamVR_Sandbox.Enums;
using SteamVR_Sandbox.Models;

using UnityEngine;

using Valve.VR;

namespace SteamVR_Sandbox.Humanoid
{
    [Serializable]
    public class Hand
    {
        // Vector3 map for each finger joint and bending direction (Z-axis based)
        private readonly Dictionary<FingerCategory, Stretch[]> _fingerStretches = new Dictionary<FingerCategory, Stretch[]>
        {
            {
                FingerCategory.Thumb,
                new[]
                {
                    new Stretch { RangeOfMotion = new Range<float>(-15f, 60f), Direction = Vector3.right },
                    new Stretch { RangeOfMotion = new Range<float>(-10f, 80f), Direction = Vector3.right },
                    new Stretch { RangeOfMotion = new Range<float>(-10f, 40f), Direction = Vector3.right }
                }
            },
            {
                FingerCategory.Index,
                new[]
                {
                    new Stretch { RangeOfMotion = new Range<float>(-15f, 90f), Direction = Vector3.right },
                    new Stretch { RangeOfMotion = new Range<float>(-10f, 100f), Direction = Vector3.right },
                    new Stretch { RangeOfMotion = new Range<float>(-10f, 80f), Direction = Vector3.right }
                }
            },
            {
                FingerCategory.Middle,
                new[]
                {
                    new Stretch { RangeOfMotion = new Range<float>(-15f, 90f), Direction = Vector3.right },
                    new Stretch { RangeOfMotion = new Range<float>(-10f, 100f), Direction = Vector3.right },
                    new Stretch { RangeOfMotion = new Range<float>(-10f, 80f), Direction = Vector3.right }
                }
            },
            {
                FingerCategory.Ring,
                new[]
                {
                    new Stretch { RangeOfMotion = new Range<float>(-15f, 90f), Direction = Vector3.right },
                    new Stretch { RangeOfMotion = new Range<float>(-10f, 100f), Direction = Vector3.right },
                    new Stretch { RangeOfMotion = new Range<float>(-10f, 80f), Direction = Vector3.right }
                }
            },
            {
                FingerCategory.Little,
                new[]
                {
                    new Stretch { RangeOfMotion = new Range<float>(-15f, 90f), Direction = Vector3.right },
                    new Stretch { RangeOfMotion = new Range<float>(-10f, 100f), Direction = Vector3.right },
                    new Stretch { RangeOfMotion = new Range<float>(-10f, 80f), Direction = Vector3.right }
                }
            }
        };

        private Axis _axis;

        // If isReserve is true, it indicates that the axis points along the Y-axis in the global axis.
        private bool _isReverse;

        [SerializeField]
        public SteamVR_Action_Skeleton Skeleton;

        public void StoreAxisDirection(Transform transform)
        {
            if (Math.Abs(Vector3.Dot(transform.forward, Vector3.down)) > .99)
            {
                _axis = Axis.Z;
                _isReverse = Vector3.Dot(transform.forward, Vector3.down) < 0;
            }
            else if (Math.Abs(Vector3.Dot(transform.right, Vector3.down)) > .99)
            {
                _axis = Axis.X;
                _isReverse = Vector3.Dot(transform.right, Vector3.down) < 0;
            }
            else
            {
                // It is assumed that the Y-axis of the local axis does not coincide with the global axis.
                throw new NotSupportedException();
            }
        }

        public void StoreStretchTransform(Transform transform, FingerCategory category, FingerJoint joint)
        {
            switch (category)
            {
                case FingerCategory.Thumb:
                    SetStretchInformation(Thumb, transform, joint);
                    break;

                case FingerCategory.Index:
                    SetStretchInformation(Index, transform, joint);
                    break;

                case FingerCategory.Middle:
                    SetStretchInformation(Middle, transform, joint);
                    break;

                case FingerCategory.Ring:
                    SetStretchInformation(Ring, transform, joint);
                    break;

                case FingerCategory.Little:
                    SetStretchInformation(Little, transform, joint);
                    break;
            }
        }

        public Quaternion CalcFingerCurlByQuaternion(FingerCategory category, FingerJoint joint)
        {
            var stretches = _fingerStretches[category];
            var stretch = (default(Stretch), default(Quaternion), default(float));

            switch (category)
            {
                case FingerCategory.Thumb:
                    stretch = GetStretchInformation(Thumb, stretches, joint);
                    break;

                case FingerCategory.Index:
                    stretch = GetStretchInformation(Index, stretches, joint);
                    break;

                case FingerCategory.Middle:
                    stretch = GetStretchInformation(Middle, stretches, joint);
                    break;

                case FingerCategory.Ring:
                    stretch = GetStretchInformation(Ring, stretches, joint);
                    break;

                case FingerCategory.Little:
                    stretch = GetStretchInformation(Little, stretches, joint);
                    break;
            }

            var curl = GetFingerCurl(category);
            var angle = Mathf.Lerp(stretch.Item1.RangeOfMotion.Min, stretch.Item1.RangeOfMotion.Max, curl * stretch.Item3);
            return stretch.Item2 * CalcAngleAxis(angle, stretch.Item1.Direction); // rotate `angle` based by `axis`
        }

        private float GetFingerCurl(FingerCategory category)
        {
            switch (category)
            {
                case FingerCategory.Thumb:
                    return Skeleton.thumbCurl;

                case FingerCategory.Index:
                    return Skeleton.indexCurl;

                case FingerCategory.Middle:
                    return Skeleton.middleCurl;

                case FingerCategory.Ring:
                    return Skeleton.ringCurl;

                case FingerCategory.Little:
                    return Skeleton.pinkyCurl;

                default:
                    throw new ArgumentOutOfRangeException(nameof(category));
            }
        }

        private Quaternion CalcAngleAxis(float angle, Vector3 vector)
        {
            switch (_axis)
            {
                case Axis.X:
                    return Quaternion.AngleAxis(_isReverse ? -angle : angle, Quaternion.Euler(0, 90, 0) * vector);

                case Axis.Z:
                    return Quaternion.AngleAxis(_isReverse ? -angle : angle, vector);

                default:
                    return Quaternion.AngleAxis(angle, vector);
            }
        }

        private (Stretch, Quaternion, float) GetStretchInformation(Finger finger, Stretch[] stretches, FingerJoint joint)
        {
            switch (joint)
            {
                case FingerJoint.Stretch1:
                    return (stretches[0], finger.Stretch1Rotation, finger.Stretch1Weight);

                case FingerJoint.Stretch2:
                    return (stretches[1], finger.Stretch2Rotation, finger.Stretch2Weight);

                case FingerJoint.Stretch3:
                    return (stretches[2], finger.Stretch3Rotation, finger.Stretch3Weight);

                default:
                    throw new ArgumentOutOfRangeException(nameof(joint));
            }
        }

        private void SetStretchInformation(Finger finger, Transform transform, FingerJoint joint)
        {
            switch (joint)
            {
                case FingerJoint.Stretch1:
                    finger.Stretch1Rotation = transform.localRotation;
                    break;

                case FingerJoint.Stretch2:
                    finger.Stretch2Rotation = transform.localRotation;
                    break;

                case FingerJoint.Stretch3:
                    finger.Stretch3Rotation = transform.localRotation;
                    break;
            }
        }

        #region Fingers

        [SerializeField]
        public Finger Index;

        [SerializeField]
        public Finger Little;

        [SerializeField]
        public Finger Middle;

        [SerializeField]
        public Finger Ring;

        [SerializeField]
        public Finger Thumb;

        #endregion
    }
}