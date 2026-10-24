namespace NexusTomorrow.Owners;

// One serializer's messages (SerializedMessage). Each pooled message keeps its body buffer, so the last release, on whichever thread
// a session's writer runs, puts the buffer back where the next serialization takes it. The pool starts stocked, so a serializer that
// never has more than StockedMessages messages out at once, each within StockedBodyBytes, never allocates one. Internal, and visible
// to the gateway's serializer alone, so every message a session sends has passed the send gate.
internal sealed class SerializedMessagePool
{
    public const int StockedMessages = 64;

    public const int StockedBodyBytes = 256;

    private readonly Lock _gate = new();
    private readonly Stack<SerializedMessage> _free = new(StockedMessages);

    public SerializedMessagePool()
    {
        for (int i = 0; i < StockedMessages; i++)
            _free.Push(new SerializedMessage(this, StockedBodyBytes));
    }

    // A message for a body of length bytes, which the caller writes through body before any session is given the message, and
    // releases if the write fails. The message holds one reference, the caller's.
    public SerializedMessage Rent(ushort opcode, Type messageType, int length, out Span<byte> body)
    {
        SerializedMessage? message = null;
        lock (_gate)
        {
            if (_free.Count > 0)
                message = _free.Pop();
        }

        message ??= new SerializedMessage(this, StockedBodyBytes);
        body = message.Take(opcode, messageType, length);
        return message;
    }

    public void Return(SerializedMessage message)
    {
        lock (_gate)
            _free.Push(message);
    }
}
