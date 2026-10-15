using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Equivalence.Tests.SelfTest.Declared;

// Declarations the self-tests hold to the twins of the same names in SelfTest.Legacy. Opcodes 0xFE01 and up
// name no client message.

public enum SelfTestKind : byte
{
    None = 0,
    First = 1,
    Last = 5,
}

[WireMessage((GameMessageOpcode)0xFE01, Direction.Both)]
public sealed partial class MatchingMessage
{
    [UInt(1)] public bool Flag { get; set; }
    [UInt(3)] public byte Small { get; private set; }
    [UInt(14)] public ushort Medium { get; set; }
    [UInt(64)] public ulong UniqueId { get; set; }
    [SInt(10)] public short Offset { get; set; }
    [UInt(3)] public SelfTestKind Kind { get; set; }
    [Float32] public float Ratio { get; set; }
    [PackedFloat] public Vector3 Position { get; set; }
    [WideString] public string Name { get; set; } = "";
    public MatchingPoint Origin { get; set; } = new();
    [Array(CountBits = 3)] public List<MatchingPoint> Points { get; } = [];
    [FixedArray(3), UInt(8)] public byte[] Slots { get; set; } = new byte[3];
}

[WireStruct]
public sealed partial class MatchingPoint
{
    [UInt(8)] public byte X { get; set; }
    [UInt(12)] public ushort Y { get; set; }
}

[WireMessage((GameMessageOpcode)0xFE02, Direction.ServerToClient)]
public sealed partial class WidthMessage
{
    [UInt(8)] public byte Value { get; set; }
    [UInt(16)] public ushort Tail { get; set; }
}

[WireMessage((GameMessageOpcode)0xFE03, Direction.ClientToServer)]
public sealed partial class ReaderMessage
{
    [UInt(12)] public ushort Value { get; set; }
    [UInt(8)] public byte Tail { get; set; }
}

[WireMessage((GameMessageOpcode)0xFE04, Direction.ServerToClient)]
public sealed partial class UnionMessage
{
    [UInt(2)] public byte Kind { get; set; }
    [Union(nameof(Kind))] public IUnionArm Arm { get; set; } = new UnionArmA();
}

public interface IUnionArm : IWireStruct
{
}

[WireStruct]
[UnionCase(0)]
public sealed partial class UnionArmA : IUnionArm
{
    [UInt(4)] public byte A { get; set; }
}

[WireStruct]
[UnionCase(1)]
public sealed partial class UnionArmB : IUnionArm
{
    [UInt(8)] public byte B { get; set; }
}

[WireStruct]
[UnionCase(2)]
public sealed partial class UnionArmC : IUnionArm
{
    [UInt(6)] public byte C { get; set; }
}

[WireMessage((GameMessageOpcode)0xFE05, Direction.Both)]
public sealed partial class OrphanMessage
{
    [UInt(8)] public byte Kept { get; set; }
    [UInt(8)] public byte Extra { get; set; }
}

[WireMessage((GameMessageOpcode)0xFE06, Direction.Both)]
public sealed partial class CountPadMessage
{
    [UInt(3), Count] public byte EntryCount { get; set; }
    [Array(CountFrom = nameof(EntryCount)), UInt(16)] public List<ushort> Keys { get; } = [];
    [Pad(5)] public Pad Reserved { get; }
    [Array(CountFrom = nameof(EntryCount)), UInt(32)] public uint[] Values { get; set; } = [];
}

[WireMessage((GameMessageOpcode)0xFE07, Direction.ServerToClient)]
public sealed partial class EmptyMessage
{
    [UInt(32)] public uint TargetId { get; set; }
    [UInt(16)] public ushort Value { get; set; }
}

[WireMessage((GameMessageOpcode)0xFE08, Direction.Both)]
public sealed partial class ArmListMessage
{
    [Array(CountBits = 3)] public List<ArmEntry> Entries { get; set; } = [];
}

[WireStruct]
public sealed partial class ArmEntry
{
    [UInt(2)] public byte Kind { get; set; }
    [Union(nameof(Kind))] public IListArm Arm { get; set; } = new ListArmA();
}

[WireMessage((GameMessageOpcode)0xFE09, Direction.Both)]
public sealed partial class ComputedSelectorMessage
{
    [UInt(2)] public byte Kind { get; set; }
    [Union(nameof(Kind))] public IListArm Arm { get; set; } = new ListArmA();
}

public interface IListArm : IWireStruct
{
}

[WireStruct]
[UnionCase(0)]
public sealed partial class ListArmA : IListArm
{
    [UInt(4)] public byte A { get; set; }
}

[WireStruct]
[UnionCase(1)]
public sealed partial class ListArmB : IListArm
{
    [UInt(8)] public byte B { get; set; }
}

[WireMessage((GameMessageOpcode)0xFE0A, Direction.Both)]
public sealed partial class BitArrayMessage
{
    [UInt(8)] public byte Kind { get; set; }
    [FixedArray(4), UInt(8)] public byte[] Flags { get; set; } = new byte[4];
}

[WireMessage((GameMessageOpcode)0xFE0B, Direction.ClientToServer)]
public sealed partial class ReversedBitArrayMessage
{
    [FixedArray(4), UInt(8)] public byte[] Flags { get; set; } = new byte[4];
}
