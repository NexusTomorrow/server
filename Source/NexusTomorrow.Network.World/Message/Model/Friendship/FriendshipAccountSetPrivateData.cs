using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets the player's private data on an account friend.
/// </summary>
[WireMessage(GameMessageOpcode.Client039D, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class FriendshipAccountSetPrivateData
{
    /// <summary>
    /// The account friend, by <see cref="FriendshipAccountList.FriendshipAccount.Id"/>.
    /// </summary>
    [UInt(64)] public ulong Id { get; private set; }

    [UInt(32)] public uint Unknown1 { get; private set; }

    /// <summary>
    /// The private note.
    /// </summary>
    [WideString] public string PrivateNote { get; private set; } = "";
}
