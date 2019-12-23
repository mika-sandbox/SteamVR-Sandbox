using System;

using UnityEngine;

namespace SteamVR_Sandbox.Models
{
    [Serializable]
    internal class Threshold
    {
        [SerializeField]
        private ComparisonMode Mode;

        [SerializeField]
        [Range(0, 1)]
        private float Value;

        public bool Compare(float value)
        {
            switch (Mode)
            {
                case ComparisonMode.GreaterThan:
                    return value > Value;

                case ComparisonMode.GreaterThanOrEquals:
                    return value >= Value;

                case ComparisonMode.Equals:
                    return Math.Abs(value - Value) <= 0;

                case ComparisonMode.LessThan:
                    return value < Value;

                case ComparisonMode.LessThanOrEquals:
                    return value <= Value;

                default:
                    throw new ArgumentOutOfRangeException(nameof(Mode));
            }
        }

        private enum ComparisonMode
        {
            GreaterThan,

            GreaterThanOrEquals,

            Equals,

            LessThan,

            LessThanOrEquals
        }
    }
}