using System.Net;
using System.Net.Sockets;
using DiscountCodeDemo.Core.Interfaces;

namespace DiscountCodeDemo.Server.Tcp;

public class DiscountCodeTcpServer : IDisposable
{
    private readonly IPAddress _ipAddress;
    private readonly int _port;
    private readonly IDiscountCodeService _discountCodeService;
    private TcpListener? _tcpListener;
    private bool _isListening;

    public DiscountCodeTcpServer(IPAddress ipAddress, int port, IDiscountCodeService discountCodeService)
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
        _ = AcceptClientsAsync();
    }

    private async Task AcceptClientsAsync()
    {
        while (_isListening)
        {
            try
            {
                var client = await _tcpListener!.AcceptTcpClientAsync();
                Console.WriteLine("[Server] Client connected.");
                var session = new ClientSession(client, _discountCodeService);
                _ = session.ProcessAsync();
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

    public void Dispose()
    {
        _isListening = false;
        _tcpListener?.Stop();
        Console.WriteLine("[Server] Stopped");
    }
}