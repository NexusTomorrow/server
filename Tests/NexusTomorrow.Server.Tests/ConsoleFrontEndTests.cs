using System.Collections.Immutable;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using NexusTomorrow.Commands;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Server.Tests;

public sealed class ConsoleFrontEndTests
{
    private static readonly TimeSpan s_bound = TimeSpan.FromSeconds(30);

    [Fact]
    public async Task EachLineRunsAsTheConsoleActorWithEveryPermissionAndItsReplyIsPrinted()
    {
        CommandActor? seen = null;
        CommandDispatcher dispatcher = Dispatcher(new CommandDefinition
        {
            Name = "ping",
            Description = "Answer pong.",
            Permission = "test.ping",
            Parameters = [],
            ExecuteAsync = (invocation, _) =>
            {
                seen = invocation.Actor;
                return Task.FromResult(new CommandReply(CommandOutcome.Done, "pong\n"));
            },
        });
        var output = new StringWriter();
        using var frontEnd = new ConsoleFrontEnd(dispatcher, enabled: true, new StringReader("\n   \nping\n"), output);

        await frontEnd.StartAsync(TestContext.Current.CancellationToken);
        await frontEnd.ExecuteTask!.WaitAsync(s_bound, TestContext.Current.CancellationToken);

        Assert.Equal(["pong"], output.ToString().Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries));
        Assert.NotNull(seen);
        Assert.Equal("console", seen.Name);
        Assert.Equal(FrontEnd.Cli, seen.FrontEnd);
        Assert.True(seen.Holds("any.permission.at.all"));
    }

    [Fact]
    public async Task ADisabledFrontEndReadsNothing()
    {
        var input = new StringReader("ping\n");
        var output = new StringWriter();
        using var frontEnd = new ConsoleFrontEnd(Dispatcher(), enabled: false, input, output);

        await frontEnd.StartAsync(TestContext.Current.CancellationToken);
        await frontEnd.ExecuteTask!.WaitAsync(s_bound, TestContext.Current.CancellationToken);

        Assert.Equal("", output.ToString());
        Assert.Equal('p', input.Peek());
    }

    [Fact]
    public async Task RedirectedInputKeepsTheFrontEndOff()
    {
        var output = new StringWriter();
        using var frontEnd = new ConsoleFrontEnd(Dispatcher(), enabled: true, input: null, output);

        await frontEnd.StartAsync(TestContext.Current.CancellationToken);
        await frontEnd.ExecuteTask!.WaitAsync(s_bound, TestContext.Current.CancellationToken);

        Assert.Equal("", output.ToString());
    }

    [Fact]
    public async Task StoppingDoesNotWaitForALineThatNeverComes()
    {
        using var input = new BlockedReader();
        using var frontEnd = new ConsoleFrontEnd(Dispatcher(), enabled: true, input, new StringWriter());
        await frontEnd.StartAsync(TestContext.Current.CancellationToken);
        await input.Reading.WaitAsync(s_bound, TestContext.Current.CancellationToken);

        await frontEnd.StopAsync(TestContext.Current.CancellationToken).WaitAsync(s_bound, TestContext.Current.CancellationToken);

        Assert.True(frontEnd.ExecuteTask!.IsCompleted);
    }

    private static CommandDispatcher Dispatcher(params CommandDefinition[] commands) =>
        new(commands, new FakeWriteBehind(), new FakeTimeProvider(), NullLogger<CommandDispatcher>.Instance);

    // A console with nobody typing: ReadLine blocks until the test ends.
    private sealed class BlockedReader : TextReader
    {
        private readonly ManualResetEventSlim _release = new();
        private readonly TaskCompletionSource _reading = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public Task Reading => _reading.Task;

        public override string? ReadLine()
        {
            _reading.TrySetResult();
            _release.Wait();
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _release.Set();
                _release.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
