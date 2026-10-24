using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// A group as the client keeps it: its id, flags, size, loot rules, members, leader and target marks.
/// </summary>
[WireStruct]
public sealed partial class GroupRecord
{
    /// <summary>
    /// The group, by id; the client refuses 0.
    /// </summary>
    [VerifiedMember, UInt(64)] public ulong GroupId { get; set; }

    /// <summary>
    /// The group's flags, by bit as <see cref="Group.GroupFlagsChanged.Flags"/>.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Flags { get; set; }

    /// <summary>
    /// How many records <see cref="Members"/> holds.
    /// </summary>
    [VerifiedMember, UInt(32), Count] public uint MemberCount { get; set; }

    /// <summary>
    /// The group's maximum size; the client refuses 0, or a size below <see cref="MemberCount"/>.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint MaxSize { get; set; }

    /// <summary>
    /// The rule GroupLib.GetLootRules reports as eNormalRule.
    /// </summary>
    [VerifiedMember, UInt(3)] public byte NormalRule { get; set; }

    /// <summary>
    /// The rule GroupLib.GetLootRules reports as eThresholdRule.
    /// </summary>
    [VerifiedMember, UInt(3)] public byte ThresholdRule { get; set; }

    /// <summary>
    /// The quality GroupLib.GetLootRules reports as eThresholdQuality.
    /// </summary>
    [VerifiedMember, UInt(4)] public byte ThresholdQuality { get; set; }

    /// <summary>
    /// The rule GroupLib.GetLootRules reports as eHarvestRule.
    /// </summary>
    [VerifiedMember, UInt(2)] public byte HarvestRule { get; set; }

    /// <summary>
    /// The group's members.
    /// </summary>
    [VerifiedMember, Array(CountFrom = nameof(MemberCount))] public List<GroupMemberRecord> Members { get; set; } = [];

    /// <summary>
    /// The group's leader, by member id.
    /// </summary>
    [VerifiedMember] public RealmScopedId LeaderId { get; set; } = new();

    [UInt(14)] public ushort Unknown10 { get; set; }

    /// <summary>
    /// The group's target marks.
    /// </summary>
    [VerifiedMember] public GroupMarks Marks { get; set; } = new();
}
