using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets the level of a friendship's character.
/// </summary>
/// <remarks>
/// The client raises FriendshipUpdate. Any answer to the client's player-info request (<see cref="Info.Client0597"/>) overwrites the
/// friend's level and last-online time, so this level must agree with what the server answers there.
/// </remarks>
[WireMessage(GameMessageOpcode.Server03C6, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipLevelUpdate
{
    /// <summary>
    /// The friendship, by <see cref="FriendshipCharacter.CharacterId"/>.
    /// </summary>
    public RealmScopedId CharacterId { get; set; } = new();

    /// <summary>
    /// The character's level, which add-ons read as nLevel.
    /// </summary>
    [UInt(32)] public uint Level { get; set; }
}
