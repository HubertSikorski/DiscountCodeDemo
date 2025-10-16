using DiscountCodeDemo.Protocol.Messages;

public class ErrorResponse : IProtocolMessage
{
    public ErrorType Error { get; set; }

    public RequestType Type => RequestType.Error;

    public static ErrorResponse FromBytes(byte[] payload)
    {
        if (payload.Length != 1)
            throw new ArgumentException("Invalid payload length for ErrorResponse");

        return new ErrorResponse { Error = (ErrorType)payload[0] };
    }

    public byte[] ToBytes()
    {
        return new[] { (byte)Error };
    }
}