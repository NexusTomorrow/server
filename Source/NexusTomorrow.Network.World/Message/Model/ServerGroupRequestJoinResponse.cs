using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerGroupRequestJoinResponse, Direction.ServerToClient)]
public sealed partial class ServerGroupRequestJoinResponse
{
    [UInt(64)] public ulong GroupId { get; set; }

    public GroupMember MemberInfo { get; set; }
}
