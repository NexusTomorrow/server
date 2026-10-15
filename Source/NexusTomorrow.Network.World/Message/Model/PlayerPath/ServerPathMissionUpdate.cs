using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathMissionUpdate)]
[WireMessage(GameMessageOpcode.ServerPathMissionUpdate, Direction.ServerToClient)]
public sealed partial class ServerPathMissionUpdate : IWritable
{
    public MissionProgress Mission { get; set; } = new();

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Mission.PathMissionId, 15);
        writer.Write(Mission.Completed);
        writer.Write(Mission.ObjectiveCompletionFlags);
        writer.Write(Mission.StateFlags);
    }
}
