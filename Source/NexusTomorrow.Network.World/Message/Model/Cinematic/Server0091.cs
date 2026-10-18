using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.Server0091, Direction.ServerToClient)]
public sealed partial class Server0091
{
    [Float32] public float Unknown0 { get; set; }
    [Float32] public float Unknown1 { get; set; }
}
