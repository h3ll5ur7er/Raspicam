using System.Runtime.Serialization;

namespace RaspicamServer
{
    [DataContract]
    public enum SensorMode
    {
        [EnumMember] Auto = 0,
        [EnumMember] m1920x1080at30fps,
        [EnumMember] m2592x1944at15fps,
        [EnumMember] m2592x1944at1fps,
        [EnumMember] m1296x972at42fps,
        [EnumMember] m1296x730at49fps,
        [EnumMember] m640x480at60fps,
        [EnumMember] m640x480at90fps
    }
}