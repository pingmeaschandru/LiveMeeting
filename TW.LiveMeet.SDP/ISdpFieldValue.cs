namespace TW.LiveMeet.SDP
{
    public interface ISdpFieldValue
    {
        string ToString();
        char FieldName { get; }
    }
}