using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Asks to make a member the group's leader: the request GroupLib.Promote sends.
/// </summary>
[WireMessage(GameMessageOpcode.Client0446, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GroupPromote
{
    /// <summary>
    /// The player's current group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

    /// <summary>
    /// The member to promote; the client never sends the player's own member.
    /// </summary>
    public RealmScopedId MemberId { get; set; } = new();
}
