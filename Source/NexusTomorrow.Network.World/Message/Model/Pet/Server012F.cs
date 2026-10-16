using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

[WireMessage(GameMessageOpcode.Server012F, Direction.ServerToClient)]
public sealed partial class Server012F
{
    public Server012EPart0 Unknown0 { get; set; } = new();
}
