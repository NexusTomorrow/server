using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server04C3, Direction.ServerToClient)]
public sealed partial class Server04C3
{
    public RealmScopedId Unknown0 { get; set; } = new();
    [UInt(3)] public byte Unknown1 { get; set; }
}
