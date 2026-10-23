using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// A group member as the server describes one: its id, flags, state and place in the group's order.
/// </summary>
[WireStruct]
public sealed partial class GroupMemberRecord
{
    /// <summary>
    /// The member, by id.
    /// </summary>
    public RealmScopedId Id { get; set; } = new();

    /// <summary>
    /// The member's flags, by bit as <see cref="Group.GroupMemberFlagsChanged.Flags"/>.
    /// </summary>
    [UInt(32)] public uint Flags { get; set; }

    /// <summary>
    /// The member's name and state.
    /// </summary>
    public GroupMemberInfo Info { get; set; } = new();

    /// <summary>
    /// The member's place in the group's order; GroupLib.GetGroupMember reports it plus one as nOrder.
    /// </summary>
    [UInt(32)] public uint Order { get; set; }
}
