using NexusTomorrow.Game.Static.Item;
using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace.Filter;

[WireStruct]
[UnionCase(3)]
public sealed partial class Client07DCUnion0Arm03 : IClient07DCUnion0
{
    [UInt(4)] public byte Unknown0 { get; private set; }
    [UInt(4)] public byte Unknown1 { get; private set; }
}
