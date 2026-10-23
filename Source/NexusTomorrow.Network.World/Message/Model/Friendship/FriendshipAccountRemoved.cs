using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Removes an account friend.
/// </summary>
/// <remarks>
/// The client raises FriendshipAccountFriendRemoved.
/// </remarks>
[WireMessage(GameMessageOpcode.Server03B0, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipAccountRemoved
{
    /// <summary>
    /// The account friend, by <see cref="FriendshipAccountList.FriendshipAccount.Id"/>.
    /// </summary>
    [UInt(64)] public ulong Id { get; set; }
}
