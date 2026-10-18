using System.Buffers;

namespace NexusTomorrow.Transport.Tests;

internal static class Segments
{
    public static ReadOnlySequence<byte> Of(params byte[][] parts)
    {
        if (parts.Length == 1)
            return new ReadOnlySequence<byte>(parts[0]);

        var first = new Segment(parts[0], 0);
        Segment last = first;
        for (int i = 1; i < parts.Length; i++)
            last = last.Append(parts[i]);
        return new ReadOnlySequence<byte>(first, 0, last, last.Memory.Length);
    }

    public static ReadOnlySequence<byte> OneBytePerSegment(byte[] bytes) =>
        Of([.. bytes.Select(b => new[] { b })]);

    private sealed class Segment : ReadOnlySequenceSegment<byte>
    {
        public Segment(byte[] bytes, long runningIndex)
        {
            Memory = bytes;
            RunningIndex = runningIndex;
        }

        public Segment Append(byte[] bytes)
        {
            var next = new Segment(bytes, RunningIndex + Memory.Length);
            Next = next;
            return next;
        }
    }
}
