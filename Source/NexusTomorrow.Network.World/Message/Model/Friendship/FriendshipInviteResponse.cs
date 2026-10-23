using NexusTomorrow.Game.Static.Friendship;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Answers a friend invite: the request RespondToInvite sends.
/// </summary>
[WireMessage(GameMessageOpcode.Client03BC, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipInviteResponse
{
    /// <summary>
    /// The invite, by <see cref="FriendshipInviteList.FriendshipInvite.Id"/>.
    /// </summary>
    [UInt(64)] public ulong Id { get; private set; }

    /// <summary>
    /// The answer.
    /// </summary>
    [UInt(3)] public FriendshipResponse Response { get; private set; }
}
