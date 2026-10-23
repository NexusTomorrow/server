using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Replaces a friendship's note.
/// </summary>
/// <remarks>
/// The client raises FriendshipUpdate.
/// </remarks>
[WireMessage(GameMessageOpcode.Server03C9, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipNoteUpdate
{
    /// <summary>
    /// The friendship, by <see cref="FriendshipCharacter.Id"/>.
    /// </summary>
    [UInt(64)] public ulong Id { get; set; }

    /// <summary>
    /// The new note, which add-ons read as strNote.
    /// </summary>
    [WideString] public string Note { get; set; } = "";
}
