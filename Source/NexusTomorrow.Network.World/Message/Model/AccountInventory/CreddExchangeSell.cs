using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Client026B, Direction.ClientToServer)]
public sealed partial class CreddExchangeSell
{
    public RealmScopedId Unknown0 { get; private set; } = new();
    [UInt(64)] public ulong Unknown1 { get; private set; }
    [UInt(1)] public bool Unknown2 { get; private set; }
}
