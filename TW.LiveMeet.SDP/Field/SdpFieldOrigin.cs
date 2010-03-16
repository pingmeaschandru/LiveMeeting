using System.Text;

namespace TW.LiveMeet.SDP.Field
{
    ///<summary>
    /// The "o=" field gives the originator of the session (his/her username and the address of 
    /// the user's host) plus a session identifier and version number. 
    /// 
    ///         origin-field =  %x6f "=" 
    ///                         username SP 
    ///                         sess-id SP 
    ///                         sess-version SP 
    ///                         nettype SP 
    ///                         addrtype SP 
    ///                         unicast-address 
    ///                         CRLF 

    ///         username =  non-ws-string 
    ///         <username> is the user's login on the originating host, or it is "-" if the
    ///                    originating host does not support the concept of user IDs.
    ///                    The <username> MUST NOT contain spaces. 

    ///         sess-id = 1*DIGIT 
    ///         <sess-id> is a numeric string such that the tuple of <username>, <sess-id>, <nettype>, 
    ///                   <addrtype>, and <unicast-address> forms a globally unique identifier for 
    ///                   the session. The method of <sess-id> allocation is up to the creating tool, 
    ///                   but it has been suggested that a Network Timing Protocol (NTP) format timestamp 
    ///                   be used to ensure uniqueness [RFC1305]. 

    ///         sess-version = 1*DIGIT 
    ///         <sess-version> is a version number for this session description. Its
    ///                        usage is up to the creating tool, so long as <sess-version> is
    ///                        increased when a modification is made to the session data. Again,
    ///                        it is RECOMMENDED that an NTP format timestamp is used. 

    ///         nettype = token 
    ///         <nettype> is a text string giving the type of network. Initially
    ///                   "IN" is defined to have the meaning "Internet", but other values
    ///                   MAY be registered in the future. 

    ///         addrtype = token 
    ///         <addrtype> is a text string giving the type of the address that
    ///                    follows. Initially "IP4" and "IP6" are defined, but other values
    ///                    MAY be registered in the future. 

    ///         unicast-address Address of the machine from which the session was created.
    ///         The FQDN (fully-qualified domain name) is the form that SHOULD
    ///         be given unless this is unavailable, in which case the globally
    ///         unique address MAY be substituted. 

    /// </summary>
 
 
    public class SdpFieldOrigin : ISdpFieldValue
    {
        public SdpFieldOrigin(string fieldValue)
        {
            Parse(fieldValue);
        }

        public SdpFieldOrigin()
        {
        }

        private void Parse(string fieldValue)
        {
            var originValues = fieldValue.Split(new[] { ' ' });
            if (originValues.Length != 6)
                throw new InvalidSdpStringFormatException();

            try
            {
                UserName = originValues[0];
                SessId = ulong.Parse(originValues[1]);
                SessVersion = ulong.Parse(originValues[2]);
                NetType = originValues[3];
                AddrType = originValues[4];
                UnicastAddress = originValues[5];
            }
            catch
            {
                throw new InvalidSdpStringFormatException();
            }
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(FieldName);
            stringBuilder.Append('=');
            stringBuilder.Append(UserName);
            stringBuilder.Append(' ');
            stringBuilder.Append(SessId);
            stringBuilder.Append(' ');
            stringBuilder.Append(SessVersion);
            stringBuilder.Append(' ');
            stringBuilder.Append(NetType);
            stringBuilder.Append(' ');
            stringBuilder.Append(AddrType);
            stringBuilder.Append(' ');
            stringBuilder.Append(UnicastAddress);

            return stringBuilder.ToString();
        }

        public string UserName { get; set; }
        public ulong SessId { get; set; }
        public ulong SessVersion { get; set; }
        public string NetType { get; set; }
        public string AddrType { get; set; }
        public string UnicastAddress { get; set; }

        public char FieldName
        {
            get { return 'o'; }
        }
    }
}