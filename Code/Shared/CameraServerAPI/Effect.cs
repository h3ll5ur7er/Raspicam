using System.Runtime.Serialization;

namespace RaspicamServer
{
    [DataContract]
    public enum Effect
    {
        [EnumMember] none,
        [EnumMember] negative,
        [EnumMember] solarise,
        [EnumMember] sketch,
        [EnumMember] denoise,
        [EnumMember] emboss,
        [EnumMember] oilpaint,
        [EnumMember] hatch,
        [EnumMember] gpen,
        [EnumMember] pastel,
        [EnumMember] watercolour,
        [EnumMember] film,
        [EnumMember] blur,
        [EnumMember] saturation,
        [EnumMember] colourswap,
        [EnumMember] washedout,
        [EnumMember] posterise,
        [EnumMember] colourpoint,
        [EnumMember] colourbalance,
        [EnumMember] cartoon
    }
}