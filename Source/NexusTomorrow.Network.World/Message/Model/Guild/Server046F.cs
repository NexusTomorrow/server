using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server046F, Direction.ServerToClient)]
public sealed partial class Server046F
{
    public RealmScopedId Unknown0 { get; set; } = new();
    public Server0111Part0 Unknown1 { get; set; } = new();
}
