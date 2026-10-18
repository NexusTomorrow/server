using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

[WireMessage(GameMessageOpcode.Server0849, Direction.ServerToClient)]
public sealed partial class Server0849
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [Float32] public Vector3 Unknown1 { get; set; }
    [UInt(15)] public uint Unknown2 { get; set; }
}
