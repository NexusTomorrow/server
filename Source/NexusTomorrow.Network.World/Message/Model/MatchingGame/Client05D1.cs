using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

[WireMessage(GameMessageOpcode.Client05D1, Direction.ClientToServer)]
public sealed partial class Client05D1
{
    public RealmScopedId Unknown0 { get; private set; } = new();
}
