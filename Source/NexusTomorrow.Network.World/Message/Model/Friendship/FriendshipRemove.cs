using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Removes a friendship from the player's list.
/// </summary>
/// <remarks>
/// The client raises FriendshipRemove and then FriendshipPostRemove.
/// </remarks>
[WireMessage(GameMessageOpcode.Server03C3, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipRemove
{
    /// <summary>
    /// The friendship, by <see cref="FriendshipCharacter.Id"/>.
    /// </summary>
    [UInt(64)] public ulong Id { get; set; }
}
