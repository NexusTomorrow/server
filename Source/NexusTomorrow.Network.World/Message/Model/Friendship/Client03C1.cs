using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Client03C1, Direction.ClientToServer)]
public sealed partial class Client03C1
{
    public RealmScopedId Unknown0 { get; private set; } = new();
    [UInt(4)] public byte Unknown1 { get; private set; }
}
