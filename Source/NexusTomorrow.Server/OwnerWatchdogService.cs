using Microsoft.Extensions.Hosting;
using NexusTomorrow.Owners;

namespace NexusTomorrow.Server;

/// <summary>
/// Runs <see cref="OwnerWatchdog.Check"/> once a second on the host's clock for as long as the host runs.
/// </summary>
public sealed class OwnerWatchdogService : BackgroundService
{
    /// <summary>
    /// How often the owners are checked.
    /// </summary>
    public static readonly TimeSpan CheckInterval = TimeSpan.FromSeconds(1);

    private readonly OwnerWatchdog _watchdog;
    private readonly TimeProvider _time;

    /// <summary>
    /// Create the service over <paramref name="watchdog"/>, ticking on <paramref name="time"/>.
    /// </summary>
    public OwnerWatchdogService(OwnerWatchdog watchdog, TimeProvider time)
    {
        _watchdog = watchdog;
        _time = time;
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(CheckInterval, _time);
        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
                _watchdog.Check();
        }
        catch (OperationCanceledException)
        {
        }
    }
}
