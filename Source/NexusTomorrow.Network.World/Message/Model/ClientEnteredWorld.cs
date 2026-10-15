using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientEnteredWorld, Direction.ClientToServer)]
public sealed partial class ClientEnteredWorld
{
    [UInt(15)] public ushort WorldZoneId { get; private set; } // 15
}
