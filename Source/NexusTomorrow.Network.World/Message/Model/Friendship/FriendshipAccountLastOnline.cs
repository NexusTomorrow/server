using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets when an account friend was last online.
/// </summary>
/// <remarks>
/// A nonzero time marks the friend offline: the client clears its characters, sets its presence to
/// <see cref="AccountPresenceState.Invisible"/> and raises FriendshipAccountDataUpdate.
/// </remarks>
[WireMessage(GameMessageOpcode.Server03B1, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipAccountLastOnline
{
    /// <summary>
    /// The account friend, by <see cref="FriendshipAccountList.FriendshipAccount.AccountId"/>; the client looks the id up only among
    /// the account friends.
    /// </summary>
    [UInt(32)] public uint AccountId { get; set; }

    /// <summary>
    /// When the account was last online, in days from now, which add-ons read as an account friend's
    /// fLastOnline: 0 clears it and a negative value is that many days ago.
    /// </summary>
    [Float32] public float LastOnlineDays { get; set; }
}
