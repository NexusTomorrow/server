using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Client0398, Direction.ClientToServer)]
public sealed partial class Client0398
{
    public RealmScopedId Unknown0 { get; private set; } = new();
    [WideString] public string Unknown1 { get; private set; } = "";
}
