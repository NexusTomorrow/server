using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

[WireMessage(GameMessageOpcode.Server0443, Direction.ServerToClient)]
public sealed partial class Server0443
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    public RealmScopedId Unknown1 { get; set; } = new();
    [UInt(6)] public byte Unknown2 { get; set; }
}
