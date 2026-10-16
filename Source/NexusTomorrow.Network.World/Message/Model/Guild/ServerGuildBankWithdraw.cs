using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server04A1, Direction.ServerToClient)]
public sealed partial class Server04A1
{
    public Client00B5Part0 Unknown0 { get; private set; }
    public Server0493Part6 Unknown1 { get; private set; }
}
