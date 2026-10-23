using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Files an episode with its missions' progress.
/// </summary>
/// <remarks>
/// The client keeps the episode under its id, where <see cref="PathCurrentEpisodeSet"/> and <see cref="PathMissionUpdate"/> find it.
/// </remarks>
[WireMessage(GameMessageOpcode.Server06B5, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class PathEpisodeProgress
{
    /// <summary>
    /// The episode, by the id a PathMission row names in its pathEpisodeId.
    /// </summary>
    [UInt(14)] public ushort EpisodeId { get; set; }

    /// <summary>
    /// The episode's missions, each under its PathMission id.
    /// </summary>
    [Array(CountBits = 16)] public List<PathMissionProgress> Missions { get; set; } = [];
}
