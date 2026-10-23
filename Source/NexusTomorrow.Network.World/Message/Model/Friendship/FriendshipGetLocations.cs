using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Asks for the zones the player's friends are in: the request FriendshipLib.GetLocations sends.
/// </summary>
[WireMessage(GameMessageOpcode.Client03BF, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipGetLocations
{
    /// <summary>
    /// The friendships, by <see cref="FriendshipCharacter.CharacterId"/>.
    /// </summary>
    [Array(CountBits = 8)] public List<RealmScopedId> CharacterIds { get; private set; } = [];

    /// <summary>
    /// The account friends, by <see cref="FriendshipAccountList.FriendshipAccount.Id"/>.
    /// </summary>
    [Array(CountBits = 8), UInt(64)] public List<ulong> AccountFriendIds { get; private set; } = [];
}
