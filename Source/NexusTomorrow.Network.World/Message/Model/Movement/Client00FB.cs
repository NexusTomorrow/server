using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Movement;

[WireMessage(GameMessageOpcode.Client00FB, Direction.ClientToServer)]
public sealed partial class Client00FB
{
    [Float32] public float Unknown0 { get; set; }
}
