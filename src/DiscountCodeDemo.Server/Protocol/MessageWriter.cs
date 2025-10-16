using System.Buffers.Binary;
using System.Net.Sockets;
using System.Text;
using DiscountCodeDemo.Server.Protocol.Messages;

namespace DiscountCodeDemo.Server.Protocol;

public class MessageWriter
{
    private readonly NetworkStream _networkStream;

    public MessageWriter(NetworkStream networkStream)
    {
        _networkStream = networkStream;
    }

    public async Task SendMessageAsync(RequestType type, byte[] payload)
    {
        var header = new byte[3];
        header[0] = (byte)type;
        BinaryPrimitives.WriteInt32BigEndian(header.AsSpan(1), (ushort)payload.Length);
        
        await _networkStream.WriteAsync(header);
        await _networkStream.WriteAsync(payload);
    }

    public async Task SendBooleanResponseAsync(RequestType type, bool success)
    {
        byte[] payload = new[] {success ? (byte)1 : (byte)0};
        await SendMessageAsync(type, payload);
    }

    public async Task SendErrorAsync(string errorMessage)
    {
        byte[] payload = Encoding.UTF8.GetBytes(errorMessage);
        await SendMessageAsync(RequestType.Error, payload);
    }
}