using DiscountCodeDemo.Server.Protocol.Messages;

namespace DiscountCodeDemo.Server.Protocol;

public class MessageWriter
{
    private readonly Stream _stream;

    public MessageWriter(Stream stream)
    {
        _stream = stream;
    }

    public async Task WriteMessageAsync(IProtocolMessage message)
    {
        var payload = message.ToBytes();
        var header = new byte[3];
        header[0] = (byte)message.Type;
        BitConverter.GetBytes((ushort)payload.Length).CopyTo(header, 1);

        await _stream.WriteAsync(header, 0, header.Length);
        if (payload.Length > 0)
            await _stream.WriteAsync(payload, 0, payload.Length);
    }
}