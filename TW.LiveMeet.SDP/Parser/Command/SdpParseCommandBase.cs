using TW.Core.IO;

namespace TW.LiveMeet.SDP.Parser.Command
{
    internal abstract class SdpParseCommandBase
    {
        protected string GetFieldValue(StringTokenizer reader)
        {
            var field = GetFieldValuePair(reader);
            return SdpConstants.GetValue(field);
        }

        protected string GetFieldValuePair(StringTokenizer reader)
        {
            string field;

            if (!reader.ReadToken(StringTokenizer.CRLF, false, out field))
                throw new CanNotFindCRLFException();

            return field;
        }
    }
}