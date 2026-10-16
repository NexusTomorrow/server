using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server00BA, Direction.ServerToClient)]
public sealed partial class Server00BA
{
    public Server00BAPart0 Unknown0 { get; set; }
}
