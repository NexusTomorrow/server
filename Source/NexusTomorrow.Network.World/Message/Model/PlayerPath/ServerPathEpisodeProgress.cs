using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathEpisodeProgress)]
[WireMessage(GameMessageOpcode.ServerPathEpisodeProgress, Direction.ServerToClient)]
public sealed partial class ServerPathEpisodeProgress : IWritable
{
    [UInt(14)] public ushort EpisodeId { get; set; }
    [Array(CountBits = 16)] public List<MissionProgress> Missions { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(EpisodeId, 14);
        writer.Write(Missions.Count, 16);
        Missions.ForEach(mission =>
        {
            writer.Write(mission.PathMissionId, 15u);
            writer.Write(mission.Completed);
            writer.Write(mission.ObjectiveCompletionFlags);
            writer.Write(mission.StateFlags);
        });
    }
}
