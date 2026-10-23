using NexusTomorrow.Game.Static.Friendship;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Removes a friendship with a character, by the character's name.
/// </summary>
[WireMessage(GameMessageOpcode.Client03C2, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipRemoveByName
{
    /// <summary>
    /// The character's name.
    /// </summary>
    [WideString] public string CharacterName { get; private set; } = "";

    /// <summary>
    /// The name of the character's realm.
    /// </summary>
    [WideString] public string RealmName { get; private set; } = "";

    /// <summary>
    /// The kind of friendship to remove.
    /// </summary>
    [UInt(4)] public FriendshipType Type { get; private set; }
}
