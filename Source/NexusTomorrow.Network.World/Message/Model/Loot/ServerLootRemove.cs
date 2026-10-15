using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

[WireMessage(GameMessageOpcode.ServerLootRemove, Direction.ServerToClient)]
public sealed partial class ServerLootRemove
{
    [UInt(32)] public uint OwnerUnitId { get; set; }
}
