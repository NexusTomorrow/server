using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets an account's public note.
/// </summary>
/// <remarks>
/// For the player's own account the client sets the player's own public note, which FriendshipLib.GetPersonalStatus returns as
/// strPublicNote; for an account friend it sets the friend's.
/// </remarks>
[WireMessage(GameMessageOpcode.Server03AD, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipAccountPublicNote
{
    /// <summary>
    /// The account, by id: the player's own, or an account friend's <see cref="FriendshipAccountList.FriendshipAccount.AccountId"/>.
    /// </summary>
    [UInt(32)] public uint AccountId { get; set; }

    /// <summary>
    /// The new public note.
    /// </summary>
    [WideString] public string PublicNote { get; set; } = "";
}
