using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerGroupPromote, Direction.ServerToClient)]
public sealed partial class ServerGroupPromote
{
    [UInt(64)] public ulong GroupId { get; set; }

    [UInt(32)] public uint LeaderIndex { get; set; }

    public Identity NewLeader { get; set; }
}
