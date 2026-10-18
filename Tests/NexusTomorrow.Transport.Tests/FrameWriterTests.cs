using System.Buffers;
using System.Buffers.Binary;
using NexusTomorrow.Network.Auth.Message.Model;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Transport.Tests;

public sealed class FrameWriterTests
{
    private const ushort LoginEnvelopeOpcode = 0x0076;
    private const ushort LoginAcceptedOpcode = 0x0591;

    [Fact]
    public void ABareFrameCarriesItsLengthOpcodeAndBody()
    {
        var accepted = new LoginAccepted { DroppedForLag = 0x04030201 };
        var output = new ArrayBufferWriter<byte>();
        int written = FrameWriter.WriteMessage(output, LoginAcceptedOpcode, accepted);

        // 6 header bytes and a 32-bit body.
        Assert.Equal(10, written);
        Assert.Equal(Convert.FromHexString("0A00000091050102" + "0304"), output.WrittenSpan.ToArray());
    }

    [Fact]
    public void AnEnvelopeIsTheDeclaredEnvelopeAroundTheEncryptedInnerFrame()
    {
        var cipher = new PacketCipher(PacketCipher.BuildKey);
        var accepted = new LoginAccepted { DroppedForLag = 1 };
        var output = new ArrayBufferWriter<byte>();
        FrameWriter.WriteEnvelope(output, LoginEnvelopeOpcode, LoginAcceptedOpcode, accepted, cipher);

        // Expected, built from the declaration: the inner frame is length 10, opcode 0x0591, body 01 00 00 00; the opcode and body are
        // encrypted, and the declared LoginEnvelope carries them as its payload after the length.
        byte[] innerPlain = Convert.FromHexString("9105" + "01000000");
        cipher.Encrypt(innerPlain);
        var envelope = new LoginEnvelope { Payload = innerPlain };
        var expected = new ArrayBufferWriter<byte>();
        FrameWriter.WriteMessage(expected, LoginEnvelopeOpcode, envelope);

        Assert.Equal(expected.WrittenSpan.ToArray(), output.WrittenSpan.ToArray());
        Assert.Equal(16u, BinaryPrimitives.ReadUInt32LittleEndian(output.WrittenSpan));
        Assert.Equal(10u, BinaryPrimitives.ReadUInt32LittleEndian(output.WrittenSpan[6..]));
    }

    [Fact]
    public void APingFrameCarriesOneIndexByte()
    {
        var output = new ArrayBufferWriter<byte>();
        FrameWriter.WritePing(output, TransportOpcodes.PingAnswer, 5);
        Assert.Equal(Convert.FromHexString("07000000010005"), output.WrittenSpan.ToArray());
    }

    [Fact]
    public void WritingEnvelopesAllocatesNothing()
    {
        var cipher = new PacketCipher(PacketCipher.BuildKey);
        var accepted = new LoginAccepted { DroppedForLag = 1 };
        var output = new ArrayBufferWriter<byte>(64 * 1024);
        for (int i = 0; i < 10; i++)
            FrameWriter.WriteEnvelope(output, LoginEnvelopeOpcode, LoginAcceptedOpcode, accepted, cipher);
        output.ResetWrittenCount();

        long before = GC.GetAllocatedBytesForCurrentThread();
        for (int i = 0; i < 1000; i++)
        {
            FrameWriter.WriteEnvelope(output, LoginEnvelopeOpcode, LoginAcceptedOpcode, accepted, cipher);
            if (output.FreeCapacity < 1024)
                output.ResetWrittenCount();
        }

        Assert.Equal(0, GC.GetAllocatedBytesForCurrentThread() - before);
    }

    [Fact]
    public void AMessageThatWritesFewerBitsThanItMeasuresIsRefused()
    {
        var output = new ArrayBufferWriter<byte>();
        Assert.Throws<CodecException>(() => FrameWriter.WriteMessage(output, 0x7FFF, new ShortWriter()));
    }

    private sealed class ShortWriter : IWireStruct
    {
        public void Read(ref BitReader reader) => throw new NotSupportedException();

        public void Write(ref BitWriter writer) => writer.WriteBits(1, 8);

        public int MeasureBits() => 16;
    }
}
