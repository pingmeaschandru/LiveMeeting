namespace TW.LiveMeet.RDAP
{
    public enum RdapMessageType
    {
        DesktopWindowImageFrameMessage = 0x01,
        MouseClickEventMessage = 0x02,
        MouseDragEventMesssage = 0x03,
        KeyboardEventMessage = 0x04,
        VideoFrameMessage = 0x05,
        AudioFrameMessage = 0x06,
        
        UnknownRdaMessage = -1
    }
}