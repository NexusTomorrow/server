using System.Buffers;
using NexusTomorrow.Network.Auth.Message.Model;

namespace NexusTomorrow.Transport.Tests;

public sealed class EnvelopeReaderTests
{
    [Fact]
    public void AnEnvelopeWrittenByTheWriterOpensToItsInnerMessage()
    {
        var cipher = new PacketCipher(PacketCipher.BuildKey);
        var output = new ArrayBufferWriter<byte>();
        FrameWriter.WriteEnvelope(output, 0x0076, 0x0591, new LoginAccepted { DroppedForLag = 7 }, cipher);

        byte[] blob = output.WrittenSpan[FrameReader.HeaderBytes..].ToArray();
        Assert.True(EnvelopeReader.TryOpen(blob, cipher, out ushort opcode, out Span<byte> body));
        Assert.Equal(0x0591, opcode);
        Assert.Equal(new byte[] { 7, 0, 0, 0 }, body.ToArray());
    }

    [Fact]
    public void ANestedBlobOpensWithoutACipher()
    {
        byte[] blob = Convert.FromHexString("09000000" + "5B02" + "AABBCC");
        Assert.True(EnvelopeReader.TryOpen(blob, cipher: null, out ushort opcode, out Span<byte> body));
        Assert.Equal(0x025B, opcode);
        Assert.Equal(new byte[] { 0xAA, 0xBB, 0xCC }, body.ToArray());
    }

    [Theory]
    [InlineData("0500000000")]          // a length below the inner header
    [InlineData("0A000000" + "5B02AA")] // a length past the blob
    [InlineData("06000000" + "5B02FF")] // a trailing byte past the length
    public void ABlobWhoseLengthIsNotItsOwnIsRefused(string hex)
    {
        Assert.False(EnvelopeReader.TryOpen(Convert.FromHexString(hex), cipher: null, out _, out _));
    }
}
