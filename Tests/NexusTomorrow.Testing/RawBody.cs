using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Testing;

// A message body that writes its bytes as they are, for frames the declarations cannot express (a tagged message holding a lying
// length, a wrapper inside a wrapper). A decoder never reads one.
public sealed class RawBody(byte[] bytes) : IWireStruct
{
    public void Read(ref BitReader reader) => throw new NotSupportedException("A raw body is only written.");

    public void Write(ref BitWriter writer) => writer.WriteBytes(bytes);

    public int MeasureBits() => bytes.Length * 8;

    public string? FindUnverifiedValue() => null;

    public bool IsDefault() => bytes.Length == 0;
}
