using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

[Message(GameMessageOpcode.ServerLootRemove)]
[WireMessage(GameMessageOpcode.ServerLootRemove, Direction.ServerToClient)]
public sealed partial class ServerLootRemove : IWritable
{
    [UInt(32)] public uint OwnerUnitId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(OwnerUnitId);
    }
}
