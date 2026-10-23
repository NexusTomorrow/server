using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Updates one mission's progress.
/// </summary>
/// <remarks>
/// The client finds the mission's episode under the PathMission row's pathEpisodeId (<see cref="PathEpisodeProgress.EpisodeId"/>), and
/// the mission in it.
/// </remarks>
[WireMessage(GameMessageOpcode.Server06BB, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class PathMissionUpdate
{
    /// <summary>
    /// The mission, by its PathMission id, and its progress.
    /// </summary>
    public PathMissionProgress Mission { get; set; } = new();
}
