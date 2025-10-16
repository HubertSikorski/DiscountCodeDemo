using DiscountCodeDemo.Server.Protocol.Messages;

namespace DiscountCodeDemo.Server.Protocol;

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