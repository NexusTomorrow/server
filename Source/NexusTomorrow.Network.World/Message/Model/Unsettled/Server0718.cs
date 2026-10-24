using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Server0718, Direction.ServerToClient)]
[Dormant]
public sealed partial class Server0718
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(15)] public ushort Unknown1 { get; set; }
    [UInt(64)] public ulong Unknown2 { get; set; }
    [Float32] public float Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
}
