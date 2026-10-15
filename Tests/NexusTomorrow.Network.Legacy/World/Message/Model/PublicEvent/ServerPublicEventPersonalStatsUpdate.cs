using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[Message(GameMessageOpcode.ServerPublicEventPersonalStatsUpdate)]
public class ServerPublicEventPersonalStatsUpdate : PublicEventStats
{
    // Not seen in sniffs as there are other options for sending the same data but this is still usable.
}
