using System.Text;

namespace TW.Core.IO
{
    public class StringTokenizer 
    {
        public static readonly string SEMICOLON = ";";
        public static readonly string COLON = ":";
        public static readonly string COMMA = ",";
        public static readonly string SLASH = "/";
        public static readonly string SP = " ";
        public static readonly string EQUALS = "=";
        public static readonly string STAR = "*";
        public static readonly string LF = "\n";
        public static readonly string CR = "\r";
        public static readonly string LESS_THAN = "<";
        public static readonly string GREATER_THAN = ">";
        public static readonly string AT = "@";
        public static readonly string DOT = ".";
        public static readonly string QUESTION = "?";
        public static readonly string POUND = "#";
        public static readonly string AND = "&";
        public static readonly string LPAREN = "(";
        public static readonly string RPAREN = ")";
        public static readonly string DOUBLE_QUOTE = "\"";
        public static readonly string QUOTE = "\'";
        public static readonly string HT = "\t";
        public static readonly string PERCENT = "%";
        public static readonly string CRLF = "\r\n";
        public static readonly string TWOCRLF = "\r\n\r\n";

        private readonly StringBuilder stringBuffer;

        public StringTokenizer()
        {
            stringBuffer = new StringBuilder();
        }

        public StringTokenizer(string strBuffer)
        {
            stringBuffer = new StringBuilder();
            stringBuffer.Append(strBuffer);
        }

        public StringTokenizer(byte[] buffer)
        {
            var strBuffer = Encoding.UTF8.GetString(buffer);
            stringBuffer = new StringBuilder();
            stringBuffer.Append(strBuffer);
        }

        public StringTokenizer(byte[] buffer, int index, int count)
        {
            var strBuffer = Encoding.UTF8.GetString(buffer, index, count);
            stringBuffer = new StringBuilder();
            stringBuffer.Append(strBuffer);
        }

        public void Write(byte[] buffer, int index, int count)
        {
            Write(Encoding.UTF8.GetString(buffer, index, count));
        }

        public void Write(string strBuffer)
        {
            stringBuffer.Append(strBuffer);
        }

        public string ReadToken(string delimiter, bool returnDelims)
        {
            string field;
            return ReadToken(delimiter, returnDelims, out field) ? field : string.Empty;
        }

        public bool ReadToken(string delimiter, bool returnDelims, out string strOutput)
        {
            var str = stringBuffer.ToString();
            strOutput = string.Empty;

            var index = str.IndexOf(delimiter);
            if (index != -1)
            {
                strOutput = returnDelims == false ? str.Substring(0, index) : str.Substring(0, index + delimiter.Length);
                stringBuffer.Remove(0, index + delimiter.Length);
            }

            return (index != -1) ? true : false;
        }

        public string ReadToken(int startIndex, int count)
        {
            var str = stringBuffer.ToString(startIndex, count);
            stringBuffer.Remove(startIndex, count);
            return str;
        }

        public string ReadToken()
        {
            var str = stringBuffer.ToString();
            stringBuffer.Remove(0, stringBuffer.Length);
            return str;
        }

        public bool ReadPeekToken(string delimiter, bool returnDelims, out string strOutput)
        {
            var str = stringBuffer.ToString();
            strOutput = string.Empty;

            var index = str.IndexOf(delimiter);
            if (index != -1)
                strOutput = returnDelims == false ? str.Substring(0, index) : str.Substring(0, index + delimiter.Length);

            return (index != -1) ? true : false;
        }

        public char ReadPeekChar()
        {
            var str = string.Empty;
            if (Length > 0) str = stringBuffer.ToString(0, 1);

            return !string.IsNullOrEmpty(str) ? str.ToCharArray()[0] : '\0';
        }

        public char ReadChar(int k)
        {
            var str = string.Empty;
            if (k < Length) str = stringBuffer.ToString(k, 1);

            if (!string.IsNullOrEmpty(str))
            {
                stringBuffer.Remove(k, 1);
                return str.ToCharArray()[0];
            }
            
            return '\0';
        }

        public int Length
        {
            get { return stringBuffer.Length; }
        }
    }
}