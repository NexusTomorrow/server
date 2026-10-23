namespace NexusTomorrow.Network.Codec.Tests.Fixtures;

// Fixture layouts for the generated send check: an unmarked member stands for a meaning not yet verified.
[WireStruct]
public sealed partial class GateInner
{
    [VerifiedMember, UInt(8)] public byte Known { get; set; }
    [UInt(8)] public byte Unread { get; set; }
}

public interface IGateUnion : IWireStruct
{
}

[WireStruct]
[UnionCase(0)]
public sealed partial class GateArmZero : IGateUnion
{
    [UInt(8)] public byte Unread { get; set; }
}

[WireStruct]
[UnionCase(1)]
public sealed partial class GateArmOne : IGateUnion
{
    [VerifiedMember, UInt(8)] public byte Known { get; set; }
    [UInt(8)] public byte Unread { get; set; }
}

[WireStruct]
public sealed partial class GateHolder
{
    [VerifiedMember, UInt(8)] public byte Known { get; set; }
    [UInt(8)] public byte Unread { get; set; }
    [UInt(1)] public bool UnreadFlag { get; set; }
    [Float32] public float UnreadFloat { get; set; }
    [WideString] public string UnreadText { get; set; } = "";
    [Array(CountBits = 4), UInt(8)] public List<byte> UnreadList { get; set; } = [];
    [FixedArray(3), UInt(16)] public ushort[] UnreadFixed { get; set; } = new ushort[3];
    public GateInner UnreadInner { get; set; } = new();
    [VerifiedMember] public GateInner KnownInner { get; set; } = new();
    [VerifiedMember, Array(CountBits = 4)] public List<GateInner> KnownList { get; set; } = [];
    [VerifiedMember, UInt(2)] public byte KnownSelector { get; set; }
    [VerifiedMember, Union(nameof(KnownSelector))] public IGateUnion KnownUnion { get; set; } = new GateArmZero();
    [UInt(2)] public byte UnreadSelector { get; set; }
    [VerifiedMember, Union(nameof(UnreadSelector))] public IGateUnion UnionOfUnreadSelector { get; set; } = new GateArmZero();
    [UInt(2)] public byte PlainSelector { get; set; }
    [Union(nameof(PlainSelector))] public IGateUnion UnreadUnion { get; set; } = new GateArmZero();
}

[WireStruct]
public partial class GateBase
{
    [UInt(8)] public byte UnreadInBase { get; set; }
}

[WireStruct]
public sealed partial class GateDerived : GateBase
{
    [VerifiedMember, UInt(8)] public byte Known { get; set; }
}
