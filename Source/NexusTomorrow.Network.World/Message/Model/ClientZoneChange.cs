using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientZoneChange, Direction.ClientToServer)]
public sealed partial class ClientZoneChange
{
    [UInt(15)] public ushort PreviousZoneId { get; private set; }
    [UInt(15)] public ushort NewZoneId { get; private set; }
}
