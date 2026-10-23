using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Activates path missions.
/// </summary>
[WireMessage(GameMessageOpcode.Server06BA, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class PathMissionsActivated
{
    /// <summary>
    /// The missions, each record led by its PathMission id.
    /// </summary>
    [Array(CountBits = 32)] public List<Server06BAPart0> Missions { get; set; } = [];
}
