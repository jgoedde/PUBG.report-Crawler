using Discord;
using Discord.WebSocket;

using DotNetEnv;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PubgReportCrawler.Config;
using PubgReportCrawler.HostedServices;
using PubgReportCrawler.Services;

using Refit;

Env.Load();

using var host = CreateHostBuilder().Build();

using var scope = host.Services.CreateScope();

await host.RunAsync();
return;

static IHostBuilder CreateHostBuilder() => Host.CreateDefaultBuilder()
        .ConfigureHostConfiguration(builder => builder.AddEnvironmentVariables())
        .ConfigureServices((hostBuilderContext, services) =>
        {
            services.AddOptions<AppSettings>()
                .Bind(hostBuilderContext.Configuration)
                .ValidateOnStart();

            services.AddRefitClient<IPubgReportApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.pubg.report/v1"));

            services.AddSingleton<StreamInfoService>();

            services
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                {
                    LogLevel = LogSeverity.Info,
                    GatewayIntents = GatewayIntents.DirectMessages
                }));

            services.AddHostedService<PubgReportHostedService>();
            services.AddHostedService<DiscordHostedService>();
        });
