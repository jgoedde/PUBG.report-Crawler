using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PubgReportCrawler.HostedServices;
using PubgReportCrawler.Services;
using Refit;

// create hosting object and DI layer
using var host = CreateHostBuilder(args).Build();

// create a service scope
using var scope = host.Services.CreateScope();

IHostBuilder CreateHostBuilder(string[] strings)
{
    return Host.CreateDefaultBuilder()
        .ConfigureServices((_, services) =>
        {
            services.AddRefitClient<IPubgReportApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.pubg.report/v1"));

            services.AddSingleton<StreamInfoService>();

            services.AddHostedService<PubgReportHostedService>();
        });
}

await host.RunAsync();