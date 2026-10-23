using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Marks an account friend invite seen: the request AccountInviteMarkSeen sends.
/// </summary>
/// <remarks>
/// The client sets bit 0 of the invite's <see cref="FriendshipAccountInviteList.FriendshipAccountInvite.Flags"/> itself.
/// </remarks>
[WireMessage(GameMessageOpcode.Client03A2, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipAccountInviteMarkSeen
{
    /// <summary>
    /// The invite, by <see cref="FriendshipAccountInviteList.FriendshipAccountInvite.Id"/>.
    /// </summary>
    [UInt(64)] public ulong Id { get; private set; }
}
