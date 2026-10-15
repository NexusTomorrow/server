using NexusTomorrow.Game.Static.Loot;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

// Notifies the player when someone else loots an item.
// Ignored if LooterUnitId is the player's UnitId
[Message(GameMessageOpcode.ServerLootNotification)]
[WireMessage(GameMessageOpcode.ServerLootNotification, Direction.ServerToClient)]
public sealed partial class ServerLootNotification : IWritable
{
    [UInt(32)] public uint LootUnitId { get; set; }
    [UInt(32)] public uint ItemId { get; set; } // Can be Item2Id, LootSpellId, VirtualItemId, AccountItemId
    [UInt(32)] public uint Amount { get; set; }
    [UInt(32)] public uint LooterUnitId { get; set; }
    [UInt(32)] public LootItemType Type { get; set; }
    [UInt(64)] public ulong RandomCircuitData { get; set; }
    [UInt(32)] public uint RandomGlyphData { get; set; }
    [UInt(32)] public uint ItemQuality2Id { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(LootUnitId);
        writer.Write(ItemId);
        writer.Write(Amount);
        writer.Write(LooterUnitId);
        writer.Write(Type, 32u);
        writer.Write(RandomCircuitData);
        writer.Write(RandomGlyphData);
        writer.Write(ItemQuality2Id);
    }
}
