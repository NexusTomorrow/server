using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

[WireMessage(GameMessageOpcode.Server0614, Direction.ServerToClient)]
public sealed partial class Server0614
{
    public RealmScopedId Unknown0 { get; set; } = new();
    public RealmScopedId Unknown1 { get; set; } = new();
}
