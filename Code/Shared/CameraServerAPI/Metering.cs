using System.Runtime.Serialization;

namespace RaspicamServer
{
    [DataContract]
    public enum Metering
    {
        [EnumMember] average,
        [EnumMember] spot,
        [EnumMember] backlit,
        [EnumMember] matrix
    }
}