using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server00E2, Direction.ServerToClient)]
public sealed partial class Server00E2
{
    public Server00E0Part0 Unknown0 { get; set; } = new();
}
