using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Sets a group's flags.
/// </summary>
/// <remarks>
/// For its current group the client then raises Group_FlagsChanged.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0410, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GroupFlagsChanged
{
    /// <summary>
    /// The group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown1 { get; set; }

    /// <summary>
    /// The group's flags, stored whole. 0x1 makes it an instance group (GroupLib.InInstance), and 0x2 a raid (GroupLib.InRaid). 0x8
    /// allows a vote to kick (MatchingGameLib.InitiateVoteToKick). 0x10 and 0x20 open and close join requests, and 0x40 and 0x80 open
    /// and close referrals, which GroupLib.GetJoinRequestMethod and GroupLib.GetReferralMethod report as a GroupLib.InvitationMethod.
    /// The client works 0x100 out again from the members' factions whenever a member is added.
    /// </summary>
    [UInt(32)] public uint Flags { get; set; }
}
