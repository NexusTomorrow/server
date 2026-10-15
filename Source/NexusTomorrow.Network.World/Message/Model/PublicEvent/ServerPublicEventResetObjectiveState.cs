using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

// Finds all objectives on the PublicEvent not associated with the provided PublicEventTeamId and resets the objective's state
[Message(GameMessageOpcode.ServerPublicEventResetObjectiveState)]
[WireMessage(GameMessageOpcode.ServerPublicEventResetObjectiveState, Direction.ServerToClient)]
public sealed partial class ServerPublicEventResetObjectiveState : IWritable
{
    [UInt(14)] public uint PublicEventId { get; set; }
    [UInt(14)] public uint PublicEventTeamId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PublicEventId, 14u);
        writer.Write(PublicEventTeamId, 14u);
    }
}
