using DiscountCodeDemo.Server.Protocol.Messages;

public class UseCodeResponse : IProtocolMessage
{
    public byte Result { get; set; }

    public RequestType Type => RequestType.Use;

    public static UseCodeResponse FromBytes(byte[] payload)
    {
        if (payload.Length != 1)
            throw new ArgumentException("Invalid payload length for UseCodeResponse");

        return new UseCodeResponse { Result = payload[0] };
    }

    public byte[] ToBytes()
    {
        return new[] { Result };
    }
}