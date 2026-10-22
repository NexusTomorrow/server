using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Sets a group member's flags: the request GroupLib.SetReady sends, among others.
/// </summary>
/// <remarks>
/// GroupLib.SetReady sends the player's own member, with 0x800 (bReady) set or cleared and 0x4000 (bHasSetReady) set.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0408, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GroupSetMemberFlags
{
    /// <summary>
    /// The player's current group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

    /// <summary>
    /// The member.
    /// </summary>
    public RealmScopedId MemberId { get; set; } = new();

    /// <summary>
    /// The member's flags as the client asks for them, by bit as <see cref="GroupMemberFlagsChanged.Flags"/>.
    /// </summary>
    [UInt(32)] public uint Flags { get; set; }

    /// <summary>
    /// The bits that differ from the member's flags as the client holds them.
    /// </summary>
    [UInt(32)] public uint ChangedFlags { get; set; }
}
