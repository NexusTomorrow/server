using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Answers an account friend invite.
/// </summary>
[WireMessage(GameMessageOpcode.Client03A1, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipAccountInviteResponse
{
    /// <summary>
    /// The invite, by <see cref="FriendshipAccountInviteList.FriendshipAccountInvite.Id"/>.
    /// </summary>
    [UInt(64)] public ulong Id { get; private set; }

    /// <summary>
    /// True to accept the invite, false to decline it.
    /// </summary>
    [UInt(1)] public bool Accepted { get; private set; }
}
