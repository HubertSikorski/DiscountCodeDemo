namespace DiscountCodeDemo.Protocol.Messages;

public enum RequestType : byte
{
    Generate = 0x01,
    Use = 0x02,
    Error = 0xFF
}