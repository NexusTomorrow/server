using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Makes an episode the player's current one.
/// </summary>
/// <remarks>
/// For any id but 0 the client makes that episode current, creating it when it holds none under the id, and raises PlayerPathRefresh. It
/// handles 0 another way.
/// </remarks>
[WireMessage(GameMessageOpcode.Server06BF, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class PathCurrentEpisodeSet
{
    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(15)] public ushort Unknown0 { get; set; }

    /// <summary>
    /// The episode, by the id a PathMission row names in its pathEpisodeId; the client files episodes under it
    /// (<see cref="PathEpisodeProgress.EpisodeId"/>).
    /// </summary>
    [UInt(14)] public ushort EpisodeId { get; set; }
}
