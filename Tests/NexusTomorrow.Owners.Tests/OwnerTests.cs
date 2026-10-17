using System.Collections.Concurrent;
using Microsoft.Extensions.Logging.Abstractions;

namespace NexusTomorrow.Owners.Tests;

public sealed class OwnerTests
{
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
    public async Task APostAfterStopIsRefused()
    {
        var owner = new Recorder();
        owner.Start();
        await owner.StopAsync();
        Assert.False(owner.Post(1));
    }

    private sealed class Recorder : Owner<object>
    {
        public const int Throws = -1;

        public Recorder()
            : base("recorder", NullLogger.Instance)
        {
        }

        public TimeSpan Delay { get; init; }

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

        protected override void OnFault(object message, Exception exception) => Faulted.Add((int)message);
    }
}
