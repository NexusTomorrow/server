using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server00AD, Direction.ServerToClient)]
public sealed partial class Server00AD
{
    [UInt(15)] public ushort Unknown0 { get; set; }
    public Client0096Part0 Unknown1 { get; set; }
    [Float32] public float Unknown2 { get; set; }
    [Float32] public float Unknown3 { get; set; }
}
