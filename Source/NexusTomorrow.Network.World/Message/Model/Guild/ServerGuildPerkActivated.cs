using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server04B6, Direction.ServerToClient)]
public sealed partial class Server04B6
{
    public Client00B5Part0 Unknown0 { get; set; }
    [UInt(14)] public ushort Unknown1 { get; set; }
    [Float32] public float Unknown2 { get; set; }
}
