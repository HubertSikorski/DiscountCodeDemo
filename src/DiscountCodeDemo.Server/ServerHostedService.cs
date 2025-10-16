using DiscountCodeDemo.Server.Tcp;
using Microsoft.Extensions.Hosting;

namespace DiscountCodeDemo.Server
{
    public class ServerHostedService : IHostedService
    {
        private readonly DiscountCodeTcpServer _server;

        public ServerHostedService(DiscountCodeTcpServer server)
        {
            _server = server;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _server.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _server.Dispose();
            return Task.CompletedTask;
        }
    }
}