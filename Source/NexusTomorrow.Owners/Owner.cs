using System.Threading.Channels;
using Microsoft.Extensions.Logging;

namespace NexusTomorrow.Owners;

/// <summary>
/// One side's state behind a single-reader queue, handled on a dedicated thread. Only <see cref="Handle"/> touches the state, so the
/// state needs no locks. Anything else reaches the owner through <see cref="Post"/>.
/// </summary>
public abstract class Owner<TMessage> : IOwner
    where TMessage : class
{
    private readonly Channel<TMessage> _queue = Channel.CreateUnbounded<TMessage>(new UnboundedChannelOptions { SingleReader = true });
    private readonly TaskCompletionSource _stopped = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private readonly ILogger _logger;
    private long _handled;

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
    public int QueueLength => _queue.Reader.Count;

    /// <inheritdoc/>
    public long Handled => Interlocked.Read(ref _handled);

    /// <summary>
    /// Start the owner's thread.
    /// </summary>
    public void Start()
    {
        var thread = new Thread(Run) { IsBackground = true, Name = Name };
        thread.Start();
    }

    /// <summary>
    /// Queue <paramref name="message"/>; false once the owner is stopping.
    /// </summary>
    public bool Post(TMessage message) => _queue.Writer.TryWrite(message);

    /// <summary>
    /// Refuse new messages, handle the queued ones, and complete when the thread ends.
    /// </summary>
    public Task StopAsync()
    {
        _queue.Writer.TryComplete();
        return _stopped.Task;
    }

    /// <summary>
    /// Handle one message, on the owner's thread.
    /// </summary>
    protected abstract void Handle(TMessage message);

    /// <summary>
    /// Called on the owner's thread after <see cref="Handle"/> threw for <paramref name="message"/>, once the failure is logged. An
    /// owner whose messages carry a session disconnects it here.
    /// </summary>
    protected virtual void OnFault(TMessage message, Exception exception)
    {
    }

    private void Run()
    {
        ChannelReader<TMessage> reader = _queue.Reader;
        while (true)
        {
            while (reader.TryRead(out TMessage? message))
            {
                try
                {
                    Handle(message);
                }
                catch (Exception exception)
                {
                    // The failure policy (spec section 1): log the message, tell its session, and keep handling the queue.
                    OwnerLog.HandlerFailed(_logger, Name, message.GetType().Name, exception);
                    OnFault(message, exception);
                }

                Interlocked.Increment(ref _handled);
            }

            // This thread is the owner's own: blocking it while the queue is empty is its whole job.
            if (!reader.WaitToReadAsync().AsTask().GetAwaiter().GetResult())
                break;
        }

        _stopped.TrySetResult();
    }
}
