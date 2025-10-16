namespace DiscountCodeDemo.Server.Protocol.Messages;

public enum RequestType : byte
{
    Generate = 0x01,
    Use = 0x02,
    Status = 0x03,
    Error = 0xFF
}