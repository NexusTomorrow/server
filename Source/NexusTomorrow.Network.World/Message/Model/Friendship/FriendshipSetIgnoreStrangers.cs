using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets whether the player ignores invites from strangers.
/// </summary>
[WireMessage(GameMessageOpcode.Client039E, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipSetIgnoreStrangers
{
    /// <summary>
    /// True to ignore invites from strangers, false to allow them.
    /// </summary>
    [UInt(1)] public bool IgnoreStrangers { get; private set; }
}
