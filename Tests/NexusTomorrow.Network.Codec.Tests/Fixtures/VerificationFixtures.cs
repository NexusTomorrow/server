using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Codec.Tests.Fixtures;

[WireStruct]
public sealed partial class VerifiedPart
{
    [UInt(8)] public byte Speed { get; set; }
}

public interface IVerifiedUnion : IWireStruct
{
}

[WireStruct]
[UnionCase(1)]
public sealed partial class VerifiedArm : IVerifiedUnion
{
    [UInt(8)] public byte Unknown0 { get; set; }
}

[WireStruct]
[Verified(Rung.Fields)]
public sealed partial class FieldsFixture
{
    [UInt(8)] public byte Selector { get; set; }
    [Union(nameof(Selector))] public IVerifiedUnion Payload { get; set; } = new VerifiedArm();
    public VerifiedPart Part { get; set; } = new();
}

[WireStruct]
[Verified(Rung.Purpose)]
public sealed partial class PurposeFixture
{
    [UInt(8)] public byte Unknown0 { get; set; }
}

[WireStruct]
[Verified(Rung.Purpose)]
public sealed partial class ServerFFFF
{
    [UInt(8)] public byte Unknown0 { get; set; }
}

[WireStruct]
public sealed partial class UnreadFixture
{
    [UInt(8)] public byte Unknown0 { get; set; }
}

[WireStruct]
[Verified(Rung.Fields)]
public sealed partial class BitFieldFixture
{
    [UInt(1)] public bool Unknown0Bit0 { get; set; }
    [UInt(3)] public byte Unknown0Bits1To3 { get; set; }
}

[WireStruct]
[Verified(Rung.Fields)]
public sealed partial class PrivateMemberFixture
{
    [UInt(8)] public byte Shown { get; set; }
    [UInt(8)] private byte Hidden { get; set; }
}
