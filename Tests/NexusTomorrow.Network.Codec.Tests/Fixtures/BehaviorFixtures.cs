using System.Numerics;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Codec.Tests.Fixtures;

// Opcodes 0xFF01 and up are not client opcodes (the facts table ends at 0x0991), so these
// declarations exercise the generator without meeting a facts row.

public enum FixtureKind : byte
{
    None = 0,
    First = 1,
    Last = 5,
}

public enum FixtureSignedKind : short
{
    Low = -2,
    Zero = 0,
    High = 1,
}

[WireMessage((GameMessageOpcode)0xFF01, Direction.Both)]
public sealed partial class ScalarsFixture
{
    [UInt(1)] public bool Flag { get; set; }
    [UInt(3)] public byte Small { get; set; }
    [UInt(14)] public ushort Medium { get; set; }
    [UInt(64)] public ulong Large { get; set; }
    [UInt(32)] public int FullWidth { get; set; }
    [SInt(10)] public short Offset { get; set; }
    [UInt(3)] public FixtureKind Kind { get; set; }
    [SInt(3)] public FixtureSignedKind SignedKind { get; set; }
    [Float32] public float Ratio { get; set; }
    [Float64] public double Precise { get; set; }
    [PackedFloat] public float Packed { get; set; }
    [Float32] public Vector3 Position { get; set; }
    [PackedFloat] public Vector3 PackedPosition { get; set; }
    [NotOnWire] public string Note { get; set; } = "";
}

[WireMessage((GameMessageOpcode)0xFF02, Direction.ServerToClient)]
public sealed partial class StringsFixture
{
    [WideString] public string Wide { get; set; } = "";
    [NarrowString] public string Narrow { get; set; } = "";
    [TerminatedWideString] public string Terminated { get; set; } = "";
}

[WireMessage((GameMessageOpcode)0xFF03, Direction.ClientToServer)]
public sealed partial class CollectionsFixture
{
    [Array(CountBits = 3), UInt(18)] public List<uint> Ids { get; } = [];
    [UInt(4), Count] public uint KeyCount { get; set; }
    [Array(CountFrom = nameof(KeyCount)), UInt(32)] public List<uint> Times { get; } = [];
    [Array(CountFrom = nameof(KeyCount)), Float32] public List<float> Values { get; } = [];
    [FixedArray(3), UInt(8)] public byte[] Fixed { get; set; } = new byte[3];
    [Array(CountBits = 2)] public List<PointFixture> Points { get; } = [];
}

[WireStruct]
public sealed partial class PointFixture
{
    [UInt(8)] public byte X { get; set; }
    [UInt(8)] public byte Y { get; set; }
}

[WireMessage((GameMessageOpcode)0xFF04, Direction.ClientToServer)]
public sealed partial class EnvelopeFixture
{
    [UInt(5)] public byte Prefix { get; set; }
    [Blob(32, Align = true, LengthIncludesSelf = true)] public byte[] Payload { get; set; } = [];
}

[WireStruct]
public partial class BaseFixture
{
    [UInt(8)] public byte BaseValue { get; set; }
}

[WireMessage((GameMessageOpcode)0xFF05, Direction.ServerToClient)]
public sealed partial class DerivedFixture : BaseFixture
{
    [Pad(12)] public Pad Reserved { get; }
    [UInt(4)] public byte DerivedValue { get; set; }
}

public sealed partial class OuterFixture
{
    [WireStruct]
    public sealed partial class InnerFixture
    {
        [UInt(6)] public byte Value { get; set; }
    }
}
