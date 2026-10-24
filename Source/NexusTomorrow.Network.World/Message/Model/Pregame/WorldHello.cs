using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// The client's greeting on a world connection, its answer to <see cref="Network.Message.Model.ServerHello"/> there.
/// </summary>
/// <remarks>
/// The client sends it under the connection's current key, and right after sending it switches the connection to a key derived from
/// <see cref="SessionKey"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Client058F, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class WorldHello
{
    /// <summary>
    /// The player's account id, as <c>RealmHandoff</c> (0x03DB) gave it to the client, sent back unchanged. The client compares the account
    /// id of <see cref="Friendship.FriendshipAccountDisplayName"/>, <see cref="Friendship.FriendshipAccountPresence"/> and
    /// <see cref="Friendship.FriendshipAccountPublicNote"/> with it to recognize the player's own account.
    /// </summary>
    [UInt(32)] public uint AccountId { get; set; }

    /// <summary>
    /// The session key the client was handed, sent back unchanged, in the GUID layout.
    /// </summary>
    [WireSecret] public NetworkGuid SessionKey { get; } = new();

    /// <summary>
    /// The client gives this value no meaning. It always sends 0.
    /// </summary>
    [UInt(64)] public ulong Unknown2 { get; set; }

    /// <summary>
    /// The account name the login server accepted.
    /// </summary>
    [WideString] public string AccountName { get; set; } = "";

    /// <summary>
    /// The client gives this value no meaning. It always sends 3.
    /// </summary>
    [UInt(32)] public uint Unknown4 { get; set; }
}
