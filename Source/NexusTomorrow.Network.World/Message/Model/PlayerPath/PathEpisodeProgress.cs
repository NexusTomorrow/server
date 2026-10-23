using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server06B5, Direction.ServerToClient)]
public sealed partial class PathEpisodeProgress
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    [Array(CountBits = 16)] public List<PathMissionProgress> Unknown1 { get; set; } = [];
}
