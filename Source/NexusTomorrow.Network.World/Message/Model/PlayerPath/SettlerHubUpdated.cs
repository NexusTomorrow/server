using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Updates a settler hub's value on each of its three avenues.
/// </summary>
/// <remarks>
/// The client keeps the three values on the hub and raises SettlerHubUpdated. PlayerPathLib.GetSettlerHubValues returns each, capped by
/// the hub row's maximum for that avenue.
/// </remarks>
[WireMessage(GameMessageOpcode.Server08B7, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class SettlerHubUpdated
{
    /// <summary>
    /// The hub, by id.
    /// </summary>
    [UInt(14)] public uint HubId { get; set; }

    /// <summary>
    /// The hub's economy value.
    /// </summary>
    [UInt(32)] public uint Economy { get; set; }

    /// <summary>
    /// The hub's security value.
    /// </summary>
    [UInt(32)] public uint Security { get; set; }

    /// <summary>
    /// The hub's quality of life value.
    /// </summary>
    [UInt(32)] public uint QualityOfLife { get; set; }
}
