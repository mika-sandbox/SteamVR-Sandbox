using System.Runtime.Serialization;

namespace SteamVR_Sandbox.Enums
{
    public enum AnimatorState
    {
        [EnumMember(Value = "FingerPoint")]
        FingerPoint,

        [EnumMember(Value = "Fist")]
        Fist,

        [EnumMember(Value = "HandGun")]
        HandGun,

        [EnumMember(Value = "HandOpen")]
        HandOpen,

        [EnumMember(Value = "RocknRoll")]
        RocknRoll,

        [EnumMember(Value = "ThumbsUp")]
        ThumbsUp,

        [EnumMember(Value = "Victory")]
        Victory
    }
}