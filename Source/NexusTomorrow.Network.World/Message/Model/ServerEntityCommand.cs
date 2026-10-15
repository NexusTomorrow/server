using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerEntityCommand, Direction.ServerToClient)]
public sealed partial class ServerEntityCommand
{
    [UInt(32)] public uint Guid { get; set; }
    [UInt(32)] public uint Time { get; set; }
    [UInt(1)] public bool TimeReset { get; set; }
    [UInt(1)] public bool ServerControlled { get; set; }
    [Array(CountBits = 5)] public List<NetworkEntityCommand> Commands { get; set; } = [];
}
