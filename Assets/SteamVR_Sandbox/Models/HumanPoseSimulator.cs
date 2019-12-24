using System.Collections;

using UnityEngine;

namespace SteamVR_Sandbox.Models
{
    // Reproduce the behavior of the HumanPose/HumanPoseHandler class.
    public static class HumanPoseSimulator
    {
        // Basic Joint Range of Motion (ROM)

        private static readonly Hashtable HumanPoseRangeOfMotionDefinitions = new Hashtable
        {
            // Left
            { MuscleName.LeftThumbSpread, new MuscleValue<float> { Min = -45f, Max = 10f } }, // no sources
            { MuscleName.LeftThumb1Stretched, new MuscleValue<float> { Min = 10f, Max = 60f, Axis = new Vector3(.4f, .2f, .7f) } },
            { MuscleName.LeftThumb2Stretched, new MuscleValue<float> { Min = -10f, Max = -80f, Axis = new Vector3(.1f, .1f, .1f) } },
            { MuscleName.LeftThumb3Stretched, new MuscleValue<float> { Min = -10f, Max = -40f, Axis = new Vector3(.2f, -.4f, .8f) } },
            { MuscleName.LeftIndexSpread, new MuscleValue<float> { Min = -10f, Max = 10f } }, // no sources
            { MuscleName.LeftIndex1Stretched, new MuscleValue<float> { Min = -15f, Max = 90f, Axis = new Vector3(1f, 0f, 0f) } },
            { MuscleName.LeftIndex2Stretched, new MuscleValue<float> { Min = -10f, Max = 100f, Axis = new Vector3(1f, 0f, 0f) } },
            { MuscleName.LeftIndex3Stretched, new MuscleValue<float> { Min = -10f, Max = 80f, Axis = new Vector3(1f, 0f, 0f) } },
            { MuscleName.LeftMiddleSpread, new MuscleValue<float> { Min = -10f, Max = 10f } }, // no sources
            { MuscleName.LeftMiddle1Stretched, new MuscleValue<float> { Min = -15f, Max = 90f, Axis = new Vector3(1f, 0f, 0f) } },
            { MuscleName.LeftMiddle2Stretched, new MuscleValue<float> { Min = -10f, Max = 100f, Axis = new Vector3(1f, 0f, 0f) } },
            { MuscleName.LeftMiddle3Stretched, new MuscleValue<float> { Min = -10f, Max = 80f, Axis = new Vector3(1f, 0f, 0f) } },
            { MuscleName.LeftRingSpread, new MuscleValue<float> { Min = -10f, Max = 10f } }, // no sources
            { MuscleName.LeftRing1Stretched, new MuscleValue<float> { Min = -15f, Max = 90f, Axis = new Vector3(1f, 0f, 0f) } },
            { MuscleName.LeftRing2Stretched, new MuscleValue<float> { Min = -10f, Max = 100f, Axis = new Vector3(1f, 0f, 0f) } },
            { MuscleName.LeftRing3Stretched, new MuscleValue<float> { Min = -10f, Max = 80f, Axis = new Vector3(1f, 0f, 0f) } },
            { MuscleName.LeftLittleSpread, new MuscleValue<float> { Min = -10f, Max = 10f } }, // no sources
            { MuscleName.LeftLittle1Stretched, new MuscleValue<float> { Min = -15f, Max = 90f, Axis = new Vector3(1f, 0f, 0f) } },
            { MuscleName.LeftLittle2Stretched, new MuscleValue<float> { Min = -10f, Max = 100f, Axis = new Vector3(1f, 0f, 0f) } },
            { MuscleName.LeftLittle3Stretched, new MuscleValue<float> { Min = -10f, Max = 80f, Axis = new Vector3(1f, 0f, 0f) } },

            // Right
            { MuscleName.RightThumbSpread, new MuscleValue<float> { Min = -45f, Max = 10f } }, // no sources
            { MuscleName.RightThumb1Stretched, new MuscleValue<float> { Min = 10f, Max = 60f, Axis = new Vector3(.4f, .2f, .7f) } },
            { MuscleName.RightThumb2Stretched, new MuscleValue<float> { Min = -10f, Max = -90f, Axis = new Vector3(.1f, .1f, .1f) } },
            { MuscleName.RightThumb3Stretched, new MuscleValue<float> { Min = -10f, Max = -80f, Axis = new Vector3(.2f, -.4f, .8f) } }, // no sources
            { MuscleName.RightIndexSpread, new MuscleValue<float> { Min = -10f, Max = 10f } }, // no sources
            { MuscleName.RightIndex1Stretched, new MuscleValue<float> { Min = -15f, Max = 90f, Axis = new Vector3(1f, 0f, 0f) } },
            { MuscleName.RightIndex2Stretched, new MuscleValue<float> { Min = -10f, Max = 100f, Axis = new Vector3(1f, 0f, 0f) } },
            { MuscleName.RightIndex3Stretched, new MuscleValue<float> { Min = -10f, Max = 80f, Axis = new Vector3(1f, 0f, 0f) } },
            { MuscleName.RightMiddleSpread, new MuscleValue<float> { Min = -10f, Max = 10f } }, // no sources
            { MuscleName.RightMiddle1Stretched, new MuscleValue<float> { Min = -15f, Max = 90f, Axis = new Vector3(1f, 0f, 0f) } },
            { MuscleName.RightMiddle2Stretched, new MuscleValue<float> { Min = -10f, Max = 100f, Axis = new Vector3(1f, 0f, 0f) } },
            { MuscleName.RightMiddle3Stretched, new MuscleValue<float> { Min = -10f, Max = 80f, Axis = new Vector3(1f, 0f, 0f) } },
            { MuscleName.RightRingSpread, new MuscleValue<float> { Min = -10f, Max = 10f } }, // no sources
            { MuscleName.RightRing1Stretched, new MuscleValue<float> { Min = -15f, Max = 90f, Axis = new Vector3(1f, 0f, 0f) } },
            { MuscleName.RightRing2Stretched, new MuscleValue<float> { Min = -10f, Max = 100f, Axis = new Vector3(1f, 0f, 0f) } },
            { MuscleName.RightRing3Stretched, new MuscleValue<float> { Min = -10f, Max = 80f, Axis = new Vector3(1f, 0f, 0f) } },
            { MuscleName.RightLittleSpread, new MuscleValue<float> { Min = -10f, Max = 10f } }, // no sources
            { MuscleName.RightLittle1Stretched, new MuscleValue<float> { Min = -15f, Max = 90f, Axis = new Vector3(1f, 0f, 0f) } },
            { MuscleName.RightLittle2Stretched, new MuscleValue<float> { Min = -10f, Max = 100f, Axis = new Vector3(1f, 0f, 0f) } },
            { MuscleName.RightLittle3Stretched, new MuscleValue<float> { Min = -10f, Max = 80f, Axis = new Vector3(1f, 0f, 0f) } }
        };

        public static float CalcFingerCurlByFloat(MuscleName muscle, float curl)
        {
            var value = (MuscleValue<float>) HumanPoseRangeOfMotionDefinitions[muscle];
            return Mathf.Lerp(value.Min, value.Max, curl);
        }

        public static Quaternion CalcFingerCurlByQuaternion(MuscleName muscle, float curl)
        {
            var value = (MuscleValue<float>) HumanPoseRangeOfMotionDefinitions[muscle];
            return Quaternion.AngleAxis(Mathf.Lerp(value.Min, value.Max, curl), value.Axis);
        }
    }
}