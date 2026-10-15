using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

// Used to scale the required amount for objectives of an event
[WireMessage(GameMessageOpcode.ServerPublicEventTeamMultiplier, Direction.ServerToClient)]
public sealed partial class ServerPublicEventTeamMultiplier
{
    [UInt(14)] public uint EventId { get; set; }
    [UInt(32)] public uint TeamMultiplier { get; set; }
}
