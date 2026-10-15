using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

[WireMessage(GameMessageOpcode.ClientLootAssignMaster, Direction.ClientToServer)]
public sealed partial class ClientLootAssignMaster
{
    [UInt(32)] public uint OwnerUnitId { get; private set; }
    [UInt(32)] public uint LootUnitId { get; private set; }
    public Identity Assignee { get; private set; } = new Identity();
}
