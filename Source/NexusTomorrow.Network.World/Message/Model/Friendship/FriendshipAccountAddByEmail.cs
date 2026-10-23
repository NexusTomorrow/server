using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Adds an account friend by email address.
/// </summary>
/// <remarks>
/// The client sends it only when both strings pass its checks.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0397, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipAccountAddByEmail
{
    /// <summary>
    /// The account's email address.
    /// </summary>
    [WideString] public string Email { get; private set; } = "";

    /// <summary>
    /// The note that goes with the request.
    /// </summary>
    [WideString] public string Note { get; private set; } = "";
}
