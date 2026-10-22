using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Leaves or disbands the player's current group: the request GroupLib.LeaveGroup and GroupLib.DisbandGroup send.
/// </summary>
[WireMessage(GameMessageOpcode.Client042B, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GroupLeave
{
    /// <summary>
    /// The player's current group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

    /// <summary>
    /// True from GroupLib.DisbandGroup, false from GroupLib.LeaveGroup.
    /// </summary>
    [UInt(1)] public bool Disband { get; set; }
}
