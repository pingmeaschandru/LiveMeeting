namespace TW.LiveMeet.RDAP
{
    public enum RdapMessageType
    {
        DesktopWindowInfoMessage = 0x01,
        MouseClickEventMessage = 0x02,
        MouseDragEventMesssage = 0x03,
        KeyboardEventMessage = 0x04,
        
        UnknownRdaMessage = -1
    }
}