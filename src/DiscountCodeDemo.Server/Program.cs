using System.Net;
using DiscountCodeDemo.Core.Interfaces;
using DiscountCodeDemo.Infrastructure;
using DiscountCodeDemo.Server;
using DiscountCodeDemo.Server.Tcp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((context, services) =>
{
    var jsonFilePath = "discount_codes.json";

    services.AddInfrastructure(jsonFilePath);

    services.AddSingleton(provider =>
    {
        var discountCodeService = provider.GetRequiredService<IDiscountCodeService>();
        return new DiscountCodeTcpServer(IPAddress.Loopback, 5000, discountCodeService);
    });

    services.AddHostedService<ServerHostedService>();
});

await builder.Build().RunAsync();