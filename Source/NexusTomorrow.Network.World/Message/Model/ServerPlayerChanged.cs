using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerPlayerChanged, Direction.ServerToClient)]
public sealed partial class ServerPlayerChanged
{
    [UInt(32)] public uint Guid { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
}
