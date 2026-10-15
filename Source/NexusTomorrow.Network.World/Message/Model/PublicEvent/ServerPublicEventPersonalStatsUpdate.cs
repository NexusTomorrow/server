using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

// Not seen in sniffs as there are other options for sending the same data but this is still usable.
[WireMessage(GameMessageOpcode.ServerPublicEventPersonalStatsUpdate, Direction.ServerToClient)]
public sealed partial class ServerPublicEventPersonalStatsUpdate
{
    [UInt(14)] public uint PublicEventId { get; set; }
    public PublicEventStats Stats { get; set; } = new();
}
