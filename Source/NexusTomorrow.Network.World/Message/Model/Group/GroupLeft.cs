using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Takes the player out of a group.
/// </summary>
/// <remarks>
/// For its current group the client raises Group_Left with <see cref="Reason"/> and makes its previous group current.
/// </remarks>
[WireMessage(GameMessageOpcode.Server042D, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GroupLeft
{
    /// <summary>
    /// The group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

    /// <summary>
    /// Why, a GroupLib.RemoveReason value.
    /// </summary>
    [UInt(4)] public byte Reason { get; set; }
}
