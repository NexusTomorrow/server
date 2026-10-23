using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Removes a friend invite.
/// </summary>
/// <remarks>
/// The client raises FriendshipInviteRemoved.
/// </remarks>
[WireMessage(GameMessageOpcode.Server03BB, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipInviteRemove
{
    /// <summary>
    /// The invite, by <see cref="FriendshipInviteList.FriendshipInvite.Id"/>.
    /// </summary>
    [UInt(64)] public ulong Id { get; set; }
}
