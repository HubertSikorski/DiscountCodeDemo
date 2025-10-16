namespace DiscountCodeDemo.Protocol.Messages;

public class GenerateResponse : IProtocolMessage
{
    public RequestType Type => RequestType.Generate;
    public byte Result { get; set; }

    public GenerateResponse() {}

    public GenerateResponse(byte result)
    {
        Result = result;
    }

    public GenerateResponse(byte[] payload)
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