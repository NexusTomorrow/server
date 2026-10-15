using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

// Triggers a repeating event for BonusEventsChanged
[WireMessage(GameMessageOpcode.ServerPublicEventTriggerUiUpdates, Direction.ServerToClient)]
public sealed partial class ServerPublicEventTriggerUiUpdates
{
    [UInt(32)] public uint EventId { get; set; }
    [UInt(1)] public bool Start { get; set; } // true = triggers PublicEventStart, false = triggers PublicEventCleared
}
