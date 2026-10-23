using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets an account's presence.
/// </summary>
/// <remarks>
/// For an account friend the client sets the friend's presence. For the player's own account it stores the presence as stated, and
/// copies it to the player's chosen presence only while that is 4, a value no <see cref="AccountPresenceState"/> member has. It prints
/// a line only on a change to <see cref="AccountPresenceState.Available"/> from <see cref="AccountPresenceState.Away"/> or
/// <see cref="AccountPresenceState.Busy"/>. When the stated and chosen presences differ it sends the chosen one again; otherwise it
/// raises FriendshipAccountPersonalStatusUpdate.
/// </remarks>
[WireMessage(GameMessageOpcode.Server03AA, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipAccountPresence
{
    /// <summary>
    /// The account, by id: the player's own, or an account friend's <see cref="FriendshipAccountList.FriendshipAccount.AccountId"/>.
    /// </summary>
    [UInt(32)] public uint AccountId { get; set; }

    /// <summary>
    /// The new presence.
    /// </summary>
    [UInt(3)] public AccountPresenceState Presence { get; set; }
}
