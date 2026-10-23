using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets an account friend's presence and characters.
/// </summary>
/// <remarks>
/// The client replaces the friend's characters with these and raises FriendshipAccountDataUpdate.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0399, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipAccountUpdate
{
    /// <summary>
    /// The account friend, by <see cref="FriendshipAccountList.FriendshipAccount.AccountId"/>.
    /// </summary>
    [UInt(32)] public uint AccountId { get; set; }

    /// <summary>
    /// The friend's presence, which add-ons read as nPresenceState.
    /// </summary>
    [UInt(3)] public AccountPresenceState Presence { get; set; }

    /// <summary>
    /// The friend's characters, in place of those it had.
    /// </summary>
    [Array(CountBits = 32)] public List<FriendshipAccountCharacter> Characters { get; set; } = [];
}
