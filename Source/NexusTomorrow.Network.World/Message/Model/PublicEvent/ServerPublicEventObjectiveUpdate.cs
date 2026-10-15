using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[WireMessage(GameMessageOpcode.ServerPublicEventObjectiveUpdate, Direction.ServerToClient)]
public sealed partial class ServerPublicEventObjectiveUpdate
{
    public PublicEventObjective Objective { get; set; }
}
