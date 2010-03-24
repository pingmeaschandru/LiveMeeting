using System;
using System.Runtime.InteropServices;

namespace TW.Core.DirectX
{
    [ComVisible(false)]
    public class Clsid
    {
        /// <summary> CLSID_SystemDeviceEnum for ICreateDevEnum </summary>
        public static readonly Guid SystemDeviceEnum = new Guid(0x62BE5D10, 0x60EB, 0x11d0, 0xBD, 0x3B, 0x00, 0xA0, 0xC9, 0x11, 0xCE, 0x86);

        /// <summary> CLSID_FilterGraph, filter Graph </summary>
        public static readonly Guid FilterGraph = new Guid(0xe436ebb3, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

        /// <summary> CLSID_CaptureGraphBuilder2, new Capture graph building </summary>
        public static readonly Guid CaptureGraphBuilder2 = new Guid(0xBF87B6E1, 0x8C27, 0x11d0, 0xB3, 0xF0, 0x0, 0xAA, 0x00, 0x37, 0x61, 0xC5);

        /// <summary> CLSID_SampleGrabber, Sample Grabber filter </summary>
        public static readonly Guid SampleGrabber = new Guid(0xC1F400A0, 0x3F08, 0x11D3, 0x9F, 0x0B, 0x00, 0x60, 0x08, 0x03, 0x9E, 0x37);

        /// <summary> CLSID_DvdGraphBuilder, DVD graph builder </summary>
        public static readonly Guid DvdGraphBuilder = new Guid(0xFCC152B7, 0xF372, 0x11d0, 0x8E, 0x00, 0x00, 0xC0, 0x4F, 0xD7, 0xC0, 0x8B);

        /// <summary> CLSID_StreamBufferSink, stream Buffer sink </summary>
        public static readonly Guid StreamBufferSink = new Guid("2db47ae5-cf39-43c2-b4d6-0cd8d90946f4");

        /// <summary> CLSID_StreamBufferSource, stream Buffer sink </summary>
        public static readonly Guid StreamBufferSource = new Guid("c9f5fe02-f851-4eb5-99ee-ad602af1e619");

        /// <summary> CLSID_VideoMixingRenderer, video mixing renderer 7 </summary>
        public static readonly Guid VideoMixingRenderer = new Guid(0xB87BEB7B, 0x8D29, 0x423f, 0xAE, 0x4D, 0x65, 0x82, 0xC1, 0x01, 0x75, 0xAC);

        /// <summary> CLSID_VideoMixingRenderer9, video mixing renderer 9 </summary>
        public static readonly Guid VideoMixingRenderer9 = new Guid(0x51b4abf3, 0x748f, 0x4e3b, 0xa2, 0x76, 0xc8, 0x28, 0x33, 0x0e, 0x92, 0x6a);

        /// <summary> CLSID_VideoRendererDefault, default vmr renderer </summary>
        public static readonly Guid VideoRendererDefault = new Guid(0x6BC1CFFA, 0x8FC1, 0x4261, 0xAC, 0x22, 0xCF, 0xB4, 0xCC, 0x38, 0xDB, 0x50);

        /// <summary> CLSID_AviSplitter, split an AVI stream into separate video and audio streams </summary>
        public static readonly Guid AviSplitter = new Guid(0x1b544c20, 0xfd0b, 0x11ce, 0x8c, 0x63, 0x0, 0xaa, 0x00, 0x44, 0xb5, 0x1e);

        /// <summary> CLSID_SmartTee, create a preview stream when device only provides a capture stream. </summary>
        public static readonly Guid SmartTee = new Guid(0xcc58e280, 0x8aa1, 0x11d1, 0xb3, 0xf1, 0x0, 0xaa, 0x0, 0x37, 0x61, 0xc5);
    }
}