using System.Collections.Concurrent;
using Microsoft.Extensions.Logging.Abstractions;

namespace NexusTomorrow.Owners.Tests;

public sealed class OwnerTests
{
    private static readonly TimeSpan s_bound = TimeSpan.FromSeconds(30);

    [Fact]
    public async Task HandlesMessagesInPostOrderOnOneThread()
    {
        var owner = new Recorder();
        owner.Start();
        for (int i = 0; i < 1000; i++)
            Assert.True(owner.Post(i));
        await owner.StopAsync();

        Assert.Equal(Enumerable.Range(0, 1000), owner.Values);
        Assert.Single(owner.Threads.Distinct());
        Assert.NotEqual(Environment.CurrentManagedThreadId, owner.Threads.First());
    }

    [Fact]
    public async Task AThrowingHandlerIsReportedAndTheNextMessageRuns()
    {
        var owner = new Recorder();
        owner.Start();
        owner.Post(Recorder.Throws);
        owner.Post(7);
        await owner.StopAsync();

        Assert.Equal([Recorder.Throws], owner.Faulted);
        Assert.Equal([7], owner.Values.Where(v => v != Recorder.Throws));
    }

    [Fact]
    public async Task StopDrainsQueuedMessagesBeforeItCompletes()
    {
        var owner = new Recorder { Delay = TimeSpan.FromMilliseconds(5) };
        owner.Start();
        for (int i = 0; i < 20; i++)
            owner.Post(i);
        await owner.StopAsync();
        Assert.Equal(20, owner.Values.Count);
        Assert.Equal(20, owner.Handled);
    }

    [Fact]
    public async Task TheQueueLengthCountsMessagesPostedAndNotYetRead()
    {
        var owner = new Recorder();
        for (int i = 0; i < 3; i++)
            Assert.True(owner.Post(i));
        Assert.Equal(3, owner.QueueLength);

        owner.Start();
        await owner.StopAsync().WaitAsync(s_bound, TestContext.Current.CancellationToken);
        Assert.Equal(0, owner.QueueLength);
        Assert.False(owner.Post(4));
        Assert.Equal(0, owner.QueueLength);
    }

    [Fact]
    public async Task APostAfterStopIsRefused()
    {
        var owner = new Recorder();
        owner.Start();
        await owner.StopAsync();
        Assert.False(owner.Post(1));
    }

    [Fact]
    public async Task AThrowingOnFaultDoesNotStopTheOwner()
    {
        var owner = new Recorder { FaultThrows = true };
        owner.Start();
        owner.Post(Recorder.Throws);
        owner.Post(7);
        await owner.StopAsync().WaitAsync(s_bound, TestContext.Current.CancellationToken);

        Assert.Equal([Recorder.Throws], owner.Faulted);
        Assert.Contains(7, owner.Values);
        Assert.Equal(2, owner.Handled);
    }

    [Fact]
    public async Task StopBeforeStartCompletes()
    {
        var owner = new Recorder();
        await owner.StopAsync().WaitAsync(s_bound, TestContext.Current.CancellationToken);
        Assert.False(owner.Post(1));
    }

    [Fact]
    public async Task StartingTwiceThrowsAndKeepsOneThread()
    {
        var owner = new Recorder();
        owner.Start();
        Assert.Throws<InvalidOperationException>(owner.Start);
        for (int i = 0; i < 100; i++)
            owner.Post(i);
        await owner.StopAsync().WaitAsync(s_bound, TestContext.Current.CancellationToken);

        Assert.Single(owner.Threads.Distinct());
        Assert.Equal(100, owner.Values.Count);
    }

    private sealed class Recorder : Owner<object>
    {
        public const int Throws = -1;

        public Recorder()
            : base("recorder", NullLogger.Instance)
        {
        }

        public TimeSpan Delay { get; init; }

        public bool FaultThrows { get; init; }

        public ConcurrentQueue<int> ValuesQueue { get; } = new();

        public List<int> Values => [.. ValuesQueue];

        public ConcurrentQueue<int> Threads { get; } = new();

        public List<int> Faulted { get; } = [];

        public bool Post(int value) => Post((object)value);

        protected override void Handle(object message)
        {
            int value = (int)message;
            Threads.Enqueue(Environment.CurrentManagedThreadId);
            ValuesQueue.Enqueue(value);
            if (Delay > TimeSpan.Zero)
                Thread.Sleep(Delay);
            if (value == Throws)
                throw new InvalidOperationException("thrown on purpose");
        }

        protected override void OnFault(object message, Exception exception)
        {
            Faulted.Add((int)message);
            if (FaultThrows)
                throw new InvalidOperationException("OnFault thrown on purpose");
        }
    }
}
