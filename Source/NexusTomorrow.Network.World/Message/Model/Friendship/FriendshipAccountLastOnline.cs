using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets when an account was last online.
/// </summary>
/// <remarks>
/// For the player's own account the client sets the player's own; for an account friend it sets the friend's.
/// </remarks>
[WireMessage(GameMessageOpcode.Server03B1, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class FriendshipAccountLastOnline
{
    /// <summary>
    /// The account, by id: the player's own, or an account friend's <see cref="FriendshipAccountList.FriendshipAccount.AccountId"/>.
    /// </summary>
    [UInt(32)] public uint AccountId { get; set; }

    /// <summary>
    /// When the account was last online, which add-ons read as an account friend's fLastOnline.
    /// </summary>
    [Float32] public float LastOnline { get; set; }
}
