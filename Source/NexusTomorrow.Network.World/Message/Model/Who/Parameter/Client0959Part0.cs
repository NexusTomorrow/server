using NexusTomorrow.Game.Static.Who;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Who.Parameter;

[WireStruct]
public sealed partial class Client0959Part0
{
    [UInt(4)] public byte Unknown0 { get; private set; }
    [Union(nameof(Unknown0))] public IClient0959Union0 Unknown1 { get; private set; }
}
