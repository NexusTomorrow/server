using NexusTomorrow.Game.Static.Friendship;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Adds a friendship with a character, by the character's name.
/// </summary>
/// <remarks>
/// The client sends it only with a note that passes its check, and otherwise reports InvalidNote itself.
/// </remarks>
[WireMessage(GameMessageOpcode.Client03B4, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipAddByName
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
    /// The kind of friendship to add.
    /// </summary>
    [UInt(4)] public FriendshipType Type { get; private set; }

    /// <summary>
    /// The friendship's note.
    /// </summary>
    [WideString] public string Note { get; private set; } = "";
}
