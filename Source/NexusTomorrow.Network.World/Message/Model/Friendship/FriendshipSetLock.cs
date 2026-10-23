using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets the player's friendship lock: the request FriendshipLib.SetLock sends, and its slash command.
/// </summary>
[WireMessage(GameMessageOpcode.Client00B8, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipSetLock
{
    /// <summary>
    /// True to set the lock, false to clear it.
    /// </summary>
    [UInt(1)] public bool Locked { get; private set; }
}
