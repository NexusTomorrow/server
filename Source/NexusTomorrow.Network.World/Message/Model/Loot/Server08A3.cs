using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

[WireMessage(GameMessageOpcode.Server08A3, Direction.ServerToClient)]
public sealed partial class Server08A3
{
    [WireStruct]
    public sealed partial class Server08A3Part0
    {
        public RealmScopedId Unknown0 { get; set; } = new RealmScopedId();
        [UInt(32)] public uint Unknown1 { get; set; }
    }

    [UInt(32)] public uint Unknown0 { get; set; }
    public Server08A3Part0 Unknown1 { get; set; } = new Server08A3Part0();
    [UInt(18)] public uint Unknown2 { get; set; }
    [Array(CountBits = 32)] public List<Server08A3Part0> Unknown3 { get; set; } = [];
}
