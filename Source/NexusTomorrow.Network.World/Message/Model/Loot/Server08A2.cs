using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

[WireMessage(GameMessageOpcode.Server08A2, Direction.ServerToClient)]
public sealed partial class Server08A2
{
    [UInt(32)] public uint Unknown0 { get; set; }
    public RealmScopedId Unknown1 { get; set; } = new RealmScopedId();
    [UInt(18)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
}
