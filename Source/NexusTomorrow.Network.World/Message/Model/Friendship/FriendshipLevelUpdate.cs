using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets the level of a friendship's character.
/// </summary>
/// <remarks>
/// The client raises FriendshipUpdate.
/// </remarks>
[WireMessage(GameMessageOpcode.Server03C6, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipLevelUpdate
{
    /// <summary>
    /// The friendship, by <see cref="Friendship.CharacterId"/>.
    /// </summary>
    public RealmScopedId CharacterId { get; set; } = new();

    /// <summary>
    /// The character's level, which add-ons read as nLevel.
    /// </summary>
    [UInt(32)] public uint Level { get; set; }
}
