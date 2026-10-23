using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

[WireMessage(GameMessageOpcode.Server041F, Direction.ServerToClient)]
public sealed partial class GroupInvited
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }

    [Array(CountBits = 32)] public List<GroupMemberInfo> Unknown3 { get; set; } = [];
}
