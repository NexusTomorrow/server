using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class AccountPendingItem
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(64)] public ulong Unknown2 { get; set; }
    [WideString] public string Unknown3 { get; set; } = "";
    [UInt(32)] public uint Unknown4 { get; set; }
    public RealmScopedId Unknown5 { get; set; } = new();
    [UInt(64)] public ulong Unknown6 { get; set; }
    [UInt(5)] public byte Unknown7 { get; set; }
    [UInt(64)] public ulong Unknown8 { get; set; }
    public RealmScopedId Unknown9 { get; set; } = new();
}
