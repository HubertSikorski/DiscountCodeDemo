using System.Text;
using DiscountCodeDemo.Server.Protocol.Messages;

public class UseCodeRequest : IProtocolMessage
{
    public string Code { get; set; } = string.Empty;

    public RequestType Type => RequestType.Use;

    public static UseCodeRequest FromBytes(byte[] payload)
    {
        if (payload.Length != 8)
            throw new ArgumentException("Invalid payload length for UseCodeRequest");

        string code = Encoding.ASCII.GetString(payload);
        return new UseCodeRequest { Code = code };
    }

    public byte[] ToBytes()
    {
        return Encoding.ASCII.GetBytes(Code);
    }
}