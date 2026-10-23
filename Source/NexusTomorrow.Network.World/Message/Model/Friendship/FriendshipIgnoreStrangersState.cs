using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets whether the player ignores invites from strangers.
/// </summary>
[WireMessage(GameMessageOpcode.Server03A5, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipIgnoreStrangersState
{
    /// <summary>
    /// Whether the player ignores invites from strangers, which FriendshipLib.GetPersonalStatus returns as bIgnoreStrangerInvites.
    /// </summary>
    [UInt(32)] public uint IgnoreStrangers { get; set; }
}
