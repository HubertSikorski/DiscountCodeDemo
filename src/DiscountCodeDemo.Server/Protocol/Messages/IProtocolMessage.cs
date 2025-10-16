namespace DiscountCodeDemo.Server.Protocol.Messages;

public interface IProtocolMessage
{
    RequestType Type { get; }
    byte[] ToBytes();
}