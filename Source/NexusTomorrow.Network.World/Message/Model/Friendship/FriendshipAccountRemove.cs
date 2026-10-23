using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Removes an account friend.
/// </summary>
[WireMessage(GameMessageOpcode.Client03AF, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipAccountRemove
{
    /// <summary>
    /// The account friend, by <see cref="FriendshipAccountList.FriendshipAccount.Id"/>.
    /// </summary>
    [UInt(64)] public ulong Id { get; private set; }
}
