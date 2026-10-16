using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace.Filter;

[WireStruct]
[UnionCase(5)]
public sealed partial class Client07DCUnion0Arm05 : IClient07DCUnion0
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
}
