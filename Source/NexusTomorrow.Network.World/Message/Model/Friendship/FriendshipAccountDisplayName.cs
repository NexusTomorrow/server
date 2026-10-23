using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets an account's display name.
/// </summary>
/// <remarks>
/// For the player's own account the client sets the player's own display name, which FriendshipLib.GetPersonalStatus returns as
/// strDisplayName; for an account friend it sets the friend's.
/// </remarks>
[WireMessage(GameMessageOpcode.Server039C, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipAccountDisplayName
{
    /// <summary>
    /// The account, by id: the player's own, or an account friend's <see cref="FriendshipAccountList.FriendshipAccount.AccountId"/>.
    /// </summary>
    [UInt(32)] public uint AccountId { get; set; }

    /// <summary>
    /// The new display name.
    /// </summary>
    [WideString] public string DisplayName { get; set; } = "";
}
