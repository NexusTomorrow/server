using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

[WireMessage(GameMessageOpcode.Server07E5, Direction.ServerToClient)]
public sealed partial class Server07E5
{
    [Float32] public Vector3 Unknown0 { get; set; }
    [Float32] public float Unknown1 { get; set; }
}
