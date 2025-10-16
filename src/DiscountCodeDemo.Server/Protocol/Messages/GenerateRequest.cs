namespace DiscountCodeDemo.Server.Protocol.Messages;

public class GenerateRequest : IProtocolMessage
{
    public RequestType Type => RequestType.Generate;
    public ushort Count { get; init; }
    public byte Length { get; init; }

    public GenerateRequest(ushort count, byte length)
    {
        Count = count;
        Length = length;
    }
}