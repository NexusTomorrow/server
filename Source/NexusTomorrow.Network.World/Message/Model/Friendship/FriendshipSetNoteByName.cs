using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets a friendship's note, by the character's name.
/// </summary>
[WireMessage(GameMessageOpcode.Client03C8, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class FriendshipSetNoteByName
{
    [WideString] public string Unknown0 { get; private set; } = "";
    [WideString] public string Unknown1 { get; private set; } = "";

    /// <summary>
    /// The new note.
    /// </summary>
    [WideString] public string Note { get; private set; } = "";
}
