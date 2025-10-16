using DiscountCodeDemo.Server.Protocol.Messages;

public class GenerateRequest : IProtocolMessage
{
    public ushort Count { get; set; }
    public byte Length { get; set; }

    public RequestType Type => RequestType.Generate;

    public static GenerateRequest FromBytes(byte[] payload)
    {
        if (payload.Length != 3)
            throw new ArgumentException("Invalid payload length for GenerateRequest");

        ushort count = BitConverter.ToUInt16(payload, 0);
        byte length = payload[2];

        return new GenerateRequest { Count = count, Length = length };
    }

    public byte[] ToBytes()
    {
        var bytes = new byte[3];
        BitConverter.GetBytes(Count).CopyTo(bytes, 0);
        bytes[2] = Length;
        return bytes;
    }
}