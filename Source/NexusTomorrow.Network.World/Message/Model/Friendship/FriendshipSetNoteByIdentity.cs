using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets a friendship's note, by the character's id.
/// </summary>
[WireMessage(GameMessageOpcode.Client03C7, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipSetNoteByIdentity
{
    /// <summary>
    /// The character, by realm and character id.
    /// </summary>
    public RealmScopedId CharacterId { get; private set; } = new();

    /// <summary>
    /// The new note.
    /// </summary>
    [WideString] public string Note { get; private set; } = "";
}
