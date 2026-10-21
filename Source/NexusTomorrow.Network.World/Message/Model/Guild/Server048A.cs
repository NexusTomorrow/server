using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server048A, Direction.ServerToClient)]
public sealed partial class Server048A
{
    public RealmScopedId Unknown0 { get; set; } = new();
    [UInt(64)] public ulong Unknown1 { get; set; }
}
