using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientEntityInteractChair, Direction.ClientToServer)]
public sealed partial class ClientEntityInteractChair
{
    [UInt(32)] public uint ChairUnitId { get; private set; }
    [UInt(1)] public bool Remove { get; private set; }
}
