using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Removes an account friend invite.
/// </summary>
[WireMessage(GameMessageOpcode.Server03A0, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipAccountInviteRemove
{
    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown0 { get; set; }

    /// <summary>
    /// The invite, by <see cref="FriendshipAccountInviteList.FriendshipAccountInvite.Id"/>.
    /// </summary>
    [UInt(64)] public ulong Id { get; set; }
}
