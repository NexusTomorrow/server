using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server0189, Direction.ServerToClient)]
public sealed partial class Server0189
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(14)] public ushort Unknown1 { get; set; }
}
