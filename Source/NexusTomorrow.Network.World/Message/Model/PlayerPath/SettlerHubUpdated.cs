using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Updates a settler hub's three values.
/// </summary>
/// <remarks>
/// The client keeps the three values on the hub and raises SettlerHubUpdated.
/// </remarks>
[WireMessage(GameMessageOpcode.Server08B7, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class SettlerHubUpdated
{
    /// <summary>
    /// The hub, by id.
    /// </summary>
    [UInt(14)] public uint HubId { get; set; }

    [UInt(32)] public uint Unknown1 { get; set; }

    [UInt(32)] public uint Unknown2 { get; set; }

    [UInt(32)] public uint Unknown3 { get; set; }
}
