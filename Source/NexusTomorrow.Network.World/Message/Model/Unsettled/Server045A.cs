using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Server045A, Direction.ServerToClient)]
public sealed partial class Server045A
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    [UInt(64)] public ulong Unknown1 { get; set; }
    [Float32] public float Unknown2 { get; set; }
    [Float32] public float Unknown3 { get; set; }
    [Float32] public float Unknown4 { get; set; }
}
