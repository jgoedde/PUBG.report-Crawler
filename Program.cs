using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using PubgReportCrawler.HostedServices;
using PubgReportCrawler.Services;

using Refit;

using IHost host = CreateHostBuilder().Build();

using IServiceScope scope = host.Services.CreateScope();

await host.RunAsync();
return;

IHostBuilder CreateHostBuilder()
{
    return Host.CreateDefaultBuilder()
        .ConfigureServices((_, services) =>
        {
            services.AddRefitClient<IPubgReportApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.pubg.report/v1"));

            services.AddSingleton<StreamInfoService>();

            services
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                {
                    LogLevel = LogSeverity.Info, GatewayIntents = GatewayIntents.DirectMessages
                }));

            services.AddHostedService<PubgReportHostedService>();
            services.AddHostedService<DiscordHostedService>();
        });
}