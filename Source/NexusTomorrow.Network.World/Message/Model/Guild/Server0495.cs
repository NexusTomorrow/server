using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server0495, Direction.ServerToClient)]
public sealed partial class Server0495
{
    public RealmScopedId Unknown0 { get; set; } = new();
}
