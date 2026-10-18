using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Connection;

[WireMessage(GameMessageOpcode.Client023E, Direction.ClientToServer)]
public sealed partial class Client023E
{
    [Float32] public float Unknown0 { get; private set; }
    [Float32] public float Unknown1 { get; private set; }
    [Float32] public float Unknown2 { get; private set; }
    [Float32] public float Unknown3 { get; private set; }
    [Float32] public float Unknown4 { get; private set; }
    [Float32] public float Unknown5 { get; private set; }
    [Float32] public float Unknown6 { get; private set; }
    [Float32] public float Unknown7 { get; private set; }
}
