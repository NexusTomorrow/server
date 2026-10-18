using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Testing;

// Bytes written byte for byte, without the declaration under test. Every tagged frame GameTestClient.SendTaggedAsync,
// GameTestClient.SendTaggedUncheckedAsync and ClientFrames.TaggedEnvelopeBody send travels in one, malformed ones included (a lying
// length, a wrapper inside a wrapper, a body that does not read as its declaration). It is only written: a decoder reads the bytes
// as the declaration their opcode names.
public sealed class RawBody(byte[] bytes) : IWireStruct
{
    public void Read(ref BitReader reader) => throw new NotSupportedException("A raw body is only written.");

    public void Write(ref BitWriter writer) => writer.WriteBytes(bytes);

    public int MeasureBits() => bytes.Length * 8;

    public string? FindUnverifiedValue() => null;

    public bool IsDefault() => bytes.Length == 0;
}
