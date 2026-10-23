using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets an account's presence.
/// </summary>
/// <remarks>
/// For the player's own account the client sets the player's own presence, prints a line, and sends the presence the player has chosen
/// again; for an account friend it sets the friend's.
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
