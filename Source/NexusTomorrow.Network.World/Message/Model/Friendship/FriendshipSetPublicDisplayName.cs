using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets the player's public display name.
/// </summary>
[WireMessage(GameMessageOpcode.Client039B, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipSetPublicDisplayName
{
    /// <summary>
    /// The display name.
    /// </summary>
    [WideString] public string DisplayName { get; private set; } = "";
}
