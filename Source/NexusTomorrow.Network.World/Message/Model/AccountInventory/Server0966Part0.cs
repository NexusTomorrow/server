using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireStruct]
public sealed partial class Server0966Part0
{
    [UInt(5)] public byte Unknown0 { get; set; }
    [UInt(64)] public ulong Unknown1 { get; set; }
}
