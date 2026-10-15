using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientP2PTradingInitiateTrade, Direction.ClientToServer)]
public sealed partial class ClientP2PTradingInitiateTrade
{
    [UInt(32)] public uint TargetUnitId { get; private set; }
}
