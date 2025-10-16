namespace DiscountCodeDemo.Server.Protocol.Messages;

public class GenerateResponse : IProtocolMessage
{
    public RequestType Type => RequestType.Generate;
    public bool Result { get; set; }

    public GenerateResponse(bool result)
    {
        Result = result;
    }
}