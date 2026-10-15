using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientP2PTradingRemoveItem, Direction.ClientToServer)]
public sealed partial class ClientP2PTradingRemoveItem
{
    [UInt(64)] public ulong ItemGuid { get; private set; }
}
