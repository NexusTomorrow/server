using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Owners;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Server.Tests;

public sealed class OwnerWatchdogServiceTests
{
    private const string Report = "Owner stuck has handled nothing for 10 s with 3 messages queued";

    [Fact]
    public async Task AnOwnerWithQueuedMessagesAndNoProgressIsReportedOnceTenSecondsHavePassedOnTheHostClock()
    {
        var time = new FakeTimeProvider();
        var logs = new CapturingLoggerProvider();
        using var loggers = new LoggerFactory([logs]);
        var owner = new StuckOwner(time);
        var service = new OwnerWatchdogService(new OwnerWatchdog([owner], time, TimeSpan.FromSeconds(10), loggers.CreateLogger<OwnerWatchdog>()), time);
        await service.StartAsync(TestContext.Current.CancellationToken);

        // The service may arm its timer after this test starts, so the clock moves until the first check has read the owner. From
        // then on every step is one period and waits for its own check.
        for (int attempt = 0; attempt < 40 && owner.Reads == 0; attempt++)
        {
            time.Advance(OwnerWatchdogService.CheckInterval);
            for (int poll = 0; poll < 10 && owner.Reads == 0; poll++)
                await Task.Delay(50, TestContext.Current.CancellationToken);
        }

        Assert.True(owner.Reads > 0);
        DateTimeOffset firstCheck = owner.FirstRead!.Value;
        for (int step = 0; step < 40 && !logs.Lines.Contains(Report); step++)
        {
            int before = owner.Reads;
            time.Advance(OwnerWatchdogService.CheckInterval);
            await Eventually.TrueAsync(() => owner.Reads > before);
            if (time.GetUtcNow() - firstCheck < TimeSpan.FromSeconds(10))
                Assert.DoesNotContain(Report, logs.Lines);
        }

        await Eventually.TrueAsync(() => logs.Lines.Contains(Report));
        Assert.True(time.GetUtcNow() - firstCheck >= TimeSpan.FromSeconds(10));

        await service.StopAsync(TestContext.Current.CancellationToken);
    }

    private sealed class StuckOwner : IOwner
    {
        private readonly FakeTimeProvider _time;
        private readonly Lock _gate = new();
        private int _reads;
        private DateTimeOffset? _firstRead;

        public StuckOwner(FakeTimeProvider time)
        {
            _time = time;
        }

        public string Name => "stuck";

        public int QueueLength => 3;

        public int Reads
        {
            get
            {
                lock (_gate)
                    return _reads;
            }
        }

        public DateTimeOffset? FirstRead
        {
            get
            {
                lock (_gate)
                    return _firstRead;
            }
        }

        public long Handled
        {
            get
            {
                lock (_gate)
                {
                    _firstRead ??= _time.GetUtcNow();
                    _reads++;
                }

                return 0;
            }
        }
    }
}
