using NexusTomorrow.Game.Static.Loot;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

[WireMessage(GameMessageOpcode.ServerLootRoll, Direction.ServerToClient)]
public sealed partial class ServerLootRoll
{
    [UInt(32)] public uint LootUnitId { get; set; }
    public Identity Roller { get; set; } = new Identity();
    [UInt(18)] public uint ItemId { get; set; } // Can be Item2Id, LootSpellId, VirtualItemId, AccountItemId
    [UInt(32)] public LootRollAction Action { get; set; }
}
