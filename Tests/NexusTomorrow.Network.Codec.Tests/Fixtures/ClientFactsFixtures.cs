using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Codec.Tests.Fixtures;

// These three declarations carry real client layouts, so the facts comparison is tested against
// real rows: 0x009A is 32, 16, 32, 1 bits; 0x0104 is 14, 32, 32 bits; 0x01C3 is 14, 64, a wide
// string, a 5-bit count of 37-bit elements, then 16 bits.

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

[WireMessage(GameMessageOpcode.Client01C3, Direction.ClientToServer)]
public sealed partial class Client01C3Fixture
{
    public Client01C3Part0Fixture Unknown0 { get; set; } = new();
    [WideString] public string Unknown1 { get; set; } = "";
    [Array(CountBits = 5)] public List<Client01C3Part1Fixture> Unknown2 { get; } = [];
    [UInt(16)] public ushort Unknown3 { get; set; }
}

[WireStruct]
public sealed partial class Client01C3Part0Fixture
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    [UInt(64)] public ulong Unknown1 { get; set; }
}

[WireStruct]
public sealed partial class Client01C3Part1Fixture
{
    [UInt(4)] public byte Unknown0 { get; set; }
    [UInt(16)] public ushort Unknown1 { get; set; }
    [UInt(16)] public ushort Unknown2 { get; set; }
    [Union(nameof(Unknown0))] public IClient01C3Union0Fixture Unknown3 { get; set; } = new Client01C3Union0Arm00Fixture();
}

public interface IClient01C3Union0Fixture : IWireStruct
{
}

[WireStruct]
[UnionCase(0)]
public sealed partial class Client01C3Union0Arm00Fixture : IClient01C3Union0Fixture
{
    [UInt(1)] public bool Unknown0 { get; set; }
}

[WireStruct]
[UnionCase(2)]
public sealed partial class Client01C3Union0Arm02Fixture : IClient01C3Union0Fixture
{
    [UInt(18)] public uint Unknown0 { get; set; }
}
