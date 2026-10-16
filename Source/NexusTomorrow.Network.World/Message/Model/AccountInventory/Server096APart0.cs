using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireStruct]
public sealed partial class Server096APart0
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(5)] public byte Unknown2 { get; set; }
    [UInt(1)] public bool Unknown3 { get; set; }
    public Client00B5Part0 Unknown4 { get; set; } = new();
}
