using System.Runtime.Serialization;

namespace RaspicamServer
{
    [DataContract]
    /// <summary>
    /// Dynamic Range Compression
    /// </summary>
    public enum DRC
    {
        [EnumMember] off,
        [EnumMember] low,
        [EnumMember] med,
        [EnumMember] high
    }
}