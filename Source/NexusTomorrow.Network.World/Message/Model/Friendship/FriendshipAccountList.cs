using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Lists the player's account friends.
/// </summary>
/// <remarks>
/// The client files each account friend with its characters and raises FriendshipAccountFriendsRecieved.
/// </remarks>
[WireMessage(GameMessageOpcode.Server03A3, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class FriendshipAccountList
{
    /// <summary>
    /// One account friend.
    /// </summary>
    [WireStruct]
    public sealed partial class FriendshipAccount
    {
        /// <summary>
        /// The friend's account, by id. The updates that name an account by id, <see cref="FriendshipAccountUpdate"/>,
        /// <see cref="FriendshipAccountPresence"/> and the others, find the friend by it.
        /// </summary>
        [UInt(32)] public uint AccountId { get; set; }

        /// <summary>
        /// The account friend's id, by which <see cref="FriendshipAccountPrivateNote"/>, <see cref="FriendshipAccountRemoved"/> and
        /// <see cref="FriendshipAccountCharacterZones"/> name it.
        /// </summary>
        [UInt(64)] public ulong Id { get; set; }

        /// <summary>
        /// The friend's public note, which add-ons read as strPublicNote.
        /// </summary>
        [WideString] public string PublicNote { get; set; } = "";

        /// <summary>
        /// When the account was last online, in days from now, as <see cref="FriendshipAccountLastOnline.LastOnlineDays"/> sets it. A
        /// nonzero value marks the account offline: the client clears its characters and sets its presence to
        /// <see cref="AccountPresenceState.Invisible"/>.
        /// </summary>
        [Float32] public float LastOnlineDays { get; set; }

        /// <summary>
        /// The private note on the friend, which add-ons read as strPrivateNote.
        /// </summary>
        [WideString] public string PrivateNote { get; set; } = "";

        /// <summary>
        /// The friend's display name, which add-ons read as strCharacterName.
        /// </summary>
        [WideString] public string DisplayName { get; set; } = "";

        /// <summary>
        /// The friend's presence, which add-ons read as nPresenceState.
        /// </summary>
        [UInt(3)] public AccountPresenceState Presence { get; set; }

        /// <summary>
        /// The friend's characters.
        /// </summary>
        [Array(CountBits = 32)] public List<FriendshipAccountCharacter> Characters { get; set; } = [];
    }

    /// <summary>
    /// The account friends.
    /// </summary>
    [Array(CountBits = 32)] public List<FriendshipAccount> Accounts { get; set; } = [];
}
