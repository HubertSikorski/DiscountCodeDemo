namespace DiscountCodeDemo.Protocol.Messages;

public interface IProtocolMessage
{
    RequestType Type { get; }
    byte[] ToBytes();
}