using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.Server08A8, Direction.ServerToClient)]
public sealed partial class Server08A8
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(18)] public uint Unknown1 { get; set; }
    [UInt(17)] public uint Unknown2 { get; set; }
    [UInt(1)] public bool Unknown3 { get; set; }
    [UInt(1)] public bool Unknown4 { get; set; }
}
