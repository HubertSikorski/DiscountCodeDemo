using System.Buffers.Binary;
using System.Net.Sockets;
using DiscountCodeDemo.Server.Protocol.Messages;

namespace DiscountCodeDemo.Server.Protocol;

public class MessageReader
{
    private readonly NetworkStream _networkStream;

    public MessageReader(NetworkStream networkStream)
    {
        _networkStream = networkStream;
    }

    public async Task<(RequestType Type, byte[] Data)> ReadMessageAsync()
    {
        var header = new byte[3];
        int bytesRead = 0;

        while (bytesRead < header.Length)
        {
            int read = await _networkStream.ReadAsync(header, bytesRead, header.Length - bytesRead);

            if (read == 0)
                throw new IOException("[Server] Connection closed while reading header");

            bytesRead += read;
        }

        var type = (RequestType)header[0];
        ushort payloadLength = BinaryPrimitives.ReadUInt16BigEndian(header.AsSpan(1));

        var payload = new byte[payloadLength];
        bytesRead = 0;

        while (bytesRead < payloadLength)
        {
            int read = await _networkStream.ReadAsync(payload, bytesRead, payloadLength - bytesRead);
            if (read == 0)
                throw new IOException("[Server] Connection closed while reading payload");

            bytesRead += read;
        }

        return (type, payload);
    }
}