using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Marks friend invites seen: the request InviteMarkSeen sends.
/// </summary>
[WireMessage(GameMessageOpcode.Client03BD, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipInviteMarkSeen
{
    /// <summary>
    /// The invites, by <see cref="FriendshipInviteList.FriendshipInvite.Id"/>.
    /// </summary>
    [Array(CountBits = 8), UInt(64)] public List<ulong> Ids { get; private set; } = [];
}
