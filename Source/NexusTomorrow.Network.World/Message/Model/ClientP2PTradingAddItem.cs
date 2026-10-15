using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientP2PTradingAddItem, Direction.ClientToServer)]
public sealed partial class ClientP2PTradingAddItem
{
    [UInt(64)] public ulong ItemGuid { get; private set; }
}
