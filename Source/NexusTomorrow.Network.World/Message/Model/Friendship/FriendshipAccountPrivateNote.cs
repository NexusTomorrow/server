using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets the private note on an account friend.
/// </summary>
[WireMessage(GameMessageOpcode.Server03AE, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipAccountPrivateNote
{
    /// <summary>
    /// The account friend, by <see cref="FriendshipAccountList.FriendshipAccount.Id"/>.
    /// </summary>
    [UInt(64)] public ulong Id { get; set; }

    /// <summary>
    /// The new private note, which add-ons read as strPrivateNote.
    /// </summary>
    [WideString] public string PrivateNote { get; set; } = "";
}
