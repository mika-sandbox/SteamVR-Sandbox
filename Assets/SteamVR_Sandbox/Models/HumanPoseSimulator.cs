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
            { MuscleName.LeftThumb1Stretched, new MuscleValue<float> { Min = 10f, Max = 60f } },
            { MuscleName.LeftThumb2Stretched, new MuscleValue<float> { Min = -10f, Max = -80f } },
            { MuscleName.LeftThumb3Stretched, new MuscleValue<float> { Min = -10f, Max = -40f } },
            { MuscleName.LeftIndexSpread, new MuscleValue<float> { Min = -10f, Max = 10f } }, // no sources
            { MuscleName.LeftIndex1Stretched, new MuscleValue<float> { Min = -10f, Max = 90f } },
            { MuscleName.LeftIndex2Stretched, new MuscleValue<float> { Min = 0f, Max = 100f } },
            { MuscleName.LeftIndex3Stretched, new MuscleValue<float> { Min = 0f, Max = 80f } },
            { MuscleName.LeftMiddleSpread, new MuscleValue<float> { Min = -10f, Max = 10f } }, // no sources
            { MuscleName.LeftMiddle1Stretched, new MuscleValue<float> { Min = -45f, Max = 90f } },
            { MuscleName.LeftMiddle2Stretched, new MuscleValue<float> { Min = 0f, Max = 100f } },
            { MuscleName.LeftMiddle3Stretched, new MuscleValue<float> { Min = 0f, Max = 80f } },
            { MuscleName.LeftRingSpread, new MuscleValue<float> { Min = -10f, Max = 10f } }, // no sources
            { MuscleName.LeftRing1Stretched, new MuscleValue<float> { Min = -45f, Max = 90f } },
            { MuscleName.LeftRing2Stretched, new MuscleValue<float> { Min = 0f, Max = 100f } },
            { MuscleName.LeftRing3Stretched, new MuscleValue<float> { Min = 0f, Max = 80f } },
            { MuscleName.LeftLittleSpread, new MuscleValue<float> { Min = -10f, Max = 10f } }, // no sources
            { MuscleName.LeftLittle1Stretched, new MuscleValue<float> { Min = -45f, Max = 90f } },
            { MuscleName.LeftLittle2Stretched, new MuscleValue<float> { Min = 0f, Max = 100f } },
            { MuscleName.LeftLittle3Stretched, new MuscleValue<float> { Min = 0f, Max = 80f } },

            // Right
            { MuscleName.RightThumbSpread, new MuscleValue<float> { Min = -45f, Max = 10f } }, // no sources
            { MuscleName.RightThumb1Stretched, new MuscleValue<float> { Min = 10f, Max = 60f } },
            { MuscleName.RightThumb2Stretched, new MuscleValue<float> { Min = -10f, Max = -80f } },
            { MuscleName.RightThumb3Stretched, new MuscleValue<float> { Min = -10f, Max = -40f } }, // no sources
            { MuscleName.RightIndexSpread, new MuscleValue<float> { Min = -10f, Max = 10f } }, // no sources
            { MuscleName.RightIndex1Stretched, new MuscleValue<float> { Min = -10f, Max = 90f } },
            { MuscleName.RightIndex2Stretched, new MuscleValue<float> { Min = 0f, Max = 100f } },
            { MuscleName.RightIndex3Stretched, new MuscleValue<float> { Min = 0f, Max = 80f } },
            { MuscleName.RightMiddleSpread, new MuscleValue<float> { Min = -10f, Max = 10f } }, // no sources
            { MuscleName.RightMiddle1Stretched, new MuscleValue<float> { Min = -20f, Max = 90f } },
            { MuscleName.RightMiddle2Stretched, new MuscleValue<float> { Min = 0f, Max = 100f } },
            { MuscleName.RightMiddle3Stretched, new MuscleValue<float> { Min = 0f, Max = 80f } },
            { MuscleName.RightRingSpread, new MuscleValue<float> { Min = -10f, Max = 10f } }, // no sources
            { MuscleName.RightRing1Stretched, new MuscleValue<float> { Min = -20f, Max = 90f } },
            { MuscleName.RightRing2Stretched, new MuscleValue<float> { Min = 0f, Max = 100f } },
            { MuscleName.RightRing3Stretched, new MuscleValue<float> { Min = 0f, Max = 80f } },
            { MuscleName.RightLittleSpread, new MuscleValue<float> { Min = -10f, Max = 10f } }, // no sources
            { MuscleName.RightLittle1Stretched, new MuscleValue<float> { Min = -20f, Max = 90f } },
            { MuscleName.RightLittle2Stretched, new MuscleValue<float> { Min = -10f, Max = 100f } },
            { MuscleName.RightLittle3Stretched, new MuscleValue<float> { Min = -10f, Max = 80f } }
        };

        public static float CalcFingerCurlByFloat(MuscleName muscle, float curl)
        {
            var value = (MuscleValue<float>) HumanPoseRangeOfMotionDefinitions[muscle];
            return Mathf.Lerp(value.Min, value.Max, curl);
        }
    }
}