using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets the player's public note.
/// </summary>
[WireMessage(GameMessageOpcode.Client03AC, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipSetPublicNote
{
    /// <summary>
    /// The public note.
    /// </summary>
    [WideString] public string PublicNote { get; private set; } = "";
}
