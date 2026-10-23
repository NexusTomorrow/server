using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets the player's presence.
/// </summary>
[WireMessage(GameMessageOpcode.Client03A7, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipPresenceChange
{
    /// <summary>
    /// The presence the player has chosen, which FriendshipLib.GetPersonalStatus returns as nPresenceState.
    /// </summary>
    [UInt(3)] public AccountPresenceState Presence { get; private set; }
}
