using System.Net.Sockets;
using System.Text;
using DiscountCodeDemo.Core.Interfaces;
using DiscountCodeDemo.Server.Protocol;
using DiscountCodeDemo.Server.Protocol.Messages;

namespace DiscountCodeDemo.Server.Tcp
{
    public class ClientSession
    {
        private readonly TcpClient _client;
        private readonly IDiscountCodeService _discountCodeService;

        public ClientSession(TcpClient client, IDiscountCodeService discountCodeService)
        {
            _client = client;
            _discountCodeService = discountCodeService;
        }

        public async Task ProcessAsync()
        {
            try
            {
                using var stream = _client.GetStream();

                while (true)
                {
                    var commandBuffer = new byte[1];
                    int bytesRead = await stream.ReadAsync(commandBuffer, 0, 1);
                    if (bytesRead == 0)
                        break;

                    byte command = commandBuffer[0];
                    switch (command)
                    {
                        case (byte)RequestType.Generate:
                            await HandleGenerateRequest(stream);
                            break;
                        case (byte)RequestType.Use:
                            await HandleUseCodeRequest(stream);
                            break;
                        default:
                            Console.WriteLine("[ClientSession] Unknown command received.");
                            return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ClientSession] Exception: {ex.Message}");
            }
            finally
            {
                _client.Close();
            }
        }

        private async Task HandleUseCodeRequest(NetworkStream stream)
        {
            var buffer = new byte[8];
            int bytesRead = 0;

            while (bytesRead < 8)
            {
                int n = await stream.ReadAsync(buffer, bytesRead, 8 - bytesRead);
                if (n == 0)
                    throw new Exception("Connection closed while reading UseCodeRequest");
                bytesRead += n;
            }

            string code = Encoding.ASCII.GetString(buffer);

            Console.WriteLine($"[ClientSession] UseCode request received: {code}");

            bool success = await _discountCodeService.UseCodesAsync(code);

            byte resultCode = success ? (byte)1 : (byte)0;

            await SendUseCodeResponse(stream, resultCode);
        }

        private async Task HandleGenerateRequest(NetworkStream stream)
        {
            var buffer = new byte[3];
            int bytesRead = 0;

            while (bytesRead < 3)
            {
                int n = await stream.ReadAsync(buffer, bytesRead, 3 - bytesRead);
                if (n == 0)
                    throw new Exception("Connection closed while reading GenerateRequest");
                bytesRead += n;
            }

            ushort count = BitConverter.ToUInt16(buffer, 0);
            byte length = buffer[2];

            Console.WriteLine($"[ClientSession] Generate request received: {count} codes, Length: {length}");

            if (count == 0 || count > ProtocolConstants.MaxCodeCount || length < ProtocolConstants.MinCodeLength || length > ProtocolConstants.MaxCodeLength)
            {
                Console.WriteLine("[ClientSession] Invalid request parameters");
                await SendGenerateResponse(stream, false);
                return;
            }

            var response = await _discountCodeService.GenerateDiscountCodesAsync(count, length);
            await SendGenerateResponse(stream, response.Result);
        }

        private async Task SendUseCodeResponse(NetworkStream stream, byte resultCode)
        {
            await stream.WriteAsync(new byte[] { resultCode }, 0, 1);
            Console.WriteLine($"[ClientSession] UseCode response sent: {resultCode}");
        }

        private async Task SendGenerateResponse(NetworkStream stream, bool success)
        {
            byte resultByte = success ? (byte)1 : (byte)0;
            await stream.WriteAsync(new byte[] { resultByte }, 0, 1);
            Console.WriteLine($"[ClientSession] Generate response sent: {success}");
        }
    }
}