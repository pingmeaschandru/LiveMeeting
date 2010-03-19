using System;

namespace TW.Core.DirectX
{
    public class PropSetID
    {
        /// <summary> AMPROPSETID_Pin</summary>
        public static readonly Guid Pin = new Guid(0x9b00f101, 0x1567, 0x11d1, 0xb3, 0xf1, 0x00, 0xaa, 0x00, 0x37, 0x61, 0xc5);

        /// <summary> PROPSETID_VIDCAP_DROPPEDFRAMES </summary>
        public static readonly Guid DroppedFrames = new Guid(0xC6E13344, 0x30AC, 0x11D0, 0xA1, 0x8C, 0x00, 0xA0, 0xC9, 0x11, 0x89, 0x56);

        /// <summary> STATIC_ENCAPIPARAM_BITRATE </summary>
        public static readonly Guid ENCAPIPARAM_BitRate = new Guid(0x49cc4c43, 0xca83, 0x4ad4, 0xa9, 0xaf, 0xf3, 0x69, 0x6a, 0xf6, 0x66, 0xdf);

        /// <summary> STATIC_ENCAPIPARAM_PEAK_BITRATE </summary>
        public static readonly Guid ENCAPIPARAM_PeakBitRate = new Guid(0x703f16a9, 0x3d48, 0x44a1, 0xb0, 0x77, 0x01, 0x8d, 0xff, 0x91, 0x5d, 0x19);

        /// <summary> STATIC_ENCAPIPARAM_BITRATE_MODE </summary>
        public static readonly Guid ENCAPIPARAM_BitRateMode = new Guid(0xee5fb25c, 0xc713, 0x40d1, 0x9d, 0x58, 0xc0, 0xd7, 0x24, 0x1e, 0x25, 0x0f);

        /// <summary> STATIC_CODECAPI_CHANGELISTS </summary>
        public static readonly Guid CODECAPI_ChangeLists = new Guid(0x62b12acf, 0xf6b0, 0x47d9, 0x94, 0x56, 0x96, 0xf2, 0x2c, 0x4e, 0x0b, 0x9d);

        /// <summary> STATIC_CODECAPI_VIDEO_ENCODER </summary>
        public static readonly Guid CODECAPI_VideoEncoder = new Guid(0x7112e8e1, 0x3d03, 0x47ef, 0x8e, 0x60, 0x03, 0xf1, 0xcf, 0x53, 0x73, 0x01);

        /// <summary> STATIC_CODECAPI_AUDIO_ENCODER </summary>
        public static readonly Guid CODECAPI_AudioEncoder = new Guid(0xb9d19a3e, 0xf897, 0x429c, 0xbc, 0x46, 0x81, 0x38, 0xb7, 0x27, 0x2b, 0x2d);

        /// <summary> STATIC_CODECAPI_SETALLDEFAULTS </summary>
        public static readonly Guid CODECAPI_SetAllDefaults = new Guid(0x6c5e6a7c, 0xacf8, 0x4f55, 0xa9, 0x99, 0x1a, 0x62, 0x81, 0x09, 0x05, 0x1b);

        /// <summary> STATIC_CODECAPI_ALLSETTINGS </summary>
        public static readonly Guid CODECAPI_AllSettings = new Guid(0x6a577e92, 0x83e1, 0x4113, 0xad, 0xc2, 0x4f, 0xce, 0xc3, 0x2f, 0x83, 0xa1);

        /// <summary> STATIC_CODECAPI_SUPPORTSEVENTS </summary>
        public static readonly Guid CODECAPI_SupportsEvents = new Guid(0x0581af97, 0x7693, 0x4dbd, 0x9d, 0xca, 0x3f, 0x9e, 0xbd, 0x65, 0x85, 0xa1);

        /// <summary> STATIC_CODECAPI_CURRENTCHANGELIST </summary>
        public static readonly Guid CODECAPI_CurrentChangeList = new Guid(0x1cb14e83, 0x7d72, 0x4657, 0x83, 0xfd, 0x47, 0xa2, 0xc5, 0xb9, 0xd1, 0x3d);

    }
}