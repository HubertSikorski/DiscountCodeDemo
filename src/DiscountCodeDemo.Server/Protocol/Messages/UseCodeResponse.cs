namespace DiscountCodeDemo.Server.Protocol.Messages;

public class UseCodeResponse : IProtocolMessage
{
    public RequestType Type => RequestType.Use;
    public bool Result { get; init; }

    public UseCodeResponse(bool result)
    {
        Result = result;
    }
}