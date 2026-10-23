using NexusTomorrow.Game.Static.Friendship;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// One of the player's friendships with a character.
/// </summary>
/// <remarks>
/// The character's name, realm, class, path and faction are not sent here: the client requests them for <see cref="CharacterId"/>.
/// </remarks>
[WireStruct]
public sealed partial class Friendship
{
    /// <summary>
    /// The friendship's id, by which <see cref="FriendshipRemove"/>, <see cref="FriendshipNoteUpdate"/> and
    /// <see cref="FriendshipTypeUpdate"/> name it.
    /// </summary>
    [UInt(64)] public ulong Id { get; set; }

    /// <summary>
    /// The character, by realm and character id. <see cref="FriendshipLevelUpdate"/> and <see cref="FriendshipLastOnlineUpdate"/> name the
    /// friendship by it, and <see cref="FriendshipGetLocations"/> sends it.
    /// </summary>
    public RealmScopedId CharacterId { get; set; } = new();

    /// <summary>
    /// The friendship's note, which add-ons read as strNote; <see cref="FriendshipNoteUpdate"/> replaces it.
    /// </summary>
    [WideString] public string Note { get; set; } = "";

    /// <summary>
    /// The kind of friendship. The client sets the friend's bFriend for <see cref="FriendshipType.Friend"/>, bIgnore for
    /// <see cref="FriendshipType.Ignore"/>, bRival for <see cref="FriendshipType.Rival"/>, and both bFriend and bRival for
    /// <see cref="FriendshipType.FriendAndRival"/>; <see cref="FriendshipTypeUpdate"/> changes it.
    /// </summary>
    [UInt(4)] public FriendshipType Type { get; set; }
}
