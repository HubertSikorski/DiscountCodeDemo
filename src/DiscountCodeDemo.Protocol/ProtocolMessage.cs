using DiscountCodeDemo.Protocol.Messages;

namespace DiscountCodeDemo.Protocol;

public class ProtocolMessage
{
    public RequestType Command { get; }
    public byte[] Payload { get; }

    public ProtocolMessage(RequestType command, byte[] payload)
    {
        Command = command;
        Payload = payload;
    }
}