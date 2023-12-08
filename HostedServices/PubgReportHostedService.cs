using Microsoft.Extensions.Hosting;
using PubgReportCrawler.Services;
using PubgReportCrawler.ValueObjects;

namespace PubgReportCrawler.HostedServices;

public class PubgReportHostedService(StreamInfoService streamInfoService) : IHostedService, IDisposable
{
    private Timer? _timer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

        return Task.CompletedTask;
    }

    private async void DoWork(object? state)
    {
        await Console.Out.WriteLineAsync("Getting streams...");

        await streamInfoService.GetStreamInfo(new PubgReportAccountId("account.1eb237a0e35b45d7a041b76681851604"),
            info =>
            {
                // TODO: Send Discord Message
            });
        
        await Console.Out.WriteLineAsync("Done getting streams. Waiting for next iteration...");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}