using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Removes a character from an account friend's characters.
/// </summary>
/// <remarks>
/// The client raises FriendshipAccountDataUpdate.
/// </remarks>
[WireMessage(GameMessageOpcode.Server039A, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipAccountCharacterRemoved
{
    /// <summary>
    /// The account friend, by <see cref="FriendshipAccountList.FriendshipAccount.AccountId"/>.
    /// </summary>
    [UInt(32)] public uint AccountId { get; set; }

    /// <summary>
    /// The character, by <see cref="FriendshipAccountCharacter.CharacterId"/>.
    /// </summary>
    public RealmScopedId CharacterId { get; set; } = new();
}
