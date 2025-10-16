using System.Net.Sockets;
using DiscountCodeDemo.Protocol;
using DiscountCodeDemo.Protocol.Messages;

namespace DiscountCodeDemo.Client.Tcp
{
    public class DiscountCodeClient : IDisposable
    {
        private readonly TcpClient _tcpClient;
        private readonly NetworkStream _stream;
        private readonly MessageReader _messageReader;

        public DiscountCodeClient(string host, int port)
        {
            _tcpClient = new TcpClient();
            _tcpClient.Connect(host, port);
            _stream = _tcpClient.GetStream();
            _messageReader = new MessageReader(_stream);
        }

        public async Task<bool> GenerateAsync(ushort count, byte length)
        {
            var request = new GenerateRequest(count, length);
            var message = new ProtocolMessage(RequestType.Generate, request.ToBytes());

            await SendMessageAsync(message);

            var response = await _messageReader.ReadMessageAsync();
            if (response == null || response.Payload.Length == 0)
                return false;

            return response.Payload[0] == 1;
        }

        public async Task<bool> UseCodeAsync(string code)
        {
            var request = new UseCodeRequest(code);
            var message = new ProtocolMessage(RequestType.Use, request.ToBytes());

            await SendMessageAsync(message);

            var response = await _messageReader.ReadMessageAsync();

            if (response == null || response.Payload.Length == 0)
                return false;

            return response.Payload[0] == 1;
        }

        private async Task SendMessageAsync(ProtocolMessage message)
        {
            var header = new byte[3];
            header[0] = (byte)message.Command;
            var lengthBytes = BitConverter.GetBytes((ushort)message.Payload.Length);
            header[1] = lengthBytes[0];
            header[2] = lengthBytes[1];

            await _stream.WriteAsync(header, 0, 3);
            if (message.Payload.Length > 0)
            {
                await _stream.WriteAsync(message.Payload, 0, message.Payload.Length);
            }
        }

        public void Dispose()
        {
            _stream?.Dispose();
            _tcpClient?.Close();
        }
    }
}
