using System.Runtime.Serialization;

namespace RaspicamServer
{
    [DataContract]
    public enum Exposure
    {
        [EnumMember] off,
        [EnumMember] auto,
        [EnumMember] night,
        [EnumMember] nightpreview,
        [EnumMember] backlight,
        [EnumMember] spotlight,
        [EnumMember] sports,
        [EnumMember] snow,
        [EnumMember] beach,
        [EnumMember] verylong,
        [EnumMember] fixedfps,
        [EnumMember] antishake,
        [EnumMember] firework
    }
}