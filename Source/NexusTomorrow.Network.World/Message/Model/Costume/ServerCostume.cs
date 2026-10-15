using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[WireMessage(GameMessageOpcode.ServerCostume, Direction.ServerToClient)]
public sealed partial class ServerCostume
{
    public Costume Costume { get; set; }
}
