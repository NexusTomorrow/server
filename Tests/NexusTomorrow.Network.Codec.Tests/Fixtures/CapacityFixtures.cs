namespace NexusTomorrow.Network.Codec.Tests.Fixtures;

[WireStruct]
public sealed partial class CapacityFixture
{
    [Array(CountBits = 8, Capacity = 3), UInt(8)] public List<byte> Values { get; } = [];
}
