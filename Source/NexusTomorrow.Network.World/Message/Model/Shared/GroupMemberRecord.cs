using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// A group member as the server describes one: its id, flags and state.
/// </summary>
[WireStruct]
public sealed partial class GroupMemberRecord
{
    /// <summary>
    /// The member, by id.
    /// </summary>
    [VerifiedMember] public RealmScopedId Id { get; set; } = new();

    /// <summary>
    /// The member's flags, by bit as <see cref="Group.GroupMemberFlagsChanged.Flags"/>.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Flags { get; set; }

    /// <summary>
    /// The member's name and state.
    /// </summary>
    [VerifiedMember] public GroupMemberInfo Info { get; set; } = new();

    [UInt(32)] public uint Unknown3 { get; set; }
}
