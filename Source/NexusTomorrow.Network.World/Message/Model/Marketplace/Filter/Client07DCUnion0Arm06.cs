using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace.Filter;

[WireStruct]
[UnionCase(6)]
public sealed partial class Client07DCUnion0Arm06 : IClient07DCUnion0
{
    [UInt(8)] public byte Unknown0 { get; private set; }
    [UInt(6)] public byte Unknown1 { get; set; }
    [UInt(8)] public byte Unknown2 { get; private set; }
    [UInt(6)] public byte Unknown3 { get; set; }
    [UInt(14)] public ushort Unknown4 { get; private set; }
    [UInt(32)] public uint Unknown5 { get; private set; }
}
