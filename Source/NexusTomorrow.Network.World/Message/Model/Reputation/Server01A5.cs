using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reputation;

[WireMessage(GameMessageOpcode.Server01A5, Direction.ServerToClient)]
public sealed partial class Server01A5
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    [Float32] public float Unknown1 { get; set; }
}
