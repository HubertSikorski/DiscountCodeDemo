namespace DiscountCodeDemo.Protocol.Messages;

public class UseCodeResponse : IProtocolMessage
{
    public RequestType Type => RequestType.Use;
    public byte Result { get; set; }

    public UseCodeResponse() {}

    public UseCodeResponse(byte result)
    {
        Result = result;
    }

    public UseCodeResponse(byte[] payload)
    {
        if (payload.Length != 1)
            throw new ArgumentException("Invalid payload length");

        Result = payload[0];
    }

    public byte[] ToBytes()
    {
        return new[] { Result };
    }
}