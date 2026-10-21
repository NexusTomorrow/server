using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Client04A8, Direction.ClientToServer)]
public sealed partial class Client04A8
{
    public RealmScopedId Unknown0 { get; private set; } = new();
    [UInt(64)] public ulong Unknown1 { get; private set; }
}
