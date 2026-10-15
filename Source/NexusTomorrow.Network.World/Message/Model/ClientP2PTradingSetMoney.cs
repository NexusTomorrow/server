using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientP2PTradingSetMoney, Direction.ClientToServer)]
public sealed partial class ClientP2PTradingSetMoney
{
    [UInt(32)] public ulong Credits { get; private set; }
}
