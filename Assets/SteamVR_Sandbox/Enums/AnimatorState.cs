using System.Runtime.Serialization;

namespace SteamVR_Sandbox.Enums
{
    public enum AnimatorState
    {
        [EnumMember(Value = "Point")]
        Point,

        [EnumMember(Value = "Fist")]
        Fist,

        [EnumMember(Value = "Gun")]
        Gun,

        [EnumMember(Value = "Palm")]
        Palm,

        [EnumMember(Value = "RockNRoll")]
        RockNRoll,

        [EnumMember(Value = "ThumbsUp")]
        ThumbsUp,

        [EnumMember(Value = "Peace")]
        Peace
    }
}