using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Adds one friendship to the player's list.
/// </summary>
/// <remarks>
/// The client files it and requests its character's details as it does for each of <see cref="FriendshipList.Friends"/>, then raises
/// FriendshipAdd.
/// </remarks>
[WireMessage(GameMessageOpcode.Server03B5, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipAdd
{
    /// <summary>
    /// The friendship.
    /// </summary>
    public FriendshipCharacter Friend { get; set; } = new();
}
