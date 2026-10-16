using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server0351, Direction.ServerToClient)]
public sealed partial class Server0351
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(32)] public uint Unknown5 { get; set; }
    [UInt(32)] public uint Unknown6 { get; set; }
    [UInt(32)] public uint Unknown7 { get; set; }
}
