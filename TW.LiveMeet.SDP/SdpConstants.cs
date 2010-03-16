namespace TW.LiveMeet.SDP
{
    public static class SdpConstants
    {
        public static string GetValue(string fieldValue)
        {
            var typeValue = fieldValue.Split(new[] { '=' }, 2);
            return typeValue[1].Trim();
        }
    }
}