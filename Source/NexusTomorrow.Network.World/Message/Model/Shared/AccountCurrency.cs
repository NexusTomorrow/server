using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class AccountCurrency
{
    [UInt(5)] public byte Unknown0 { get; set; }
    [UInt(64)] public ulong Unknown1 { get; set; }
}
