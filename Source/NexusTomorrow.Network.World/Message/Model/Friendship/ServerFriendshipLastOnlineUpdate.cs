using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server03CA, Direction.ServerToClient)]
public sealed partial class Server03CA
{
    public Client00B5Part0 Unknown0 { get; set; }
    [Float32] public float Unknown1 { get; set; }
}
