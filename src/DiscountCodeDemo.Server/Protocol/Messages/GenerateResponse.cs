using DiscountCodeDemo.Server.Protocol.Messages;

public class GenerateResponse : IProtocolMessage
{
    public byte Result { get; set; }

    public RequestType Type => RequestType.Generate;

    public static GenerateResponse FromBytes(byte[] payload)
    {
        if (payload.Length != 1)
            throw new ArgumentException("Invalid payload length for GenerateResponse");

        return new GenerateResponse { Result = payload[0] };
    }

    public byte[] ToBytes()
    {
        return new[] { Result };
    }
}