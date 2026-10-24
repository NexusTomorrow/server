using System.Numerics;

namespace NexusTomorrow.Owners;

/// <summary>
/// A server message serialized once for many sessions: its opcode, its type, and its body. Each holder takes a reference and releases
/// it exactly once; the last release clears the body and returns the message to its pool, so it is not touched afterwards. Only a
/// serializer (<see cref="IMessageSerializer"/>) makes one, holding the reference of the caller that asked for it.
/// </summary>
/// <remarks>
/// A pooled message keeps its body buffer, so the last release, on whichever thread a session's writer runs, puts the buffer back
/// where the next serialization takes it. The pool starts stocked, so a process that never holds more than
/// <see cref="StockedMessages"/> messages at once, each within <see cref="StockedBodyBytes"/>, never allocates one.
/// </remarks>
public sealed class SerializedMessage
{
    /// <summary>
    /// How many messages the pool holds before the first is taken.
    /// </summary>
    public const int StockedMessages = 64;

    /// <summary>
    /// The body buffer each stocked message starts with.
    /// </summary>
    public const int StockedBodyBytes = 256;

    private static readonly Lock s_poolGate = new();
    private static readonly Stack<SerializedMessage> s_pool = Stock();

    private byte[] _body;
    private int _length;
    private int _references;

    private SerializedMessage(int bodyBytes)
    {
        _body = new byte[bodyBytes];
    }

    /// <summary>
    /// The message's opcode.
    /// </summary>
    public ushort Opcode { get; private set; }

    /// <summary>
    /// The message's declared type, which a reader of <see cref="Body"/> creates.
    /// </summary>
    public Type MessageType { get; private set; } = typeof(object);

    /// <summary>
    /// The serialized body. A session reads it only while it holds a reference, and never writes to it.
    /// </summary>
    public ReadOnlySpan<byte> Body => _body.AsSpan(0, _length);

    /// <summary>
    /// How many holders have not yet released the message.
    /// </summary>
    public int References => Volatile.Read(ref _references);

    /// <summary>
    /// Take a message from the pool for a body of <paramref name="length"/> bytes, which the caller writes through
    /// <paramref name="body"/> before any session is given the message, and releases the message if the write fails. The message holds
    /// one reference, the caller's.
    /// </summary>
    public static SerializedMessage Rent(ushort opcode, Type messageType, int length, out Span<byte> body)
    {
        SerializedMessage? message = null;
        lock (s_poolGate)
        {
            if (s_pool.Count > 0)
                message = s_pool.Pop();
        }

        message ??= new SerializedMessage(StockedBodyBytes);
        if (message._body.Length < length)
            message._body = new byte[BitOperations.RoundUpToPowerOf2((uint)length)];

        message.Opcode = opcode;
        message.MessageType = messageType;
        message._length = length;
        Volatile.Write(ref message._references, 1);
        body = message._body.AsSpan(0, length);
        return message;
    }

    /// <summary>
    /// Take a reference for a new holder, which releases it once.
    /// </summary>
    /// <exception cref="InvalidOperationException">The message was released by every holder and has gone back to its pool.</exception>
    public void AddReference()
    {
        if (Interlocked.Increment(ref _references) > 1)
            return;

        Interlocked.Decrement(ref _references);
        throw new InvalidOperationException("A message every holder has released cannot take a reference.");
    }

    /// <summary>
    /// Release one holder's reference; the last clears the body and returns the message to its pool.
    /// </summary>
    /// <exception cref="InvalidOperationException">No reference is left to release.</exception>
    public void Release()
    {
        int left = Interlocked.Decrement(ref _references);
        if (left > 0)
            return;

        if (left < 0)
        {
            Interlocked.Increment(ref _references);
            throw new InvalidOperationException("The message has no reference left to release.");
        }

        // Cleared like every returned send buffer: a pooled body holds no message bytes.
        _body.AsSpan(0, _length).Clear();
        _length = 0;
        lock (s_poolGate)
            s_pool.Push(this);
    }

    private static Stack<SerializedMessage> Stock()
    {
        var pool = new Stack<SerializedMessage>(StockedMessages);
        for (int i = 0; i < StockedMessages; i++)
            pool.Push(new SerializedMessage(StockedBodyBytes));

        return pool;
    }
}
