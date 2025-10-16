using System.Net.Sockets;
using DiscountCodeDemo.Core.Interfaces;
using DiscountCodeDemo.Protocol;
using DiscountCodeDemo.Protocol.Messages;

namespace DiscountCodeDemo.Server.Tcp;

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
            var reader = new MessageReader(stream);
            var writer = new MessageWriter(stream);

            while (true)
            {
                var message = await reader.ReadMessageAsync();
                if (message == null)
                {
                    Console.WriteLine("[ClientSession] Connection closed by client.");
                    break;
                }

                switch (message.Command)
                {
                    case RequestType.Generate:
                        await HandleGenerateRequest(writer, message.Payload);
                        break;
                    case RequestType.Use:
                        await HandleUseCodeRequest(writer, message.Payload);
                        break;
                    default:
                        Console.WriteLine("[ClientSession] Unknown command.");
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

    private async Task HandleGenerateRequest(MessageWriter writer, byte[] payload)
    {
        GenerateRequest request;
        try
        {
            request = GenerateRequest.FromBytes(payload);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ClientSession] Failed to parse GenerateRequest: {ex.Message}");
            await writer.WriteMessageAsync(new GenerateResponse(0));
            return;
        }

        Console.WriteLine($"[ClientSession] Generate request received: {request.Count} codes, Length: {request.Length}");

        if (request.Count == 0 || request.Count > ProtocolConstants.MaxCodeCount ||
            request.Length < ProtocolConstants.MinCodeLength || request.Length > ProtocolConstants.MaxCodeLength)
        {
            Console.WriteLine("[ClientSession] Invalid request parameters");
            await writer.WriteMessageAsync(new GenerateResponse(0));
            return;
        }

        var response = await _discountCodeService.GenerateDiscountCodesAsync(request.Count, request.Length);
        byte resultByte = response.Result ? (byte)1 : (byte)0;

        await writer.WriteMessageAsync(new GenerateResponse(resultByte));
        Console.WriteLine($"[ClientSession] Generate response sent: {resultByte}");
    }

    private async Task HandleUseCodeRequest(MessageWriter writer, byte[] payload)
    {
        UseCodeRequest request;
        try
        {
            request = UseCodeRequest.FromBytes(payload);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ClientSession] Failed to parse UseCodeRequest: {ex.Message}");
            await writer.WriteMessageAsync(new UseCodeResponse(0));
            return;
        }

        Console.WriteLine($"[ClientSession] UseCode request received: {request.Code}");

        bool success = await _discountCodeService.UseCodesAsync(request.Code);
        byte resultByte = success ? (byte)1 : (byte)0;

        await writer.WriteMessageAsync(new UseCodeResponse(resultByte));
        Console.WriteLine($"[ClientSession] UseCode response sent: {resultByte}");
    }
}