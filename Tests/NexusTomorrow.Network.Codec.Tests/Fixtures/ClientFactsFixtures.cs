using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Codec.Tests.Fixtures;

// These two declarations carry real client layouts, so the facts comparison is tested against real
// rows: 0x009A is 32, 16, 32, 1 bits; 0x0104 is 14, 32, 32 bits.

[WireMessage(GameMessageOpcode.Client009A, Direction.ClientToServer)]
public sealed partial class Client009AFixture
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(16)] public ushort Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(1)] public bool Unknown3 { get; set; }
}

[WireMessage(GameMessageOpcode.Server0104, Direction.ServerToClient)]
public sealed partial class Server0104Fixture
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
}
