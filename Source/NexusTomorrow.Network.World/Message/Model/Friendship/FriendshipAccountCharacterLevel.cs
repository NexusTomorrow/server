using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets the level of a character on an account friend's account.
/// </summary>
/// <remarks>
/// The client raises FriendshipAccountCharacterLevelUpdate only when the level changed.
/// </remarks>
[WireMessage(GameMessageOpcode.Server03B2, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipAccountCharacterLevel
{
    /// <summary>
    /// The account friend, by <see cref="FriendshipAccountList.FriendshipAccount.AccountId"/>.
    /// </summary>
    [UInt(32)] public uint AccountId { get; set; }

    /// <summary>
    /// The character, by <see cref="FriendshipAccountCharacter.CharacterId"/>.
    /// </summary>
    public RealmScopedId CharacterId { get; set; } = new();

    /// <summary>
    /// The character's new level.
    /// </summary>
    [UInt(8)] public uint Level { get; set; }
}
