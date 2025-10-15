using System.Net;
using System.Net.Sockets;
using DiscountCodeDemo.Core.Interfaces;

namespace DiscountCodeDemo.Server;

public class DiscountCodeTcpServer : IDisposable
{
    private readonly IPAddress _ipAddress;
    private readonly int _port;
    private readonly IDiscountCodeService _discountCodeService;
    private TcpListener? _tcpListener;
    private bool _isListening;

    public DiscountCodeTcpServer(
        IPAddress ipAddress,
        int port,
        IDiscountCodeService discountCodeService)
    {
        _ipAddress = ipAddress;
        _port = port;
        _discountCodeService = discountCodeService;
    }

    public void Start()
    {
        _tcpListener = new TcpListener(_ipAddress, _port);
        _tcpListener.Start();
        _isListening = true;
        Console.WriteLine($"[Server] Started on {_ipAddress}:{_port}");
        _ = AcceptClientAsync();
    }

    private async Task AcceptClientAsync()
    {
        while (_isListening)
        {
            try
            {
                var client = await _tcpListener.AcceptTcpClientAsync();
                Console.WriteLine($"[Server] Client connected...");
                _ = HandleClientAsync(client);
            }
            catch (ObjectDisposedException)
            {
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Server] Error accepting client: {ex.Message}");
            }
        }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        try
        {
            using var stream = client.GetStream();

            while (true)
            {
                var requestTypeBuffer = new byte[1];
                int readBytes = await stream.ReadAsync(requestTypeBuffer, 0, 1);

                if (readBytes == 0)
                    break;
                
                var requestType = requestTypeBuffer[0];

                switch (requestType)
                {
                    case 0x01:
                        await HandleGenerateRequest(stream);
                        break;
                    case 0x002:
                        await HandleUseCodeRequest(stream);
                        break;
                    default:
                        Console.WriteLine($"[Server] Unknown request type");
                        return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Server] Error handling client: {ex.Message}");
        }
        finally
        {
            client.Close();
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

        string code = System.Text.Encoding.ASCII.GetString(buffer);

        Console.WriteLine($"[Server] UseCode request received: {code}");

        bool success = await _discountCodeService.UseCodesAsync(code);

        byte resultCode = success ? (byte)1 : (byte)0;

        await SendUseCodeResponse(stream, resultCode);
    }

    // private async Task HandleGenerateRequest(NetworkStream stream)
    // {
    //     var buffer = new byte[3];
    //     int bytesRead = 0;
    //
    //     while (bytesRead < 3)
    //     {
    //         int n = await stream.ReadAsync(buffer, bytesRead, 3 - bytesRead);
    //         if(n == 0) throw new Exception("Connection closerd while reading GenerateRequest");
    //         bytesRead += n;
    //     }
    //     
    //     ushort count = BitConverter.ToUInt16(buffer, 0);
    //     byte length = buffer[2];
    //     
    //     Console.WriteLine($"[Server] Generate request recived: {count} bytes, Length: {length}");
    //
    //     if (count == 0 || count > 2000 || length < 7 || length > 8)
    //     {
    //         Console.WriteLine("[Server] Invalid request parameters");
    //         await SendGeneratedResponse(stream, false);
    //         return;
    //     }
    //     
    //     bool result = await _discountCodeService.GenerateDiscountCodesAsync(count, length);
    //     await SendGeneratedResponse(stream, result);
    // }
    
    private async Task HandleGenerateRequest(NetworkStream stream)
    {
        var buffer = new byte[3];
        int bytesRead = 0;

        while (bytesRead < 3)
        {
            int n = await stream.ReadAsync(buffer, bytesRead, 3 - bytesRead);
            if (n == 0) throw new Exception("Connection closed while reading GenerateRequest");
            bytesRead += n;
        }

        ushort count = BitConverter.ToUInt16(buffer, 0);
        byte length = buffer[2];

        Console.WriteLine($"[Server] Generate request received: {count} codes, Length: {length}");

        if (count == 0 || count > 2000 || length < 7 || length > 8)
        {
            Console.WriteLine("[Server] Invalid request parameters");
            await SendGeneratedResponse(stream, false);
            return;
        }

        var response = await _discountCodeService.GenerateDiscountCodesAsync(count, length);
        await SendGeneratedResponse(stream, response.Result);
    }


    private async Task SendUseCodeResponse(NetworkStream stream, byte resultCode)
    {
        await stream.WriteAsync(new byte[] { resultCode }, 0, 1);
        Console.WriteLine($"[Server] UseCode response sent: {resultCode}");
    }
    
    private async Task SendGeneratedResponse(NetworkStream stream, bool result)
    {
        byte[] response = new byte[] { result ? (byte)1 : (byte)0 };
        await stream.WriteAsync(response, 0, 1);
        Console.WriteLine($"[Server] Generate response sent: {result}");
    }

    public void Dispose()
    {
        _isListening = false;
        _tcpListener?.Stop();
        Console.WriteLine("[Server] Stopped");
    }
}