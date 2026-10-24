using System.Numerics;

namespace NexusTomorrow.Owners;

/// <summary>
/// A server message serialized once for many sessions: its opcode, its type, and its body. Each holder takes a reference and releases
/// it exactly once; the last release clears the body and returns the message to its serializer's pool, so it is not touched
/// afterwards. Only a serializer (<see cref="IMessageSerializer"/>) makes one, holding the reference of the caller that asked for it.
/// </summary>
public sealed class SerializedMessage
{
    private readonly SerializedMessagePool _pool;
    private byte[] _body;
    private int _length;
    private int _references;

    internal SerializedMessage(SerializedMessagePool pool, int bodyBytes)
    {
        _pool = pool;
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
        _pool.Return(this);
    }

    // Taken from the pool for a body of length bytes, the buffer grown to the next power of two when it is smaller; holds one reference.
    internal Span<byte> Take(ushort opcode, Type messageType, int length)
    {
        if (_body.Length < length)
            _body = new byte[BitOperations.RoundUpToPowerOf2((uint)length)];

        Opcode = opcode;
        MessageType = messageType;
        _length = length;
        Volatile.Write(ref _references, 1);
        return _body.AsSpan(0, length);
    }
}
