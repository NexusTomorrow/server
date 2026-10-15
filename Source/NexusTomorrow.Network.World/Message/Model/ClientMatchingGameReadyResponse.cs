using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientMatchingMatchReadyResponse, Direction.ClientToServer)]
public sealed partial class ClientMatchingGameReadyResponse
{
    [UInt(1)] public bool Response { get; private set; } // decline = false, join = true
}
