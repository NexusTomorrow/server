using System.Buffers;

namespace NexusTomorrow.Transport.Tests;

public sealed class FrameReaderTests
{
    private const int Cap = 1024;

    // Length 9 (counting itself), opcode 0x0592, body AA BB CC.
    private static readonly byte[] s_frame = [0x09, 0x00, 0x00, 0x00, 0x92, 0x05, 0xAA, 0xBB, 0xCC];

    [Fact]
    public void AWholeFrameIsTaken()
    {
        ReadOnlySequence<byte> buffer = new(s_frame);
        Assert.Equal(FrameStatus.Complete, FrameReader.TryRead(ref buffer, Cap, out ushort opcode, out ReadOnlySequence<byte> body));
        Assert.Equal(0x0592, opcode);
        Assert.Equal(new byte[] { 0xAA, 0xBB, 0xCC }, body.ToArray());
        Assert.Equal(0, buffer.Length);
    }

    [Fact]
    public void ReaderTakesAFrameFedOneByteAtATime()
    {
        for (int length = 0; length < s_frame.Length; length++)
        {
            ReadOnlySequence<byte> prefix = new(s_frame[..length]);
            Assert.Equal(FrameStatus.Incomplete, FrameReader.TryRead(ref prefix, Cap, out _, out _));
        }

        ReadOnlySequence<byte> split = Segments.OneBytePerSegment(s_frame);
        Assert.Equal(FrameStatus.Complete, FrameReader.TryRead(ref split, Cap, out ushort opcode, out ReadOnlySequence<byte> body));
        Assert.Equal(0x0592, opcode);
        Assert.Equal(new byte[] { 0xAA, 0xBB, 0xCC }, body.ToArray());
    }

    [Fact]
    public void TwoFramesInOneBufferAreBothTaken()
    {
        ReadOnlySequence<byte> buffer = new([.. s_frame, .. s_frame]);
        Assert.Equal(FrameStatus.Complete, FrameReader.TryRead(ref buffer, Cap, out _, out _));
        Assert.Equal(FrameStatus.Complete, FrameReader.TryRead(ref buffer, Cap, out _, out _));
        Assert.Equal(FrameStatus.Incomplete, FrameReader.TryRead(ref buffer, Cap, out _, out _));
    }

    [Theory]
    [InlineData(0u)]
    [InlineData(5u)]
    public void ALengthShorterThanTheHeaderIsMalformed(uint length)
    {
        byte[] bytes = [(byte)length, 0, 0, 0, 0, 0, 0, 0];
        ReadOnlySequence<byte> buffer = new(bytes);
        Assert.Equal(FrameStatus.Malformed, FrameReader.TryRead(ref buffer, Cap, out _, out _));
    }

    [Fact]
    public void ALengthOverTheCapIsMalformedBeforeTheBodyArrives()
    {
        byte[] bytes = [0x01, 0x04, 0x00, 0x00]; // 1025
        ReadOnlySequence<byte> buffer = new(bytes);
        Assert.Equal(FrameStatus.Malformed, FrameReader.TryRead(ref buffer, Cap, out _, out _));
    }

    [Fact]
    public void ALengthPastSixteenBitsIsReadAtFullWidth()
    {
        byte[] bytes = new byte[70_000];
        bytes[0] = 0x70;
        bytes[1] = 0x11;
        bytes[2] = 0x01; // 70,000
        ReadOnlySequence<byte> buffer = new(bytes);
        Assert.Equal(FrameStatus.Complete, FrameReader.TryRead(ref buffer, 100_000, out _, out ReadOnlySequence<byte> body));
        Assert.Equal(70_000 - FrameReader.HeaderBytes, body.Length);
    }
}
