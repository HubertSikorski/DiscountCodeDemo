using System.Buffers.Binary;

namespace DiscountCodeDemo.Protocol.Messages;

public class GenerateRequest : IProtocolMessage
{
    public RequestType Type => RequestType.Generate;
    public ushort Count { get; set; }
    public byte Length { get; set; }

    public GenerateRequest() {}

    public GenerateRequest(ushort count, byte length)
    {
        Count = count;
        Length = length;
    }

    public byte[] ToBytes()
    {
        var bytes = new byte[3];
        BinaryPrimitives.WriteUInt16LittleEndian(bytes, Count);
        bytes[2] = Length;
        return bytes;
    }

    public static GenerateRequest FromBytes(ReadOnlySpan<byte> span)
    {
        if (span.Length != 3)
            throw new ArgumentException("Invalid GenerateRequest length");

        return new GenerateRequest
        {
            Count = BinaryPrimitives.ReadUInt16LittleEndian(span),
            Length = span[2]
        };
    }
}