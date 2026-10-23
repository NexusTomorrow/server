using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Updates the status of one of a settler hub's improvements.
/// </summary>
/// <remarks>
/// The client finds the improvement in the hub under its id, as it does for each of <see cref="SettlerBuildStatusList"/>, and raises
/// SettlerBuildStatusUpdate for the hub the player is using.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0671, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class SettlerBuildStatus
{
    /// <summary>
    /// The hub, by id.
    /// </summary>
    [UInt(14)] public ushort HubId { get; set; }

    /// <summary>
    /// The improvement's status, under its improvement id.
    /// </summary>
    public SettlerImprovementStatus Improvement { get; set; } = new();
}
