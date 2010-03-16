using System.Text;

namespace TW.Core.Helper
{
    public static class StringHelper
    {
        public static byte[] GetAsciiBytes(string unicodeString)
        {
            var unicodeBytes = Encoding.Unicode.GetBytes(unicodeString);
            return Encoding.Convert(Encoding.Unicode, Encoding.ASCII, unicodeBytes);
        }

        public static string GetStringFromAscii(byte[] asciiBytes, int startIndex, int count)
        {
            var asciiChars = new char[Encoding.ASCII.GetCharCount(asciiBytes, startIndex, count)];
            Encoding.ASCII.GetChars(asciiBytes, startIndex, count, asciiChars, 0);
            return new string(asciiChars);
        }
    }
}
