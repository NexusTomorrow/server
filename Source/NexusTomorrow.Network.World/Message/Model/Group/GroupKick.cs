using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Asks to remove a member from the group: the request GroupLib.Kick sends.
/// </summary>
[WireMessage(GameMessageOpcode.Client0428, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GroupKick
{
    /// <summary>
    /// The player's current group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

    /// <summary>
    /// The member to remove; the client never sends the player's own member.
    /// </summary>
    public RealmScopedId MemberId { get; set; } = new();
}
