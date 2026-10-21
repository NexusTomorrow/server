using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server0470, Direction.ServerToClient)]
public sealed partial class Server0470
{
    public RealmScopedId Unknown0 { get; set; } = new();
    [UInt(64)] public ulong Unknown1 { get; private set; }
}
