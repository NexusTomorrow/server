using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reputation;

[WireMessage(GameMessageOpcode.Server08D7, Direction.ServerToClient)]
public sealed partial class Server08D7
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(14)] public ushort Unknown1 { get; set; }
    [Float32] public float Unknown2 { get; set; }
}
