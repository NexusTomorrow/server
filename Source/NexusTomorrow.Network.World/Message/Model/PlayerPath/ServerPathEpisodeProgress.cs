using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathEpisodeProgress, Direction.ServerToClient)]
public sealed partial class ServerPathEpisodeProgress
{
    [UInt(14)] public ushort EpisodeId { get; set; }
    [Array(CountBits = 16)] public List<MissionProgress> Missions { get; set; } = [];
}
