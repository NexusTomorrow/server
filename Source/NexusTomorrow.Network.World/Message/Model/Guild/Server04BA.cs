using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server04BA, Direction.ServerToClient)]
public sealed partial class Server04BA
{
    public RealmScopedId Unknown0 { get; set; } = new();
    [UInt(14)] public ushort Unknown1 { get; set; }
}
