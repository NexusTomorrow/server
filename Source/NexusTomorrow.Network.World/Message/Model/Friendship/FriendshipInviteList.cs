using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Lists the friend invites the player has received.
/// </summary>
/// <remarks>
/// The client files each invite and raises FriendshipInvitesRecieved with their tables.
/// </remarks>
[WireMessage(GameMessageOpcode.Server03BA, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class FriendshipInviteList
{
    /// <summary>
    /// One friend invite.
    /// </summary>
    [WireStruct]
    public sealed partial class FriendshipInvite
    {
        /// <summary>
        /// The invite's id, by which <see cref="FriendshipInviteRemove"/>, <see cref="FriendshipInviteResponse"/> and
        /// <see cref="FriendshipInviteMarkSeen"/> name it.
        /// </summary>
        [UInt(64)] public ulong Id { get; set; }

        public RealmScopedId Unknown1 { get; set; } = new();

        /// <summary>
        /// Whether the player has seen the invite: it is new while this is 0, which add-ons read as bIsNew, and the client sets 1 when
        /// it marks the invite seen.
        /// </summary>
        [UInt(3)] public uint SeenState { get; set; }

        /// <summary>
        /// The days until the invite expires, which add-ons read as fDaysUntilExpired.
        /// </summary>
        [Float32] public float DaysUntilExpired { get; set; }

        /// <summary>
        /// The invite's note, which add-ons read as strNote.
        /// </summary>
        [WideString] public string Note { get; set; } = "";

        /// <summary>
        /// The character's name, which add-ons read as strCharacterName.
        /// </summary>
        [WideString] public string Name { get; set; } = "";

        /// <summary>
        /// The character's class id, which add-ons read as nClassId.
        /// </summary>
        [UInt(14)] public ushort ClassId { get; set; }

        /// <summary>
        /// The character's path id, which add-ons read as nPathId.
        /// </summary>
        [UInt(3)] public byte PathId { get; set; }

        /// <summary>
        /// The character's level, which add-ons read as nLevel.
        /// </summary>
        [UInt(8)] public byte Level { get; set; }
    }

    /// <summary>
    /// The invites.
    /// </summary>
    [Array(CountBits = 16)] public List<FriendshipInvite> Invites { get; set; } = [];
}
