using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[WireMessage(GameMessageOpcode.Server0642, Direction.ServerToClient)]
public sealed partial class Server0642
{
    [UInt(17)] public uint Unknown0 { get; set; }
    [UInt(9)] public uint Unknown1 { get; set; }
    [UInt(21)] public uint Unknown2 { get; set; }
}
