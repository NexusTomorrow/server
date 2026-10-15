using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[Message(GameMessageOpcode.ServerPublicEventObjectiveUpdate)]
public class ServerPublicEventObjectiveUpdate : IWritable
{
    public PublicEventObjective Objective { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Objective.Write(writer);
    }
}
