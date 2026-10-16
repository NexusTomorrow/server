using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Story.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

[WireMessage(GameMessageOpcode.Server0738, Direction.ServerToClient)]
public sealed partial class Server0738
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [Array(CountBits = 8)] public List<Server0738Part0> Unknown2 { get; set; } = [];
}
