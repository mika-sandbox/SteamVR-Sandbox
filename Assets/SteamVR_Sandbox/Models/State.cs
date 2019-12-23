using System.Runtime.Serialization;

namespace SteamVR_Sandbox.Models
{
    public enum State
    {
        [EnumMember(Value = "FINGER_EMOTE_FINGERPOINT")]
        FingerEmoteFingerPoint,

        [EnumMember(Value = "FINGER_EMOTE_FIST")]
        FingerEmoteFist,

        [EnumMember(Value = "FINGER_EMOTE_HANDGUN")]
        FingerEmoteHandGun,

        [EnumMember(Value = "FINGER_EMOTE_HANDOPEN")]
        FingerEmoteHandOpen,

        [EnumMember(Value = "FINGER_EMOTE_ROCKNROLL")]
        FingerEmoteRocknRoll,

        [EnumMember(Value = "FINGER_EMOTE_THUMBSUP")]
        FingerEmoteThumbsUp,

        [EnumMember(Value = "FINGER_EMOTE_VICTORY")]
        FingerEmoteVictory
    }
}