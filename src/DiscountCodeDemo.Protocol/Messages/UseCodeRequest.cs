using System.Text;

namespace DiscountCodeDemo.Protocol.Messages;

public class UseCodeRequest : IProtocolMessage
{
    public RequestType Type => RequestType.Use;
    public string Code { get; set; } = "";

    public UseCodeRequest() {}

    public UseCodeRequest(string code)
    {
        Code = code;
    }

    public byte[] ToBytes()
    {
        return Encoding.ASCII.GetBytes(Code);
    }

    public static UseCodeRequest FromBytes(byte[] payload)
    {
        return new UseCodeRequest(Encoding.ASCII.GetString(payload));
    }
}