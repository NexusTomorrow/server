using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerGroupMemberAdd, Direction.ServerToClient)]
public sealed partial class ServerGroupMemberAdd
{
    [UInt(64)] public ulong GroupId { get; set; }
    [UInt(32)] public uint Unknown0 { get; set; }
    public GroupMember AddedMember { get; set; }
}
