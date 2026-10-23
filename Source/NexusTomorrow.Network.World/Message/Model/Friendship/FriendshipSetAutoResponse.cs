using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets the player's away and busy messages.
/// </summary>
[WireMessage(GameMessageOpcode.Client03B7, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipSetAutoResponse
{
    /// <summary>
    /// The message for when the player is away, which FriendshipLib.GetPersonalStatus returns as strAwayMsg.
    /// </summary>
    [WideString] public string AwayMessage { get; private set; } = "";

    /// <summary>
    /// The message for when the player is busy, which FriendshipLib.GetPersonalStatus returns as strBusyMsg.
    /// </summary>
    [WideString] public string BusyMessage { get; private set; } = "";
}
