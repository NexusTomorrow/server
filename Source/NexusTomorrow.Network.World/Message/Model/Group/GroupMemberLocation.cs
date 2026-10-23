using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Updates where a group member is.
/// </summary>
/// <remarks>
/// The client stores the other fields on the member, for whichever of its current and previous groups has the id; one of them is
/// a World table id, which decides whether GroupLib.GotoGroupInstance may be sent.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0467, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class GroupMemberLocation
{
    /// <summary>
    /// The group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

    /// <summary>
    /// The member.
    /// </summary>
    public RealmScopedId MemberId { get; set; } = new();

    [UInt(14)] public uint Unknown2 { get; set; }
    [UInt(15)] public uint Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(32)] public uint Unknown5 { get; set; }
    [UInt(1)] public bool Unknown6 { get; set; }
}
