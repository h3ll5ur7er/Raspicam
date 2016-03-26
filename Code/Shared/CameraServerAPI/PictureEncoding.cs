using System.Runtime.Serialization;

namespace RaspicamServer
{
    [DataContract]
    public enum PictureEncoding
    {
        [EnumMember] jpg,
        [EnumMember] bmp,
        [EnumMember] gif,
        [EnumMember] png
    }
}