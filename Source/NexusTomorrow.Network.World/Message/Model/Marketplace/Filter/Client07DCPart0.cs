using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace.Filter;

[WireStruct]
public sealed partial class Client07DCPart0 : IClient07DCUnion0
{
    [UInt(3)] public byte Unknown0 { get; private set; }
    [Union(nameof(Unknown0))] public IClient07DCUnion0 Unknown1 { get; private set; } = new Client07DCUnion0Arm00();
}
