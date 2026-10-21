using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Client04A6, Direction.ClientToServer)]
public sealed partial class Client04A6
{
    public RealmScopedId Unknown0 { get; private set; } = new();
    public Client0481Part0 Unknown1 { get; private set; } = new();
}
