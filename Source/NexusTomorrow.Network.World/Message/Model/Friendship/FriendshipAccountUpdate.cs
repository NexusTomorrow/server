using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets an account friend's presence and merges in its characters.
/// </summary>
/// <remarks>
/// The client merges <see cref="Characters"/> into the characters it holds for the friend: one it already holds, matched on
/// <see cref="FriendshipAccountCharacter.CharacterId"/>, has its level and zone refreshed, a new one is added, and none is dropped. It
/// then raises FriendshipAccountDataUpdate.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0399, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
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
    /// The friend's characters, merged into those the client holds; <see cref="FriendshipAccountCharacterRemoved"/> removes one.
    /// </summary>
    [Array(CountBits = 32)] public List<FriendshipAccountCharacter> Characters { get; set; } = [];
}
