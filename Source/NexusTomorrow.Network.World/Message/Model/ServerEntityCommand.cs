using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Message0638, Direction.ServerToClient)]
public sealed partial class Message0638
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(1)] public bool Unknown2 { get; set; }
    [UInt(1)] public bool Unknown3 { get; set; }
    [Array(CountBits = 5)] public List<Server0262Part5> Unknown4 { get; set; } = [];
}
