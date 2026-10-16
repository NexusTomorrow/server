using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Server0958, Direction.ServerToClient)]
public sealed partial class Server0958
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(16)] public ushort Unknown1 { get; set; }
    [UInt(16)] public ushort Unknown2 { get; set; }
    [Array(CountBits = 8)] public List<Server0958Part0> Unknown3 { get; set; } = [];
}
