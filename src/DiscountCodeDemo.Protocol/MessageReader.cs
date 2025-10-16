using DiscountCodeDemo.Protocol.Messages;

namespace DiscountCodeDemo.Protocol;

public class MessageReader
{
    private readonly Stream _stream;

    public MessageReader(Stream stream)
    {
        _stream = stream;
    }

    public async Task<ProtocolMessage?> ReadMessageAsync()
    {
        var header = new byte[3];
        int read = 0;

        while (read < 3)
        {
            int n = await _stream.ReadAsync(header, read, 3 - read);
            if (n == 0) return null;
            read += n;
        }

        var command = (RequestType)header[0];
        var length = BitConverter.ToUInt16(header, 1);

        byte[] payload = Array.Empty<byte>();

        if (length > 0)
        {
            payload = new byte[length];
            int bytesRead = 0;
            while (bytesRead < length)
            {
                int n = await _stream.ReadAsync(payload, bytesRead, length - bytesRead);
                if (n == 0) throw new IOException("Stream closed unexpectedly");
                bytesRead += n;
            }
        }

        return new ProtocolMessage(command, payload);
    }
}