using NexusTomorrow.Game.Static.Friendship;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Changes a friendship's kind.
/// </summary>
/// <remarks>
/// The client requests the character's details again and raises FriendshipUpdate.
/// </remarks>
[WireMessage(GameMessageOpcode.Server03CB, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipTypeUpdate
{
    /// <summary>
    /// The friendship, by <see cref="FriendshipCharacter.Id"/>.
    /// </summary>
    [UInt(64)] public ulong Id { get; set; }

    /// <summary>
    /// The new kind of friendship, as <see cref="FriendshipCharacter.Type"/> gives it.
    /// </summary>
    [UInt(4)] public FriendshipType Type { get; set; }
}
