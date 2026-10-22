using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Removes a member from one of the player's groups.
/// </summary>
/// <remarks>
/// For its current group the client raises Group_Remove with the member's name and <see cref="Reason"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0450, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GroupRemove
{
    /// <summary>
    /// The group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown1 { get; set; }

    /// <summary>
    /// The member to remove.
    /// </summary>
    public RealmScopedId MemberId { get; set; } = new();

    /// <summary>
    /// Why, a GroupLib.RemoveReason value.
    /// </summary>
    [UInt(4)] public byte Reason { get; set; }
}
