using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireStruct]
public sealed partial class AccountInventoryItem : IWritable
{
    [UInt(64)] public ulong UserInventoryId { get; set; }
    [UInt(32)] public uint AccountItemId { get; set; }
    [UInt(5)] public byte Unused1 { get; set; } // sometimes set to 6, value not used by client
    [UInt(1)] public bool Unused2 { get; set; }
    public Identity Owner { get; set; } = new(); // some account items are bound to a specific character
                                                 // value is ent by server but not used by client

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UserInventoryId);
        writer.Write(AccountItemId);
        writer.Write(Unused1, 5u);
        writer.Write(Unused2);
        Owner.Write(writer);
    }
}
