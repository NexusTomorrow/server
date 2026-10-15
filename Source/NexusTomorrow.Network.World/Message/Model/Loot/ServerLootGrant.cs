using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

[WireMessage(GameMessageOpcode.ServerLootGrant, Direction.ServerToClient)]
public sealed partial class ServerLootGrant
{
    [UInt(32)] public uint OwnerUnitId { get; set; }
    [UInt(32)] public uint LooterUnitId { get; set; }
    public LootItem LootItem { get; set; }
}
