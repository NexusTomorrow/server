using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Sets a group member's flags.
/// </summary>
/// <remarks>
/// For its current group the client raises Group_MemberFlagsChanged with the member, <see cref="IsFromPromotion"/> and the
/// changed flags by name, when any changed.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0437, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GroupMemberFlagsChanged
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
    /// The member.
    /// </summary>
    public RealmScopedId MemberId { get; set; } = new();

    /// <summary>
    /// The member's flags, by bit under the names the event's tFlags uses: 0x2 bCanInvite, 0x4 bCanKick, 0x8 bDisconnected,
    /// 0x10 bPending, 0x20 bTank, 0x40 bHealer, 0x80 bDPS, 0x100 bMainTank, 0x200 bMainAssist, 0x400 bRaidAssistant, 0x800 bReady,
    /// 0x1000 bRoleLocked, 0x2000 bCanMark, 0x4000 bHasSetReady.
    /// </summary>
    [UInt(32)] public uint Flags { get; set; }

    /// <summary>
    /// Passed to the event as bIsFromPromotion.
    /// </summary>
    [UInt(1)] public bool IsFromPromotion { get; set; }
}
