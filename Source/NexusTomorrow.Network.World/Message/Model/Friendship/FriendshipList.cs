using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Lists the player's friendships with characters.
/// </summary>
/// <remarks>
/// The client files each friendship and requests its character's details and realm, then raises FriendshipLoaded.
/// </remarks>
[WireMessage(GameMessageOpcode.Server03BE, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipList
{
    /// <summary>
    /// The friendships.
    /// </summary>
    [Array(CountBits = 16)] public List<FriendshipCharacter> Friends { get; set; } = [];
}
