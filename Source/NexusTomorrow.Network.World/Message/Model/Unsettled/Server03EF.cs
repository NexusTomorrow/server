using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Server03EF, Direction.ServerToClient)]
public sealed partial class Server03EF
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [Array(CountBits = 32), UInt(8)] public List<byte> Unknown1 { get; } = [];
}
