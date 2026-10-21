using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Client052B, Direction.ClientToServer)]
public sealed partial class Client052B
{
    public RealmScopedId Unknown0 { get; } = new();
    [UInt(32)] public uint Unknown1 { get; private set; }
}
