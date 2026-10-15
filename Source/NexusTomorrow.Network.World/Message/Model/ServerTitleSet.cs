using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerTitleSet, Direction.ServerToClient)]
public sealed partial class ServerTitleSet
{
    [UInt(32)] public uint Guid { get; set; }
    [UInt(14)] public ushort Title { get; set; }
}
