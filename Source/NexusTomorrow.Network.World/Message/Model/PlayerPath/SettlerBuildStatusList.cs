using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Updates the status of a settler hub's improvements.
/// </summary>
/// <remarks>
/// The client finds each improvement in the hub under its id, and raises SettlerBuildStatusUpdate for the hub the player is using.
/// </remarks>
[WireMessage(GameMessageOpcode.Server066E, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class SettlerBuildStatusList
{
    /// <summary>
    /// The hub, by id.
    /// </summary>
    [UInt(14)] public ushort HubId { get; set; }

    /// <summary>
    /// The improvements' status, each under its improvement id.
    /// </summary>
    [Array(CountBits = 32)] public List<SettlerImprovementStatus> Improvements { get; set; } = [];
}
