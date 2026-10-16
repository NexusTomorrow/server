using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.Server07D5, Direction.ServerToClient)]
public sealed partial class Server07D5
{
    [UInt(14)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [FixedArray(4), UInt(8)] public byte[] Unknown2 { get; set; } = new byte[4];
    [UInt(32)] public uint Unknown3 { get; set; }
    [UInt(1)] public bool Unknown4 { get; set; }
}
