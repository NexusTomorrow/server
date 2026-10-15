using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireStruct]
public sealed partial class PendingAccountItem
{
    [UInt(64)] public ulong PendingItemGroupId { get; set; } // never seen to be used
    [UInt(32)] public uint AccountItemId { get; set; }
    [UInt(64)] public ulong Unused2 { get; set; }
    [WideString] public string TransactionId { get; set; } // used a format like 89d2a447-df3c-4c3c-a135-d4eb8dfa4fd7 but could be anything
    [UInt(32)] public uint Unused3 { get; set; }
    public Identity Unused4 { get; set; } = new();
    [UInt(64)] public ulong Unused5 { get; set; }
    [UInt(5)] public byte Unknown { get; set; } // only see values 6 and 7 in sniffs, value of 6 means item is not giftable
    [UInt(64)] public ulong Unused6 { get; set; }
    public Identity Unused7 { get; set; } = new();
}
