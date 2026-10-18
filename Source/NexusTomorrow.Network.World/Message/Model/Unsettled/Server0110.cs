using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Server0110, Direction.ServerToClient)]
public sealed partial class Server0110
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(18)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(8)] public byte Unknown3 { get; set; }
}
