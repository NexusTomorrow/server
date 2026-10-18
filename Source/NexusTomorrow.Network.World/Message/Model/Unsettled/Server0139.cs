using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Server0139, Direction.ServerToClient)]
public sealed partial class Server0139
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [Array(CountBits = 5), UInt(32)] public List<uint> Unknown1 { get; } = [];
}
