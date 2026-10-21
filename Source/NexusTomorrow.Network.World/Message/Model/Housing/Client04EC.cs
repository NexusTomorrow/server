using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Client04EC, Direction.ClientToServer)]
public sealed partial class Client04EC
{
    public RealmScopedId Unknown0 { get; } = new();
}
