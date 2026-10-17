using System.Threading.Channels;
using Microsoft.Extensions.Logging;

namespace NexusTomorrow.Owners;

/// <summary>
/// One side's state behind a single-reader queue, handled on a dedicated thread. Only <see cref="Handle"/> touches the state, so the
/// state needs no locks. Anything else reaches the owner through <see cref="Post"/>. The owner has one thread for its lifetime: it
/// starts once, and <see cref="StopAsync"/> completes whether or not it ever started and whatever a handler or fault hook throws.
/// </summary>
public abstract class Owner<TMessage> : IOwner
    where TMessage : class
{
    private readonly Channel<TMessage> _queue = Channel.CreateUnbounded<TMessage>(new UnboundedChannelOptions { SingleReader = true });
    private readonly TaskCompletionSource _stopped = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private readonly ILogger _logger;
    private long _handled;
    private int _claimed;

    // A single-reader channel cannot report its length (CanCount is false), so the owner counts what is posted and not yet read.
    private int _queued;

    /// <summary>
    /// Create an owner named <paramref name="name"/>.
    /// </summary>
    protected Owner(string name, ILogger logger)
    {
        Name = name;
        _logger = logger;
    }

    /// <inheritdoc/>
    public string Name { get; }

    /// <inheritdoc/>
    public int QueueLength => Volatile.Read(ref _queued);

    /// <inheritdoc/>
    public long Handled => Interlocked.Read(ref _handled);

    /// <summary>
    /// Start the owner's thread. Throws <see cref="InvalidOperationException"/> if the owner was already started or stopped, so
    /// <see cref="Handle"/> never runs on two threads.
    /// </summary>
    public void Start()
    {
        if (Interlocked.Exchange(ref _claimed, 1) != 0)
            throw new InvalidOperationException($"Owner {Name} was already started or stopped.");

        var thread = new Thread(Run) { IsBackground = true, Name = Name };
        thread.Start();
    }

    /// <summary>
    /// Queue <paramref name="message"/>; false once the owner is stopping.
    /// </summary>
    public bool Post(TMessage message)
    {
        // Counted before the write, so the reader's decrement can never run first.
        Interlocked.Increment(ref _queued);
        if (_queue.Writer.TryWrite(message))
            return true;

        Interlocked.Decrement(ref _queued);
        return false;
    }

    /// <summary>
    /// Refuse new messages, handle the queued ones, and complete when the thread ends. An owner that was never started completes at
    /// once and can no longer be started.
    /// </summary>
    public Task StopAsync()
    {
        _queue.Writer.TryComplete();
        if (Interlocked.Exchange(ref _claimed, 1) == 0)
            _stopped.TrySetResult();

        return _stopped.Task;
    }

    /// <summary>
    /// Handle one message, on the owner's thread.
    /// </summary>
    protected abstract void Handle(TMessage message);

    /// <summary>
    /// Called on the owner's thread after <see cref="Handle"/> threw for <paramref name="message"/>, once the failure is logged. An
    /// owner whose messages carry a session disconnects it here. An exception thrown from here is logged and the owner keeps
    /// handling the queue.
    /// </summary>
    protected virtual void OnFault(TMessage message, Exception exception)
    {
    }

    private void Run()
    {
        try
        {
            ChannelReader<TMessage> reader = _queue.Reader;
            while (true)
            {
                while (reader.TryRead(out TMessage? message))
                {
                    Interlocked.Decrement(ref _queued);
                    try
                    {
                        Handle(message);
                    }
                    catch (Exception exception)
                    {
                        // The failure policy (spec section 1): log the message, tell its session, and keep handling the queue.
                        Fault(message, exception);
                    }

                    Interlocked.Increment(ref _handled);
                }

                // This thread is the owner's own: blocking it while the queue is empty is its whole job.
                if (!reader.WaitToReadAsync().AsTask().GetAwaiter().GetResult())
                    break;
            }
        }
        finally
        {
            _stopped.TrySetResult();
        }
    }

    private void Fault(TMessage message, Exception exception)
    {
        // Each step is guarded alone, so a failing logger cannot skip OnFault and a failing OnFault cannot end the thread.
        Guarded(() => OwnerLog.HandlerFailed(_logger, Name, message.GetType().Name, exception));
        Guarded(() => OnFault(message, exception));
    }

    private void Guarded(Action step)
    {
        try
        {
            step();
        }
        catch (Exception exception)
        {
            try
            {
                OwnerLog.FaultStepFailed(_logger, Name, exception);
            }
            catch (Exception)
            {
                // The logger itself throws: there is nowhere left to report, and the queue must keep draining.
            }
        }
    }
}
