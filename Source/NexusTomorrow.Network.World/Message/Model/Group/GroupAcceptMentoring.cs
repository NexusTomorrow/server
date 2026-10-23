using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Asks to mentor a group member: the request GroupLib.AcceptMentoring sends.
/// </summary>
/// <remarks>
/// The client sends it only for a member who is not the player and not already the player's mentee.
/// </remarks>
[WireMessage(GameMessageOpcode.Client043E, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GroupAcceptMentoring
{
    /// <summary>
    /// The player's current group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

    /// <summary>
    /// The member to mentor.
    /// </summary>
    public RealmScopedId MenteeId { get; set; } = new();
}
