using System.Buffers;

namespace NexusTomorrow.Owners;

/// <summary>
/// A server message serialized once for many sessions: its opcode, its type, and its body in a buffer rented from
/// <see cref="ArrayPool{T}.Shared"/>. Each holder takes a reference and releases it exactly once; the last release returns the body,
/// cleared, to the pool and this holder to a pool of its own, so neither is touched afterwards. Only a serializer
/// (<see cref="IMessageSerializer"/>) makes one, holding the reference of the caller that asked for it.
/// </summary>
public sealed class SerializedMessage
{
    private static readonly Lock s_poolGate = new();
    private static readonly Stack<SerializedMessage> s_pool = new();

    private byte[] _body = [];
    private int _length;
    private int _references;

    private SerializedMessage()
    {
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
    /// Wrap <paramref name="body"/>, rented from <see cref="ArrayPool{T}.Shared"/> and holding <paramref name="length"/> bytes. The
    /// message owns the body from here, and holds one reference, the caller's.
    /// </summary>
    public static SerializedMessage FromRentedBody(ushort opcode, Type messageType, byte[] body, int length)
    {
        SerializedMessage message;
        lock (s_poolGate)
            message = s_pool.Count > 0 ? s_pool.Pop() : new SerializedMessage();

        message.Opcode = opcode;
        message.MessageType = messageType;
        message._body = body;
        message._length = length;
        Volatile.Write(ref message._references, 1);
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
    /// Release one holder's reference; the last returns the body and the message to their pools.
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

        byte[] body = _body;
        _body = [];
        _length = 0;
        ArrayPool<byte>.Shared.Return(body, clearArray: true);
        lock (s_poolGate)
            s_pool.Push(this);
    }
}
