using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerGroupInviteReceived, Direction.ServerToClient)]
public sealed partial class ServerGroupInviteReceived
{
    [UInt(64)] public ulong GroupId { get; set; }
    [UInt(32)] public uint LeaderIndex { get; set; }
    [UInt(32)] public uint InviterIndex { get; set; }

    [Array(CountBits = 32)] public List<GroupCharacter> Members = [];
}
