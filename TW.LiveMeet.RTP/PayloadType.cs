namespace TW.LiveMeet.RTP
{
    public enum PayloadType : byte
    {
        /// <summary>
        /// PCMU audio codec. Defined in RFC 3551.
        /// </summary>
        PCMU = 0,

        /// <summary>
        /// G721 audio codec. Defined in RFC 3551.
        /// </summary>
        G721 = 2,

        /// <summary>
        /// GSM audio codec. Defined in RFC 3551.
        /// </summary>
        GSM = 3,

        /// <summary>
        /// G723 audio codec.
        /// </summary>
        G723 = 4,

        /// <summary>
        /// DVI4 8khz audio codec.  Defined in RFC 3551.
        /// </summary>
        DVI4_8000 = 5,

        /// <summary>
        /// DVI4 16khz audio codec.  Defined in RFC 3551.
        /// </summary>
        DVI4_16000 = 6,

        /// <summary>
        /// LPC audio codec. Defined in RFC 3551.
        /// </summary>
        LPC = 7,

        /// <summary>
        /// PCMA audio codec. Defined in RFC 3551.
        /// </summary>
        PCMA = 8,

        /// <summary>
        /// G722 audio codec. Defined in RFC 3551.
        /// </summary>
        G722 = 9,

        /// <summary>
        /// L16 1 channel audio codec. Defined in RFC 3551.
        /// </summary>
        L16_1CH = 10,

        /// <summary>
        /// L16 2 channel audio codec. Defined in RFC 3551.
        /// </summary>
        L16_2CH = 11,

        /// <summary>
        /// QCELP audio codec.
        /// </summary>
        QCELP = 12,

        /// <summary>
        /// Comfort Noise.
        /// </summary>
        CN = 13,

        /// <summary>
        /// MPA audio codec. Defined in RFC 3551.
        /// </summary>
        MPA = 14,

        /// <summary>
        /// G728 audio codec. Defined in RFC 3551.
        /// </summary>
        G728 = 15,

        /// <summary>
        /// DVI4 11025hz audio codec.
        /// </summary>
        DVI4_11025 = 16,

        /// <summary>
        /// DVI4 220505hz audio codec.
        /// </summary>
        DVI4_22050 = 17,

        /// <summary>
        /// G729 audio codec.
        /// </summary>
        G729 = 18,

        /// <summary>
        /// CELB video codec. Defined in RFC 2029.
        /// </summary>
        CELB = 25,

        /// <summary>
        /// JPEG video codec. Defined in RFC 2435.
        /// </summary>
        JPEG = 26,

        /// <summary>
        /// NV video codec. Defined in RFC 3551.
        /// </summary>
        NV = 28,

        /// <summary>
        /// H261 video codec. Defined in RFC 2032.
        /// </summary>
        H261 = 31,

        /// <summary>
        /// H261 video codec. Defined in RFC 2250.
        /// </summary>
        MPV = 32,

        /// <summary>
        /// MP2T video codec. Defined in RFC 2250.
        /// </summary>
        MP2T = 33,

        /// <summary>
        /// H263 video codec.
        /// </summary>
        H263 = 34,

        /// <summary>
        /// H264 video codec.
        /// </summary>
        H264 = 97,

        /// <summary>
        /// Unkown codec.
        /// </summary>
        UNKNOWN = 255
    }
}