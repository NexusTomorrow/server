using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Lists the account friend invites the player has received.
/// </summary>
/// <remarks>
/// The client files each invite and raises FriendshipAccountInvitesRecieved with them as tInviteList.
/// </remarks>
[WireMessage(GameMessageOpcode.Server039F, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipAccountInviteList
{
    /// <summary>
    /// One account friend invite.
    /// </summary>
    [WireStruct]
    public sealed partial class FriendshipAccountInvite
    {
        /// <summary>
        /// The invite's id, by which <see cref="FriendshipAccountInviteRemove"/>, <see cref="FriendshipAccountInviteResponse"/> and
        /// <see cref="FriendshipAccountInviteMarkSeen"/> name it.
        /// </summary>
        [UInt(64)] public ulong Id { get; set; }

        /// <summary>
        /// The kind of invite, which add-ons read as strInviteType.
        /// </summary>
        [UInt(2)] public byte InviteType { get; set; }

        /// <summary>
        /// The display name, which add-ons read as strDisplayName.
        /// </summary>
        [WideString] public string DisplayName { get; set; } = "";

        /// <summary>
        /// The invite's note, which add-ons read as strNote.
        /// </summary>
        [WideString] public string Note { get; set; } = "";

        /// <summary>
        /// The days left before the invite expires, which add-ons read as fDaysUntilExpired.
        /// </summary>
        [Float32] public float DaysUntilExpired { get; set; }

        /// <summary>
        /// The invite's flags, which add-ons read as bIsNew. The client sets bit 0 as it sends
        /// <see cref="FriendshipAccountInviteMarkSeen"/> for the invite.
        /// </summary>
        [UInt(32)] public uint Flags { get; set; }
    }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown0 { get; set; }

    /// <summary>
    /// The invites.
    /// </summary>
    [Array(CountBits = 32)] public List<FriendshipAccountInvite> Invites { get; set; } = [];
}
