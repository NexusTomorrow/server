using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireStruct]
public sealed partial class Server0507Part0
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(64)] public ulong Unknown1 { get; set; }
    public RealmScopedId Unknown2 { get; set; } = new();
    [UInt(32)] public uint Unknown3 { get; set; }
}
