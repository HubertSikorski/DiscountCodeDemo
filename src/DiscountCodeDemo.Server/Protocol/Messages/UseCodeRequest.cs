namespace DiscountCodeDemo.Server.Protocol.Messages;

public class UseCodeRequest : IProtocolMessage
{
    public RequestType Type => RequestType.Use;
    public string Code { get; init; }

    public UseCodeRequest(string code)
    {
        Code = code;
    }
}