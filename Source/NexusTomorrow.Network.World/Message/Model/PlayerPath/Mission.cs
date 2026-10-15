using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

public class Mission : IWritable
{
    public uint PathMissionId { get; set; }
    public bool Completed { get; set; }
    public uint ObjectiveCompletionFlags { get; set; }
    public uint StateFlags { get; set; }
    public PathMissionState State { get; set; }
    public uint GiverUnitId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PathMissionId, 15);
        writer.Write(Completed);
        writer.Write(ObjectiveCompletionFlags);
        writer.Write(StateFlags);
        writer.Write(State, 3);
        writer.Write(GiverUnitId);
    }
}
