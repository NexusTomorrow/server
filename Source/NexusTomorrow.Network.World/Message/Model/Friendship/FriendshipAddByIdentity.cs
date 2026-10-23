using NexusTomorrow.Game.Static.Friendship;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Adds a friendship with a character, by character id.
/// </summary>
/// <remarks>
/// The client sends it only with a note that passes its check, and otherwise reports InvalidNote itself.
/// </remarks>
[WireMessage(GameMessageOpcode.Client03B3, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipAddByIdentity
{
    /// <summary>
    /// The character, by realm and character id.
    /// </summary>
    public RealmScopedId CharacterId { get; set; } = new();

    /// <summary>
    /// The kind of friendship to add.
    /// </summary>
    [UInt(4)] public FriendshipType Type { get; set; }

    /// <summary>
    /// The friendship's note.
    /// </summary>
    [WideString] public string Note { get; set; } = "";
}
