using System.Runtime.InteropServices;

namespace TW.Core.DirectX
{
    [Guid("54C39221-8380-11d0-B3F0-00AA003761C5"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IAMAudioInputMixer
    {
        [PreserveSig]
        int put_Enable([In, MarshalAs(UnmanagedType.Bool)] bool fEnable);

        [PreserveSig]
        int get_Enable([Out, MarshalAs(UnmanagedType.Bool)] out bool pfEnable);

        [PreserveSig]
        int put_Mono([In, MarshalAs(UnmanagedType.Bool)] bool fMono);

        [PreserveSig]
        int get_Mono([Out, MarshalAs(UnmanagedType.Bool)] out bool pfMono);

        [PreserveSig]
        int put_MixLevel([In] double Level);

        [PreserveSig]
        int get_MixLevel([Out] out double pLevel);

        [PreserveSig]
        int put_Pan([In] double Pan);

        [PreserveSig]
        int get_Pan([Out] out double pPan);

        [PreserveSig]
        int put_Loudness([In, MarshalAs(UnmanagedType.Bool)] bool fLoudness);

        [PreserveSig]
        int get_Loudness([Out, MarshalAs(UnmanagedType.Bool)] out bool pfLoudness);

        [PreserveSig]
        int put_Treble([In] double Treble);

        [PreserveSig]
        int get_Treble([Out] out double pTreble);

        [PreserveSig]
        int get_TrebleRange([Out] out double pRange);

        [PreserveSig]
        int put_Bass([In] double Bass);

        [PreserveSig]
        int get_Bass([Out] out double pBass);

        [PreserveSig]
        int get_BassRange([Out] out double pRange);
    }
}