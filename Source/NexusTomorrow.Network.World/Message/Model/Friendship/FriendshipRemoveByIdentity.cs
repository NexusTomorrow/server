using NexusTomorrow.Game.Static.Friendship;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Removes a friendship with a character, by character id: the request FriendshipLib.Remove sends.
/// </summary>
[WireMessage(GameMessageOpcode.Client03C1, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipRemoveByIdentity
{
    /// <summary>
    /// The character, by realm and character id.
    /// </summary>
    public RealmScopedId CharacterId { get; private set; } = new();

    /// <summary>
    /// The kind of friendship to remove.
    /// </summary>
    [UInt(4)] public FriendshipType Type { get; private set; }
}
