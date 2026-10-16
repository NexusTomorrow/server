using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server04AC, Direction.ServerToClient)]
public sealed partial class Server04AC
{
    public Client00B5Part0 Unknown0 { get; set; } = new();
}
