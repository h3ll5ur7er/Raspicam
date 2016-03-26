using System.Runtime.Serialization;

namespace RaspicamServer
{
    [DataContract]
    public enum AWB
    {
        [EnumMember] off,
        [EnumMember] auto,
        [EnumMember] sun,
        [EnumMember] cloud,
        [EnumMember] shade,
        [EnumMember] tungsten,
        [EnumMember] fluorescent,
        [EnumMember] incandescent,
        [EnumMember] flash,
        [EnumMember] horizon
    }
}