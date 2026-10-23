using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets a friendship's note, by the character's name.
/// </summary>
[WireMessage(GameMessageOpcode.Client03C8, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipSetNoteByName
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
    /// The new note.
    /// </summary>
    [WideString] public string Note { get; private set; } = "";
}
