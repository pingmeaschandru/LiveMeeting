namespace TW.LiveMeet.RTP
{
    internal static class RTPMessageHeaderField
    {
        internal readonly static byte MASK_VER = 0xC0;
        internal readonly static byte MASK_PAD = 0x20;
        internal readonly static byte MASK_EXT = 0x10;
        internal readonly static byte MASK_CC = 0x0F;
        internal readonly static byte MASK_MARKER = 0x80;
        internal readonly static byte MASK_PAYLOAD = 0x7F;

        internal readonly static int RTP_HEADER_LEN = 12;

        internal readonly static int RTP_VERSION_PADDING_EXTENSION_CC_LEN = 1;
        internal readonly static int RTP_PAYLOAD_MARKER_LEN = 1;
        internal readonly static int RTP_SEQUENCE_NUM_LEN = 2;
        internal readonly static int RTP_TIMESTAMP_LEN = 4;
        internal readonly static int RTP_SSRC_LEN = 4;
        internal readonly static int RTP_CSRC_LEN = 4;

        internal readonly static int RTP_VERSION_PADDING_EXTENSION_CC_POS = 0;
        internal readonly static int RTP_PAYLOAD_MARKER_POS;
        internal readonly static int RTP_SEQUENCE_NUM_POS;
        internal readonly static int RTP_TIMESTAMP_POS;
        internal readonly static int RTP_SSRC_POS;
        internal readonly static int RTP_CSRC_POS;

        static RTPMessageHeaderField()
        {
            RTP_PAYLOAD_MARKER_POS = RTP_VERSION_PADDING_EXTENSION_CC_POS + RTP_VERSION_PADDING_EXTENSION_CC_LEN;
            RTP_SEQUENCE_NUM_POS = RTP_PAYLOAD_MARKER_POS + RTP_PAYLOAD_MARKER_LEN;
            RTP_TIMESTAMP_POS = RTP_SEQUENCE_NUM_POS + RTP_SEQUENCE_NUM_LEN;
            RTP_SSRC_POS = RTP_TIMESTAMP_POS + RTP_TIMESTAMP_LEN;
            RTP_CSRC_POS = RTP_SSRC_POS + RTP_SSRC_LEN;
        }
    }
}