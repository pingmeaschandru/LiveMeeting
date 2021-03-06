using System;
using System.Collections.Generic;
using TW.Core.IO;
using TW.LiveMeet.SDP.Field;

namespace TW.LiveMeet.SDP
{
    /// <summary>
    /// An SDP session description consists of a number of lines of text of the 
    /// form: <type>=<value> where <type> MUST be exactly one case-significant character 
    /// and <value> is structured text whose format depends on <type>. Each element of 
    /// the following rule is a hyperlink within this page, and for each element, 
    /// the '<type>=' is shown in comment, with optional items marked with a '*'. 
    /// 
    /// Grammer for session-description :
    ///       proto-version       ; v= 
    ///       origin-field        ; o= 
    ///       session-name-field  ; s= 
    ///       information-field   ; i=* 
    ///       uri-field           ; u=* 
    ///       email-fields        ; e=* 
    ///       phone-fields        ; p=* 
    ///       connection-field    ; c=* 
    ///       bandwidth-fields    ; b=* (zero or more) 
    ///       time-fields         ; t= r=* (one or more, with zero or more 'r=') 
    ///       key-field           ; k=* 
    ///       attribute-fields    ; a=* (zero or more) 
    ///       media-descriptions  ; m= i=* c=* b=* k=* a=* (zero or more)
    /// 
    /// </summary>

    public class SdpMessage 
    {
        public SdpMessage()
        {
            BandWidth = new List<SdpFieldBandwidth>();
            Times = new List<SdpTimings>();
            Attributes = new List<SdpFieldAttribute>();
            Medias = new List<SdpMediaDescriptions>();
        }

        //public SdpMessage(string message)
        //{
        //    BandWidth = new List<SdpFieldBandwidth>();
        //    Times = new List<SdpTimings>();
        //    Attributes = new List<SdpFieldAttribute>();
        //    Medias = new List<SdpMediaDescriptions>();

        //    Parse(message);
        //}

        //private void Parse(string message)
        //{
        //    var strTok = new StringTokenizer(message);

        //    while (strTok.Length > 0)
        //    {
        //        string field;
        //        var type = strTok.ReadPeekChar();
        //        strTok.ReadToken(StringTokenizer.CRLF, false, out field);
        //        var strFieldValue = SdpConstants.GetValue(field);

        //        if (type == 'v') Version = new SdpFieldVersion(strFieldValue);
        //        else if (type == 'o') Origin = new SdpFieldOrigin(strFieldValue);
        //        else if (type == 's') SessionName = new SdpFieldSessionName(strFieldValue);
        //        else if (type == 'i') SessionInformation = new SdpFieldSessionInformation(strFieldValue);
        //        else if (type == 'u') Uri = new SdpFieldUri(strFieldValue);
        //        else if (type == 'e') Email = new SdpFieldEmailAddress(strFieldValue);
        //        else if (type == 'p') Phone = new SdpFieldPhoneNumber(strFieldValue);
        //        else if (type == 'c') ConnectionData = new SdpFieldConnection(strFieldValue);
        //        else if (type == 'b') BandWidth.Add(new SdpFieldBandwidth(strFieldValue));
        //        else if (type == 't')
        //        {
        //            var time = new SdpTimings();
        //            time.AddFieldValue(field);
        //            while (strTok.Length > 0)
        //            {
        //                type = strTok.ReadPeekChar();
        //                strTok.ReadToken(StringTokenizer.CRLF, false, out field);
        //                if (type == 't') time.AddFieldValue(field);
        //                else if (type == 'z') time.AddFieldValue(field);
        //                else break;
        //            }
        //            Times.Add(time);
        //        }
        //        else if (type == 'k') EncryptionKey = new SdpFieldEncryptionKey(strFieldValue);
        //        else if (type == 'a') Attributes.Add(new SdpFieldAttribute(strFieldValue));
        //        else if (type == 'm')
        //        {
        //            var media = new SdpMediaDescriptions();
        //            media.AddFieldValue(field);
        //            while (strTok.Length > 0)
        //            {
        //                type = strTok.ReadPeekChar();
        //                strTok.ReadToken(StringTokenizer.CRLF, false, out field);
        //                if (type == 'i') media.AddFieldValue(field);
        //                else if (type == 'c') media.AddFieldValue(field);
        //                else if (type == 'b') media.AddFieldValue(field);
        //                else if (type == 'k') media.AddFieldValue(field);
        //                else if (type == 'a') media.AddFieldValue(field);
        //                else break;
        //            }
        //            Medias.Add(media);
        //        }
        //        else throw new Exception();
        //    }
        //}

        public SdpFieldVersion Version { get; set; }
        public SdpFieldOrigin Origin { get; set; }
        public SdpFieldSessionName SessionName { get; set; }
        public SdpFieldSessionInformation SessionInformation { get; set; }
        public SdpFieldUri Uri { get; set; }
        public SdpFieldEmailAddress Email { get; set; }
        public SdpFieldPhoneNumber Phone { get; set; }
        public SdpFieldConnection ConnectionData { get; set; }
        public List<SdpFieldBandwidth> BandWidth { get; private set; }
        public List<SdpTimings> Times { get; private set; }
        public SdpFieldEncryptionKey EncryptionKey { get; set; }
        public List<SdpFieldAttribute> Attributes { get; private set; }
        public List<SdpMediaDescriptions> Medias { get; private set; }
    }
}