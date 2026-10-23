using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Server0978, Direction.ServerToClient)]
public sealed partial class CreddOperationHistory
{
    [WireStruct]
    public sealed partial class CreddOperationHistoryEntry
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
        [UInt(1)] public bool Unknown2 { get; set; }
        [UInt(32)] public uint Unknown3 { get; set; }
        public RealmScopedId Unknown4 { get; set; } = new();
        public RealmScopedId Unknown5 { get; set; } = new();
        [UInt(64)] public ulong Unknown6 { get; set; }
        [UInt(64)] public ulong Unknown7 { get; set; }
    }

    [Array(CountBits = 32)] public List<CreddOperationHistoryEntry> Unknown0 { get; set; } = [];
}
