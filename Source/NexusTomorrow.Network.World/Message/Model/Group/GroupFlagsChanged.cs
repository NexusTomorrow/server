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
[Verified(Rung.Purpose)]
public sealed partial class GroupFlagsChanged
{
    /// <summary>
    /// The group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

    [UInt(32)] public uint Unknown1 { get; set; }

    /// <summary>
    /// The group's flags, stored whole.
    /// </summary>
    [UInt(32)] public uint Flags { get; set; }
}
