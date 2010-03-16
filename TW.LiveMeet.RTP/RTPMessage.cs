using System;
using System.Net;

namespace TW.LiveMeet.RTP
{
    public class RTPMessage 
    {
        private enum FieldType : byte
        {
            Version = 0,
            Padding = 1,
            Extension = 2,
            CC = 3,
            Marker = 4,
            PayloadType = 5
        }

        private readonly byte[] buffer;

        public RTPMessage(byte[] buffer)
        {
            this.buffer = buffer;
            if (!CheckValidRTP())
                throw new RTPInvalidMessageException();
        }

        private bool CheckValidRTP()
        {
            // TODO : Add some more check to get correct RTP Packet

            /*############ Partial RTP Packet Check ###########*/
            if (Version != 0x80)
                return false;

            if (CheckPayloadType(PayloadType) == false)
                return false;

            if (SSRC == 0)
                return false;
            /*############ Partial RTP Packet Check ###########*/

            return true;
        }

        private bool CheckPayloadType(PayloadType type)
        {
            switch (type)
            {
                case PayloadType.CN: return true;
                case PayloadType.PCMA: return true;
                case PayloadType.PCMU: return true;
                case PayloadType.G721: return true;
                case PayloadType.G723: return true;
                case PayloadType.G729: return true;
                case PayloadType.H263: return true;
                case PayloadType.H264: return true;
            }

            return false;
        }

        private byte GetValue(FieldType fieldType)
        {
            byte field = 0;

            switch (fieldType)
            {
                case FieldType.Version:
                    field = Convert.ToByte(VerPadExtCC & RTPMessageHeaderField.MASK_VER);
                    break;
                case FieldType.Padding:
                    field = Convert.ToByte(VerPadExtCC & RTPMessageHeaderField.MASK_PAD);
                    break;
                case FieldType.Extension:
                    field = Convert.ToByte(VerPadExtCC & RTPMessageHeaderField.MASK_EXT);
                    break;
                case FieldType.CC:
                    field = Convert.ToByte(VerPadExtCC & RTPMessageHeaderField.MASK_CC);
                    break;
                case FieldType.Marker:
                    field = Convert.ToByte(PayloadTypeMarker & RTPMessageHeaderField.MASK_MARKER);
                    break;
                case FieldType.PayloadType:
                    field = Convert.ToByte(PayloadTypeMarker & RTPMessageHeaderField.MASK_PAYLOAD);
                    break;
            }

            return field;
        }

        private byte VerPadExtCC
        {
            get
            {
                return Convert.ToByte(buffer[RTPMessageHeaderField.RTP_VERSION_PADDING_EXTENSION_CC_POS]);
            }
        }
        private byte PayloadTypeMarker
        {
            get
            {
                return Convert.ToByte(buffer[RTPMessageHeaderField.RTP_PAYLOAD_MARKER_POS]);
            }
        }

        public byte Version { get { return GetValue(FieldType.Version); } }
        public bool Padding { get { return (GetValue(FieldType.Padding) == RTPMessageHeaderField.MASK_PAD); } }
        public bool Extension { get { return (GetValue(FieldType.Extension) == RTPMessageHeaderField.MASK_EXT); } }
        public uint CSRCCount { get { return GetValue(FieldType.CC); } }
        public bool Marker { get { return (GetValue(FieldType.Marker) == RTPMessageHeaderField.MASK_MARKER); } }
        public PayloadType PayloadType { get { return (PayloadType)(GetValue(FieldType.PayloadType)); } }
        public ushort SequenceNumber
        {
            get
            {
                return (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, RTPMessageHeaderField.RTP_SEQUENCE_NUM_POS));
            }
        }
        public uint TimeStamp
        {
            get
            {
                return (uint)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buffer, RTPMessageHeaderField.RTP_TIMESTAMP_POS));
            }
        }
        public uint SSRC
        {
            get
            {
                return (uint)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buffer, RTPMessageHeaderField.RTP_SSRC_POS));
            }
        }
        public uint[] CSRC
        {
            get
            {
                if (CSRCCount > 0)
                {
                    var csrc = new uint[CSRCCount];
                    var startIndex = RTPMessageHeaderField.RTP_CSRC_POS;
                    for (var i = 0; i < CSRCCount; i++)
                    {
                        csrc[i] = (uint)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buffer, startIndex));
                        startIndex += RTPMessageHeaderField.RTP_TIMESTAMP_LEN;
                    }
                    return csrc;
                }

                return null;
            }
        }
        public byte[] Buffer
        {
            get
            {
                
                var startIndex = (int) (RTPMessageHeaderField.RTP_HEADER_LEN + (CSRCCount*RTPMessageHeaderField.RTP_CSRC_LEN));
                var len = buffer.Length - startIndex;
                var rtpPayload = new byte[len];
                Array.Copy(buffer, startIndex, rtpPayload, 0, rtpPayload.Length);

                return rtpPayload;
            }
        }
    }
}