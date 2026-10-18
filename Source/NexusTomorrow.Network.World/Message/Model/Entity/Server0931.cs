using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

[WireMessage(GameMessageOpcode.Server0931, Direction.ServerToClient)]
public sealed partial class Server0931
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [Array(CountBits = 32), Float32] public List<float> Unknown1 { get; set; } = [];
}
